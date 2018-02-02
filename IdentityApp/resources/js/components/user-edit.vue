<template>
   <div v-if="loaded">
      
		<div class="row">
			<div class="col-sm-3">
				<h2>{{ title  }}</h2>
			</div>
			<div class="col-sm-3">
				
			</div>
			<div class="col-sm-3" style="margin-top: 20px;">
				
			</div>
			<div class="col-sm-3" style="margin-top: 20px;">

				<a @click.prevent="cancel" href="#" class="btn btn-default pull-right title-controll">
					<i class="fa fa-arrow-circle-o-left" aria-hidden="true"></i>
					返回
				</a>
			</div>
		</div>
      <hr/>
	
      <form @submit.prevent="onSubmit" @keydown="clearErrorMsg($event.target.name)" class="form-horizontal">
			
			<div class="form-group">
				<label class="col-md-2 control-label">UserName</label>
				<div class="col-md-10">
					<input name="user.name" v-model="form.user.name" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('user.name')" v-text="form.errors.get('user.name')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">姓名</label>
				<div class="col-md-10">
					<input name="profile.fullname" v-model="form.user.profile.fullname" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('profile.fullname')" v-text="form.errors.get('profile.fullname')"></small>
				
				</div>
         </div>
			<div v-if="isCreate" class="form-group">
				<label class="col-md-2 control-label">角色</label>
				<div class="col-md-10">
					<select  v-model="role" class="form-control"  style="max-width:180px">
						<option v-for="(item,index) in roleOptions" :key="index" :value="item.value" v-text="item.text"></option>
					</select>
				</div>
         </div>
			<div v-else class="form-group">
				<label class="col-md-2 control-label">角色</label>
				<div class="col-md-10">
					<check-box-list :options="roleOptions" :default_values="form.roles" 
						@select-changed="onRolesChanged">
					</check-box-list>
					<small class="text-danger" v-if="rolesError" v-text="rolesError"></small>
				</div>
         </div>
			
			
			<div class="form-group">
				<label class="col-md-2 control-label"></label>
				
				<div v-if="submitting"  class="col-md-10">
					<button class="btn btn-default">
                  <i class="fa fa-spinner fa-spin"></i> 
                  處理中
               </button>
				</div>
				<div v-else class="col-md-10">
					<button class="btn btn-success" type="submit">
						<i class="fa fa-floppy-o" aria-hidden="true"></i>
							確認存檔
						</button>
					&nbsp;&nbsp;&nbsp;
					<button @click.prevent="cancel" class="btn btn-default">
						
						取消
					</button>
				</div>
         </div>
			
      </form>
   </div>      
</template>

<script>
export default {
	name:'UserEdit',
	props:{
		id:{
			type:String,
			default:''
		}
	},
	data(){
		return {
			loaded:false,
			title:'',
			role:'',
			roleOptions:[],
			rolesError:'',

			form:{},
			submitting:false
			
		}
	},
	computed:{
		isCreate(){
         return !this.id;
      }
	},
	beforeMount() {
		this.init();
	},
	methods:{
		cancel(){
			this.$emit('cancel');
		},
		init(){
			if(this.isCreate){
				this.title = '新增使用者';
			}else{
				this.title += '編輯使用者';				
			}
         
			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=UserAdmin.create();                  
			else  getData=UserAdmin.edit(this.id);  

			getData.then(model => {

				this.form = new Form({
					   roles:model.roles,
						user:{
							...model.user
						}
					});

				if(this.isCreate){
					this.role = model.roles[0];
				}

				this.roleOptions=model.roleOptions.slice(0);

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onRolesChanged(selectedValues){
			this.form.roles=selectedValues.slice(0);
			this.rolesError='';
		},
		onSubmit(){
			

			if(this.isCreate) this.form.roles=[this.role];

			if(!this.form.roles || !this.form.roles.length){
				this.rolesError='請選擇角色';
				return;
			} 

			this.submitting=true;

			let save=null;

			if(this.isCreate) save=UserAdmin.store(this.form); 
			else  save=UserAdmin.update(this.id,this.form);  

			save.then(() => {

					this.submitting=false;
				   this.$emit('saved');
					Helper.BusEmitOK('資料已存檔');
				
				})
				.catch(error => {
					Helper.BusEmitError(error,'存檔失敗');
					this.submitting=false;
				})


			
		},
		clearErrorMsg(name) {
      	this.form.errors.clear(name);
      },
	}
}
</script>


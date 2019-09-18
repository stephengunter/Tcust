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
				<label class="col-md-2 control-label">Email</label>
				<div class="col-md-10">
					<input name="user.email" v-model="form.user.email" class="form-control" :disabled="!isCreate"/>
					<small class="text-danger" v-if="emailError" v-text="emailError"></small>


					<input type="hidden" name="userId" v-model="form.user.userId" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('user.userId')" v-text="form.errors.get('user.userId')"></small>
				</div>
         </div>
         <div v-show="!isCreate" class="form-group">
				<label class="col-md-2 control-label">姓名</label>
				<div class="col-md-10">
					<input name="user.name" v-model="form.user.name" class="form-control"  :disabled="!isCreate"/>
					<small class="text-danger" v-if="form.errors.has('user.name')" v-text="form.errors.get('user.name')"></small>
				
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
			type:Number,
			default:0
		}
	},
	data(){
		return {
			loaded:false,
			title:'',
			email:'',
			emailError:'',
			

			form:{},
			submitting:false
			
		}
	},
	computed:{
		isCreate(){
         return this.id == 0;
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
			let getData = null
			if(this.isCreate) getData = Manage.create();                  
			else getData = Manage.edit(this.id);  

			getData.then(model => {
				this.form = new Form({
					user:{
						...model.user
					}
				});
				this.loaded = true;
				
			})
			.catch(error => {
				Helper.BusEmitError(error);                   
				this.loaded = false;
			})
		},
		onSubmit(){
			let email = this.form.user.email
			if(!email){
				this.emailError = '請填寫Email';
				return;
			}

			this.submitting = true;
			let find = Api.getUserByEmail(email);
			find.then(user => {
			   this.submit(user);
			})
			.catch(error => {
				this.emailError = '這個Email不存在';
				this.submitting = false;
			})			
		},
		submit(user){

			this.form.user.userId = user.id;
			this.form.user.name = user.profile.fullname;

			let save = null;

			if(this.isCreate) save = Manage.store(this.form);            
			else save = Manage.update(this.id,this.form);  

			save.then(user => {
					Helper.BusEmitOK();
				   this.$emit('saved');
				})
				.catch(error => {
					Helper.BusEmitError(error,'存檔失敗');
					this.submitting=false;
				})
		},
		clearErrorMsg(name) {
			if(name == 'user.email'){
				this.emailError = '';
				name = 'user.userId'
			} 
      	this.form.errors.clear(name);
      },
	}
}
</script>


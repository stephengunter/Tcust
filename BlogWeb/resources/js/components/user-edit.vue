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
					<input name="email" v-model="email" class="form-control" />
					<small class="text-danger" v-if="emailError" v-text="emailError"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">名稱</label>
				<div class="col-md-10">
					<input name="user.name" v-model="form.user.name" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('user.name')" v-text="form.errors.get('user.name')"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">權限</label>
				<div class="col-md-10">
					<check-box :value="1" :default="true" text="權限A"></check-box>
				</div>
         </div>
			
         <div class="form-group">
				<label class="col-md-2 control-label">備註</label>
				<div class="col-md-10">
					<input name="user.ps" v-model="form.user.ps" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('user.ps')" v-text="form.errors.get('user.ps')"></small>
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
		permissions:{
			type:Array,
			default:null
		},
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
			let getData=null

			if(this.isCreate)   getData=Manage.create();                  
			else  getData=Manage.edit(this.id);  

			getData.then(model => {
			
				this.form = new Form({
					...model
				});

				//if(this.isCreate) this.form.user.categoryId=this.category.value;
				

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onCategorySelected(category){
			this.form.user.categoryId=category.value
		},
		setDate(val){
			this.form.user.date=val;
		},
		setContent(val){
			this.form.user.content=val
			this.onSubmit()
      },
		onSubmit(){
			this.submitting=true;

			let medias=this.$refs.mediaEdit.getMedias();
			this.form.user.medias=medias;

			let contentValue=this.$refs.contentEditor.getValue();
			this.form.user.content=contentValue;
			
			let save=null;

			if(this.isCreate)  save=UserAdmin.store(this.form);            
			else  save=UserAdmin.update(this.id,this.form);  

			save.then(user => {

					let setUser = new Promise( (resolve, reject) => {
						this.form = new Form({
							user:user
						});
						resolve(true);
					
					});

					setUser.then(()=>{
						this.submitMedias();	
					});

					
				
				})
				.catch(error => {
					Helper.BusEmitError(error,'存檔失敗');
				})


			
		},
		submitMedias(){
			let save=this.$refs.mediaEdit.submit();	
			save.then(result => {
					this.submitting=false;
				   this.$emit('saved');
					Helper.BusEmitOK('資料已存檔');
				})
				.catch(error => {
					Helper.BusEmitError(error,'存檔失敗');
				})
		},
		clearErrorMsg(name) {
      	this.form.errors.clear(name);
      },
	}
}
</script>


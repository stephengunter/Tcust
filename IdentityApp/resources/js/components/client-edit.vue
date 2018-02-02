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
			<div v-if="isCreate" class="form-group">
				<label class="col-md-2 control-label">Type</label>
				<div class="col-md-10">
					<select  v-model="form.type" @change="onTypeChanged" class="form-control"  style="max-width:180px">
						<option v-for="(item,index) in typeOptions" :key="index" :value="item.value" v-text="item.text"></option>
					</select>
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">ClientId</label>
				<div class="col-md-10">
					<input name="client.clientId" v-model="form.client.clientId" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('client.clientId')" v-text="form.errors.get('client.clientId')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">名稱</label>
				<div class="col-md-10">
					<input name="client.title" v-model="form.client.title" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('client.title')" v-text="form.errors.get('client.title')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">密碼</label>
				<div class="col-md-10">
					<input name="client.secret" v-model="form.client.secret" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('client.secret')" v-text="form.errors.get('client.secret')"></small>
				
				</div>
         </div>
			<div v-if="isCreate">
				<div class="form-group">
					<label class="col-md-2 control-label">Uri</label>
					<div class="col-md-10">
						<input name="client.uri" v-model="form.client.uri" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('client.uri')" v-text="form.errors.get('client.uri')"></small>
					
					</div>
				</div>
			</div>
			<div v-else>
				<div class="form-group">
					<label class="col-md-2 control-label">RedirectUri</label>
					<div class="col-md-10">
						<input name="client.redirectUri" v-model="form.client.redirectUri" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('client.redirectUri')" v-text="form.errors.get('client.redirectUri')"></small>
					
					</div>
				</div>

				<div class="form-group">
					<label class="col-md-2 control-label">PostRedirectUri</label>
					<div class="col-md-10">
						<input name="client.postLogoutRedirectUri" v-model="form.client.postLogoutRedirectUri" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('client.postLogoutRedirectUri')" v-text="form.errors.get('client.postLogoutRedirectUri')"></small>
					
					</div>
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">FrontChannelLogoutUri</label>
					<div class="col-md-10">
						<input name="client.frontChannelLogoutUri" v-model="form.client.frontChannelLogoutUri" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('client.frontChannelLogoutUri')" v-text="form.errors.get('client.frontChannelLogoutUri')"></small>
					
					</div>
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
	name:'ClientEdit',
	props:{
		category:{
			type:Object,
			default:null
		},
		categories:{
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

			typeOptions:[],

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
				this.title = '新增 Client';
			}else{
				this.title += '編輯 Client';				
			}
         
			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=ClientAdmin.create();                  
			else  getData=ClientAdmin.edit(this.id);  

			getData.then(model => {

				if(this.isCreate){
					this.typeOptions=model.typeOptions.slice(0);
					this.form = new Form({
						type:model.type,
						client:{
							...model.client
						}
					});
				}else{
					this.form = new Form({
					
						client:{
							...model.client
						}
					});

				}

				
				

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onTypeChanged(){

		},
		onSubmit(){
			this.submitting=true;

			let save=null;

			if(this.isCreate)  save=ClientAdmin.store(this.form);            
			else  save=ClientAdmin.update(this.id,this.form);  

			save.then(() => {

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


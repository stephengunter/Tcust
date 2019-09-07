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
				<label class="col-md-2 control-label">名稱</label>
				<div class="col-md-10">
					<input name="name" v-model="form.name" class="form-control" style="max-width:280px"/>
					<small class="text-danger" v-if="form.errors.has('name')" v-text="form.errors.get('name')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">代碼</label>
				<div class="col-md-10">
					<input name="code" v-model="form.code" class="form-control" style="max-width:280px"/>
					<small class="text-danger" v-if="form.errors.has('code')" v-text="form.errors.get('code')"></small>
				
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
	name:'DepartmentEdit',
	props:{
		id:{
			type:Number,
			default:0
		}
	},
	data(){
		return {
			loaded : false,
			title:'',

			form: {},
			submitting : false
			
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
				this.title = '新增部門';
			}else{
				this.title += '編輯部門';				
			}

			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate) getData = DepartmentAdmin.create();                  
			else getData = DepartmentAdmin.edit(this.id);  

			getData.then(model => {

				this.form = new Form({ ...model });

				this.loaded = true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded = false;
			})
		},
		onSubmit(){
			this.submitting = true;
			
			let save = null;

			if(this.isCreate)  save = DepartmentAdmin.store(this.form);            
			else save = DepartmentAdmin.update(this.id,this.form);  

			save.then(term => {
				Helper.BusEmitOK();
				this.$emit('saved');
			
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


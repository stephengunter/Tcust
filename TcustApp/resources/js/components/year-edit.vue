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
				<label class="col-md-2 control-label">年度</label>
				<div class="col-md-10">
					<input name="year.year" v-model="form.year.year" class="form-control" style="max-width:180px"/>
					<small class="text-danger" v-if="form.errors.has('year.year')" v-text="form.errors.get('year.year')"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">名稱</label>
				<div class="col-md-10">
					<input name="year.title" v-model="form.year.title" class="form-control" style="max-width:180px" />
					<small class="text-danger" v-if="form.errors.has('year.title')" v-text="form.errors.get('year.title')"></small>
				
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
	name:'YearEdit',
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
				this.title = '新增學年度';
			}else{
				this.title += '編輯學年度';				
			}

			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=YearAdmin.create();                  
			else  getData=YearAdmin.edit(this.id);  

			getData.then(model => {
				
				
				this.form = new Form({
					year:{
						...model.year
					}
				});

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onSubmit(){
			this.submitting=true;
			
			let save=null;

			if(this.isCreate)  save=YearAdmin.store(this.form);            
			else  save=YearAdmin.update(this.id,this.form);  

			save.then(year => {
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


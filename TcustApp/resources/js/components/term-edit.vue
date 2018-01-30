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
					<select  v-model="form.term.termYearId" @change="onYearChanged" class="form-control"  style="max-width:180px">
						<option v-for="(item,index) in yearOptions" :key="index" :value="item.value" v-text="item.text"></option>
					</select>
					<small class="text-danger" v-if="form.errors.has('term.termYearId')" v-text="form.errors.get('term.termYearId')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">順序</label>
				<div class="col-md-10">
					<select  v-model="form.order"  @change="onOrderChanged" class="form-control"  style="max-width:180px">
						<option v-for="(item,index) in orderOptions" :key="index" :value="item.value" v-text="item.text"></option>
					</select>
					<small class="text-danger" v-if="form.errors.has('order')" v-text="form.errors.get('order')"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">名稱</label>
				<div class="col-md-10">
					<input name="term.title" v-model="form.term.title" class="form-control" style="max-width:180px" />
					<small class="text-danger" v-if="form.errors.has('term.title')" v-text="form.errors.get('term.title')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">進行中</label>
				<div class="col-md-10">
					<input type="hidden" v-model="form.term.active"  >
               <toggle :items="activeOptions"   :default_val="form.term.active" @selected="setActive"></toggle>
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
	name:'termEdit',
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

			activeOptions:TermAdmin.activeOptions(),
			yearOptions:[],
			orderOptions:TermAdmin.orderOptions(),

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
				this.title = '新增學期';
			}else{
				this.title += '編輯學期';				
			}

			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=TermAdmin.create();                  
			else  getData=TermAdmin.edit(this.id);  

			getData.then(model => {

				this.yearOptions=model.yearOptions.slice(0);

				this.form = new Form({
					order:model.order,
					term:{
						...model.term
					}
				});

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onYearChanged(){
			this.clearErrorMsg('order');
		},
		onOrderChanged(){
			this.clearErrorMsg('order');
		},
		setActive(val){
			this.form.term.active=val;
		},
		onSubmit(){
			this.submitting=true;
			
			let save=null;

			if(this.isCreate)  save=TermAdmin.store(this.form);            
			else  save=TermAdmin.update(this.id,this.form);  

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


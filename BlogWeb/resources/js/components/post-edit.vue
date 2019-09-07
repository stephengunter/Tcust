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
					<label class="col-md-2 control-label">分類</label>
					<div class="col-md-10">
					
						<check-box-list  :options="categoryOptions" :default_values="form.post.categoryIds" 
							@select-changed="onCategorySelected">
						</check-box-list>
					</div>
					
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">學年編號</label>
					<div class="col-md-4">
						<input name="post.termNumber" v-model="form.post.termNumber" class="form-control" style="max-width:180px"/>
						<small class="text-danger" v-if="form.errors.has('post.termNumber')" v-text="form.errors.get('post.termNumber')"></small>
					
					</div>
					<label class="col-md-2 control-label">文章編號</label>
					<div class="col-md-4">
						<input name="post.number" v-model="form.post.number" class="form-control" style="max-width:180px" />
						<small class="text-danger" v-if="form.errors.has('post.number')" v-text="form.errors.get('post.number')"></small>
					
					</div>
						
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">發稿單位</label>
					<div class="col-md-10 form-inline">
						<check-box-list  :options="departmentOptions" :default_values="form.issuerIds" 
							@select-changed="onIssueByChanged">
						</check-box-list>
						<small class="text-danger" v-if="form.errors.has('issuerIds')" v-text="form.errors.get('issuerIds')"></small>
					
					</div>
						
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">活動單位</label>
					<div class="col-md-10 form-inline">
						<check-box :value="true" text="同上"
							:default="sameAsIssueBy" 
							@selected="onSameAsIssueBy" @unselected="onNotSameAsIssueBy">

						</check-box>

						<small class="text-danger" v-if="form.errors.has('departmentIds')" v-text="form.errors.get('departmentIds')"></small>

						<input  v-if="!sameAsIssueBy" readonly :value="departmentNames" class="form-control"/>
						<button v-if="!sameAsIssueBy" @click.prevent="departmentSelector.show=true" class="btn btn-primary btn-sm" >
							<i class="fa fa-edit"></i> 
						</button>
					</div>
					
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">標題</label>
					<div class="col-md-10">
						<input name="post.title" v-model="form.post.title" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('post.title')" v-text="form.errors.get('post.title')"></small>
					
					</div>
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">作者</label>
					<div class="col-md-10">
						<input name="post.author" v-model="form.post.author" class="form-control" />
						<small class="text-danger" v-if="form.errors.has('post.author')" v-text="form.errors.get('post.author')"></small>
					</div>
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">內容</label>
					<div class="col-md-10">
						<html-editor ref="contentEditor" :model="form.post.content" :toolbar="textEditor.toolbar"
							:height="textEditor.height"     @html-value="setContent">  
						</html-editor>  

						<small class="text-danger" v-if="form.errors.has('post.content')" v-text="form.errors.get('post.content')"></small>
					</div>
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">日期</label>
					<div class="col-md-10">
						<div class="form-group row">
							<div class="col-md-3">
								<datetime-picker :date="form.post.beginDate" @selected="setBeginDate"></datetime-picker>
							</div>
							<div class="col-md-1 text-center" style="height:3em">
								<span style="position: absolute;top: 10%;font-size:1.2em">   ~ </span>	
							</div>	
							<div class="col-md-3">
								<datetime-picker :date="form.post.endDate" @selected="setEndDate" :can_clear="true"></datetime-picker>
							</div>
						</div>
					</div>
				</div>
				<div v-if="canReview" class="form-group">
					<label class="col-md-2 control-label">置頂文章</label>
					<div class="col-md-10">
						<input type="hidden" v-model="form.post.top"  >
						<toggle :items="topOptions"   :default_val="form.post.top" @selected="setTop"></toggle>
					</div>
				</div>
				<div class="form-group">
					<label class="col-md-2 control-label">圖片/影片</label>
					<div class="col-md-10">
						<media-edit :post="form.post" ref="mediaEdit"></media-edit>
					</div>
					
				</div>
				<div v-if="canReview" v-show="!isCreate" class="form-group">
					<label class="col-md-2 control-label">狀態</label>
					<div class="col-md-10">
						<input type="hidden" v-model="form.post.reviewed"  >
						<toggle :items="reviewedOptions"   :default_val="form.post.reviewed" @selected="setReviewed"></toggle>
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

		<modal :showbtn="false"  :show.sync="departmentSelector.show"  :title="departmentSelector.title" effect="fade" :width="1200"
				@closed="departmentSelector.show=false"  >
			<div slot="modal-body" class="modal-body">
				<check-box-list v-if="departmentSelector.show"  :options="departmentOptions" :default_values="form.departmentIds" 
					@select-changed="onDepartmentIdsChanged">
				</check-box-list>
			</div>
		</modal>
   </div>      
</template>

<script>
import MediaEdit from './media-edit';
export default {
	name:'PostEdit',
	components: {
      'media-edit':MediaEdit
   },
	props:{
		category:{
			type:Object,
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

			canReview:0,

			categoryOptions:[],
				
			departmentOptions:[],

			defaultDepartment:{},  //文宣公關組

			departmentSelector:{
				title:'請選擇活動單位',
				show:false,
				selected:0,
			},

			

			sameAsIssueBy:true,

			departmentNames:'',

			topOptions:Post.topOptions(),

			reviewedOptions:Post.reviewedOptions(),

			textEditor:{
				height:360,
            	toolbar:[]
         	},

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
	mounted(){
		$('#postContent').summernote();
		
	},
	methods:{
		cancel(){
			this.$emit('cancel');
		},
		init(){
			if(this.isCreate){
				this.title = '新增文章';
			}else{
				this.title += '編輯文章';				
			}

			this.fetchData();
			this.loadDepartments();
			
		},
		loadDepartments(){
			let getData=Api.getDepartments();
			getData.then(departments => {
				this.departmentOptions=departments.map(item=>{
					return {
						text:item.name,
						value:item.id
					};
				});

				this.departmentOptions.push({
					text:'其他',
					value:0
				});

				this.defaultDepartment=departments.find(item=>{
					return item.code=='103010';  //文宣公關組
				});
			})
			.catch(error=> {
				Helper.BusEmitError(error);   
			})
		},
		fetchData(){
			let getData=null;

			if(this.isCreate) getData=PostAdmin.create();         
			else  getData=PostAdmin.edit(this.id);  

			getData.then(model => {
			
				
				this.form = new Form({
					post:{
						...model.post
					},
					issuerIds:model.issuerIds.slice(0),
					departmentIds:model.departmentIds.slice(0)

				});

				this.canReview=model.canReview;

				this.categoryOptions=model.categoryOptions.map(item=>{
					return { text:item.text , value: parseInt(item.value) };
				});

				

				if(this.isCreate){
					if(parseInt(this.category.value)){
						this.form.post.categoryIds.push(parseInt(this.category.value));
					}  
					

					this.getActiveTerm();
				} 
				

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		getActiveTerm(){
			let getData=Api.getActiveTerm();
			getData.then(term => {

				this.form.post.termNumber=term.number;
				
			})
			.catch(error=> {
				
			})
		},
		onCategorySelected(values){
			this.form.post.categoryIds=values.slice(0);
		},
		onIssueByChanged(values){
            this.form.issuerIds=values.slice(0);
			if(this.sameAsIssueBy){
				this.onDepartmentIdsChanged(this.form.issuerIds);
			}

            if(values.length)  this.form.errors.clear('issuerIds');
        },
		onDepartmentIdsChanged(values){
            this.form.departmentIds=values.slice(0);
            if(values.length){
				if(this.sameAsIssueBy){
					let index= this.form.departmentIds.findIndex(id=>{
						
						return id == this.defaultDepartment.id;
					});

					if(index>=0){
						this.form.departmentIds.splice(index, 1);
					}
					
				}
				this.loadDepartmentNames();
				this.form.errors.clear('departmentIds');
			}else{
				this.departmentNames='';
			}  
        },
		loadDepartmentNames(){
			
			if(this.form.departmentIds.length){
				let departments=this.departmentOptions.filter(item=>{
					return this.form.departmentIds.includes(item.value);
				});

				this.departmentNames=departments.map(item=>{
					return item.text;
				});
			}else{
				this.departmentNames='';
			}

			
		},
		onSameAsIssueBy(){
			this.sameAsIssueBy=true;
			this.onDepartmentIdsChanged(this.form.issuerIds);
		},
		onNotSameAsIssueBy(){
			this.sameAsIssueBy=false;
			this.departmentSelector.show=true;
		},
		setBeginDate(val){
			this.form.post.beginDate=val;
		},
		setEndDate(val){
			this.form.post.endDate=val;
		},
		setReviewed(val) {
         	this.form.post.reviewed = val;
		},
		setTop(val) {
         	this.form.post.top = val;
		},
		setContent(val){
			this.form.post.content=val;
			this.onSubmit();
		},
		onSubmit(){
			this.submitting=true;

			let medias=this.$refs.mediaEdit.getMedias();
			this.form.post.medias=medias;

			let contentValue=this.$refs.contentEditor.getValue();
			this.form.post.content=contentValue;
			
			let save=null;

			if(this.isCreate)  save=PostAdmin.store(this.form);            
			else  save=PostAdmin.update(this.id,this.form);  

			save.then(post => {

					let setPost = new Promise( (resolve, reject) => {
						this.form = new Form({
							post:post
						});
						resolve(true);
					
					});

					setPost.then(()=>{
						this.submitMedias();	
					});

					
				
				})
				.catch(error => {
					Helper.BusEmitError(error,'存檔失敗');
					this.submitting=false;
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


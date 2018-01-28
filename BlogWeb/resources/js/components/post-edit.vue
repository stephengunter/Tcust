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
				<label class="col-md-2 control-label">學年編號</label>
				<div class="col-md-10">
					<input name="post.termNumber" v-model="form.post.termNumber" class="form-control" style="max-width:180px"/>
					<small class="text-danger" v-if="form.errors.has('post.termNumber')" v-text="form.errors.get('post.termNumber')"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">文章編號</label>
				<div class="col-md-10">
					<input name="post.number" v-model="form.post.number" class="form-control" style="max-width:180px" />
					<small class="text-danger" v-if="form.errors.has('post.number')" v-text="form.errors.get('post.number')"></small>
				
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">分類</label>
				<div class="col-md-10">
					<drop-down :items="categoryOptions" :selected="form.post.categoryId"
					  @selected="onCategorySelected">
					</drop-down>
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
					<datetime-picker :date="form.post.date" @selected="setDate"></datetime-picker>
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
		test(){
			alert(this.form.errors.get('title'));
		},
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
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=PostAdmin.create();                  
			else  getData=PostAdmin.edit(this.id);  

			getData.then(model => {
			
				
				this.form = new Form({
					post:{
						...model.post
					}
				});

				this.canReview=model.canReview;

				this.categoryOptions=model.categoryOptions.slice(0);

				if(this.isCreate){
					if(parseInt(this.category.value))  this.form.post.categoryId=this.category.value;
					else this.form.post.categoryId=model.categoryOptions[0].value;
				} 
				

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		onCategorySelected(category){
			this.form.post.categoryId=category.value;
		},
		setDate(val){
			this.form.post.date=val;
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


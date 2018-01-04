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
					<input name="post.termNumber" v-model="form.post.termNumber" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('post.termNumber')" v-text="form.errors.get('post.termNumber')"></small>
				
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">文章編號</label>
				<div class="col-md-10">
					<input name="post.number" v-model="form.post.number" class="form-control" />
					<small class="text-danger" v-if="form.errors.has('post.number')" v-text="form.errors.get('post.number')"></small>
				
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
					<textarea rows="12" name="post.content"  v-model="form.post.content" class="form-control" >
               </textarea>
					<small class="text-danger" v-if="form.errors.has('post.content')" v-text="form.errors.get('post.content')"></small>
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">日期</label>
				<div class="col-md-10">
					<datetime-picker :date="form.post.date" @selected="setDate"></datetime-picker>
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">圖片</label>
				<div class="col-md-10">
					<media-edit :post="form.post" ref="mediaEdit"></media-edit>
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
		test(){
			alert(this.form.errors.get('title'));
		},
		cancel(){
			this.$emit('cancel');
		},
		init(){
			if(this.isCreate){
				this.title = '新增日誌';
			}else{
				this.title += '編輯日誌';				
			}

			this.fetchData();
			
		},
		fetchData(){
			let getData=null

			if(this.isCreate)   getData=Post.create();                  
			else  getData=Post.edit(this.id);  

			getData.then(model => {
			
				this.form = new Form({
					...model
				});

				this.loaded=true;
				
			})
			.catch(error=> {
				Helper.BusEmitError(error);                   
				this.loaded=false;
			})
		},
		setDate(val){
			this.form.post.date=val;
		},
		onSubmit(){
			this.submitting=true;

			let medias=this.$refs.mediaEdit.getMedias();
			this.form.post.medias=medias;
			
			let save=null;

			if(this.isCreate)  save=Post.store(this.form);            
			else  save=Post.update(this.id,this.form);  

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


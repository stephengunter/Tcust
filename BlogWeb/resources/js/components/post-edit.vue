<template>
   <div>
      <h2>{{ title  }}</h2>
      <hr/>
      <form @submit.prevent="onSubmit"  class="form-horizontal">
         <div class="form-group">
				<label class="col-md-2 control-label">標題</label>
				<div class="col-md-10">
					<input name="title" v-model="post.title" class="form-control" />
					<span class="text-danger"></span>
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">作者</label>
				<div class="col-md-10">
					<input v-model="post.author" name="title" class="form-control" />
					<span class="text-danger"></span>
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">內容</label>
				<div class="col-md-10">
					<textarea rows="12" v-model="post.content" class="form-control" >
               </textarea>
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label">日期</label>
				<div class="col-md-10">
					<datetime-picker :date="post.date" @selected="setDate"></datetime-picker>
				</div>
         </div>
			<div class="form-group">
				<label class="col-md-2 control-label"></label>
				<div class="col-md-10">
					<button class="btn btn-success" type="submit">存檔</button>
					&nbsp;&nbsp;&nbsp;
					<button class="btn btn-default">取消</button>
				</div>
         </div>
			
      </form>
   </div>      
</template>

<script>
export default {
	name:'PostEdit',
	props:{
		id:{
			type:Number,
			default:0
		}
	},
	data(){
		return {
			post:{
				title:'',
				date:'',
				author:'',
				content:'',

			}
			
			
		}
	},
	computed:{
		title(){
			return '新增文章';
		},

	}, 
	methods:{
		setDate(val){
			this.post.date=val;
		},
		onSubmit(){
			let url='/admin/posts/store';
			axios.post(url, this.post)
			.then(response => {
				alert('then');
			})
			.catch(error => {
				alert('err');
			})
		}
	}
}
</script>


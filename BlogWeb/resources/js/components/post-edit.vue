<template>
   <div>
      <h2>{{ title  }}</h2>
      <hr/>
      <form  class="form-horizontal">
         <div class="form-group">
				<label class="col-md-2 control-label">標題</label>
				<div class="col-md-10">
					<input name="title" class="form-control" />
					<span class="text-danger"></span>
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">作者</label>
				<div class="col-md-10">
					<input name="title" class="form-control" />
					<span class="text-danger"></span>
				</div>
         </div>
         <div class="form-group">
				<label class="col-md-2 control-label">內容</label>
				<div class="col-md-10">
					 <textarea rows="12"  class="form-control" >
                </textarea>
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
    computed:{
      title(){
         return '新增文章';
      },

    }, 
}
</script>


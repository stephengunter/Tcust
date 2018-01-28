<template>
   <div v-if="model" class="row">
      <div class="col-sm-8 blog-main">
         <div class="blog-post">
            <h5 class="blog-post-title" style="font-size:1.5em">{{ model.post.title }}</h5>
            <span class="blog-post-meta">
               {{ model.post.author }}   
               <br />
               
					
					<i class="fa fa-eye" aria-hidden="true"></i> {{ model.post.clickCount}}
					<button class="btn btn-light btn-sm btn-copy" :data-clipboard-text="model.post.url"
						data-toggle="tooltip" title="複製連結" data-placement="top">
						 <i class="fa fa-clipboard" aria-hidden="true"></i>
					</button>
            </span>
            <hr style="margin-top:0">
            <p v-html="model.post.content">
             
            </p>
            
         </div><!-- /.blog-post -->
      </div>
      <div  class="col-sm-1">
            
      </div>
      <div class="col-sm-3">
			<div class="sidebar-module">
				<a v-for="(item,index) in model.post.medias" :key="index" data-fancybox="gallery" :data-caption="item.title" 
					:href="item.path"  >
					<img :alt="item.title" :src="item.previewPath" style="width:210px;padding-top:5px">
				</a>
			</div>
			
      </div>  

      <nav class="blog-pagination">
        <a @click.prevent="back" class="btn btn-outline-primary"  href="#">
            返回
        </a>
		  

      </nav>
		
   </div>
</template>

<script>
export default {
   name:'PostDetailsView',
   props: {
      model: {
         type: Object,
         default: null
      }
   },
   data(){
      return {
         
      }
	},
	mounted(){
		
		let clipboard = new Clipboard('.btn-copy');

		clipboard.on('success', function(e) {
			Helper.BusEmitOK('連結已複製');
			e.clearSelection();
		});
   },
   methods:{
      fetchData(id){
			
         let getData = Post.details(id);

         getData.then(model => {

				this.$emit('details-fetched', model);

         })
         .catch(error => {
            Helper.BusEmitError(error);
            
         })
		},
      back(){
         this.$emit('back');
      }
   }
}
</script>


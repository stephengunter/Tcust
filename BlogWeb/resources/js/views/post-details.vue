<template>
   <div v-if="model" class="row">
      <div class="col-sm-8 blog-main">
         <div class="blog-post">
            <h5 class="blog-post-title" style="font-size:1.5em">{{ model.post.title }}</h5>
            <span class="blog-post-meta">
               {{ model.post.author }}   
               <br />
               <button  id="copy-url" class="btn btn-light btn-sm btn-copy" data-clipboard-text="" 
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
      id: {
         type: Number,
         default: 0
      }
   },
   data(){
      return {
         model:null,
      }
   },
   beforeMount(){
      this.fetchData();
   },
   methods:{
      fetchData(){
         let getData = Post.details(this.id);

         getData.then(model => {

            this.model={ ...model };

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


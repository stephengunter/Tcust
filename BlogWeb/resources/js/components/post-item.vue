<template>
   
   <article v-if="post">
      <div class="line-item hf-item-odd clearfix">
         <div class="content-image">
            <a class="image-link article-link" href="#" @click.prevent="onDetails">
               <img class="img-thumbnail summary-img" :src="post.cover.previewPath" />
               <span class="overlay article-overlay"></span>
            </a>
         </div>
         <div class="hf-info">
            <h2 class="post-title">
               <a class="article-link" href="#" @click.prevent="onDetails">
                  {{ post.title }}
                  <span class="overlay article-overlay"></span>
               </a>
            </h2>
            <div class="blog-post-meta author-text">
            {{ post.author }}
            </div>

            
            <div class="summary summary-text">
            {{ post.summary }}
            </div>
         </div>
        

        

      </div>
   </article>
  
</template>

<script>
   export default {
      name:'PostItem',
      props: {
         post: {
            type: Object,
            default: null
         },
      },
      methods:{
         onDetails(){
            
            this.$emit('details' , this.post.id);
         }
      }
   }
</script>

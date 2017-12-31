<template>
    
   <div v-if="model" id="index-view" class="row">
      
      <div id="summary-list" class="col-sm-8 blog-main">

         <post-item v-for="(post,index) in model.viewList" :key="index"
         :post="post" v-on:details="onDetails" >

         </post-item>

      </div>


      <div class="col-sm-3 offset-sm-1 blog-sidebar">
         


      </div>


   </div>
</template>


<script>
   import PostItem from '../components/post-item';
   export default {
      name:'PostIndexView',
      components: {
         PostItem
      },
      props: {
         model: {
            type: Object,
            default: null
         },
      },
      methods:{
         onDetails(id){
            alert(id);
            
         }
      }
   }
</script>



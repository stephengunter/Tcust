<template>
   <div>
      <div v-if="model" v-show="indexMode">
         
         <div class="row">
            <div class='col-sm-3'>
               
            </div>
            <div class='col-sm-3'>

            </div>
            <div class='col-sm-3' style="margin-top: 20px;">
               <searcher></searcher>
            </div>
            <div class='col-sm-3' style="margin-top: 20px;">

               <a @click.prevent="onCreate" href="#" class="btn btn-primary title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增文章
               </a>
            </div>
         </div>

         <hr/>

         <post-table :posts="model.viewList"></post-table>

      </div>
      <post-edit v-if="editting">
         

      </post-edit>

   </div> 
</template>


<script>
   import Searcher from '../components/searcher';
   import PostTable from '../components/post-table';
   import PostEdit from '../components/post-edit';
   export default {
      name:'PostAdminView',
      components: {
         Searcher,
         'post-table':PostTable,
         'post-edit':PostEdit
      },
      props: {
         model: {
            type: Object,
            default: null
         },
      },
      data(){
         return {
            selected:0,
            create:false,
         }
      },
      computed:{
         editting(){
            return this.create;
         },
         indexMode(){
            if(this.editting) return false;
            return true;
         }
      }, 
      methods:{
         onCreate(){
            this.create=true;
         },
         onDetails(id){
            alert(id);
            
         }
      }
   }
</script>



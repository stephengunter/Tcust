<template>
   <div class="row"> 
      <div  class="col-md-2 pagination-item paging-controll">
         <span>每頁: </span>
            <select v-model="model.pageSize" @change="onPageSizeChanged">
               <option>10</option>
               <option>25</option>
               <option>50</option>
            </select>
         <span>筆資料</span>
      </div>
      <div  class="col-md-2 pagination-item paging-controll">
         <small>Showing {{ first }} - {{ last }} of {{model.totalItems}}</small>
      </div>
      <div class="col-md-8 pagination-item">
         <pager   :total-page="model.totalPages"  :init-page="model.pageNumber"  @go-page="onPageChanged"></pager>
      </div>
   </div> 
</template>



<script>

import Pager from '../packages/components/pager';

export default {
   props: {
      model: {
         type: Object,
         default: null
      },
   },
   components: {
      Pager
   },
   data(){
		return {
         
		}
   },
   computed:{
	
		first(){
         if(!this.model) return 0;
         return this.model.pageSize * (this.model.pageNumber-1) + 1;
         
      },
      last(){
         if(!this.model) return 0;
         return this.first + this.model.viewList.length - 1;
      }
   },
   methods:{
      onPageChanged(params){
         
         this.$emit('page-changed', params.page);
      },
      onPageSizeChanged(){
         this.$emit('pagesize-changed');
      }
   }
  
}
</script>


<style scoped>

.paging-controll{
  margin-top: 15px;
}

</style>



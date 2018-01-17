<template>
   <div>
      <div v-if="model" >
         <div class="row">
            <div class="col-sm-3">
               <h2>文章審核</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
					
            </div>
            
            <div class="col-sm-3" style="margin-top: 20px;">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               
            </div>
         </div>

         <hr/>

         <post-table :model="model">
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </post-table>

      </div>
      
   </div> 
</template>


<script>
   import PostTable from '../components/post-table';
   export default {
      name:'ReviewAdminView',
      components: {
         'post-table':PostTable,
      },
      props: {
         init_model: {
            type: Object,
            default: null
			}
      },
      data(){
         return {
				model:null,
            
            
				params:{
					page:1,
					pageSize:10
				},
         }
      },
      beforeMount() {
         if(this.init_model){
				this.model={...this.init_model };
				this.params.page=this.init_model.pageNumber;
				this.params.pageSize=this.init_model.pageSize;
         }	
         
         
		},
      methods:{
         
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
         fetchData() {
				
            let getData = PostReview.index(this.params);

            getData.then(model => {

               this.model={ ...model };

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
         },
         
      }
   }
</script>






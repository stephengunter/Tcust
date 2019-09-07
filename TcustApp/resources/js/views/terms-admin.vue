<template>
   <div>
      <div v-if="model" v-show="indexMode">
         <div class="row">
            <div class="col-sm-3">
               <h2>學期管理</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
					
            </div>
            
            <div class="col-sm-3" style="margin-top: 20px;">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <a @click.prevent="onCreate" href="#" class="btn btn-primary pull-right title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增學期
               </a>
            </div>
         </div>

         <hr/>
         <term-table :model="model"  @edit="onEdit" >
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </term-table>
         

      </div>
      
      <term-edit v-if="editting"  :id="selected"   
         @saved="onIndex" @cancel="onIndex">
      </term-edit>
      
   </div>
</template>


<script>
   import Searcher from '../components/searcher';
   import TermTable from '../components/term-table';
   import TermEdit from '../components/term-edit';
   export default {
      name:'TermAdminView',
      components: {
         'searcher':Searcher,
         'term-table':TermTable,
         'term-edit':TermEdit
      },
      props: {
         init_model: {
            type: Object,
            default: null
			},
			can_delete:{
				type:Boolean,
				default:false
			}
      },
      data(){
         return {
				loaded:false,

				model:null,
				
            selected:0,
				create:false,
				
				
				params:{
					page:1,
					pageSize:10
				}
         }
      },
      beforeMount() {
         if(this.init_model){
				this.model={...this.init_model };
				this.params.page=this.init_model.pageNumber;
				this.params.pageSize=this.init_model.pageSize;
			}
			

		},
      computed:{
         editting(){
            if(this.selected) return true;
            return this.create;
         },
         indexMode(){
            if(this.editting) return false;
            return true;
			}
      }, 
      methods:{
         onIndex(){
            this.fetchData();

            this.selected=0;
            this.create=false;
			},
         onCreate(){
            this.create=true;
         },
         onEdit(id){
            this.selected=id;
         },
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
         fetchData() {
				
            let getData = TermAdmin.index(this.params);

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






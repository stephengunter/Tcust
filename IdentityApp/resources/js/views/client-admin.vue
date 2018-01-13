<template>
   <div>
      <div v-if="model" v-show="indexMode">
         <div class="row">
            <div class="col-sm-3" >
					<h2>Clients</h2>
            </div>
            <div class="col-sm-3">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <searcher @search="onSearch">
					</searcher>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               
            </div>
         </div>

         <hr/>

          <client-table  :model="model"  @edit="onEdit" @remove="onDelete" >
          
			  <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </client-table>

      </div>
      <client-edit v-if="editting"  :id="selected" 
         @saved="onIndex" @cancel="onIndex">
      </client-edit>
      
      <!-- <delete-confirm :showing="deleteConfirm.showing" :message="deleteConfirm.message"
        @close="deleteConfirm.showing=false" @confirmed="deleteUser">
      </delete-confirm> -->
   </div> 
</template>


<script>
   import Searcher from '../components/searcher';
   import ClientTable from '../components/client-table';
   import ClientEdit from '../components/client-edit';
   export default {
      name:'ClientAdminView',
      components: {
         Searcher,
         'client-table':ClientTable,
         'client-edit':ClientEdit
      },
      props: {
         init_model: {
            type: Object,
            default: null
			},
			categories:{
				type:Array,
				default:null
			}
      },
      data(){
         return {
				model:null,
				
            selected:0,
				create:false,
				
				params:{
					category:0,
					keyword:'',
					page:1,
					pageSize:10
				},
				
				category:null,

            deleteConfirm:{
               id:0,
               showing:false,
               message:''
            }
         }
      },
      beforeMount() {
			
         if(this.init_model){
				this.model={...this.init_model };
				this.params.page=this.init_model.pageNumber;
				this.params.pageSize=this.init_model.pageSize;
			}
			
			// if(this.categories){
			// 	this.setCategory(this.categories[0]);
				
			// }	

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
         onDetails(id){
            alert(id);
         },
         onEdit(id){
            this.selected=id;
         },
         onDelete(user){
            this.deleteConfirm.id=user.id;
            this.deleteConfirm.message='確定要刪除 ' + user.title + ' 嗎?';
            this.deleteConfirm.showing=true;
         },
         deleteUser(){
				let remove=UserAdmin.remove(this.deleteConfirm.id);
				
				remove.then(() => {
					this.fetchData();
               Helper.BusEmitOK('刪除成功');

            })
            .catch(error => {
               Helper.BusEmitError(error);
				})
				
				this.deleteConfirm.showing=false;
			},
			onCategorySelected(category){
				this.setCategory(category);
				this.fetchData();
			},
			setCategory(category){
				this.category=category;
				this.params.category=category.value;
			},
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
			onSearch(keyword){
				this.params.keyword=keyword;
				this.fetchData();
			},
         fetchData() {
				
            let getData = ClientAdmin.index(this.params);

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






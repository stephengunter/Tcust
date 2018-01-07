<template>
   <div>
      <div v-if="model" v-show="indexMode">
         <div class="row">
            <div class="col-sm-3" style="margin-top: 20px;">
					<drop-down :items="categories" :selected="category.value"
					  @selected="onCategorySelected">
					</drop-down>
            </div>
            <div class="col-sm-3">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <searcher @search="onSearch">
					</searcher>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               <a @click.prevent="onCreate" href="#" class="btn btn-primary pull-right title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增文章
               </a>
            </div>
         </div>

         <hr/>

         <post-table :model="model"  @edit="onEdit" @remove="onDelete" >
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </post-table>

      </div>
      <post-edit v-if="editting"  :id="selected" :category="category"  :categories="categories"
         @saved="onIndex" @cancel="onIndex">
      </post-edit>
      
      <delete-confirm :showing="deleteConfirm.showing" :message="deleteConfirm.message"
        @close="deleteConfirm.showing=false" @confirmed="deletePost">
      </delete-confirm>
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
			
			if(this.categories){
				this.setCategory(this.categories[0]);
				
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
         onDetails(id){
            alert(id);
         },
         onEdit(id){
            this.selected=id;
         },
         onDelete(post){
            this.deleteConfirm.id=post.id;
            this.deleteConfirm.message='確定要刪除 ' + post.title + ' 嗎?';
            this.deleteConfirm.showing=true;
         },
         deletePost(){
				let remove=PostAdmin.remove(this.deleteConfirm.id);
				
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
				
            let getData = PostAdmin.index(this.params);

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






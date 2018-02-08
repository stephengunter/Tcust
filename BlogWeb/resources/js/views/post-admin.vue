<template>
   <div>
      <div v-if="model" v-show="indexMode">
         <div class="row">
				<div class="col-sm-7 form-inline" style="margin-top: 20px;">
					<div class="form-group">
						<drop-down :items="categories" :selected="category.value"
							@selected="onCategorySelected">
						</drop-down>
					</div>
					<div class="form-group" style="padding-left:1cm;">
						<toggle :items="reviewedOptions"   :default_val="params.reviewed" @selected="setReviewed"></toggle>
					</div>
					<year-term-filter @changed="onYearTermChanged"></year-term-filter>
					
				</div>

            <div class="col-sm-3" style="margin-top: 20px;">
               <searcher @search="onSearch">
					</searcher>
            </div>
            <div class="col-sm-2" style="margin-top: 20px;">

               <a @click.prevent="onCreate" href="#" class="btn btn-primary pull-right title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增文章
               </a>
            </div>
         </div>

         <hr/>

         <post-table :model="model" :can_delete="can_delete" :desc="desc" :sortby="params.sortby" 
				@edit="onEdit" @remove="onDelete" @sort="onSort">
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </post-table>

      </div>
      <post-edit v-if="editting"  :id="selected" :category="category"  
         @saved="onIndex" @cancel="onIndex">
      </post-edit>
      
      <delete-confirm :showing="deleteConfirm.showing" :message="deleteConfirm.message"
        @close="deleteConfirm.showing=false" @confirmed="deletePost">
      </delete-confirm>
   </div> 
</template>


<script>
	import YearTermFilter from '../components/year-term-filter';
   import Searcher from '../components/searcher';
   import PostTable from '../components/post-table';
   import PostEdit from '../components/post-edit';
   export default {
      name:'PostAdminView',
      components: {
			'year-term-filter':YearTermFilter,
         'searcher':Searcher,
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
					terms:'',
					category:0,
					reviewed:true,
					sort:'desc',
					sortby:'',
					keyword:'',
					page:1,
					pageSize:10
				},

				reviewedOptions:Post.reviewedOptions(),
				
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
			},
			desc(){
            return this.params.sort=='desc';
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
			
			setReviewed(val) {
        	   this.params.reviewed = val;
				this.fetchData();
         },
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
			onSearch(keyword){
				this.params.keyword=keyword;
				this.fetchData();
			},
			onYearTermChanged(terms){
				
				this.params.terms=terms;
				this.fetchData();
			},
			onSort(key){
				
				if(key=='date'){
					this.params.sort='desc';
					this.params.sortby=key;
				}else{
					if(this.params.sortby!=key){

						this.params.sortby=key;
						
					}else{
						if(this.desc) this.params.sort='asc';
            		else  this.params.sort='desc';
					}
					
				}
				

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






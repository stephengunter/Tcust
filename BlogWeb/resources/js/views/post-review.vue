<template>
   <div>
      <div v-if="model" v-show="indexMode">
         <div class="row">
            <div class="col-sm-3">
               <h2>文章審核</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
					
            </div>
            
            <div class="col-sm-3" style="margin-top: 20px;">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               <button class="btn btn-success" @click.prevent="submit" :disabled="selectedIds.length<1">
						<i class="fa fa-check" aria-hidden="true"></i>
						審核通過
					</button>
            </div>
         </div>

         <hr/>

         <post-table :type="type" ref="postTable" :model="model" :can_check="true"
						@edit="onEdit"	 @remove="onDelete" @check-changed="onCheckChanged">
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </post-table>

      </div>
      <post-edit v-if="editting"  :id="selected"  
         @saved="onIndex" @cancel="onIndex">
      </post-edit>
      
      <delete-confirm :showing="deleteConfirm.showing" :message="deleteConfirm.message"
        @close="deleteConfirm.showing=false" @confirmed="deletePost">
      </delete-confirm>
   </div> 
</template>


<script>
	import PostTable from '../components/post-table';
	import PostEdit from '../components/post-edit';
   export default {
      name:'ReviewAdminView',
      components: {
			'post-table':PostTable,
			'post-edit':PostEdit
      },
      props: {
         init_model: {
            type: Object,
            default: null
			}
      },
      data(){
         return {
				type:'review',
				model:null,
				selected:0,
				params:{
					page:1,
					pageSize:10
				},

				selectedIds:[],

				deleteConfirm:{
               id:0,
               showing:false,
               message:''
            }
         }
      },
		computed:{
         editting(){
            return this.selected;
         },
         indexMode(){
				return !this.editting;
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
			onIndex(){
            this.fetchData();

            this.selected=0;
            
         },
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
         fetchData() {
				
            let getData = PostReview.index(this.params);

            getData.then(model => {

					this.model={ ...model };
					
					this.$refs.postTable.unCheckAll();

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
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
			onCheckChanged(checked_ids){
				this.selectedIds=checked_ids.slice(0);
			},
			submit(){
				let save = PostReview.store(this.selectedIds)
				save.then(() => {
						Helper.BusEmitOK();
						this.onIndex();
					}).catch(error => {
						Helper.BusEmitError(error);           
					})
			}
         
      }
   }
</script>






<template>
   <div>
      <div v-if="model">
         <div class="row">
            <div class="col-sm-3">
               <h2>置頂文章</h2>
            </div>
				<div class="col-sm-4 form-inline" style="margin-top: 20px;">
					<div class="form-group">
						<drop-down :items="categories" :selected="category.value"
							@selected="onCategorySelected">
						</drop-down>
					</div>
					
					
				</div>

               
            <div class="col-sm-5" style="margin-top: 20px;">
					<h5>說明：重要性越大排序越前面，設為負數可取消置頂。</h5>
            </div>
         </div>

         <hr/>

         <post-table :type="type" :top="true" :can_edit="false" :model="model"  @submit-orders="onSubmit">
          
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
      name:'TopPostAdminView',
      components: {
         'post-table':PostTable,
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
      },
      data(){
         return {
				type:'tops',
				model:null,
				
				params:{
					category:0,
					page:1,
					pageSize:10
				},
				
				category:null,
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
      methods:{
			fetchData() {
				
            let getData = TopAdmin.index(this.params);

            getData.then(model => {

               this.model={ ...model };

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
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
			getPosts(){
				if(this.model) return this.model.viewList;
				return [];
			},
			getTopPostEditForms(){
				return new Promise((resolve, reject) => {
					let posts=this.getPosts();
					let models=[];
					posts.forEach((post) => {
						if(isNaN(post.order)){

						}else{
							models.push({ id:post.id, order:post.order});
						} 
						
						
					});
					resolve(models);

				})
			},
			onSubmit(){
				let getPostForms=this.getTopPostEditForms();
				
				getPostForms.then((posts)=>{
					
					if(!posts.length) return;

					let save= TopAdmin.store(posts);
					
					save.then(() => {
						Helper.BusEmitOK('資料已存檔');	
						this.fetchData();
					
					})
					.catch(error => {
						Helper.BusEmitError(error,'存檔失敗');
					})


				});
				

				
			}
         
         
      }
   }
</script>






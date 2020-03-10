<template>
   <div>
      <div  v-show="indexMode">
         <div class="row">
            <div class="col-sm-3" >
					<h2>使用者管理</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <drop-down :items="permissions" :selected="permission.value"
					  @selected="onPermissionSelected">
					  <label slot="label" style="font-size:1.2em">  權限：
      			  </label> 
					 
					</drop-down>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <searcher @search="onSearch">
					</searcher>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               <a @click.prevent="onCreate" href="#" class="btn btn-primary pull-right title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增使用者
               </a>
            </div>
         </div>

         <hr/>

          <user-table :model="model" @edit="onEdit" @remove="onDelete" >
          
			  <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </user-table>

      </div>
      <user-edit v-if="editting"  :id="selected" 
         @saved="onIndex" @cancel="onIndex">
      </user-edit>
      
      <delete-confirm :showing="deleteConfirm.showing" :message="deleteConfirm.message"
        @close="deleteConfirm.showing=false" @confirmed="deleteUser">
      </delete-confirm>
   </div> 
</template>


<script>
   import Searcher from '../components/searcher';
   import UserTable from '../components/user-table';
   import UserEdit from '../components/user-edit';
   export default {
      name:'ManageView',
      components: {
         Searcher,
         'user-table':UserTable,
         'user-edit':UserEdit
      },
      props: {
         init_model: {
            type: Object,
            default: null
			},
			permissions:{
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
					permission:0,
					keyword:'',
					page:1,
					pageSize:10
				},
				
				permission:null,

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
			
			if(this.permissions){
				this.setPermission(this.permissions[0]);
				
			}
			
			console.log('Api source',Api.source());

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
            this.deleteConfirm.message='確定要刪除 ' + user.name + ' 嗎?';
            this.deleteConfirm.showing=true;
         },
         deleteUser(){
				
				let remove=Manage.remove(this.deleteConfirm.id);
				
				remove.then(() => {
					this.fetchData();
               Helper.BusEmitOK('刪除成功');

            })
            .catch(error => {
               Helper.BusEmitError(error);
				})
				
				this.deleteConfirm.showing=false;
			},
			onPermissionSelected(permission){
				this.setPermission(permission);
				this.fetchData();
			},
			setPermission(permission){
				this.permission=permission;
				this.params.permission=permission.value;
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
				
            let getData = Manage.index(this.params);

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






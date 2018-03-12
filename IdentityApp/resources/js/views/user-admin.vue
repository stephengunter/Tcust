<template>
   <div>
		<div v-if="model" v-show="indexMode">
			<div class="row">
				<div class="col-sm-3" >
						<h2>使用者管理</h2>
				</div>
				<div class="col-sm-3" style="margin-top: 20px;">
				<drop-down :items="roles" :selected="role.value"
							@selected="onRoleSelected">
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

			<user-table  :model="model"  @edit="onEdit" @remove="onDelete" >
			
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
			@close="deleteConfirm.showing=false" @confirmed="deleteuser">
		</delete-confirm>
	</div> 
</template>


<script>
   import Searcher from '../components/searcher';
   import UserTable from '../components/user-table';
   import UserEdit from '../components/user-edit';
   export default {
      name:'userAdminView',
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
			roles:{
				type:Array,
				default:null
			},
      },
      data(){
         return {
				model:null,
				
            	selected:'',
				create:false,

				role:null,
				
				params:{
					role:'',
					keyword:'',
					page:1,
					pageSize:10
				},

            deleteConfirm:{
               id:'',
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

			if(this.roles){
				this.setRole(this.roles[0]);
				
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

            this.selected='';
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
            this.deleteConfirm.message='確定要刪除 ' + user.userId + ' 嗎?';
            this.deleteConfirm.showing=true;
         },
         deleteuser(){
				let remove=userAdmin.remove(this.deleteConfirm.id);
				
				remove.then(() => {
					this.fetchData();
               Helper.BusEmitOK('刪除成功');

            })
            .catch(error => {
               Helper.BusEmitError(error);
				})
				
				this.deleteConfirm.showing=false;
			},
			onRoleSelected(role){
				this.setRole(role);
				this.fetchData();
			},
			setRole(role){
				this.role=role;
				this.params.role=role.value;
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
				
            let getData = UserAdmin.index(this.params);

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






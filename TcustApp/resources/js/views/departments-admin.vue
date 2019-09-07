<template>
   <div>
      <div v-if="list" v-show="indexMode">
         <div class="row">
            <div class="col-sm-3">
               <h2>部門管理</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
					
            </div>
            
            <div class="col-sm-3" style="margin-top: 20px;">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
               <a @click.prevent="onCreate" href="#" class="btn btn-primary pull-right title-controll">
                  <i class="fa fa-plus" aria-hidden="true"></i>
                  新增部門
               </a>
            </div>
         </div>

         <hr/>
         <department-table :list="list"  @edit="onEdit" >
         </department-table>
         

      </div>
      
      <department-edit v-if="editting"  :id="selected"   
         @saved="onIndex" @cancel="onIndex">
      </department-edit>
      
   </div>
</template>


<script>
   import DepartmentTable from '../components/department-table';
   import DepartmentEdit from '../components/department-edit';

   export default {
      name:'DepartmentsAdminView',
      components: {
         'department-table': DepartmentTable,
         'department-edit': DepartmentEdit
      },
      props: {
         init_model: {
            type: Array,
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

				list:[],
				
            selected: 0,
				create: false,
         }
      },
      beforeMount() {
         if(this.init_model){
				this.list = this.init_model.slice(0);
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

            this.selected = 0;
            this.create = false;
         },
         onSelected(){

         },
         onCreate(){
            this.create = true;
         },
         onEdit(id){
            this.selected=id;
         },
         fetchData() {
				
            let getData = DepartmentAdmin.index();

            getData.then(list => {

               this.list = list;

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
         },
         
      }
   }
</script>






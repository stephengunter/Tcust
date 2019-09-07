<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                  <th>名稱</th>
                  <th>代碼</th>
                  <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(item,index) in list" :key="index">
                  <td> {{ item.name }}</td>
                  <td> {{ item.code }}</td>
                  <td v-if="can_edit">
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(item.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button v-if="can_delete" class="btn btn-sm btn-danger" @click.prevent="remove(item)">
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                     </button>
                  </td>
               </tr>
            </tbody>

         </table>
      </div>
      <slot name="table-footer"> 
     
      </slot> 
          
   </div>
</template>

<script>
export default {
   name:'DepartmentTable',
   props: {
      list: {
         type: Array,
         default: null
      },
      can_edit:{
         type: Boolean,
         default: true
		},
		can_delete:{
			type:Boolean,
			default:false
		},
	},
	data() {
		return {
			
		};
	},
	computed:{
		
   }, 
	watch: {
		
	},
   methods:{
      edit(id){
         this.$emit('edit',id);
      }
		
   }
}
</script>


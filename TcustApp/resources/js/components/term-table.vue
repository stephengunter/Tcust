<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                  <th style="width:10%">狀態</th>
                  <th>編號</th>
                  <th style="width:20%">學年度</th>
                  <th style="width:20%">名稱</th>
                  <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(term,index) in model.viewList" :key="index">
						
						<td>
                     <span v-if="term.active" class="label label-success">進行中</span>
                  </td>
                  <td> {{ term.number }}</td>
                   <td>{{ term.termYear.year }}</td>
                  <td>{{ term.title }}</td>
                  <td v-if="can_edit">
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(term.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button v-if="can_delete" class="btn btn-sm btn-danger" @click.prevent="remove(term)">
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
   name:'TermTable',
   props: {
      model: {
         type: Object,
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
      },
		getTermList(){
			if(this.model) return this.model.viewList;
			return null;
		},
		
   }
}
</script>


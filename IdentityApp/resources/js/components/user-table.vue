<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                    <th style="width:15%">UserName</th>
                    <th style="width:15%">姓名</th>
                    <th style="width:15%">Email</th>
                    <th>角色</th>
                    <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(user,index) in model.viewList" :key="index">
                  
                  <td>{{ user.name }}</td>
                  <td>{{ user.profile.fullname }}</td>
                   <td>{{ user.email }}</td>
                  <td>{{ user.roles }}</td>
                  <td>
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(user.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <!-- <button class="btn btn-sm btn-danger" @click.prevent="remove(user)">
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                     </button>     -->

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
   name:'UserTable',
   props: {
      model: {
         type: Object,
         default: null
      },
   },
   methods:{
      edit(id){
         this.$emit('edit',id);
      },
      remove(user){
         this.$emit('remove',user);
      }
   }
}
</script>


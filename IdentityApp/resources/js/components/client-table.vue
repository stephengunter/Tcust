<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                  <th style="width:15%">名稱</th>
                  <th style="width:25%">ClientId</th>
                  <th style="width:25%">Uri</th>
                  <th style="width:10%">日期</th>
                  <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(client,index) in model.viewList" :key="index">
                  
                  <td>{{ client.title }}</td>
                  <td>{{ client.clientId }}</td>
                  <td>{{ client.uri }}</td>
                  <td></td>
                  <td>
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(client.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button class="btn btn-sm btn-danger" @click.prevent="remove(client)">
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
   name:'ClientTable',
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
         this.$emit('remove',client);
      }
   }
}
</script>


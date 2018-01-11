<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                  <th style="width:10%">#</th>
                  <th style="width:10%">編號</th>
                  <th style="width:25%">標題</th>
                  <th style="width:25%">作者</th>
                  <th style="width:10%">日期</th>
                  <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(user,index) in model.viewList" :key="index">
                  <td>
                    {{  index+1 }}
                  </td>
                  <td>{{ user.number }}</td>
                  <td>{{ user.title }}</td>
                  <td>{{ user.author }}</td>
                  <td>{{ user.date }}</td>
                  <td>
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(user.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button class="btn btn-sm btn-danger" @click.prevent="remove(user)">
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


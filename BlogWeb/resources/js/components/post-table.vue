<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
                  <th style="width:10%">&nbsp;</th>
                  <th style="width:10%">編號</th>
                  <th style="width:25%">標題</th>
                  <th style="width:25%">作者</th>
                  <th style="width:10%">日期</th>
                  <th style="width:10%"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(post,index) in model.viewList" :key="index">
                  <td>
                     <img v-if="post.cover" class="thumbnail" style="max-width:60px" :src="post.cover.path" />
                     
                  </td>
                  <td>{{ post.number }}</td>
                  <td>{{ post.title }}</td>
                  <td>{{ post.author }}</td>
                  <td>{{ post.date }}</td>
                  <td>
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(post.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button class="btn btn-sm btn-danger" @click.prevent="remove(post)">
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
   name:'PostTable',
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
      remove(post){
         this.$emit('remove',post);
      }
   }
}
</script>


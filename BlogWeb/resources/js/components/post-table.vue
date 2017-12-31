<template>
   <div class="panel panel-default">
      <table class="table table-striped">
         <thead>
            <tr>
               <th style="width:25%">&nbsp;</th>
               <th style="width:25%">標題</th>
               <th>作者</th>
               <th>最後更新</th>
            </tr>
         </thead>
         <tbody>
            <tr v-for="(post,index) in posts" :key="index">
               <td></td>
               <td>{{ post.title }}</td>
               <td>{{ post.author }}</td>
               <td>{{ post.summary }}</td>
            </tr>
         </tbody>

      </table>
   </div>
</template>

<script>
export default {
   name:'PostTable',
   props: {
      posts: {
         type: Array,
         default: null
      },
   },
}
</script>


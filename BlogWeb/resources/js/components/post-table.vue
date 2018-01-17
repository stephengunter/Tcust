<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
						<th style="width:5%">
							<check-box :value="0" :default="checkAll"
							 @selected="onCheckAll" @unselected="unCheckAll">
							 </check-box>
						</th>
                  <th style="width:8%">&nbsp;</th>
                  <th style="width:8%" v-if="!can_edit">
                     <a href="#" @click.prevent="onSort">
                     點擊數
                       <i v-if="desc" class="fa fa-sort-desc" aria-hidden="true"></i>
                       <i v-else class="fa fa-sort-asc" aria-hidden="true"></i>
                     </a>
                  </th>
						<th style="width:10%">分類</th>
                  <th style="width:10%">編號</th>
                  <th style="width:20%">標題</th>
                  <th style="width:20%">作者</th>
                  <th style="width:8%">日期</th>
                  <th style="width:8%" v-if="can_edit"></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(post,index) in model.viewList" :key="index">
						<td>
							<check-box :value="post.id" :default="beenChecked(post.id)"
								@selected="onChecked" @unselected="unChecked">
							</check-box>
						</td>
                  <td>
                     <img v-if="post.cover" class="thumbnail" style="max-width:60px" :src="post.cover.previewPath" />
                     
                  </td>
                  <td v-if="!can_edit">
                        {{ post.clickCount }} 
                  </td>
						<td>{{ post.categoryName }}</td>
                  <td>{{ post.number }}</td>
                  <td>{{ post.title }}</td>
                  <td>{{ post.author }}</td>
                  <td>{{ post.date }}</td>
                  <td v-if="can_edit">
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
      can_edit:{
         type: Boolean,
         default: true
      },
      desc:{
         type: Boolean,
         default: true
      }
	},
	data() {
		return {
			checked_ids:[],
			checkAll: false
		};
	},
	watch: {
		checked_ids() {
			this.$emit('check-changed',this.checked_ids);
		}
	},
   methods:{
      edit(id){
         this.$emit('edit',id);
      },
      remove(post){
         this.$emit('remove',post);
      },
      onSort(){
         this.$emit('sort');
		},
		getPostList(){
			if(this.model) return this.model.viewList;
			return null;
		},
		beenChecked(id){
         return this.checked_ids.includes(id);
		},
		onChecked(id){
				
			if(!this.beenChecked(id))  this.checked_ids.push(id);
		},
		unChecked(id){
				
			let index= this.checked_ids.indexOf(id);
			if(index >= 0)  this.checked_ids.splice(index, 1); 
				
		},
		onCheckAll(){
			let postList = this.getPostList();
			if(!postList)  return false;

			postList.forEach( post => {
				this.onChecked(post.id)
			});
		},
		unCheckAll(){
			this.checked_ids=[];
		},
   }
}
</script>


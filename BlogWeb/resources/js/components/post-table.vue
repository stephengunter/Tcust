<template>
   <div class="panel panel-default">
      <div class="panel-body">
         <table class="table table-striped">
            <thead>
               <tr>
						<th v-if="can_check" style="width:5%">
							<check-box :value="0" :default="checkAll"
							 @selected="onCheckAll" @unselected="unCheckAll">
							 </check-box>
						</th>
                  <th style="width:8%">&nbsp;</th>
                  <th style="width:8%" v-if="clicks">
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
                  <th style="width:10%">日期</th>
                  <th style="width:8%" v-if="can_edit"></th>
						<th style="width:8%" v-if="top">
							重要性 &nbsp;
							
							<button v-show="canSubmitOrders" @click.prevent="submitOrders" class="btn btn-xs btn-success">
								<i class="fa fa-floppy-o" aria-hidden="true"></i>
								
							</button>
						</th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(post,index) in model.viewList" :key="index">
						<td v-if="can_check">
							<check-box :value="post.id" :default="beenChecked(post.id)"
								@selected="onChecked" @unselected="unChecked">
							</check-box>
						</td>
                  <td>
							
                     <img v-if="post.cover" class="thumbnail" style="max-width:60px" :src="post.cover.previewPath" />
                     
                  </td>
                  <td v-if="clicks">
                     {{ post.clickCount }} 
                  </td>
						<td>{{ post.categoryName }}</td>
                  <td>{{ post.number }}</td>
                  <td>
							<span v-if="post.top" style="color:#f4d742">
								<i class="fa fa-star" aria-hidden="true"></i>
							</span>
							{{ post.title }}
						</td>
                  <td>{{ post.author }}</td>
                  <td>{{ post.date }}</td>
                  <td v-if="can_edit">
                     <button class="btn btn-sm btn-primary" @click.prevent="edit(post.id)">
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button v-if="can_delete" class="btn btn-sm btn-danger" @click.prevent="remove(post)">
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                     </button>    

                  </td>
						<td v-if="top">
                     <input class="form-control" type="text" v-model="post.order">	  

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
		can_delete:{
			type:Boolean,
			default:false
		},
		can_check:{
			type:Boolean,
			default:false
		},
		clicks:{
         type: Boolean,
         default: false
      },
		top:{
         type: Boolean,
         default: false
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
	computed:{
		canSubmitOrders(){
			let posts=this.getPostList();
			if(!posts) return false;
			return posts.length > 0;
		}
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
			this.checkAll=true;
			
			let postList = this.getPostList();
			if(!postList)  return false;

			postList.forEach( post => {
				this.onChecked(post.id)
			});
		},
		unCheckAll(){
			this.checkAll=false;
			this.checked_ids=[];
		},
		submitOrders(){
			
			this.$emit('submit-orders');
		}
   }
}
</script>


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
							<a href="#" @click.prevent="onSort('clicks')">
								點擊數
								<span v-show="sortByClicks">
									<i v-if="desc" class="fa fa-sort-desc" aria-hidden="true"></i>
									<i v-else class="fa fa-sort-asc" aria-hidden="true"></i>
								</span>
							</a>
									
						</th>
						<th style="width:10%">分類</th>
						<th style="width:10%">
									
							<a v-if="isDefaultMode" href="#" @click.prevent="onSort('number')">
								編號
								<span v-show="sortByNumber">
									<i v-if="desc" class="fa fa-sort-desc" aria-hidden="true"></i>
									<i v-else class="fa fa-sort-asc" aria-hidden="true"></i>
								</span>
							</a>
							<span  v-else >編號</span>
						</th>
						<th style="width:20%">標題</th>
						<th style="width:20%">作者</th>
						<th style="width:10%">
								
							<a  v-if="isDefaultMode" href="#" @click.prevent="onSort('date')">
								日期
								<span v-show="sortby=='date'">
									<i  class="fa fa-sort-desc" aria-hidden="true"></i>
								</span>
							</a>
							<span v-else>日期</span>
						</th>
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
				<tr v-for="(post,index) in posts" :key="index">
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
					<td>{{ post.categoryNames }}</td>
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
		type:{
         type: String,
         default: 'posts'
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
      },
		sortby:{
			type: String,
         default: ''
		},
		
	},
	data() {
		return {
			checked_ids:[],
			checkAll: false,
		};
	},
	computed:{
		posts(){
			if(!this.model) return [];
			return this.model.viewList.map(post => {
				let cover = new Photo(post.cover);
				cover.previewPath = cover.getThumbnailUrlByWidth(60);
				return { ...post, cover:cover };
			})
		},
		canSubmitOrders(){
			return this.posts.length > 0;
		},
		sortByNumber(){
			return this.sortby == 'number';
		},
		sortByClicks(){
			return this.sortby == 'clicks';
		},
		isDefaultMode(){
			return this.type == 'posts';
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
      onSort(key){
         this.$emit('sort',key);
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
			this.checkAll = true;

			this.posts.forEach(post => {
				this.onChecked(post.id)
			});
		},
		unCheckAll(){
			this.checkAll = false;
			this.checked_ids=[];
		},
		submitOrders(){
			this.$emit('submit-orders');
		}
   }
}
</script>


<template>
  

	<div v-if="posts" class="intxt5 scrollFrame" v-infinite-scroll="loadMore" infinite-scroll-disabled="disabled_scroll" infinite-scroll-distance="10">
		<ul class="Search" v-for="(post,index) in posts" :key="index">
			<li class="Seartxt">
				<a href="#" @click.prevent="selected(post.id)">{{ post.title }}</a> 
				<span class="pull-right" > {{ getDate(post) }} </span> 
			</li>
			
		</ul>
	</div>
</template>

<script>
export default {
   name:'DaAiSearchList',
	props: {
      posts: {
         type: Array,
         default: null
		},
		disabled_scroll: {
         type: Boolean,
         default: true
		}
	},
	data() {
		return{
			
		}
		
	},
	mounted(){
		
   },
	methods: {
		getDate(post){
			return Post.getDate(post);
			
		},
		loadMore(){
			if(this.posts.length<10) return;
			
			if(this.busy) return;

			
			this.$emit('next-page');
			
		},
		
		selected(id){
			this.$emit('selected',id);
		}
	}

}
</script>


<style>
   .menu5 {
		margin-top: 20px;
	}


	.Search {
		text-align: left !important;
		padding-left: 50px;
		overflow: hidden;
	}
</style>
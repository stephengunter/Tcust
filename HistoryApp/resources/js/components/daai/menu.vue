<template>
   <div class="intxt4" v-infinite-scroll="loadMore" infinite-scroll-disabled="disabled_scroll" infinite-scroll-distance="10">
      <div v-for="(item,index) in posts" :key="index" :class="{'scroll-el':true, selected:item.id==selected_id}">
         <span class="title_07">
               {{ postDate(item).format('YYYY-MM-DD') }}
               <br>
               {{ item.title }}
         </span>
         <a href="#" @click.prevent="onSelected(item.id)" class="title_07">
            <img class="glow" width="210" height="145" :src="item.cover.previewPath">
         </a>
      </div>
      

   </div>
</template>

<script>
export default {
   name:'DaAiMenu',
   props:{
      posts: {
         type: Array,
         default: null
      },
      disabled_scroll: {
         type: Boolean,
         default: true
		},
		selected_id:{
			type:Number,
         default:0
		}
    
   },
   methods:{
      postDate(post){
			if(!post) return null;
			if(post.date) return moment(post.date,'YYYY-MM-DD');
			return moment(post.createdAt);
      },
      loadMore(){
			if(!this.posts) return;
			if(!this.posts.length) return;
			if(this.busy) return;

			this.$emit('next-page');
		},
		onSelected(id){
			this.$emit('selected',id);
		}
   }

}
</script>

<style>
.selected {
    border-color: #CCCCCC;
    border-style: solid;
    border-width: 2px;
    border-radius: 10px 10px 10px 10px;
    padding: 0px 0px 0px 5px;
}
</style>



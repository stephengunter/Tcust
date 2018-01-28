<template>
   <div>
      <div class="pic">
         <img @click.prevent="onSelected" :src="photoSrc" width="1920">
      </div>
      <div class="picWhite">
         <div class="picL">
            <ul class="picYear">{{ postDate.year() }}</ul>
            <ul class="picMonth">{{ postDate.format('MM.DD') }}</ul>
         </div>
         <div class="picR">
            <ul class="picRtxt">
                  {{ post.title  }}
            </ul>
         </div>
      </div>
   </div>
</template>

<script>
export default {
   name:'HonorTopItem',
   props:{
      post: {
         type: Object,
         default: null
      }
   },
   computed:{
		postDate(){
			if(this.post.date) return moment(this.post.date,'YYYY-MM-DD');
			return moment(this.post.createdAt);
      },
      photoSrc(){
         if(!this.getPhoto()) return '';
         return this.getPhoto().previewPath;
      }
   },
   methods:{
		getPhoto(){
         if(!this.post) return null;
         return this.post.cover;
			
      },
      onSelected(){
         
         this.$emit('selected');
      }
	}
}
</script>


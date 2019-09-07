<template>
   
   <img v-if="previewPath" class="img-thumbnail summary-img" :src="previewPath" />
</template>

<script>
   export default {
      name:'CoverPhoto',
      props: {
         media: {
            type: Object,
            default: null
         },
         width: {
            type: Number,
            default: 180
         }
      },
      data(){
         return {
            previewPath: ''
         }
      },
      watch: {
         media: {
            handler: function () {
               this.mapPhoto();
            },
            deep: true
         },
         version: function () {
            this.fetchData()
         }
         
      },
      beforeMount(){
         this.mapPhoto();         
      },
      methods:{
         mapPhoto(){
            if(this.media){
               let cover = new Photo(this.media);
               this.previewPath = cover.getThumbnailUrlByWidth(this.width);
            }
         },
         onDetails(){
            this.$emit('details' , this.post.id);
         }
      }
   }
</script>

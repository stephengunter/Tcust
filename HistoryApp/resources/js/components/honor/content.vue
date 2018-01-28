<template>
   <div id="DailyDiv" class="WINglow">
      <div>
         <ul class="WINTitle">活動日誌</ul>
         <ul class="WINTitle2">照片紀錄</ul>
         <ul class="WINTitle3">
            <li class="WINclose">
               <a href="#" @click.prevent="cancel"></a>
               
            </li>
         </ul>
      </div>
      <div class="WINin1">
         <ul class="inTxtL">
            <li class="inTxt1">
               {{ post.title  }}
            </li>
            <li class="inTxt2">
              <p>{{ post.author }}</p>
              <p v-html="post.content "></p>
            </li>
            <li>

            </li>
         </ul>
         
      </div>
      <div class="WINin2">
         <ul class="inTxtR">
            <a v-for="(item,index) in post.medias" :key="index" data-fancybox="gallery" :data-caption="item.title" 
              :href="item.path"  >
              <img :alt="item.title" :src="item.previewPath" style="width:210px;padding-top:5px">
            </a>
            
         </ul>
      </div>
   </div>
</template>

<script>
export default {
   name:'HonorContent',
   props:{
      post:{
         type: Object,
         default: null
      }
   },
   methods:{
      cancel(){
         this.$emit('cancel');
      }
   }
}
</script>


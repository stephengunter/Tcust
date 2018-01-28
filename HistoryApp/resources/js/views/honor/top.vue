<template>
   <slick v-if="model" ref="slick" class="carousel" :options="slickOptions" >
      <top-item v-for="(post,index) in getPosts()" :key="index" :post="post"
         @selected="onSelected">

      </top-item>
   </slick>
</template>

<script>
import Slick from 'vue-slick';
import TopItem from '../../components/honor/top-item';

export default {
   name:'HonorTopView',
   components: {
      Slick,
      'top-item':TopItem
   },
   data(){
      return{
         slickOptions: {
            prevArrow: false,
    			nextArrow: false,
				
            autoplay: true,
            autoplaySpeed: 2000,
            dots: false,
            pauseOnHover:false
           
         },

         model:null
      
      }
   },
   beforeMount() {
      this.fetchData();
   },
   methods: {
      fetchData(){
         //let fetchHonors=Api.getHonorList();
         let fetchHonors=Api.getDiaryList();

         fetchHonors.then(model => {
            this.model = { ...model };
         })
         .catch(error => {
				Helper.BusEmitError(error);
         })
      },
      getPosts(){
			if(this.model) return this.model.viewList;
			return null;
      },
      onSelected(){
         this.$emit('selected');
      }
   }
}
</script>


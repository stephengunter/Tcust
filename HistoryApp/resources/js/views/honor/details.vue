<template>
   <div>
      <div id="UpDiv">
         <ul class="arrowU">
            <a href="#" @click.prevent="changeIndex(false)"></a>
         </ul>
      </div>
      <div id="DownDiv">
         <ul class="arrowD" @click.prevent="changeIndex(true)">
            <a href="#"></a>
         </ul>
      </div>
      <div class="wbox">
         <div id="SliderDiv3">
            <div class="DailyDiv" style="overflow:hidden;">
               <div :class="{ DailyTitle : true , conversionYear: year==3000  }">
                 {{ title }} 
               </div>
              
               <div  class="verticalPic" id="container" > 
						<honor-slick v-for="(post,index) in posts" :key="index" :post="post" 
						  :active="post.id==selected" @selected="onSelected">
						</honor-slick>		
              
               </div>
            </div>
         </div>
         <honor-content v-if="post" :post="post" @cancel="onCancel"></honor-content>
      </div>         
   </div>
</template>

<script>
import Slick from 'vue-slick';
import HonorSlick from '../../components/honor/slick';
import HonorContent from '../../components/honor/content';
export default {
   name:'HonorDetailsView',
   components: {
      Slick,
		'honor-slick':HonorSlick,
		'honor-content':HonorContent
   },
   props: {
      year: {
         type: Number,
         default: 0
      },
      id:{
         type: Number,
         default: 0
		},
		posts:{
			type: Array,
         default: null
		}
   },
   data(){
      return {
			post:null,
			selected:0,
         honorsModels:[],
      }
   },
   beforeMount() {
		this.selected=this.id;
		this.fetchPost();
		
   },
   mounted(){
		this.onReady();
		this.setSlideIndex();
   },
   computed:{
		title(){
			if(this.year==3000) return '傑出校友';
			return	 this.year;
		},
      lockWidth(){
         return Photo.lockWidth(this.getPhoto());
         
      },
   },
   methods:{
		getSlick(){
			return $('.verticalPic');
		},
		onReady(){
			this.getSlick().slick({
				dots: false,
				slidesToScroll: 2,
				arrows: false,
				centerMode: false,
				dots: false,
				vertical: true,
				slidesToShow: 2,
				infinite: true,
				draggable: true,
				onAfterChange:this.onSlickIndexChanged,
				swipe: true,
				swipeToSlide: true,
				touchMove: true,
				variableWidth: true
			});
		},
		fetchPost(){
			let getData=Api.postDetails(this.selected);

         getData.then(post => {
				this.post = { ...post }; 
				
         })
         .catch(error => {
				Helper.BusEmitError(error);
         })
		},
		changeIndex(plus){
			let slick=this.getSlick();
			let currentIndex=slick.slickCurrentSlide();

			if(plus) slick.slickGoTo(currentIndex+1);
			else slick.slickGoTo(currentIndex-1);
			
		},
		onSlickIndexChanged(slider,index){
			let post=this.posts[index];
			if(post.id==this.selected) return;

			this.onSelected(post.id);
		},
		onSelected(id){
			this.selected=id;

			this.fetchPost();
			this.setSlideIndex();
		},
		setSlideIndex(){
			let index=this.posts.findIndex(post=>{
				 return post.id==this.selected;
			});
			let slick=this.getSlick();
			let currentIndex=slick.slickCurrentSlide();
			if(currentIndex!=index) slick.slickGoTo(index);
		},
		onCancel(){
			
			this.$emit('cancel');
		}
	}
}
</script>


<style scoped>

#keyDiv1 {
	position: absolute;
	width: 308px;
	height: 52px;
	z-index: 99;
	left: 1565px;
	top: 25px;
}

#SliderDiv3 {
	position: absolute;
	width: 505px;
	height: 1080px;
	z-index: 40;
	left: 0px;
	visibility: visible;
}

#UpDiv {
	position: absolute;
	width: 70px;
	height: 36px;
	z-index: 51;
	left: 168px;
	top: 101px;
}

#DownDiv {
	position: absolute;
	width: 423px;
	height: 56px;
	z-index: 51;
	left: 0px;
	bottom: 0px;
	background-image: url(/images/HonourList/Downdivbk.png);
}

#DailyDiv {
	position: absolute;
	width: 1450px;
	height: 925px;
	z-index: 30;
	left: 318px;
	top: 109px;
}

.yearUp1 {
	width: 365px !important;
}

video {
	width: 1210px;
	-webkit-background-size: cover;
	-moz-background-size: cover;
	-o-background-size: cover;
	background-size: cover;
}

h1, h2, h3, h4, h5, h6, .h1, .h2, .h3, .h4, .h5, .h6 {
	font-family: "華康黑體","微軟正黑體";
}

.conversionYear {
	font-size: 65px;
	margin: -30px 0px 30px 0px;
	padding: 50px 0px 0px 39px;
}

.inTxt2 {
	font-size: 22px;
	line-height: 35px;
}

</style>



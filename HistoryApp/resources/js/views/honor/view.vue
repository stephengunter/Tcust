<template>
   <div>
      <search-form @search="onSearch"></search-form>
      <div class="wbox">
         
			
         <div class="swiper-container">
            <div class="swiper-wrapper empty">
               <empty />
               <year-item  title="傑出校友" :items="famers" key="3000"
               	@selected="onSelected">
               </year-item>

               

               <year-item v-for="(model,index) in honorsModels" :key="index" 
                   :title="String(model.year)" :items="model.posts"
                   @selected="onSelected" >

               </year-item>
               
               <empty />
               <empty />
            </div>
         </div>

         <div id="flowerDiv1">
            <canvas id="canvas" width="645" height="700"></canvas>
         </div>   
      </div>
   </div>
</template>

<script>
import SearchForm from '../../components/honor/search-form';
import Empty from '../../components/honor/empty';
import YearItem from '../../components/honor/year-item';

export default {
   name: 'HonorView',
   components: {
      'search-form': SearchForm,
		'year-item' : YearItem,
      Empty,
   },
   data() {
      return {
			busy:false,

         params: {
            terms: '',
            keyword: '',
            page: 1,
            pageSize: 10
         },

         honorsModels:[],

         famerModel:null,

         model: null,

         searchMode:false,


         selectedYear:3000,
         
         swiper:null,

			
      }
   },
   computed: {
      famers(){
         if(this.famerModel) return this.famerModel.viewList;
         return [];
      }
   },
   beforeMount() {
      this.fetchData();
   },
   mounted() {
    
   },
   methods: {
		onSearch(keyword){
         this.$emit('search',keyword);
			
		},
      fetchData(){
        
         let fetchFamers=Api.getFamerList();
         let fetchHonors=Api.getHonorList();
         Promise.all([fetchFamers, fetchHonors]).then(values => { 
            this.famerModel=  { ...values[0] }; 
            this.honorsModels= values[1].slice();

            this.onReady();
         });
      },
      onReady(){
         this.$nextTick(() => {
            this.init();
         })
      },
      init(){
        
			$(".swiper-container").css({
				"width": 2270,
				"height": 1080,
				"left": -800,
			});

			$("div[class*='yearDiv']").css("margin-top", "-20px")

			this.opacityInit(1, $(".swiper-slide").length);

			this.swiper = new Swiper('.swiper-container', {
				slidesPerView: 'auto',
				touchRatio: 3.0,
				initialSlide: 0,
				keyboardControl: true,
				mousewheelControl: true,
				watchActiveIndex: true,
				
				tdFlow: {
					rotate: 0,
					stretch: 200,
					depth: 300,
					modifier: 1,
					shadows: false
				},
				onTouchEnd: this.onTouchEnd
			});

			var marginTop = 0;
			for (var i = 1; i < $(".swiper-slide").length; i++) {
				var marginTopStr = marginTop + "px";
				$(".swiper-slide").not(".empty").eq(i).css("margin-top", marginTopStr);
				marginTop = marginTop + 8;
			}
      },
		onSelected(year,id){
			
			if(isNaN(year)){
            this.$emit('selected',3000, id );
            this.moveToIndex(1);

            this.selectedYear=3000
			}else{
            
            this.$emit('selected',parseInt(year), id );
            let index=this.honorsModels.findIndex(model=>{
				   return model.year==year;
            });

           

            this.moveToIndex(index+2);
            
            this.selectedYear=year;
           
            
         }
			
		},
		getPostsByYear(year){
			return new Promise((resolve, reject) => {
				
				if(year==3000){
					resolve(this.famerModel.viewList);
				}else{
					let model=this.honorsModels.find(item=>{
						return item.year==year
					});
					resolve(model.posts);
				}
				

			})
			
      },
      onTouchEnd(){
         var length = $(".swiper-slide").length;
         var currrntSlide = this.swiper.activeIndex + 1;
         this.opacityInit(currrntSlide, length);
      },
      moveToIndex(index) {
        
         this.swiper.swipeTo(index - 1, 500);
         var length = $(".swiper-slide").length;
         var currrntSlide = this.swiper.activeIndex + 1;
         this.opacityInit(currrntSlide, length);
      },
      opacityInit(currrntSlide, length){
         var brightness = 100;
         for (var i = currrntSlide ; i < length ; i++) {
            var filterVal = "brightness(" + brightness + "%)";
            $(".swiper-slide").not(".empty").eq(i)
            .css('filter',filterVal)
            .css('webkitFilter',filterVal)
            .css('mozFilter',filterVal)
            .css('oFilter',filterVal)
            .css('msFilter',filterVal);

            brightness = brightness - 4;
         }
      }
   }
}
</script>


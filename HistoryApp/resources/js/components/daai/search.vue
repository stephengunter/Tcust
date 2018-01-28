<template>
   <div  class="rightbox">
		<div class="div2">
			<section class="main">
				
				<div class="wrapper-demo">
					<year-month-drop v-if="loaded" :options="yearOptions" :selected="params.year" custom_style="z-index:3"
					   @selected="onYearSelected">

					</year-month-drop>
				</div>
			</section>
			<section class="main">
				<div class="wrapper-demo">
					<year-month-drop v-if="loaded" :options="monthOptions" :selected="params.month" custom_style="z-index:2"
					   @selected="onMonthSelected">
					</year-month-drop>

					
				</div>
			</section>
		</div>
		<div>
			<div class="menu5">
				<input name="textfield" v-model="params.keyword"  type="text" class="textbox" id="keyWordInput" placeholder="輸入" size="10" maxlength="5">
			</div>
			
			<slot name="title"> 
     
         </slot> 
			
			<div class="btnright">
				<a @click.prevent="onSubmit" href="#">搜尋</a>
			</div>
			<div class="btnIcon">
				<a @click.prevent="leave" href="#"></a>
			</div>
		</div>
		

	</div>
</template>

<script>
import YearMonthDrop from './year-month-drop';
export default {
	name:'DiarySearch',
	components: {
		'year-month-drop':YearMonthDrop,
	},
   props: {
      model: {
         type: Object,
         default: null
      }
   },
   data(){
      return {
         loaded:false,
			
         yearOptions:[{
            value:0,
            text:'年度'
         }],
         monthOptions:[{
            value:0,
            text:'月份'
         }],

			params:{
				year:0,
				month:0,
				keyword:'',
			},
         
      }
   },
	beforeMount() {
      this.fetchData();
	},
   methods:{
		init(){
			
			this.loaded=false;
         
         this.yearOptions=[{
            value:0,
            text:'年度'
         }];
         this.monthOptions=[{
            value:0,
            text:'月份'
			}];
			
			this.params={
				year:0,
				month:0,
				keyword:'',
			}
			
			this.fetchData();

		},
		fetchData(){
			let years=Api.getDaAiNewsYears();
			years.then(data => {
				this.initYearMonth(data);
			})
			.catch(error => {
				Helper.BusEmitError(error);
				
			})
		},
      initYearMonth(years){
         
         years.forEach((year)=>{
            this.yearOptions.push( {
               value:year,
               text:year
            });
			});
			
			this.loadMonthOptions();

         this.loaded=true;
         
		},
      onYearSelected(item){
			this.params.year=item.value;
      },
      loadMonthOptions(){
         for(let i=1; i<=12; i++){
				this.monthOptions.push( {
               value:i,
               text:i + '月'
            });
			}
         
      },
      onMonthSelected(item){
         this.params.month=item.value;
      },
		onSubmit(){
			
			this.$emit('submit',this.params);
			
		},
      back(){
         this.$emit('back');
		},
		leave(){
         this.$emit('leave');
		},
   }
}
</script>


<style scoped>
   .menu5 {
      margin-top: 20px !important;
   } 
</style>



<template>
	<div class="form-group" style="padding-left:1cm;">
		<div class="form-group" style="padding-left:1cm;">
			<drop-down v-if="loaded" :items="yearOptions" :selected="yearValue"
				@selected="onYearSelected">
			</drop-down>
		</div>
		<div class="form-group">
			<drop-down v-if="loaded" v-show="yearValue" :items="termOptions" :selected="termValue"
				@selected="onTermSelected">
			</drop-down>
		</div>
	</div>
</template>

<script>
export default {
	name:'YearTermFilter',
	data(){
		return {
			loaded:false,

			yearTerms:[],
			yearOptions:[{
				value:0,
				text:'學年度'
			}],
			termOptions:[{
				value:0,
				text:'學期'
			}],
			yearTerm:null,
			termId:0,
			
		}
	},
	computed:{
		yearValue(){
			if(this.yearTerm) return this.yearTerm.id;
			return '';
		},
		termValue(){
			if(this.termId) return this.termId;

			if(!this.yearValue) return 0;
		
			if(!this.termOptions || !this.termOptions.length)   return 0;

			return this.termOptions[0].value;
		}
	}, 
	beforeMount() {

		
		
		let terms=Api.getTermYears();
		terms.then(data => {
			this.initYearTerms(data);
			
		})
		.catch(error => {
			Helper.BusEmitError(error);
			
		})


	},
	methods:{
		initYearTerms(yearTerms){
			this.yearTerms=yearTerms;
			
			yearTerms.forEach((item)=>{
				this.yearOptions.push( {
					value:item.id,
					text:item.year
				});
			});

			this.loaded=true;
			this.$emit('loaded');
			
		},
		setTerm(term){
			
		},	
		onYearSelected(year){
			this.termId=0;
			if(!year.value){
				this.yearTerm=null;
				this.$emit('changed',this.getQuery());
				return;
			} 
			

			let getYear = new Promise((resolve, reject)=> {
				let yearTerm=this.yearTerms.find((item)=>{
					return item.id==year.value;
				});
				resolve(yearTerm);
			});

			getYear.then((yearTerm) => {
				if(yearTerm){
					this.yearTerm=yearTerm;
					this.$emit('changed',this.getQuery());
					this.loadTermOptions();
				}else{
					this.yearTerm=null;
					this.$emit('changed',this.getQuery());
				}  
				
			});
		},
		loadTermOptions(){
			let termOptions=[{
					value:0,
					text:'--------'
				}];

			let getOptions = new Promise((resolve, reject)=> {
				
				this.yearTerm.terms.forEach((item)=>{
					termOptions.push({
						value:item.id,
						text:item.title
					});
				});
				resolve(termOptions);
			});

			getOptions.then((options) => {
				this.termOptions=options;
			});
			
		},
		onTermSelected(term){
			
			this.termId=term.value;
			this.$emit('changed',this.getQuery());
		},
		getQuery(){
			if(!this.yearTerm) return '';

			if(this.termId){
			
				let term=this.yearTerm.terms.find((item)=>{
				return item.id==this.termId;
				});

				return term.number;

			}else{
			
				let termNumbers = this.yearTerm.terms.map((item)=>{
				return item.number;
				});

				return termNumbers.join();
			} 

		}
	}
}
</script>


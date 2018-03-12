<template>
   <div>
      <search-form :init_keyword="params.keyword" @search="onSearch"></search-form>
      <div class="wbox">
         
         <search-list v-if="getPosts()" :posts="getPosts()" 
				@selected="onSelected" @next-page="pageNext" >
         </search-list>
         <div class="rightbox">
            <div v-show="loaded">
               <ul class="btnIcon">
               	<a href="#" @click.prevent="onBack"></a>
               </ul>
            </div>
         </div> 
      </div>
   </div>
</template>

<script>
import SearchForm from '../../components/honor/search-form';
import SearchList from '../../components/honor/search-list';
export default {
   name: 'HonorSearch',
   components: {
      'search-form': SearchForm,
		'search-list':SearchList,
   },
   props: {
      init_keyword: {
         type: String,
         default: ''
      }
   },
   data() {
      return {
         keyword:'',
         busy:false,
			
			loaded:false,
			params: {
            keyword: '',
            page: 1,
            pageSize: 10
         },
			model: null,
			
      }
   },
   beforeMount() {
      this.params.keyword=this.init_keyword;
      this.searchPosts();
   },
   methods:{
      onSearch(keyword){
			this.params.keyword=keyword;
			this.model=null;
         this.searchPosts();
      },
      searchPosts(){
			this.busy=true;

         let getData = Api.searchHonorList(this.params);

         getData.then(model => {
            if(this.model){
               this.model.viewList=this.model.viewList.concat(model.viewList);
					this.model.hasNextPage=model.hasNextPage;

					this.busy=false;
					
            }else{
              
               this.model = { ...model };
              
					this.busy=false;

					this.onReady();
            }  

         })
         .catch(error => {
				Helper.BusEmitError(error);
				this.busy=false;

         })
		},
		onReady(){
			this.loaded=true;
			this.$emit('loaded');
		},
		pageNext() {
			if(this.model.hasNextPage)
			{
				this.params.page += 1;
				this.searchPosts();
			}
			
      },
      getPosts(){
			if(this.model) return this.model.viewList;
			return null;
		},
		onSelected(id){
			this.$emit('selected',id);
		},
		onBack(){
			this.$emit('back');
		},
   }
}
</script>

<style>

</style>



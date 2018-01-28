<template>

   <div class="wbox1" style="width:1920px;">

      <div class="leftbox">
        <a @click.prevent="init"  href="#">

			  <img :src="title_img_path" width="134" height="750">
        </a>
      </div>
		<div  class="centerbox">
			<daai-search-list v-if="searchMode"  :disabled_scroll="!canSrollMore" :posts="getViewList()" 
				@next-page="pageNext" @selected="onDetails">

			</daai-search-list>
			<div v-else class="tztable2">
				<div id="postContent">
					<ul class="title_01">
						<li class="title_02">{{ postYear }} </li>
						<li v-show="post" class="title_03">____________</li>
						<li class="title_04">{{ postMonthDate }}</li>
					</ul>
					<ul  class="title_05">
						<span v-if="post" v-text="post.title"></span>
						
					</ul>

					<ul  class="title_06">
						<span  class="glow">
							<video id="main-video"  width="690" height="470" controls="" poster="">
								<source id="main-video-source" src="">
							</video>
						</span>
					</ul>
					<ul v-if="post"  class="intxt3">

						<li class="news2" style="height:400px;overflow-y: scroll; overflow-x: hidden;">
							<p>{{ post.author }} </p>
							<p v-html="post.content">

							</p>

						</li>
					</ul>
				</div>
				<daai-menu v-if="getViewList()" :posts="getViewList()" :selected_id="selectedId" :busy="busy"
					 :disabled_scroll="!canSrollMore" @next-page="onMenuPageNext" @selected="onDetails">
				</daai-menu>

			</div>
			
		</div>

      <daai-search ref="daaiSearch"  @submit="onSearch" @leave="leave">
         <div slot="title" class="righttitle">
				<a @click.prevent="init" href="#">
            	<img :src="search_img_path" width="210" height="55">
				</a>
         </div>

      </daai-search>





   </div>
</template>

<script>
import DaAiMenu from '../../components/daai/menu';
import DiarySearch from '../../components/daai/search';
import DiarySearchList  from '../../components/daai/search-list';
export default {
   name:'DaAiView',
   components: {
		'daai-menu':DaAiMenu,
      'daai-search':DiarySearch,
		'daai-search-list':DiarySearchList,
   },
   props: {
      title_img_path: {
         type: String,
         default: ''
      },
      search_img_path: {
         type: String,
         default: ''
      },
   },
   data() {
      return {
			fetchingMenus:false,
			fetchingDetails:false,

         params: {
            year: 0,
            month:0,
            keyword: '',
            page: 1,
            pageSize: 10
         },
         model: null,

			searchMode:false,


        
			post:null,

      }
	},
	computed:{
		busy(){
			if(this.fetchingMenus) return true;
			if(this.fetchingDetails) return true;
			
			return false;
			
		},
		canSrollMore(){
			if(!this.model) return false;
			if(this.busy) return false;
			return this.model.hasNextPage;
		},
		postYear(){
			if(this.post) return  this.postDate(this.post).year();
			return '';
		},
		postMonthDate(){
			if(this.post) return  this.postDate(this.post).format('MMDD');
			return '';
			
		},
		selectedId(){
			if(this.post) return this.post.id;
			return 0;
		}

	},
   watch:{

   },
   beforeMount() {
		this.fetchData();
		
   },
   mounted(){


   },
   methods: {
		init(){
			this.fetchingMenus=false;
			this.fetchingDetails=false;

			this.params={
            year: 0,
            month:0,
            keyword: '',
            page: 1,
            pageSize: 10
			};

			this.searchMode=false;

			this.model=null;

			this.$refs.daaiSearch.init();

			
			this.post=null;

			this.fetchData();

		},
      getViewList(){
         if(this.model) return this.model.viewList;
         return [];
      },
      fetchData() {
			this.fetchingMenus=true;

         let getData = Api.getDaAiNews(this.params);

         getData.then(model => {
				
				if(this.model){
               this.model.viewList=this.model.viewList.concat(model.viewList);
					this.model.hasNextPage=model.hasNextPage;
               this.onPostsLoaded();
            }else{

					this.model = { ...model };
					

					this.onPostsLoaded();
            }

         })
         .catch(error => {
				Helper.BusEmitError(error);
				this.fetchingMenus=false;

         })
      },
		onPostsLoaded(){
			this.fetchingMenus=false;

			if(this.searchMode){


			}else{
				if(!this.post) this.onDetailsLoaded(this.model.viewList[0]);
			}

		},
		postDate(post){
			if(!post) return null;
			if(post.date) return moment(post.date,'YYYY-MM-DD');
			return moment(post.createdAt);
		},
		onMenuPageNext(){
			let items=this.getViewList();
			if(!items) return;
			if(items.length < 5) return;

			this.pageNext();
		},
		pageNext() {
			this.params.page += 1;
			this.searchPosts();
		},
      pageNext() {
			if(this.model.hasNextPage){
				this.params.page += 1;
        		this.fetchData();
			}
      },
      onSearch(params) {
			
			
         this.params.year = params.year;
         this.params.month = params.month;
         this.params.keyword = params.keyword;


			this.searchMode=true;
			this.model=null;

			this.fetchData();
		},
		onDetails(id){

			if(this.searchMode) this.searchMode=false;
			
			this.fetchingDetails=true;
         let getData = Api.postDetails(id);
			
         getData.then(post => {
				this.onDetailsLoaded(post);
				
         })
         .catch(error => {
				Helper.BusEmitError(error);
				this.onDetailsLoaded();
         })

		},
		onDetailsLoaded(post){
			$('.title_06').fadeOut();

			if(post){
				if(post.cover){
					this.post = { ...post };
				}else{
					this.post = { 
					...post ,
					 cover:{ ...post.medias[0] }
					}; 
				}
				
				let videoContainer = document.getElementById('main-video');
				videoContainer.pause();
				

				let videoSource = document.getElementById('main-video-source');
				videoSource.setAttribute('src',this.post.cover.path);
				videoContainer.load();

				videoContainer.setAttribute('poster',this.post.cover.previewPath);
				videoContainer.play();
				
			}

			$('.title_06').fadeIn();
			this.fetchingDetails=false;
		},
		leave(){
			alert('leave');
			this.init();
			//this.$emit('leave');
		}



   }



}
</script>

<style>

	.title_05 {
    height: 90px;
    width: 600px;
    overflow: hidden;
    word-break: break-all;
	}

	.tztable2{

		opacity: 1;
	}

</style>









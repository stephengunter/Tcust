<template>
    
   <div v-if="model" class="row">
      
      <div class="col-sm-8 blog-main">

         <post-item v-for="(post,index) in model.viewList" :key="index"
         :post="post" v-on:details="onDetails" >

         </post-item>

         <div v-if="hasData" >
            <ul class="pager">
               <li :class="{ 'previous': true, 'disabled': !model.hasPreviousPage }" >
                  <a @click.prevent="onPreviousPage" href="#" rel="prev">← Newer</a>
               </li>
               <li :class="{ 'next': true, 'disabled': !model.hasNextPage }">
                  <a @click.prevent="onNextPage" href="#" rel="next">Older →</a>
               </li>
            </ul>
         </div>

      </div>
      <div  class="col-sm-1">
            
      </div>
      <div class="col-sm-3">
         
         <div class="sidebar-module">
  
            <div v-if="params.keyword">
					關鍵字：{{ params.keyword }}  &nbsp;  
					<span class="badge badge-pill badge-success">{{ model.totalItems  }}</span>
					<a href="/">
						<i class="close fa fa-times"></i>
					</a>
        		</div>
   
            <archieves :items="archive_items" :year="params.year"
              @selected="onArchiveSelected" >
            </archieves>


         </div>   

      </div>


   </div>
</template>


<script>
   import PostItem from '../components/post-item';
   import Archieves from '../components/archieves';
   export default {
      name:'PostIndexView',
      components: {
         PostItem,
         Archieves
      },
      props: {
         model: {
            type: Object,
            default: null
			},
			params:{
				type: Object,
            default: null
			},
         archive_items: {
            type: Array,
            default: null
         },
      },
      data(){
         return {
				
           
         }
      },
      computed:{
         hasData(){
            if(!this.model) return false;
            return this.model.totalPages > 0;
         }
      },
      beforeMount(){
          this.init();
      },
      methods:{
         init(){
            
           
			},
         onArchiveSelected(item){
				this.$emit('year-changed',parseInt(item.text));
         },
         onPreviousPage(){
				let page=parseInt(this.params.page) - 1;
				this.$emit('page-changed',page);
           
         },
         onNextPage(){
            let page=parseInt(this.params.page) + 1;
				this.$emit('page-changed',page);
			},
			
         fetchData(){
            let getData = Post.index(this.params);

            getData.then(model => {
					this.$emit('posts-fetched',model);

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
         },
         onDetails(id){
			
            this.$emit('details',id);
            
         }
      }
   }
</script>

<style>
.pagination {
  display: inline-block;
  padding-left: 0;
  margin: 20px 0;
  border-radius: 4px;
}

.pagination > li {
  display: inline;
}

.pagination > li > a,
.pagination > li > span {
  position: relative;
  float: left;
  padding: 6px 12px;
  margin-left: -1px;
  line-height: 1.428571429;
  text-decoration: none;
  background-color: #ffffff;
  border: 1px solid #dddddd;
}

.pagination > li:first-child > a,
.pagination > li:first-child > span {
  margin-left: 0;
  border-bottom-left-radius: 4px;
  border-top-left-radius: 4px;
}

.pagination > li:last-child > a,
.pagination > li:last-child > span {
  border-top-right-radius: 4px;
  border-bottom-right-radius: 4px;
}

.pagination > li > a:hover,
.pagination > li > span:hover,
.pagination > li > a:focus,
.pagination > li > span:focus {
  background-color: #eeeeee;
}

.pagination > .active > a,
.pagination > .active > span,
.pagination > .active > a:hover,
.pagination > .active > span:hover,
.pagination > .active > a:focus,
.pagination > .active > span:focus {
  z-index: 2;
  color: #ffffff;
  cursor: default;
  background-color: #428bca;
  border-color: #428bca;
}

.pagination > .disabled > span,
.pagination > .disabled > a,
.pagination > .disabled > a:hover,
.pagination > .disabled > a:focus {
  color: #999999;
  cursor: not-allowed;
  background-color: #ffffff;
  border-color: #dddddd;
}

.pagination-lg > li > a,
.pagination-lg > li > span {
  padding: 10px 16px;
  font-size: 18px;
}

.pagination-lg > li:first-child > a,
.pagination-lg > li:first-child > span {
  border-bottom-left-radius: 6px;
  border-top-left-radius: 6px;
}

.pagination-lg > li:last-child > a,
.pagination-lg > li:last-child > span {
  border-top-right-radius: 6px;
  border-bottom-right-radius: 6px;
}

.pagination-sm > li > a,
.pagination-sm > li > span {
  padding: 5px 10px;
  font-size: 12px;
}

.pagination-sm > li:first-child > a,
.pagination-sm > li:first-child > span {
  border-bottom-left-radius: 3px;
  border-top-left-radius: 3px;
}

.pagination-sm > li:last-child > a,
.pagination-sm > li:last-child > span {
  border-top-right-radius: 3px;
  border-bottom-right-radius: 3px;
}

.pager {
  padding-left: 0;
  margin: 20px 0;
  text-align: center;
  list-style: none;
}

.pager:before,
.pager:after {
  display: table;
  content: " ";
}

.pager:after {
  clear: both;
}

.pager:before,
.pager:after {
  display: table;
  content: " ";
}

.pager:after {
  clear: both;
}

.pager li {
  display: inline;
}

.pager li > a,
.pager li > span {
  display: inline-block;
  padding: 5px 14px;
  background-color: #ffffff;
  border: 1px solid #dddddd;
  border-radius: 15px;
}

.pager li > a:hover,
.pager li > a:focus {
  text-decoration: none;
  background-color: #eeeeee;
}

.pager .next > a,
.pager .next > span {
  float: right;
}

.pager .previous > a,
.pager .previous > span {
  float: left;
}

.pager .disabled > a,
.pager .disabled > a:hover,
.pager .disabled > a:focus,
.pager .disabled > span {
  color: #999999;
  cursor: not-allowed;
  background-color: #ffffff;
}
</style>




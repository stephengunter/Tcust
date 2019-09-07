new Vue({
	el: '#app',
	data: {

      selected: 0,

      model: null,
      archives: [],
      categories: [],
      params: {
         category: 0,
         keyword: '',
         year: 0,
         page: 1
      }


	},
	beforeMount() {
      let categoryList = @Html.Raw(ViewData["categories"]);
      this.categories = categoryList;

      this.params.category = @ViewData["category"];

      this.params.keyword = '@Html.Raw(ViewData["keyword"])';

      this.params.year = @ViewData["year"];

      this.archives = @Html.Raw(ViewData["archives"]);

      this.model = @Html.Raw(ViewData["list"]);


      this.init();


	},
	mounted() {
	   this.$refs.navBar.setKeyword(this.params.keyword);
	},
	methods: {
      init() {

         this.params.page = this.model.pageNumber;
         this.params.pageSize = this.model.pageSize;

      },
      fetchPosts(){
         let params = {};
         if(this.params.category) params.category = this.params.category;
         if(this.params.keyword) params.keyword = this.params.keyword;
         if(this.params.year) params.year = this.params.year;
         params.page = this.params.page;
         let url = Helper.buildQuery('/posts', params);
         document.location = url;

      },
      onYearChanged(year) {
         this.params.year = year;
         this.params.page = 1;
         this.fetchPosts();
      },
      onSearch(keyword) {
         let url = Helper.buildQuery('/posts', { keyword });
         document.location = url;
      },
      onPageChanged(page) {
         this.params.page = page;
         this.fetchPosts();
      },
      onDetails(id) {
         document.location = `/posts/${id}`;
      }
   }

});
new Vue({
	el: '#app',
	data: {
      category: 0,
      categories: [],

      detailsModel: null

	},
	beforeMount() {
      let categoryList = @Html.Raw(ViewData["categories"]);
      this.categories = categoryList;

      this.category = @ViewData["category"];

     
      this.detailsModel = @Html.Raw(ViewData["model"]);

	},
	methods: {
      onSearch(keyword) {
         let url = Helper.buildQuery('/posts', { keyword });
         document.location = url;
      },
      onBack(){
         window.history.back();
      }
   }

});
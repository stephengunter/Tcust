class PostReview {
   

	static source() {
		return '/admin/review';
   }
	
	static storeUrl() {
		return this.source()
   }
	

	static index(params){
		let url = this.source();
		url=Helper.buildQuery(url, params);


		return new Promise((resolve, reject) => {
			axios.get(url)
				  .then(response => {
						resolve(response.data);
				  })
				  .catch(error => {
						reject(error);
				  })

		})
	}
	

	static store(post_ids) {
		let url = this.storeUrl()
		let method = 'post'
		let form = new Form({
			 postIds: post_ids
		})

		return new Promise((resolve, reject) => {
			 form.submit(method, url)
				  .then(data => {
						resolve(data);
				  })
				  .catch(error => {
						reject(error);
				  })
		})
   }


	
   
   
   

   
}


export default PostReview;
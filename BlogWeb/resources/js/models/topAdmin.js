class TopAdmin {
   

	static source() {
		return '/admin/tops';
   }
	
	static storeUrl() {
		return this.source();
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

	static store(posts){
		
		let url = this.storeUrl()
      let method = 'post'
      return new Promise((resolve, reject) => {
			axios.post(url, posts)
					.then(response => {
						resolve(response.data);
					})
					.catch(error => {
						reject(error);
					})

      })
	}
	
	
   

   
}


export default TopAdmin;
class Post {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

	}

	static source() {
		return '/admin/posts'
   }
	static createUrl() {
		return this.source() + '/create'
	}
	static storeUrl() {
		return this.source() + '/store'
   }
	
	static create() {
		let url = this.createUrl();

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
	
	static store(form){
		let url = this.storeUrl();
		let method = 'post'
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


export default Post;
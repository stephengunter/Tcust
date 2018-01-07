class Post {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

	}

	static source() {
		return '/posts';
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
	
	

	static details(id) {
		let url =  this.source() + `/${id}`;

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

	
   
   

   
}


export default Post;
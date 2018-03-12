class Api {
   static source() {
	   
		return 'http://api.tcust.edu.tw';
   }
   
   static getTermYears(){
      let url =`${this.source()}/terms`;
    
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
	

	static getDiaryList(params){
		let url =`${this.source()}/posts/GetDiaryList`;

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
	static getHonorList(params){
		let url =`${this.source()}/posts/GetHonorList`;

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
	static searchHonorList(params){
		let url =`${this.source()}/posts/SearchHonorList`;

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
	static getFamerList(params){
		let url =`${this.source()}/posts/GetFamerList`;

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
	static getDaAiNews(params){
		let url =`${this.source()}/posts/GetDaAiNews`;
		
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
	static getDaAiNewsYears(){
		let url =`${this.source()}/posts/GetDaAiNewsYears`;

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

	static postDetails(id){
		let url =`${this.source()}/posts/${id}`;

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


export default Api;
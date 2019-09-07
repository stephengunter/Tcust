class Department {
   

	static source() {
		return '/admin/departments';
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

	static store(items){
		
		let url = this.storeUrl()
        let method = 'post'
        return new Promise((resolve, reject) => {
                axios.post(url, items)
                        .then(response => {
                            resolve(response.data);
                        })
                        .catch(error => {
                            reject(error);
                        })

        })
	}
	
	
   

   
}


export default Department;
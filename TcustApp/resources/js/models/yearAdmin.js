class YearAdmin {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

	}

	static source() {
		return '/admin/years';
	}
	static createUrl() {
		return this.source() + '/create';
	}
	static storeUrl() {
		return this.source();
	}
	static editUrl(id) {
		return `${this.source()}/${id}/edit`;
	}
	static updateUrl(id) {
		return this.source() + `/${id}`;
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

	static store(form){
		let url = this.storeUrl();
		let method = 'post';
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

	static edit(id) {
		let url = this.editUrl(id);

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

	static update(id,form){
		let url = this.updateUrl(id);
		let method = 'put';
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

	
  
	
	static activeOptions() {
		return [{
			 text: '是',
			 value: true
		}, {
			 text: '否',
			 value: false
		}]
   }
   

   
}


export default YearAdmin;
class Attachment {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

   }
   static source() {
		return '/admin/uploads';
   }
	
	static storeUrl() {
		return this.source();
	}

	static deleteUrl(id){
		return `${this.source()}/delete/${id}`;
		
	}
	
	static store(form){
		let url = this.storeUrl()
      let method = 'post'
      return new Promise((resolve, reject) => {
			axios.post(url, form)
					.then(response => {
						resolve(response.data);
					})
					.catch(error => {
						reject(error);
					})

      })
	}

	static remove(id) {
		let url = this.deleteUrl(id);
		
		return new Promise((resolve, reject) => {
			 axios.delete(url)
				  .then(response => {
						resolve(response.data);
				  })
				  .catch(error => {
						reject(error);
				  })

		})
	}
   
   

   
}


export default Attachment;
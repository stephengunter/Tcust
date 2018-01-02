class Attachment {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

   }
   static source() {
		return '/admin/uploads'
   }
	
	static storeUrl() {
		return this.source() + '/store'
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
   
   

   
}


export default Attachment;
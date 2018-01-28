class Api {
   static source() {
		return 'http://localhost:50001/api';
   }
   static setToken(token) {
      axios.defaults.headers.common.Authorization='Bearer ' + token
   }
   static getUserByEmail(email){
     
      let url =`${this.source()}/users/GetUserByEmail?email=${email}`;
    
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
   
   

   
}


export default Api;
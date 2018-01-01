class Attachment {
   constructor(data) {

      for (let property in data) {
          this[property] = data[property];
      }

   }
   
   
   

   
}


export default Attachment;
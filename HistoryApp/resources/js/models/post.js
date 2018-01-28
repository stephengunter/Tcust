class Post {
   
   static getDate(post){
      
      let date =null;

      if(post.date) date = moment(post.date);
      else date = moment(post.createdAt);
      
      return date.format("YYYY-MM-DD") ;
   }

}


export default Post;
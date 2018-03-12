class  Photo {
    
  
	static lockWidth(photo){
     
		if(!photo) return true;

        let widthHeightRatio = 423 / 260;

        try {
            let imageRatio = Number(photo.width) / Number(photo.height);
            return widthHeightRatio <= imageRatio;
        }
        catch(err) {
            
            return true;
        }
   }
   
   
   static lockCoverWidth(photo){
		if(!photo) return true;

      let widthHeightRatio = 443 / 325;

      try {
         imageRatio = Number(photo.width) / Number(photo.height);

         return widthHeightRatio <= imageRatio;
      }
      catch(err) {
         return true;
      }
   }
   

   
}


export default Photo;
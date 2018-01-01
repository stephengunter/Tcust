<template>
   <label  class="btn btn-primary btn-file" >
      <i class="fa fa-plus"></i> {{ title  }}
      <input type="file" multiple accept="image/png,image/gif,image/jpeg" 
      style="display: none;" @change="onFileChange" > 
   </label>
  
</template>

<script>
export default {
   name:'FileUpload',
   props:{
      title: {
         type: String,
         default: '新增圖片'
      },
   },
   data(){
      return {
         files:[],
         thumbnails:[],
      }
   },
   methods:{
      onFileChange(e) {
         let files = e.target.files || e.dataTransfer.files;
         if (!files.length)  return;

         for (let i=0; i<files.length; i++) {
           
            if(!this.fileExist(files[i].name)){
               this.addFile(files[i]);
            }
         }
                
      },
      fileExist(name){
          let index=this.findFileIndex(name);
          return index >=0;
      },
      findFileIndex(name){
         let index=this.files.findIndex((item)=>{
            return item.name==name;
         });
         return index ;
      },
      addFile(file){
         let image= this.createImage(file);
         image.then(data=>{
            let thumb={
               data:data,
               name:file.name
            }
            this.files.push(file);
            this.thumbnails.push(thumb);

            this.$emit('file-added');
         }).catch(error=>{
            console.log(error);
         })  
      },
      removeFile(name){
         let index=this.findFileIndex(name);
         if(index>=0) this.files.splice(index, 1);

         this.removeThumb(name);
        
      },
      removeThumb(name){
         let thumbIndex=this.thumbnails.findIndex((item)=>{
            return item.name==name;
         });
         if(thumbIndex>=0) this.thumbnails.splice(thumbIndex, 1);
      },
      createImage(file) {
         const reader = new FileReader();
         return new Promise((resolve, reject) => {
            reader.onerror = (error) => {
               reader.abort();
               reject(error);
            };

            reader.onload = () => {
               resolve(reader.result);
            };
            reader.readAsDataURL(file);
         });
      },
      getFiles(){
			return this.files;
      },
      getThunbnails(){
         return this.thumbnails;
      }
      
   }
}
</script>


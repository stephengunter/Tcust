<template>
   <label  class="btn btn-primary btn-file" >
      <i class="fa fa-plus"></i> {{ title  }}
      <input type="file" multiple accept="accept" 
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
		exclude:{
			type: Array,
         default: null
		}
   },
   data(){
      return {
			image_types:['image/png','image/gif','image/jpeg'],
			video_types:['vidoe/mp4'],
			accept:'',
         files:[],
         thumbnails:[],
      }
   },
	beforeMount() {
		this.init();
	},
   methods:{
		init(){
			this.accept=this.image_types.toString() + ',' +  this.video_types.toString();
		},
      onFileChange(e) {
			
			let files = e.target.files || e.dataTransfer.files;
		
         if (!files.length)  return;

			let addFiles=[];
         for (let i=0; i<files.length; i++) {
				
				if(this.fileCanAdd(files[i])){
					addFiles.push(this.addFile(files[i]));
				}
			}


			Promise.all(addFiles).then(() => { 
			    this.$emit('file-added');
			});
			
                
		},
		fileCanAdd(file){
			if(this.fileExist(file.name)) return false;
			if(this.exclude.includes(file.name)) return false;
			return true;
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
		   return new Promise((resolve, reject) => {
            let image= this.createImage(file);
				image.then(data=>{
					let thumb={
						data:data,
						name:file.name,
						type:file.type,
					}
					this.files.push(file);
					this.thumbnails.push(thumb);
					
					resolve(true);
					
				}).catch(error=>{
					console.log(error);
					reject();
				})  
         });
         
      },
		isImage(type){
			return this.image_types.includes(type);
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
         
         return new Promise((resolve, reject) => {
				if(!this.isImage(file.type)) resolve(null);

				const reader = new FileReader();
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


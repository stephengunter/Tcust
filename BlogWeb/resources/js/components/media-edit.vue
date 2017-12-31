<template>
   <div>
      <div>
         <table class="table table-hover">
            <thead>
               <tr>
                  <th></th>
                  <th>標題</th>
                  <th></th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(media,index) in medias" :key="index">
                  <td>
                     <!-- <img v-if="file.thumb" :src="file.thumb" width="80" height="auto" /> -->
                  </td>
                  <td>
                      {{media.name}}
                  </td>
                  <td>
                     <button class="btn btn-sm btn-primary" >
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button class="btn btn-sm btn-danger" @click.prevent="removeMedia(media)">
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                     </button>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      <div>
         <label  class="btn btn-success btn-file" >
             <i class="fa fa-plus"></i>
             新增圖片
              <input type="file" multiple accept="image/png,image/gif,image/jpeg" 
               style="display: none;" @change="onFileChange" >
                      
         </label>

         <button type="button" class="btn btn-success"  @click.prevent="testUpload">
           <i class="fa fa-arrow-up" aria-hidden="true"></i>
           Start Upload
         </button>
      </div>
   </div>
</template>

<script>
export default {
   name:'MediaEdit',
      props:{
         id:{
            type:Number,
            default:0
         }
      },
      data(){
         return {
            files:[],
            medias:[],

            filedata:{
               name:'',
               type:'',
               size:'',

            },
            autoCompress: 1024 * 1024,
         }
      },
      computed:{

      }, 
      methods:{
         test(){
            let i=this.medias.findIndex((item)=>{
               return item.name=='Koala.jpg'
            });
            alert(i);
         },
         fileExist(name){
            let index=this.medias.findIndex((item)=>{
               return item.name==name;
            });
            return index >=0 ;
         },
         addFile(){

         },
         testUpload(){
            let form = new FormData();
            form.append('width', '122');
            form.append('height','211');    
            for (let i = 0; i < this.files.length; i++) {
               form.append('image_files', this.files[i]);
            }
            let url='/admin/posts/upload';
            axios.post(url, form)
                .then(response => {
                     alert('then');
                })
                .catch(error => {
                     alert('err');
                })


         },
         onFileChange(e) {
            let files = e.target.files || e.dataTransfer.files;
            if (!files.length)  return;

            this.addFiles(files);
                
         },
         addFiles(files){
            for (let i=0; i<files.length; i++) {
               if(!this.fileExist(files[i].name)){
                  this.files.push(files[i]);
                  this.medias.push({
                     name:files[i].name,
                     order:i,
                     title:''
                  });
               }
            }
        
            
         },
         removeMedia(media){
            alert('removeMedia:' + media.name);
         },
         onAdd(){
            this.create=true;
         },
         inputFilter(newFile, oldFile, prevent){
            
            if (newFile && !oldFile) {
               // Before adding a file
               // Filter system files or hide files
               if (/(\/|^)(Thumbs\.db|desktop\.ini|\..+)$/.test(newFile.name)) {
                  return prevent();
               }
               // Filter php html js file
               if (/\.(php5?|html?|jsx?)$/i.test(newFile.name)) {
                  return prevent();
               }
               
            }

            if (newFile && (!oldFile || newFile.file !== oldFile.file)) {

               // Create a blob field
               newFile.blob = ''
               let URL = window.URL || window.webkitURL
               if (URL && URL.createObjectURL) {
                  newFile.blob = URL.createObjectURL(newFile.file)
               }

               // Thumbnails
               newFile.thumb = ''
               if (newFile.blob && newFile.type.substr(0, 6) === 'image/') {
                  newFile.thumb = newFile.blob
               }
            }
         },
         inputFile(newFile, oldFile) {
            if (newFile && oldFile) {
               // update
               if (newFile.active && !oldFile.active) {
                  // beforeSend
                  console.log('update');
                  // min size
                  if (newFile.size >= 0 && this.minSize > 0 && newFile.size < this.minSize) {
                        this.$refs.upload.update(newFile, {
                           error: 'size'
                        })
                  }
               }

                    if (newFile.progress !== oldFile.progress) {
                        // progress
                    }

                    if (newFile.error && !oldFile.error) {
                        // error
                    }

                    if (newFile.success && !oldFile.success) {
                        // success
                    }
            }

            if (!newFile && oldFile) {
               // remove
               if (oldFile.success && oldFile.response.id) {
                  // $.ajax({
                  //   type: 'DELETE',
                  //   url: '/upload/delete?id=' + oldFile.response.id,
                  // })
               }
            }


            // Automatically activate upload
             
            if (Boolean(newFile) !== Boolean(oldFile) || oldFile.error !== newFile.error) {
               if (this.uploadAuto && !this.$refs.upload.active) {
                  this.$refs.upload.active = true
               }
            }
         }
         
      }
}
</script>


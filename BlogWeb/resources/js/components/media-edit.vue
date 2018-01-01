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
							<img v-if="media.thumb" class="thumbnail" style="max-width:120px" :src="media.thumb" />
                    
                  </td>
                  <td>
                      {{ media.title }}
                  </td>
                  <td>
                     <button class="btn btn-sm btn-primary" >
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                     </button>
                     <button class="btn btn-sm btn-danger" @click.prevent="removeMedia(media)">
                        <i class="fa fa-trash-o" aria-hidden="true"></i>
                     </button>
							<button class="btn btn-sm btn-default" v-if="medias.length>1" @click.prevent="up(media,index)">
								<i class="fa fa-arrow-up" aria-hidden="true"></i>
							</button>
							<button class="btn btn-sm btn-default" v-if="medias.length>1" @click.prevent="down(media,index)">
								<i class="fa fa-arrow-down" aria-hidden="true"></i>
							</button>
                  </td>
               </tr>
            </tbody>
         </table>
      </div>
      <div>
         <file-upload ref="fileUpload" @file-added="onFileAdded"></file-upload>

         <button type="button" class="btn btn-success"  @click.prevent="testUpload">
           <i class="fa fa-arrow-up" aria-hidden="true"></i>
           Start Upload
         </button>
      </div>
   </div>
</template>

<script>
import FileUpload from './file-upload';
export default {
   name:'MediaEdit',
      props:{
         id:{
            type:Number,
            default:0
         }
		},
		components: {
         'file-upload':FileUpload
      },
      data(){
         return {
           
            medias:[],
			
            // filedata:{
            //    name:'',
            //    type:'',
            //    size:'',

            // },
            // autoCompress: 1024 * 1024,
         }
      },
      computed:{

      }, 
      methods:{
         fileExist(name){
				let index=this.findFileIndex(name);
				return index >=0;
			},
			findFileIndex(name){
				let index=this.medias.findIndex((item)=>{
					return item.name==name;
				});
				return index ;
			},
			
			addMedia(name,thumb){
				let media={
					id:0,
					order:this.findMinOrder() - 1,
					title:name,
					name:name,
					thumb:thumb
				};
				this.medias.push(media);
				this.sortMedias();
			},
         testUpload(){
				for (let i=0; i<this.files.length; i++) {
               alert(this.files[i].name);
            }
            // let form = new FormData();
            // form.append('width', '122');
            // form.append('height','211');    
            // for (let i = 0; i < this.files.length; i++) {
            //    form.append('image_files', this.files[i]);
            // }
            // let url='/admin/posts/upload';
            // axios.post(url, form)
            //     .then(response => {
            //          alert('then');
            //     })
            //     .catch(error => {
            //          alert('err');
            //     })


			},
			removeMedia(media){
				let index=this.findFileIndex(media.name);
				if(index< 0) return;

				this.medias.splice(index, 1);

				if(media.id){

				}else{
					this.$refs.fileUpload.removeFile(media.name);
				}
			},
			onFileAdded(){
			   let thumbs = this.$refs.fileUpload.getThunbnails();
			   for (let i=0; i<thumbs.length; i++) {
					let name=thumbs[i].name;
					if(!this.fileExist(name)){
						this.addMedia(thumbs[i].name, thumbs[i].data);
					}
					
            }
			},
			up(media,index){
				let upper=this.medias[index-1];
				if(!upper) return;

				let upperOrder=upper.order;
				let downOrder=media.order;

				upper.order=downOrder;
				media.order=upperOrder;

				this.sortMedias();

			},
			down(media,index){
				let downer=this.medias[index+1];
				if(!downer) return;

				let downerOrder=downer.order;
				let upperOrder=media.order;

				downer.order=upperOrder;
				media.order=downerOrder;

				this.sortMedias();
			},
			sortMedias(){
				this.medias.sort((a,b)=>{
					return b.order- a.order;
				})
			},
			findMinOrder(){
				if(!this.medias.length) return 0;

			   let arr=this.medias;
				let min = arr[0].order;
				for (let i = 1, len=arr.length; i < len; i++) {
					let v = arr[i].order;
					min = (v < min) ? v : min;
				
				}

				return min;
			}
         
			
         
         
         
        
         
      }
}
</script>


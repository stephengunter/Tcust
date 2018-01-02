<template>
   
	<div>
		<table v-show="medias.length" class="table table-hover">
			<thead>
				<tr>
					<th style="width:25%"></th>
					<th style="width:50%">標題</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr v-for="(media,index) in medias" :key="index">
					<td>
						<img v-if="media.thumb" class="thumbnail" style="max-width:120px" :src="media.thumb" />
						
					</td>
					<td v-if="edittingIndex==index">
						<input class="form-control" type="text" v-model="edittingMedia.title">
					</td>
					<td v-else>
						{{ media.title }} 
					</td>
					<td v-if="edittingIndex==index">
						<button class="btn btn-sm btn-success" @click.prevent="updateMedia(index)">
							<i class="fa fa-floppy-o" aria-hidden="true"></i>
						</button>
						<button class="btn btn-sm btn-default" @click.prevent="cancelEditMedia(index)">
							<i class="fa fa-undo" aria-hidden="true"></i>
						</button>
					</td>
					<td v-else>
						<button class="btn btn-sm btn-primary" @click.prevent="editMedia(media,index)">
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
		<file-upload ref="fileUpload" @file-added="onFileAdded"></file-upload>
	
	</div>
      
  
</template>

<script>
import FileUpload from './file-upload';
export default {
   name:'MediaEdit',
      props:{
         post_id:{
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
				edittingIndex:-1,

				edittingMedia:{},

				test:[],
			
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
					title:name.split('.')[0],
					name:name,
					thumb:thumb,
					path:''
				};
				this.medias.push(media);
				this.sortMedias();
			},
			getMedias(){
				let copyMedias= this.medias.map(media=>{
					return {...media , thumb:''};
				});
				return copyMedias;
			},
			submit(){
				const files=this.$refs.fileUpload.getFiles();
				let form = new FormData();
				form.append('postId', this.post_id);
				
				for (let i = 0; i < files.length; i++) {
					form.append('files', files[i]); 
				} 
				let save=Attachment.store(form);
				save.then(attachments => {
					alert('then');
				})
				.catch(error=> {
					alert('err');
				})
				
				// if(save){
				// 	let copyMedias= this.medias.map(media=>{
				// 		return {...media , thumb:''};
				// 	});

				// 	console.log(copyMedias);

				// 	return copyMedias;
				// }
				
			},
			saveFiles(){
				return new Promise((resolve, reject) => {
					const files=this.$refs.fileUpload.getFiles();
					let form = new FormData();
					for (let i = 0; i < files.length; i++) {
						form.append('files', files[i]); 
					}
					let save=Attachment.store(form);
					save.then(attachments => {
					   
						if(!attachments.length) resolve(null);
					   for(let x = 0; x < attachments.length; x++){
							let media=this.medias.find(item=>{
								return item.name==attachments[x].name;
							});
							media.path=attachments[x].path;
						}

						resolve(this.medias);

						// return true;

						// let copyMedias= this.medias.map(media=>{
						// 	return {...media , thumb:''};
						// });

						// console.log(copyMedias);

						// return copyMedias;
					})
					.catch(error=> {
						Helper.BusEmitError(error);                   
						reject(error);
					})
            })
				
			},		
			setMedias(attachments){
				return new Promise((resolve, reject) => {
					attachments.forEach((attachment) => {
						let media=this.medias.find(item=>{
							return item.name==attachment.name;
						});
						if(media){
							media.path=attachment.path;
						}else{
							reject();
						}
					});
					resolve(true);

				})
				
				
				
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
			editMedia(media,index){
				this.edittingIndex=index;
				this.edittingMedia={
					title:media.title
				};
			},
			cancelEditMedia(){
				this.edittingIndex=-1;
				this.edittingMedia={
					title:''
				};
			},
			updateMedia(index){
				let media=this.medias[index];
				media.title= this.edittingMedia.title;

				this.edittingIndex=-1;
				this.edittingMedia={
					title:''
				};
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


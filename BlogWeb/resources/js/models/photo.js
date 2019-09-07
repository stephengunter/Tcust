class Photo {

	constructor(data) {
		this.original = data;

		let parts = data.previewPath.split('?');
		if(parts.length === 2){
			this.url = parts[0];
			this.path = parts[1].split('path=')[1];
		}else{
			this.url = parts[0];
			this.path = '';
		}
		
	}

	getThumbnailUrlByWidth(width){

		if(!this.path) return this.url;
		
		let ratio = width / this.original.width;
		let height = Math.round(this.original.height * ratio);
		
		return `${this.url}?type=default&width=${width}&height=${height}&path=${this.path}`;
	}

	getThumbnailUrlByHeight(height){
		if(!this.path) return this.url;
		
		let ratio = height / this.original.height;
		let width = Math.round(this.original.width * ratio);
		
		return `${this.url}?type=default&width=${width}&height=${height}&path=${this.path}`;
	}

}

export default Photo;
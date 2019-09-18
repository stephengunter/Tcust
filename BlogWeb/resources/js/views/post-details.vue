<template>
   <div v-if="model" class="row">
      <div class="col-sm-8 blog-main">
			<div class="blog-post">
				<h5 class="blog-post-title" style="font-size:1.5em">{{ model.post.title }}</h5>
				<span class="blog-post-meta">
				{{ model.post.author }}   
				<br />
					<span style="margin-right:1em">{{ date }}</span>
					<i class="fa fa-eye" aria-hidden="true"></i> {{ model.post.clickCount }}
				</span>
				<hr style="margin-top:0">
				<p v-html="model.post.content">
				
				</p>
				
			</div><!-- /.blog-post -->
      </div>
      <div  class="col-sm-1">
            
      </div>
      <div class="col-sm-3">
			<div class="sidebar-module">
				<a v-for="(item,index) in model.post.medias" :key="index" data-fancybox="gallery" 
				   :data-caption="item.title" 
					:href="item.path"  
				>
					<img :alt="item.title" :src="item.previewPath" style="width: 210px;padding-top:5px">
				</a>
			</div>
			
      </div>  

      <nav class="blog-pagination">
        <a @click.prevent="back" class="btn btn-outline-primary"  href="#">
            返回
        </a>
		  

      </nav>
		
   </div>
</template>

<script>
export default {
	name:'PostDetailsView',
	props: {
		model: {
			type: Object,
			default: null
		}
	},
	data(){
		return {
			
			imageWidth: 200,
			fancyOptions:{
				'iframe' : {
					'css' : { 'max-width' : '640px', 'max-height' : '360px' }
				}
			},
		}
	},
	computed: {
		date(){
			if(!this.model) return '';
			let model = this.model.post;
			if(!model.beginDate) return model.date;
			if(!model.endDate) return model.beginDate;
			return `${model.beginDate} ~ ${model.endDate}`;

		}
	},
	methods:{
		fetchData(id){
				
			let getData = Post.details(id);

			getData.then(model => {

				this.$emit('details-fetched', model);

			})
			.catch(error => {
				Helper.BusEmitError(error);
				
			})
		},
		back(){
			this.$emit('back');
		}
	}
}
</script>


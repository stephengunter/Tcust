<template>
    
   <div class="archives list-group">
               
      <a @click.prevent="onSelected(item)" v-for="(item,index) in items" :key="index" href="#"  :class="getClass(item)" >
         {{ item.text }} &nbsp; <span class="badge badge-pill badge-success">{{ item.count }}</span>
      </a>

   </div>
</template>


<script>
   
   export default {
      name:'Archieves',
      props: {
         items: {
            type: Array,
            default: null
         },
         year:{
            type: Number,
            default: 0
         }
      },
      beforeMount(){
			console.log(this.year);
		},
      methods:{
         getClass(item){
            let style= 'list-group-item';
            if(this.isActive(item)) style += ' active';
            
            return style;
         },
         isActive(item){
            return parseInt(item.text) == this.year;
         },
         onSelected(item){
            this.$emit('selected',item);            
         }
        
      }
   }
</script>



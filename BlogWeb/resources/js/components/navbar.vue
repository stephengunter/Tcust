<template>
   
   <nav class="navbar navbar-expand-md navbar-dark">
      <a class="navbar-brand" href="/" style="font-size:1.6em">慈濟科大校園日誌</a>
      <button class="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#menu-items" aria-controls="menu-items" aria-expanded="false" aria-label="Toggle navigation">
         <span class="navbar-toggler-icon"></span>
      </button>

      <div class="navbar-collapse collapse" id="menu-items">
         <ul class="navbar-nav mr-auto"></ul>

         <div class="form-inline my-2 my-lg-0">
            <form @submit.prevent="onSubmit" class="input-group stylish-input-group">
               <input name="keyword" type="text" v-model="keyword"  class="form-control" style="min-width:180px">
               <span class="input-group-addon">
                  <button type="submit" id="btn-search">
                     <i class="fa fa-search" aria-hidden="true"></i>
                  </button>
               </span>
            </form>
         </div>
      </div>
   </nav>
  
</template>

<script>
   export default {
      name:'Navbar',
      data(){
         return {
             keyword:''
         }
        
      },
      created() {
         Bus.$on('set-keyword',this.setKeyword);
      },
      methods:{
         setKeyword(keyword){
            this.keyword=keyword;
         },
         onSubmit(){
            Bus.$emit('search',this.keyword);
         }
      }
   }
</script>

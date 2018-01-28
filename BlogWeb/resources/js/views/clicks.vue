<template>
   <div>
      <div v-if="model" >
         <div class="row">
            <div class="col-sm-3">
               <h2>點擊率分析</h2>
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">
					<drop-down :items="periods" :selected="period.value"
					  @selected="onPeriodSelected">
					</drop-down>
            </div>
            
            <div class="col-sm-3" style="margin-top: 20px;">
               
            </div>
            <div class="col-sm-3" style="margin-top: 20px;">

               
            </div>
         </div>

         <hr/>

         <post-table :clicks="true" :model="model" :desc="desc" :can_edit="can_edit" @sort="onSort">
          
			   <div v-show="model.totalItems>0" slot="table-footer" class="panel-footer pagination-footer">
					<page-controll   :model="model" @page-changed="onPageChanged"
						@pagesize-changed="fetchData">
					</page-controll>
            
            </div>
         </post-table>

      </div>
      
   </div> 
</template>


<script>
   import PostTable from '../components/post-table';
   export default {
      name:'ClicksAdminView',
      components: {
         'post-table':PostTable,
      },
      props: {
         init_model: {
            type: Object,
            default: null
			},
			periods:{
				type:Array,
				default:null
			}
      },
      data(){
         return {
				model:null,
            can_edit:false,
            
				params:{
               period:'',
					sort:'desc',
					page:1,
					pageSize:10
				},

            period:null,
         }
      },
      computed: {
         desc(){
            return this.params.sort=='desc';
         }
      },
      beforeMount() {
         if(this.init_model){
				this.model={...this.init_model };
				this.params.page=this.init_model.pageNumber;
				this.params.pageSize=this.init_model.pageSize;
         }	
         
         if(this.periods){
				this.setPeriod(this.periods[0]);
				
			}	

		},
      methods:{
         onPeriodSelected(period){
				this.setPeriod(period);
				this.fetchData();
			},
			setPeriod(period){
				this.period=period;
				this.params.period=period.value;
			},
         onSort(){
            
            if(this.desc) this.params.sort='asc';
            else  this.params.sort='desc';

            this.fetchData();
         },
			onPageChanged(page){
				this.params.page=page;
				this.fetchData();
				
			},
		
         fetchData() {
				
            let getData = Clicks.index(this.params);

            getData.then(model => {

               this.model={ ...model };

            })
            .catch(error => {
               Helper.BusEmitError(error);
               
            })
         },
         
      }
   }
</script>






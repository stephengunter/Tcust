<template>
    <div>
        <div v-if="model" >
            <div class="row">
                <div class="col-sm-3">
                    <h2>發稿統計</h2>
                </div>
                <div class="col-sm-6 form-inline" style="margin-top: 20px;">
                     <div class="form-group" style="padding-left:1em;">
                        <drop-down :items="terms" :selected="params.term"
                                @selected="onTermSelected">
                            <span slot="label" >
                                    學期
                            
                            </span>
                        </drop-down>
                    </div>
                    <div class="form-group" style="padding-left:1em;">
                        <toggle :items="typeOptions"   :default_val="params.type" @selected="setType"></toggle>
                        
                    </div>
                </div>
                
               
                <div class="col-sm-3" style="margin-top: 20px;">

                
                </div>
            </div>

            <hr/>

            <department-table :model="model"  :can_edit="can_edit" :version="version" 
             :type="params.type" :monthes="monthes"
              @cancel-edit="cancelEdit"  @submit-targets="submitTargets" >
                
            </department-table>

        </div>
        
    </div> 
</template>


<script>
    import departmentTable from '../components/department-table';
    export default {
        name:'DepartmentsAdminView',
        components: {
            'department-table':departmentTable,
        },
        props: {
            init_model: {
                type: Object,
                default: null
            },
            terms: {
                type: Array,
                default: null
            },
                
        },
        data(){
            return {
                   
                model:null,
                can_edit:false,

                typeOptions:[
                   
                    { value:0 , text:'執行率總計'},
                    { value:1 , text:'按月份統計'}
                ],
                    
                monthes:[],
                
                params:{
                    term:'',
                    type:0
                },

                version:0,

                
            }
        },
        computed: {
            
        },
        beforeMount() {
            if(this.init_model){
               this.model={...this.init_model };
            }	

            this.params.term=this.terms[0].value;

        },
        methods:{
            onTermSelected(item){
				
				this.params.term=item.value;
                this.fetchData();
                
			},
            getList(){
				if(this.model) return this.model.viewList;
				return [];
			},
            getTargetEditForms(){
				return new Promise((resolve, reject) => {
					let list=this.getList();
					let models=[];
					list.forEach((item) => {
						if(isNaN(item.newTarget)){

						}else{
							models.push({ id:item.id, target:item.newTarget});
						} 
						
						
					});
					resolve(models);

				})
            },
            setType(val){
                this.params.type=val;
                this.fetchData();
            },
            cancelEdit(){
                let list=this.getList();
					
                list.forEach((item) => {
                    item.newTarget=item.target;
                    
                });

                this.version+=1;
            },   
            submitTargets(){
                let getEditForms=this.getTargetEditForms();
				
				getEditForms.then((items)=>{
					
                    if(!items.length) return;

					let save= Department.store(items);
					
					save.then(() => {
						Helper.BusEmitOK('資料已存檔');	
						this.fetchData();
					
					})
					.catch(error => {
						Helper.BusEmitError(error,'存檔失敗');
					})


				});
            },
            fetchData() {
                 
                let getData = Department.index(this.params);

                getData.then(model => {
                    this.version+=1;
                  

                    if(this.params.type==1){
                       
                        this.monthes=model.monthes.slice(0);
                        this.model={ ...model.monthlyViewList };
                    }else{
                       
                        this.model={ ...model.targetViewList };
                    }

                })
                .catch(error => {
                    Helper.BusEmitError(error);
                
                })
            },
            
        }
    }
</script>






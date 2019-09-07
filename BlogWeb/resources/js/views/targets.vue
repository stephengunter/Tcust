<template>
    <div>
        <div v-if="model" >
            <div class="row">
                <div class="col-sm-3">
                    <h2>發稿統計</h2>
                </div>
                <div class="col-sm-3" style="margin-top: 20px;">
                    <year-term-filter @changed="onYearTermChanged"></year-term-filter>
                </div>
                
                <div class="col-sm-3" style="margin-top: 20px;">
                
                </div>
                <div class="col-sm-3" style="margin-top: 20px;">

                
                </div>
            </div>

            <hr/>

            <target-table :model="model"  :can_edit="can_edit" >
                
            </target-table>

        </div>
        
    </div> 
</template>


<script>
    import TargetTable from '../components/target-table';
    export default {
        name:'DepartmentsAdminView',
        components: {
            'target-table':TargetTable,
        },
        props: {
            init_model: {
                type: Object,
                default: null
            },
                
        },
        data(){
            return {
                   
                model:null,
                can_edit:false,
                
                params:{
                    term:'',
                },

                
            }
        },
        computed: {
            
        },
        beforeMount() {
            if(this.init_model){
               this.model={...this.init_model };
            }	

        },
        methods:{
           
            
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






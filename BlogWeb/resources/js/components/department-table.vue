<template>
    <div class="panel panel-default">
        <div class="panel-body">
            <table v-if="type==0" class="table table-striped">
                <thead>
                    <tr>
                        <th style="width:20%">部門</th>
                        <th v-if="editting"  style="width:15%">目標件數
                            <button @click.prevent="submitTargets"  class="btn btn-xs btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>
                            </button>
                            <button @click.prevent="cancelEdit" class="btn btn-xs btn-default">
                                <i class="fa fa-undo" aria-hidden="true"></i>
                            </button>
                            
                        </th> 
                        <th v-else style="width:15%">目標件數
                           
                            <button @click.prevent="editting=true" class="btn btn-xs btn-primary">
                                <i class="fa fa-edit" aria-hidden="true"></i>
                            </button>
                            
                        </th>  
                        <th>
                           文章總數
                        </th>
                        <th>
                           代撰件數
                        </th>
                        <th>
                           實際件數
                        </th>
                        
                        <th>
                           執行率
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(target,index) in model.viewList" :key="index">
                        <td>{{ target.departmentName }}</td>

                        <td v-if="editting">
                            <input class="form-control" type="text" v-model="target.newTarget">	
                        </td>
                        <td v-else>{{ target.target }}</td>

                        <td>{{ target.total }}</td>
                        <td>{{ target.sub }}</td>
                        <td v-text="getActual(target)"></td>
                        <td v-text="getRate(target)"></td>
                    </tr>
                </tbody>

            </table>
            <table v-else class="table table-striped" style="table-layout:fixed">
                <thead v-if="monthes">
                    <th></th>
                    <th v-for="(month,index) in monthes" :key="index">
                        {{ month }} 月
                    </th>
                </thead>
                <tbody v-if="hasData">
                    <tr v-for="(department,departmentIndex) in model.viewList" :key="departmentIndex">
                        <td>{{ department.departmentName }}</td>
                        <td v-for="(month,monthIndex) in monthes" :key="monthIndex">
                            <span v-if="department.monthPostCounts">
                                 {{ department.monthPostCounts[monthIndex] }}
                            </span>
                           
                        </td>
                        
                    </tr>
                    
                    <tr>
                        <th>合計</th>
                        
                        <th v-for="(total,totalIndex) in totalList" :key="totalIndex">
                           {{ total }}
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
        <slot name="table-footer"> 
        
        </slot> 
            
    </div>
</template>

<script>
export default {
    name:'TargetTable',
    props: {
        model: {
            type: Object,
            default: null
        },
        type:{
            type: Number,
            default: 0
        },
        monthes:{
            type: Array,
            default: null
        },
        can_edit:{
            type: Boolean,
            default: true
        },
        version:{
            type: Number,
            default: 0
        }
            
    },
    data() {
        return {
            editting:false
        };
    },
    computed:{
        hasData(){
            if(!this.model) return false;
            if(!this.model.viewList) return false;
            return this.model.viewList.length > 0;
            
        },
        totalList(){
            let result=[];
            if(!this.type) return result;
            if(!this.hasData) return result;
            if(!this.monthes) return result;

            for(let i=0; i<this.monthes.length ; i++){
                result.push(this.getTotal(i));
            }

            return result;

           
        }
            
    }, 
    watch: {
		version() {
			this.editting=false;
		}
	},
    methods:{
        
        getActual(target){
            return target.total - target.sub;
        },
        getRate(target){
            if(!target.target) return '';
            let rate= this.getActual(target) * 100 / target.target ;
            return Math.floor(rate) + ' %';
        },
        getTotal(index){
            let total=0;
            
            this.model.viewList.forEach(item=>{
                if(item.monthPostCounts){
                    total+=item.monthPostCounts[index];
                }
            });

            return total;
        },
        cancelEdit(){
            this.$emit('cancel-edit');
        },
        submitTargets(){
            this.$emit('submit-targets');
        },
        
    }
}
</script>


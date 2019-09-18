import './bootstrap';



Vue.component('alert', require('./packages/components/Alert'));
Vue.component('page-controll', require('./components/page-controll'));
Vue.component('modal', require('./packages/components/Modal'));
Vue.component('toggle', require('./packages/components/Toggle'));
Vue.component('delete-confirm', require('./packages/components/DeleteConfirm'));
Vue.component('drop-down', require('./packages/components/DropDown'));
Vue.component('html-editor', require('./packages/components/HtmlEditor'));
Vue.component('check-box', require('./packages/components/Checkbox'));
Vue.component('check-box-list', require('./packages/components/CheckboxList'));
Vue.component('tree-item', require('./packages/components/TreeItem'));
Vue.component('tree-view', require('./packages/components/TreeView'));


Vue.component('terms-admin', require('./views/terms-admin'));
Vue.component('years-admin', require('./views/years-admin'));
Vue.component('departments-admin', require('./views/departments-admin'));
Vue.component('manage', require('./views/manage'));

import ErrorHandler from './utilities/error-handler';

new Vue({
    el: '#footer',
    data() {
        return {
            
            showAlert: false,
            alertSetting: {
                type: 'success',
                title: '資料儲存成功',
                text: '',
                dismissable: false,
                duration: 2500,
                class: 'fa fa-check-circle-o'
            },

            
        }
    },
    created() {
        Bus.$on('errors',this.onErrors);
        Bus.$on('okmsg',this.showSuccessMsg);
     
    },
    methods: {
        closeAlert() {
            this.showAlert = false;
        },
        setAlertText(msg) {
            let title = msg.title ? msg.title : '處理您的要求時發生錯誤'
            let text = msg.text
            this.alertSetting.title = title
            this.alertSetting.text = text
        },
        // Bus Event Handlers
        onErrors(error,msg){ 
            

            if(msg){
                this.showErrorMsg({
                    title:msg
                });
            }else{
                let errorHandler=new ErrorHandler(error);
                msg =errorHandler.handle();

                if(msg) this.showErrorMsg(msg);
            }   
            
        },
        showErrorMsg(msg) {
            
            this.setAlertText(msg);
            this.alertSetting.class = 'fa fa-exclamation-circle'
            this.alertSetting.type = 'danger'

            this.showAlert = true;
            this.showModal = false;
        },
        showSuccessMsg(msg) {
            this.setAlertText(msg);
            this.alertSetting.class = 'fa fa-check-circle-o'
            this.alertSetting.type = 'success'

            this.showAlert = true;
            this.showModal = false;
        },

        
        

    },
});
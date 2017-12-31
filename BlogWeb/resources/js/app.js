import './bootstrap';


const VueUploadComponent = require('vue-upload-component');
Vue.component('file-upload', VueUploadComponent);


Vue.component('navbar', require('./components/navbar'));
Vue.component('post-index', require('./views/post-index'));



Vue.component('post-admin', require('./views/post-admin'));



Vue.component('test', require('./views/test'));
new Vue({
    el: '#header',
});
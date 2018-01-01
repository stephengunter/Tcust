import './bootstrap';


Vue.component('datetime-picker', require('./components/datetime-picker'));

Vue.component('navbar', require('./components/navbar'));
Vue.component('post-index', require('./views/post-index'));



Vue.component('post-admin', require('./views/post-admin'));



Vue.component('test', require('./views/test'));
new Vue({
    el: '#header',
});
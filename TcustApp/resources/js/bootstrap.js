import Vue from 'vue';
window.Vue = Vue;

window._ = require('lodash');

window.axios = require('axios');
window.axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

window.moment = require('moment');

import infiniteScroll from 'vue-infinite-scroll';
Vue.use(infiniteScroll);


try {
    window.$ = window.jQuery = require('jquery');
    require('bootstrap-sass');
} catch (e) {}



require('./packages/fancybox/dist/jquery.fancybox.css');
require('./packages/fancybox/dist/jquery.fancybox.js');
/**
 * Next we will register the CSRF Token as a common header with Axios so that
 * all outgoing HTTP requests automatically have it attached. This is just
 * a simple convenience so we don't have to attach every token manually.
 */

let token = document.head.querySelector('meta[name="csrf-token"]');

if (token) {
    window.axios.defaults.headers.common['X-CSRF-TOKEN'] = token.content;
} else {
    //console.error('CSRF token not found: https://laravel.com/docs/csrf#csrf-x-csrf-token');
}

require('../libs/slick/slick.css');


import Helper from './helper';
window.Helper = Helper;

import Form from './utilities/form';
window.Form = Form;


import Api from './models/api';
window.Api = Api;

import Post from './models/post';
import Photo from './models/photo';

window.Post =Post;
window.Photo =Photo;


window.Bus = new Vue({});
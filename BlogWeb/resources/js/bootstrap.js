import Vue from 'vue';
window.Vue = Vue;

window._ = require('lodash');

window.axios = require('axios');
window.axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';



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

require('../css/blog.css');
require('../css/site.css');

require('./packages/fancybox/dist/jquery.fancybox.css');
require('./packages/fancybox/dist/jquery.fancybox.js');

window.Clipboard=require('clipboard');


import Auth from './packages/auth.js'
Vue.use(Auth)


import Helper from './helper';
window.Helper = Helper;

import Form from './utilities/form';
window.Form = Form;

import Api from './models/api';
import Manage from './models/manage';
import Post from './models/post';
import PostAdmin from './models/postAdmin';
import TopAdmin from './models/topAdmin';
import PostReview from './models/review';
import Clicks from './models/clicks';
import Attachment from './models/attachment';

window.Manage=Manage;
window.Api = Api;
window.Post = Post;
window.TopAdmin = TopAdmin;
window.PostAdmin = PostAdmin;
window.PostReview=PostReview;
window.Clicks = Clicks;
window.Attachment = Attachment;


window.Bus = new Vue({});
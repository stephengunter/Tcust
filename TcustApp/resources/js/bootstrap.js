import Vue from 'vue';
window.Vue = Vue;

window._ = require('lodash');

window.axios = require('axios');
window.axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

window.moment = require('moment');




let token = document.head.querySelector('meta[name="csrf-token"]');

if (token) {
    window.axios.defaults.headers.common['X-CSRF-TOKEN'] = token.content;
} else {
    //console.error('CSRF token not found: https://laravel.com/docs/csrf#csrf-x-csrf-token');
}


import Helper from './helper';
window.Helper = Helper;

import Form from './utilities/form';
window.Form = Form;

import YearAdmin from './models/yearAdmin';
import TermAdmin from './models/termAdmin';
window.YearAdmin = YearAdmin;
window.TermAdmin = TermAdmin;




window.Bus = new Vue({});
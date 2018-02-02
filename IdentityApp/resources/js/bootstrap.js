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






import Vue from 'vue';
window.Vue = Vue;


import Helper from './helper';
window.Helper = Helper;

import Form from './utilities/Form';
window.Form = Form;

import ClientAdmin from './models/clientAdmin';
import UserAdmin from './models/userAdmin';

window.ClientAdmin = ClientAdmin;
window.UserAdmin = UserAdmin;


window.Bus = new Vue({});
import Vue from 'vue';

import LoginForm from '../../areas/security/auth/login-form.vue';

import { getRequestVerificationToken } from './layout.entry';


const v = new Vue({
    el: '#_index',
    data: {
        selectedDiseaseId: 7    //TODO : Get the selectedDiseaseId from url
    },
    components: {
        LoginForm
    },
    methods: {
        onDiseaseSelected: function (disease) {
            //console.log(disease);
            this.selectedDiseaseId = disease.id;

            //eh.onDiseaseSelected(disease.id);
        }
    },
    created: function () {

        //console.log(getRequestVerificationToken);
        console.log("-> whats _x : ");
        console.log(getRequestVerificationToken());

        console.log("-> vue created");
    }
});
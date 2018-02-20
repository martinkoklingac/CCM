
Vue.component("user-form", {
    template: "#_userForm",
    data: function () {
        return {
            model: {
                username: null,
                email: null
            }
        };
    },
    methods: {
        onNext: function () {
            console.log("-> clicked");
        }
    },
    created: function () {
        console.log("-> created x");
    }
});

Vue.component("password-form", {
    template: "#_passwordForm",
    data: function () {
        return {
            model: {
                password: null
            }
        };
    },
    methods: {
        onNext: function () {
            console.log("-> next");
        }
    }
});

let v = new Vue({
    el: "#_app",
    data: {
    }
});
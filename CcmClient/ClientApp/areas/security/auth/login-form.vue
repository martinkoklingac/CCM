<template>
    <div class="login-form">
        <div>
            <label>User Name</label>
            <input v-model="userName" />
        </div>
        <div>
            <label>Password</label>
            <input v-model="password" type="password" />
        </div>
        <div>
            <a v-on:click.prevent="onLogin">Login</a>
        </div>
    </div>
</template>
<script>
    import $ from 'jquery';
    import { getRequestVerificationToken } from '../../../root/entrypoints/layout.entry';

    export default {
        data: function () {
            return {
                userName: null,
                password: null
            };
        },
        methods: {
            onLogin: function () {
                console.log("-> login");
                
                let data = {
                    userName: this.userName,
                    password: this.password
                };

                //console.log("-> token");
                //console.log(getRequestVerificationToken());

                Object.assign(data, getRequestVerificationToken());

                console.log(data);

                $.post("/security/auth/login", data).done(function (d) {
                    console.log("-> done");
                    console.log(d);
                });
            }
        },
        created: function () {
            console.log("-> login-form created");
        }
    }
</script>
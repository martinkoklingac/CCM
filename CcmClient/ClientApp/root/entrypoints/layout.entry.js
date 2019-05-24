import $ from 'jquery';


var _token = null;

let getRequestVerificationToken = () => {
    if (!_token) {
        console.log("-> creating token");

        let name = '__RequestVerificationToken';
        let value = $("input[name='__RequestVerificationToken']").val();

        _token = {};
        _token[name] = value;
    }

    console.log("-> toekn ready");

    return _token;
};


export { getRequestVerificationToken };
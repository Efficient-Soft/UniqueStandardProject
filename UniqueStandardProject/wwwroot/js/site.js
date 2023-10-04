function notification(mode, title, description) {
    Command: toastr[mode](description, title);
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": 2500,
        "extendedTimeOut": 500,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
};

function addNotiStorage(notification) {
    console.log(notification);
}

function cryptoEncrypt(inputStr) {
    var key = CryptoJS.enc.Utf8.parse('8056483646328763');
    var iv = CryptoJS.enc.Utf8.parse('8056483646328763');
    var encrypted_utf = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(inputStr), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }).toString();
    return encrypted_utf;
}

function cryptoDecrypt(inputBytes) {
    var key = CryptoJS.enc.Utf8.parse('8056483646328763');
    var iv = CryptoJS.enc.Utf8.parse('8056483646328763');
    var decrypt_utf = CryptoJS.AES.decrypt(inputBytes, key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
    return decrypt_utf.toString(CryptoJS.enc.Utf8);
}
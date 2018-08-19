define(['jquery', 'tool/jsaes'], function ($, jseaes) {
    return {
        escape: function (html) {
            var key = CryptoJS.enc.Utf8.parse("123456789easemob");
            var iv = CryptoJS.enc.Utf8.parse('ABCDEF1234123412');
            var srcs = CryptoJS.enc.Utf8.parse(html);
            var encrypted = CryptoJS.AES.encrypt(srcs, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
            return encrypted.ciphertext.toString().toUpperCase();
        },
        enescape: function (html) {
            var key = CryptoJS.enc.Utf8.parse("123456789easemob");
            var iv = CryptoJS.enc.Utf8.parse('ABCDEF1234123412'); 
            var encryptedHexStr = CryptoJS.enc.Hex.parse(html);
            var srcs = CryptoJS.enc.Base64.stringify(encryptedHexStr);
            var decrypt = CryptoJS.AES.decrypt(srcs, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
            var decryptedStr = decrypt.toString(CryptoJS.enc.Utf8);
            return decryptedStr.toString();
        }
    }
})
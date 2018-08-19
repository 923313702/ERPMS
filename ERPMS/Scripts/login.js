$(function () {
    $('#ExampleCaptcha_CaptchaImageDiv a').removeAttr("style").empty();
    //$('#ExampleCaptcha_ReloadLink').hide();
    $('#ExampleCaptcha_CaptchaImage').click(function () {
        //$("#ExampleCaptcha_ReloadLink").trigger("click");
        $(this).attr("src", $(this).attr("src") + "&d=" + new Date().getTime())
    }); 
    $("#myForm").validate({
        onkeyup:false,
        rules: {
            Account: {
                required: true,
                rangelength: [4, 10],
                checkAccount: true
            },
            Password: {
                required: true,
                rangelength: [6, 10],
                checkPassword: true
            },
            //CaptchaCode: {
            //    required: true,
            //    remote: {
            //        type: 'POST',
            //        url: '/Home/CheckCaptchaCode',
            //        data: {
            //            BDC_VCID_ExampleCaptcha: $('#BDC_VCID_ExampleCaptcha').val()
            //        }
            //    }
            //},
        },
        messages: {
            Account: {
                required: "请输入用户名",
                rangelength: "账号长度为5到10位！"
            },
            Password: {
                required: "请输入密码",
                rangelength: "密码长度为6到20位！"
            },
            //CaptchaCode: {
            //    required: '请输入验证码',
            //    remote: '验证码错误'
            //}
        },
        errorPlacement: function (error, element) {
            $('#myerror').empty();
            console.log(error);
            error.appendTo($('#myerror'));
        }, 
        //提交表单后，（第一个）未通过验证的表单获得焦点  
        focusInvalid: true,
        //当未通过验证的元素获得焦点时，移除错误提示  
        focusCleanup: true,
        errorClass: "addErrorClass",
        success: function (label) {
            label.html('');
        },
        submitHandler: function (form) {//通过后回调
            var data = $('#myForm').serialize();
            $.post('/Home/Login', data, function (response) {
                if (response.success == 0) {
                    location.href = response.msg;
                } else {
                    $('#myerror').empty().html(response.msg);
                   // $("#myForm").validate().resetForm();
                    $("#myForm")[0].reset();
                    var captcha=$('#ExampleCaptcha_CaptchaImage')
                    captcha.attr("src", captcha.attr("src") + "&d=" + new Date().getTime())
                }
            })
        }
    });
    $.validator.addMethod('checkAccount', function (value, element) {
        var reg = /^[a-zA-Z0-9_\.]+$/;
        return this.optional(element) || (reg.test(value));
    }, '账号只能是数字和字母_.');
    $.validator.addMethod('checkPassword', function (value, element) {
        var reg = /^[a-zA-Z0-9_\.]+$/;
        return this.optional(element) || (reg.test(value));
    }, '密码只能是数字和字母_.');

});


//function checkAccount() {
//    var msg = $('#myerror');
//    msg.empty();
//    var account = $('#Account').val();
//    var reg = /^[a-zA-Z0-9_\.]+$/;
//    if (account == null || account == '') {
//        msg.html('账号没能为空');
//        return false;
//    }
//    if (!reg.test(account)) {
//        msg.html('账号只能是数字和字母_.');
//        return false;
//    }
//    if (account.length < 4 || account > 20) {
//        msg.html('用户名长度4-20之间');
//        return false;
//    }
//    return true;
//}


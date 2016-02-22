var isposting = false;

var successfn = function (data) {
    var logininfo = $("#loginInfor");
    if (data == "Success") {
        document.getElementById('form1').submit();
    }
    else {
        smp.show("用户名已被占用！", "warn");
    }
}
var errorfn = function (data) {
    smp.show("服务器错误，请联系管理员！", "warn");
}

var beforesend = function () {
    isposting = true;
    var submitbutton = $("#loginButton");
    var submitInterval = 2;

    var timer = setInterval(function () {
        submitInterval--;
        $("#loginButton").text("登录(" + submitInterval + ")")
        if (submitInterval == 0) {
            $("#loginButton").text("登录");
            isposting = false;
            clearInterval(timer);
        }
    }, 1000);
}
var complete = function () {
}

function doSubmit() {
    if (!isposting) {
        if (simple.validator.validate() == true) {
            var username = $("#UserName").val();
            var data = { 'username': username };
            core.AJAX(data, "/Admin/User/CheckUser", beforesend, successfn, errorfn, complete);
        }
    };
}

if (typeof (core) == "undefined") {
    core = {};
}

//提交POST Ajax请求，需要引用jQuery库
//data：Json格式数据
//url：请求地址
//success：成功回调函数
//error:失败回调函数
core.AJAX = function (data, url, beforesendfn, onsuccessfn, onerrorfn, oncomplete, functiontype) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: beforesendfn,
        success: onsuccessfn,
        error: onerrorfn,
        complete: oncomplete
    });
}

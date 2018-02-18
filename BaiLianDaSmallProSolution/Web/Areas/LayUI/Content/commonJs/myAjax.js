layui.define(['jquery'], function (exports) {
    var $ = layui.jquery;


    var currentObj = {
        'test': function () {
            layer.msg('hello world');
        },
        //同步请求
        'postSyncReq': function (url,postData,successFn) {
            $.ajax({
                url: url,
                type: "POST",
                data: postData,
                dataType: 'JSON',
                async: false,
                success: function (res) {
                    if (successFn) {
                        successFn(res);
                    }
                },
                error: function (xhr, status, error) {
                    layer.msg('网络出现异常：原因为：' + JSON.stringify(xhr));
                }
            });
        },
        'dealResult': function (data,successFn,errorFn) {
            if (data.IsSuccess) {
                if (successFn) {
                    successFn();
                } else {
                    layer.msg(data.ReturnMsg);
                }
            } else {
                if (errorFn) {
                    errorFn();
                } else {
                    layer.msg(data.ReturnMsg);
                }
            }
        }
    };

    exports('myAjax', currentObj);
});
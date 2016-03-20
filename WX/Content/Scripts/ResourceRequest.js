$(function () {
    /*资源帮帮，提交资源请求*/
    $("#btn_submit").click(function () {
        var request_content = $("#request").val();
        var request_contact = $("#contact").val();
        var request_code = $("#code").val();
        if (request_content.replace(/\s/g, '') == "")
            return false;
        if (request_code.replace(/\s/g, '') == "") {
            $("#code").css("border", "1px solid Red");
            return false;
        }
        if (request_contact.replace(/\s/g, '') == "") {
            if (confirm("确定不填写联系方式吗？")) {
                $.post("/request/add",
                    {
                        "content": request_content,
                        "contact": request_contact,
                        "code": request_code
                    },
                    function (data) {
                        if (data == 0) {
                            alert('抱歉，提交请求失败，请稍后再试。');
                        } else if (data == 1) {
                            alert('提交请求成功，请耐心等待回复。');
                            $("#request").val('');
                            $("#contact").val('');
                            $("#code").val('');
                            $("#code").css("border", "0px solid Red");
                        } else {
                            alert(data);
                        }
                    })
            }
        } else {
            $.post("/request/add",
                    {
                        "content": request_content,
                        "contact": request_contact,
                        "code": request_code
                    },
                    function (data) {
                        if (data == 0) {
                            alert('抱歉，提交请求失败，请稍后再试。');
                        } else if (data == 1) {
                            alert('提交请求成功，请耐心等待回复。');
                            $("#request").val('');
                            $("#contact").val('');
                            $("#code").val('');
                            $("#code").css("border", "0px solid Red");
                        } else {
                            alert(data);
                        }
                    })
        }

    });

    /*资源帮帮，未响应信息*/
    $("#my_request").click(function () {
        alert('功能开发中，请稍作等待……');
    });
    $("#hot_request").click(function () {
        alert('功能开发中，请稍作等待……');
    });
    $("#fresh_request").click(function () {
        alert('功能开发中，请稍作等待……');
    });
});

var Helper = {
    ShowErrorDialog: function (message) {
        var dialog = $("#Dialog");
        dialog.find(".modal-title").text("错误");
        dialog.find(".modal-body").empty();
        dialog.find(".modal-body").append(message);
        dialog.modal({
            keyboard: false,
            show: true
        });
    },
    ShowSuccessDialog: function (message) {
        var dialog = $("#Dialog");
        dialog.find(".modal-title").text("成功");
        dialog.find(".modal-body").empty();
        dialog.find(".modal-body").append(message);
        dialog.modal({
            keyboard: false,
            show: true
        });
    },
    ShowConfirmationDialog: function (parm) {
        var dialog = $("#Confirmation");
        dialog.find(".modal-title").text("提示");
        dialog.find(".modal-body").empty();
        dialog.find(".modal-body").append(parm.message);
        dialog.modal("show");
        callback = parm.confirmFunction;
    },
    getGuid:function() {
        function s4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }
        return (s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4());
    }
};
var callback;
var Messages = {
    UploadFileError: "文件类型错误，仅支持xls、xlsx",
    Success: "操作成功",
};
$(function () {
    var dialog = $("#Confirmation");
    var confirmbtn = dialog.find(".btn-primary");
    confirmbtn.click(function () {
        dialog.modal("hide");
        if (callback != null) {
            callback();
        }
    });
});

var Reconciliation = {
    viewModel: {
        fileId: ko.observable(),
        models: ko.observableArray()
    }
};
Reconciliation.viewModel.Next = function() {
    Reconciliation.viewModel.fileId(Helper.GetQueryString("fileId"));
    $.get("/api/Reconciliation/" + Reconciliation.viewModel.fileId(), function(result) {
        $("#uploadresult").hide();
        ko.mapping.fromJS(result, {}, Reconciliation.viewModel.models);
    });
};
ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || "YYYY/MM/DD";

        var output = "-";
        if (valueUnwrapped !== null && valueUnwrapped !== undefined && valueUnwrapped.length > 0) {
            output = moment(valueUnwrapped).format(pattern);
        }

        if ($(element).is("input") === true) {
            $(element).val(output);
        } else {
            $(element).text(output);
        }
    }
};
function validation() {
    var arr = [];
    if ($("#filePath").val() === "") {
        arr.push("未选择上传的文件");
    }
    if ($("#DistributorId").val() === "") {
        arr.push("未选择发货经销商");
    }
    return arr;
};
$(function () {
    ko.applyBindings(Reconciliation);
    $("#filePath").val(null);
    $("#selectfile").click(function () {
        $("#uploadfile").click();
    });
    $("#uploadfile").change(function () {
        var file = $("#uploadfile").val();
        var fileExtend = file.substring(file.lastIndexOf(".")).toLowerCase();
        if (fileExtend === ".xls" || fileExtend === ".xlsx") {
            $("#filePath").val(file);
        } else {
            Helper.ShowErrorDialog(Messages.UploadFileError);
        }
    });
    //提交表单
    $("#uploadForm").submit(function (e) {
        var arr = validation();
        if (arr.length !== 0) {
            e.preventDefault();
            var str = "";
            for (var i = 0; i < arr.length; i++) {
                str += "<p>" + arr[i].toString() + "</p>";
            }
            Helper.ShowErrorDialog(str);
        }
    });
})
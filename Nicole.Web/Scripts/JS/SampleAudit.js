var SampleAudit = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        SampleModel: {
            Id: ko.observable(),
            IsApproved: ko.observable(),
            Code: ko.observable(),
            PositionModel: {
                Id: ko.observable(),
                Name: ko.observable(),
                CurrentEmployeeModel: {
                    Name: ko.observable()
                }

            },
            CustomerModel: {
                Id: ko.observable(),
                Name: ko.observable(),
                Code: ko.observable()
            },
            ProductModel: {
                Id: ko.observable(),
                PartNumber: ko.observable()
            },
            CurrentSampleReview: {
                Id: ko.observable(),
                ReturnComments: ko.observable()
            },
            Qty: ko.observable(),
            Remark: ko.observable(),
            CreatedTime: ko.observable()
        },
        SearchSampleModel: {
            //客户编号
            CustomerCode: ko.observable(),
            //客户编号名称
            CustomerName: ko.observable(),
            //料号
            PartNumber: ko.observable(),
            PositionId: ko.observable(),
            //是否通过审核
            IsApproved: ko.observable(),
            //申请编号
            Code: ko.observable()
        }
    }
};
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = {
            locale: "zh-cn",
            format: "YYYY年MM月DD日"
        };
        $(element).datetimepicker(options).on("dp.change", function (ev) {
            var observable = valueAccessor();
            observable(ev.date.toJSON());
        });
    }
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

SampleAudit.viewModel.UpdatePagination = function () {
    var allPage = SampleAudit.viewModel.Page.AllPage() === 0 ? 1 : SampleAudit.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: SampleAudit.viewModel.Page.CurrentPageIndex() });
};
SampleAudit.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(SampleAudit.viewModel.SearchSampleModel);
    model.pageIndex = SampleAudit.viewModel.Page.CurrentPageIndex();
    $.get("/api/SampleReview", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleAudit.viewModel.Page);
        SampleAudit.viewModel.UpdatePagination();
    });
};
SampleAudit.viewModel.ShowSearch = function () {
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
}
//通过审核
SampleAudit.viewModel.Approve = function() {
    var model = ko.mapping.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认通过?",
        confirmFunction: function () {
            $.ajax({
                type: "post",
                url: "/api/SampleReview",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(model.CurrentSampleReview),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        SampleAudit.viewModel.GotoPage();
                    }
                }
            });
        }
    });
};
//退回
SampleAudit.viewModel.Return = function () {
    var item = ko.mapping.toJS(this);
    var model = {
        key: {
            Id: item.Id
        }
    }
    $.get("/api/Sample",model, function (result) {
        ko.mapping.fromJS(result.Models[0], {}, SampleAudit.viewModel.SampleModel);
        $("#returndialog").modal({
            show: true,
            backdrop: "static"
        });
    });
}
//确认退回
SampleAudit.viewModel.ReturnSave = function () {
    var model = ko.mapping.toJS(SampleAudit.viewModel.SampleModel);
    Helper.ShowConfirmationDialog({
        message: "是否确认退回?",
        confirmFunction: function () {
            $.ajax({
                type: "put",
                url: "/api/SampleReview",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(model.CurrentSampleReview),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        $("#returndialog").modal("hide");
                        SampleAudit.viewModel.GotoPage();
                    }
                }
            });
        }
    });
}
//确定搜索
SampleAudit.viewModel.Search = function () {
    SampleAudit.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(SampleAudit.viewModel.SearchSampleModel);
    $.get("/api/SampleReview", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleAudit.viewModel.Page);
        SampleAudit.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//编辑申请
SampleAudit.viewModel.Edit = function() {
    var model = ko.mapping.toJS(this);
    $.get("/api/Sample/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, SampleAudit.viewModel.SampleModel);
        $("#editdialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
//保存编辑并审核通过
SampleAudit.viewModel.EditSave = function () {
    var model = ko.mapping.toJS(SampleAudit.viewModel.SampleModel);
    $.ajax({
        type: "put",
        url: "/api/Sample?Id=" + model.Id,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $("#editdialog").modal("hide");
                SampleAudit.viewModel.GotoPage();
            }
        }
    });
};
$(function () {
    ko.applyBindings(SampleAudit);
    SampleAudit.viewModel.Search();
    //初始化页码
    $("#page-selection").bootpag({
        total: 1,
        page: 1,
        maxVisible: 5,
        leaps: true,
        firstLastUse: true,
        first: "First",
        last: "Last",
        wrapClass: "pagination",
        activeClass: "active",
        disabledClass: "disabled",
        nextClass: "next",
        prevClass: "prev",
        lastClass: "last",
        firstClass: "first"
    }).on("page", function (event, num) {
        if (num != null) {
            SampleAudit.viewModel.Page.CurrentPageIndex(num);
            SampleAudit.viewModel.GotoPage();
        }
    });
});
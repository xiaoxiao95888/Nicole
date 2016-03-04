var SampleApply = {
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
                Name: ko.observable()
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
        },
        CustomerModels: ko.observableArray(),
        ProductModels: ko.observableArray()
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
SampleApply.viewModel.Reason = function() {
    var model = ko.mapping.toJS(this);
    $.get("/api/Sample/" + model.Id, function (result) {
        Helper.ShowMessageDialog(result.CurrentSampleReview.ReturnComments, result.Code);
    });
};
SampleApply.viewModel.UpdatePagination = function () {
    var allPage = SampleApply.viewModel.Page.AllPage() === 0 ? 1 : SampleApply.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: SampleApply.viewModel.Page.CurrentPageIndex() });
};
SampleApply.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(SampleApply.viewModel.SearchSampleModel);
    model.pageIndex = SampleApply.viewModel.Page.CurrentPageIndex();
    $.get("/api/Sample", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleApply.viewModel.Page);
        SampleApply.viewModel.UpdatePagination();
    });
};
SampleApply.viewModel.ShowSearch = function () {
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
}
//确定搜索
SampleApply.viewModel.Search = function () {
    SampleApply.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(SampleApply.viewModel.SearchSampleModel);
    $.get("/api/Sample", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleApply.viewModel.Page);
        SampleApply.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//新增申请
SampleApply.viewModel.ShowApply = function () {
    var model = {
        Id: null,
        Code: null,
        CustomerModel: {
            Id: null,
            Name: null,
            Code: null
        },
        ProductModel: {
            Id: null,
            PartNumber: null
        },
        Qty: null,
        Remark: null
    };
    ko.mapping.fromJS(model, {}, SampleApply.viewModel.SampleModel);
    $("#createdialog").modal({
        show: true,
        backdrop: "static"
    });
};
//搜索客户
SampleApply.viewModel.SearchCustomer = function() {
    var sampleModel = ko.mapping.toJS(SampleApply.viewModel.SampleModel);
    var model = {
        Name: sampleModel.CustomerModel.Name
    }
    $.get("/api/Customer", model, function (result) {
        if (result.Models.length > 0) {
            ko.mapping.fromJS(result.Models, {}, SampleApply.viewModel.CustomerModels);
            $("#selectcustomerdialog").modal({
                show: true,
                backdrop: "static"
            });
        } else {
            Helper.ShowErrorDialog("找不到相关客户");
        }
    });
};
//选择客户
SampleApply.viewModel.SelectCustomer = function() {
    var customer = ko.mapping.toJS(this);
    SampleApply.viewModel.SampleModel.CustomerModel.Code(customer.Code);
    SampleApply.viewModel.SampleModel.CustomerModel.Id(customer.Id);
    SampleApply.viewModel.SampleModel.CustomerModel.Name(customer.Name);
    $("#selectcustomerdialog").modal("hide");
};
//搜索产品
SampleApply.viewModel.SearchProduct = function () {
    var sampleModel = ko.mapping.toJS(SampleApply.viewModel.SampleModel);
    var model= {
        PartNumber: sampleModel.ProductModel.PartNumber
    }
    $.get("/api/ProductSearch", model, function (result) {
        if (result.ProductModels.length > 0) {
            ko.mapping.fromJS(result.ProductModels, {}, SampleApply.viewModel.ProductModels);
            $("#selectproductdialog").modal({
                show: true,
                backdrop: "static"
            });
        } else {
            Helper.ShowErrorDialog("找不到相关产品");
        }
    });
};
//选择产品
SampleApply.viewModel.SelectProduct = function () {
    var product = ko.mapping.toJS(this);
    SampleApply.viewModel.SampleModel.ProductModel.PartNumber(product.PartNumber);
    SampleApply.viewModel.SampleModel.ProductModel.Id(product.Id);
    $("#selectproductdialog").modal("hide");
};
//提交
SampleApply.viewModel.Submit = function() {
    var model = ko.mapping.toJS(SampleApply.viewModel.SampleModel);
    $.ajax({
        type: "post",
        url: "/api/Sample",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $("#createdialog").modal("hide");
                SampleApply.viewModel.GotoPage();
            }
        }
    });
};
//保存编辑
SampleApply.viewModel.EditSave = function () {
    var model = ko.mapping.toJS(SampleApply.viewModel.SampleModel);
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
                SampleApply.viewModel.GotoPage();
            }
        }
    });
};
//删除申请
SampleApply.viewModel.DeleteApply = function () {
    var model = ko.mapping.toJS(SampleApply.viewModel.SampleModel);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: "delete",
                url: "/api/Sample?Id=" + model.Id,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        $("#editdialog").modal("hide");
                        SampleApply.viewModel.GotoPage();
                    }
                }
            });
        }
    });
};
//编辑申请
SampleApply.viewModel.Edit = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/Sample/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, SampleApply.viewModel.SampleModel);
        $("#editdialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
$(function () {
    ko.applyBindings(SampleApply);
    SampleApply.viewModel.Search();
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
            SampleApply.viewModel.Page.CurrentPageIndex(num);
            SampleApply.viewModel.GotoPage();
        }
    });
});
var AccountReceivable = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        FinancePageModel: {
            OrderModel: ko.observable(),
            CustomerModel: ko.observable(),
            RealAmount: ko.observable(),
            OrderCompleted: ko.observable()
        },
        FinanceModel: {
            Id: ko.observable(),
            //应收款
            TotalPrice: ko.observable(),
            //收款
            Amount: ko.observable(),
            PayDate: ko.observable(),
            HasFaPiao: ko.observable(false),
            FaPiaoNumbers: ko.observable(),
            FaPiaoModels: ko.observableArray(),
            OrderId: ko.observable(),
            Remark: ko.observable()
        },
        FinanceDetailModels: ko.observableArray(),
        FaPiaoModel: {
            Id: ko.observable(),
            Code: ko.observable(),
            FinanceId: ko.observable()
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
        var pattern = allBindings.format || 'YYYY/MM/DD';

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
ko.bindingHandlers.time = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || "YYYY/MM/DD h:m:s";

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
AccountReceivable.viewModel.UpdatePagination = function () {
    var allPage = AccountReceivable.viewModel.Page.AllPage() === 0 ? 1 : AccountReceivable.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: AccountReceivable.viewModel.Page.CurrentPageIndex() });
};
AccountReceivable.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(AccountReceivable.viewModel.FinancePageModel);
    model.pageIndex = AccountReceivable.viewModel.Page.CurrentPageIndex();
    $.get("/api/Finance", model, function (result) {
        ko.mapping.fromJS(result, {}, AccountReceivable.viewModel.Page);
        AccountReceivable.viewModel.UpdatePagination();
    });
};
//确定搜索
AccountReceivable.viewModel.Search = function () {
    AccountReceivable.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(AccountReceivable.viewModel.FinancePageModel);
    $.get("/api/Finance", model, function (result) {
        ko.mapping.fromJS(result, {}, AccountReceivable.viewModel.Page);
        AccountReceivable.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//收款
AccountReceivable.viewModel.Receipt = function () {
    $(".date .form-control").val("");
    var model = ko.mapping.toJS(this);
    var financeModel = {
        TotalPrice: model.OrderModel.TotalPrice,
        //收款
        Amount: null,
        PayDate: null,
        HasFaPiao: false,
        FaPiaoNumbers: null,
        OrderId: model.OrderModel.Id,
        Remark: null
    }

    ko.mapping.fromJS(financeModel, {}, AccountReceivable.viewModel.FinanceModel);
    $("#receiptdialog").modal({
        show: true,
        backdrop: "static"
    });
}
//收款确认
AccountReceivable.viewModel.ReceiptSave = function () {
    var model = ko.mapping.toJS(AccountReceivable.viewModel.FinanceModel);
    $.post("/api/Finance", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            $("#receiptdialog").modal("hide");
            AccountReceivable.viewModel.GotoPage();
        }
    });
};
//收款详细
AccountReceivable.viewModel.ShowAmountDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/FinanceByOrder/" + model.OrderModel.Id, function (result) {
        ko.mapping.fromJS(result, {}, AccountReceivable.viewModel.FinanceDetailModels);
        $("#amountdetaildialog").modal({
            show: true,
            backdrop: "static"
        });
    });
}
//删除收款
AccountReceivable.viewModel.Remove = function () {
    var model = ko.mapping.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: "delete",
                url: "/api/Finance?Id=" + model.Id,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        $.get("/api/FinanceByOrder/" + model.OrderId, function (data) {
                            ko.mapping.fromJS(data, {}, AccountReceivable.viewModel.FinanceDetailModels);
                            AccountReceivable.viewModel.GotoPage();
                        });
                    }
                }
            });
        }
    });
};
//编辑发票
AccountReceivable.viewModel.EditPapiao = function () {
    var finance = ko.mapping.toJS(this);
    AccountReceivable.viewModel.FaPiaoModel.FinanceId(finance.Id);
    $.get("/api/Finance/" + finance.Id, function (result) {
        ko.mapping.fromJS(result, {}, AccountReceivable.viewModel.FinanceModel);
        $("#editfapiaodialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
//删除发票
AccountReceivable.viewModel.RemovePapiao = function () {
    var model = ko.mapping.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: "delete",
                url: "/api/FaPiao?Id=" + model.Id,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        $.get("/api/Finance/" + model.FinanceId, function (data) {
                            ko.mapping.fromJS(data, {}, AccountReceivable.viewModel.FinanceModel);
                            $.get("/api/FinanceByOrder/" + data.OrderId, function (finances) {
                                ko.mapping.fromJS(finances, {}, AccountReceivable.viewModel.FinanceDetailModels);
                            });
                        });
                    }
                }
            });
        }
    });
};
//新增发票
AccountReceivable.viewModel.AddFaPiao= function() {
    var model = ko.mapping.toJS(AccountReceivable.viewModel.FaPiaoModel);
    $.post("/api/FaPiao", model, function(result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            $.get("/api/Finance/" + model.FinanceId, function (data) {
                ko.mapping.fromJS(data, {}, AccountReceivable.viewModel.FinanceModel);
                $.get("/api/FinanceByOrder/" + data.OrderId, function (finances) {
                    ko.mapping.fromJS(finances, {}, AccountReceivable.viewModel.FinanceDetailModels);
                    AccountReceivable.viewModel.FaPiaoModel.Code("");
                });
            });
        }
    });
}
$(function () {
    ko.applyBindings(AccountReceivable);
    AccountReceivable.viewModel.Search();

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
            AccountReceivable.viewModel.Page.CurrentPageIndex(num);
            AccountReceivable.viewModel.GotoPage();
        }
    });
    //$("#paydate").datetimepicker({
    //    locale: "zh-cn",
    //    format: "YYYY年MM月DD日"
    //});
});
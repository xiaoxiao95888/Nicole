var AllOrders = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        OrderModel: {
            Id: ko.observable(),
            Code: ko.observable(),
            UnitPrice: ko.observable(),
            Qty: ko.observable(),
            Remark: ko.observable(),
            State: ko.observable(),
            ContractAmount: ko.observable(),
            OrderDate: ko.observable(),
            PayPeriodModel: ko.observable(),
            PositionModel: {
                Id: ko.observable(),
                Name: ko.observable(),
                CurrentEmployeeModel: {
                    Id: ko.observable(),
                    Name: ko.observable(),
                    Mail: ko.observable(),
                    PhoneNumber: ko.observable(),
                    JoinDate: ko.observable()
                }
            },
            CustomerModel: {
                Id: ko.observable(),
                Code: ko.observable(),
                Name: ko.observable()
            },
            OrderDetailModels: ko.observableArray(),
            CurrentOrderReview: {
                Id: ko.observable(),
                ReturnComments: ko.observable()
            }
        },
        FinanceDetailModels: ko.observableArray()
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
AllOrders.viewModel.UpdatePagination = function () {
    var allPage = AllOrders.viewModel.Page.AllPage() === 0 ? 1 : AllOrders.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: AllOrders.viewModel.Page.CurrentPageIndex() });
};

AllOrders.viewModel.GotoPage = function () {
    var data = ko.mapping.toJS(AllOrders.viewModel.OrderModel);
    data.pageIndex = AllOrders.viewModel.Page.CurrentPageIndex();
    var model = {
        key: {
            Code: data.Code,
            CustomerModel: {
                Code: data.CustomerModel.Code,
                Name: data.CustomerModel.Name
            }
        }, pageIndex: data.pageIndex
    };
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, AllOrders.viewModel.Page);
        AllOrders.viewModel.UpdatePagination();
    });
};
//确定搜索
AllOrders.viewModel.Search = function () {
    AllOrders.viewModel.Page.CurrentPageIndex(1);
    var data = ko.mapping.toJS(AllOrders.viewModel.OrderModel);
    var model = {
        key: {
            Code: data.Code,
            CustomerModel: {
                Code: data.CustomerModel.Code,
                Name: data.CustomerModel.Name
            }
        },
        pageIndex: 1
    };
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, AllOrders.viewModel.Page);
        AllOrders.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//弹出搜索框
AllOrders.viewModel.ShowSearch = function () {
    AllOrders.viewModel.ClearSearch();
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
};
//清空搜索项
AllOrders.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(AllOrders.viewModel.OrderModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, AllOrders.viewModel.OrderModel);
};
//订单详细
AllOrders.viewModel.ShowOrderDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/OrderDetail/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, AllOrders.viewModel.OrderModel.OrderDetailModels);
        $("#detaildialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
//收款详细
AllOrders.viewModel.ShowAmountDetail=function() {
    var model = ko.mapping.toJS(this);
    $.get("/api/FinanceByOrder/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, AllOrders.viewModel.FinanceDetailModels);
        $("#amountdetaildialog").modal({
            show: true,
            backdrop: "static"
        });
    });
}
$(function () {
    ko.applyBindings(AllOrders);
    AllOrders.viewModel.Search();
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
            AllOrders.viewModel.Page.CurrentPageIndex(num);
            AllOrders.viewModel.GotoPage();
        }

    });
});
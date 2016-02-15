var OrderReview = {
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
            TotalPrice: ko.observable(),
            OrderDate: ko.observable(),
            EnquiryModel: {
                Id: ko.observable(),
                Price: ko.observable(),
                ProductModel: {
                    PartNumber: ko.observable(),
                    ProductType: ko.observable(),
                    Voltage: ko.observable(),
                    Capacity: ko.observable(),
                    Pitch: ko.observable(),
                    Level: ko.observable(),
                    SpecificDesign: ko.observable()
                },
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
                }
            },
            CurrentOrderReview: {
                Id: ko.observable(),
                ReturnComments: ko.observable()
            }
        }
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
        var pattern = allBindings.format || 'YYYY/MM/DD h:m:s';

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
OrderReview.viewModel.UpdatePagination = function () {
    var allPage = OrderReview.viewModel.Page.AllPage() == 0 ? 1 : OrderReview.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: OrderReview.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
OrderReview.viewModel.Search = function () {
    OrderReview.viewModel.Page.CurrentPageIndex(1);
    var data = ko.mapping.toJS(OrderReview.viewModel.OrderModel);
    var model = {
        key: {
            Code: data.Code,
            EnquiryModel: {
                ProductModel: {
                    PartNumber: data.EnquiryModel.ProductModel.PartNumber
                },
                CustomerModel: {
                    Code: data.EnquiryModel.CustomerModel.Code,
                    Name: data.EnquiryModel.CustomerModel.Name
                }
            }
        }
    };
    $.get("/api/OrderReview", model, function (result) {
        ko.mapping.fromJS(result, {}, OrderReview.viewModel.Page);
        OrderReview.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
//弹出搜索框
OrderReview.viewModel.ShowSearch = function () {
    OrderReview.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//清空搜索项
OrderReview.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(OrderReview.viewModel.OrderModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, OrderReview.viewModel.OrderModel);
};
//订单详细
OrderReview.ShowOrderDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get('/api/Order/' + model.Id, function(result) {
        ko.mapping.fromJS(result, {}, OrderReview.viewModel.OrderModel);
        $('#detaildialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
};
//审核通过
OrderReview.viewModel.Approve = function () {
    var model = ko.mapping.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认通过?",
        confirmFunction: function () {
            $.ajax({
                type: 'post',
                url: '/api/OrderReview',
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(model.CurrentOrderReview),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        OrderReview.viewModel.ClearSearch();
                        OrderReview.viewModel.Search();
                    }
                }
            });
        }
    });
};
//退回
OrderReview.viewModel.Return=function() {
    var model = ko.mapping.toJS(this);
    $.get('/api/OrderReview/' + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, OrderReview.viewModel.OrderModel);
        $('#returndialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
}
//确认退回
OrderReview.viewModel.ReturnSave = function () {
    var model = ko.mapping.toJS(OrderReview.viewModel.OrderModel);
    Helper.ShowConfirmationDialog({
        message: "是否确认退回?",
        confirmFunction: function () {
            $.ajax({
                type: 'put',
                url: '/api/OrderReview',
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(model.CurrentOrderReview),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        $('#returndialog').modal("hide");
                        OrderReview.viewModel.ClearSearch();
                        OrderReview.viewModel.Search();
                    }
                }
            });
        }
    });
}
$(function () {
    ko.applyBindings(OrderReview);
    OrderReview.viewModel.Search();
    //初始化页码
    $('#page-selection').bootpag({
        total: 1,
        page: 1,
        maxVisible: 5,
        leaps: true,
        firstLastUse: true,
        first: 'First',
        last: 'Last',
        wrapClass: 'pagination',
        activeClass: 'active',
        disabledClass: 'disabled',
        nextClass: 'next',
        prevClass: 'prev',
        lastClass: 'last',
        firstClass: 'first'
    }).on("page", function (event, num) {
        if (num != null) {
            OrderReview.viewModel.Page.CurrentPageIndex(num);
            OrderReview.viewModel.GotoPage();
        }

    });
});
var OrderManager = {
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
OrderManager.viewModel.UpdatePagination = function () {
    var allPage = OrderManager.viewModel.Page.AllPage() == 0 ? 1 : OrderManager.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: OrderManager.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
OrderManager.viewModel.Search = function () {
    OrderManager.viewModel.Page.CurrentPageIndex(1);
    var data = ko.mapping.toJS(OrderManager.viewModel.OrderModel);
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
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, OrderManager.viewModel.Page);
        OrderManager.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
//弹出搜索框
OrderManager.viewModel.ShowSearch = function () {
    OrderManager.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//清空搜索项
OrderManager.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(OrderManager.viewModel.OrderModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, OrderManager.viewModel.OrderModel);
};
//订单详细
OrderManager.ShowOrderDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get('/api/Order/' + model.Id, function(result) {
        ko.mapping.fromJS(result, {}, OrderManager.viewModel.OrderModel);
        $('#detaildialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
    
};
OrderManager.viewModel.Approve = function () {
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
                        OrderManager.viewModel.ClearSearch();
                        OrderManager.viewModel.Search();
                    }
                }
            });
        }
    });
};

$(function () {
    ko.applyBindings(OrderManager);
    OrderManager.viewModel.Search();
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
            OrderManager.viewModel.Page.CurrentPageIndex(num);
            OrderManager.viewModel.GotoPage();
        }

    });
});
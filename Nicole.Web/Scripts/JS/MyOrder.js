var MyOrder = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        OrderModel: {
            Id: ko.observable(),
            Code: ko.observable(),
            Remark: ko.observable(),
            State: ko.observable(),
            OrderDate: ko.observable(),
            //是否已开发票
            HasFaPiao: ko.observable(false),
            //预计交货日期
            EstimatedDeliveryDate: ko.observable(),
            RealAmount: ko.observable(),
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
        PayPeriodModels: ko.observableArray(),
        FinanceDetailModels: ko.observableArray(),
        CustomerModels: ko.observableArray(),
        EnquiryModels: ko.observableArray(),
        OrderDetailModel: {
            EnquiryModel: {
                Id: ko.observable(),
                Price: ko.observable(),
                ProductModel: {
                    Id: ko.observable(),
                    PartNumber: ko.observable()
                }
            },
            UnitPrice: ko.observable(),
            Qty: ko.observable()
        }
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
MyOrder.viewModel.UpdatePagination = function () {
    var allPage = MyOrder.viewModel.Page.AllPage() == 0 ? 1 : MyOrder.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: MyOrder.viewModel.Page.CurrentPageIndex() });
};
MyOrder.viewModel.GotoPage = function () {
    var data = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    var model = {
        key: {
            Code: data.Code,
            CustomerModel: {
                Code: data.CustomerModel.Code,
                Name: data.CustomerModel.Name
            }
        }
    };
    model.pageIndex = MyOrder.viewModel.Page.CurrentPageIndex();
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.Page);
        MyOrder.viewModel.UpdatePagination();
    });
};
//生成合同
MyOrder.viewModel.ShowCreate = function () {
    $.get("/api/PayPeriod/", function (payPeriodModels) {
        ko.mapping.fromJS(payPeriodModels, {}, MyOrder.viewModel.PayPeriodModels);
        $("#orderdate").datetimepicker({
            locale: "zh-cn",
            format: "YYYY年MM月DD日"
        });
        $("#editdialog").modal({
            show: true,
            backdrop: "static"
        });
        MyOrder.viewModel.OrderModel.OrderDetailModels.removeAll();
        $("#createorderdialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
//搜索客户
MyOrder.viewModel.SearchCustomer = function () {
    var customerModel = ko.mapping.toJS(MyOrder.viewModel.OrderModel.CustomerModel);
    var model = {
        Name: customerModel.Name
    }
    $.get("/api/Customer", model, function (result) {
        if (result.Models.length > 0) {
            ko.mapping.fromJS(result.Models, {}, MyOrder.viewModel.CustomerModels);
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
MyOrder.viewModel.SelectCustomer = function() {
    var customer = ko.mapping.toJS(this);
    MyOrder.viewModel.OrderModel.CustomerModel.Code(customer.Code);
    MyOrder.viewModel.OrderModel.CustomerModel.Id(customer.Id);
    MyOrder.viewModel.OrderModel.CustomerModel.Name(customer.Name);
    $("#selectcustomerdialog").modal("hide");
};
//搜索报价
MyOrder.viewModel.SearchEnquiry = function () {
    var customerId = MyOrder.viewModel.OrderModel.CustomerModel.Id();
    if (customerId != null) {
        $.get("/api/MyEnquiryByCustomer/" + customerId, function (result) {
           
            if (result.length > 0) {
                ko.mapping.fromJS(result, {}, MyOrder.viewModel.EnquiryModels);
                $("#selectenquirydialog").modal({
                    show: true,
                    backdrop: "static"
                });
            } else {
                Helper.ShowErrorDialog("没有相关的询价结果");
            }
            
        });
    }
};
//选择报价
MyOrder.viewModel.SelectEnquiry = function () {
    var model = ko.mapping.toJS(this);
    ko.mapping.fromJS(model, {}, MyOrder.viewModel.OrderDetailModel.EnquiryModel);
    $("#inputqtypricedialog").modal({
        show: true,
        backdrop: "static"
    });
};
//填写数量单价
MyOrder.viewModel.SaveInputQtyPrice = function() {
    var orderdetail = ko.mapping.toJS(MyOrder.viewModel.OrderDetailModel);
    $.post("/api/OrderCheckUnitPrice", orderdetail, function(result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            $("#inputqtypricedialog").modal("hide");
            
            MyOrder.viewModel.OrderModel.OrderDetailModels.push(MyOrder.viewModel.OrderDetailModel);
        }
    });
};
//移除报价
MyOrder.viewModel.RemoveOrderDetail = function() {
    MyOrder.viewModel.OrderModel.OrderDetailModels.remove(this);
};
//确定搜索
MyOrder.viewModel.Search = function () {
    MyOrder.viewModel.Page.CurrentPageIndex(1);
    var data = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    var model = {
        key: {
            Code: data.Code,
            CustomerModel: {
                Code: data.CustomerModel.Code,
                Name: data.CustomerModel.Name
            }
        }
    };
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.Page);
        MyOrder.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//弹出搜索框
MyOrder.viewModel.ShowSearch = function () {
    MyOrder.viewModel.ClearSearch();
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
};
//清空搜索项
MyOrder.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, MyOrder.viewModel.OrderModel);

};
//编辑合同
MyOrder.viewModel.Edit = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/Order/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.OrderModel);
        $.get("/api/PayPeriod/", function (payPeriodModels) {
            ko.mapping.fromJS(payPeriodModels, {}, MyOrder.viewModel.PayPeriodModels);
            ko.utils.arrayForEach(MyOrder.viewModel.PayPeriodModels(), function (item) {
                if (item.Id() === model.PayPeriodModel.Id) {
                    MyOrder.viewModel.OrderModel.PayPeriodModel(item);
                }
            });
            $("#orderdate").datetimepicker({
                locale: "zh-cn",
                format: "YYYY年MM月DD日"
            });
            $("#editdialog").modal({
                show: true,
                backdrop: "static"
            });
        });

    });
};
//新增提交审核
MyOrder.viewModel.SubmitSave = function() {
    var orderModel = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    orderModel.OrderDate = $("#orderdate").val();
    $.ajax({
        type: "post",
        url: "/api/Order",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(orderModel),
        success: function(result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $("#createorderdialog").modal("hide");
                MyOrder.viewModel.GotoPage();
            }
        }
    });
};
////提交审核
//MyOrder.viewModel.SubmitAudit = function () {
//    var model = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
//    model.OrderDate = $("#orderdate").val();
//    $.ajax({
//        type: "put",
//        url: "/api/Order?Id=" + model.Id,
//        contentType: "application/json",
//        dataType: "json",
//        data: JSON.stringify(model),
//        success: function (result) {
//            if (result.Error) {
//                Helper.ShowErrorDialog(result.Message);
//            } else {
//                Helper.ShowSuccessDialog(Messages.Success);
//                $("#editdialog").modal("hide");
//                MyOrder.viewModel.ClearSearch();
//                MyOrder.viewModel.Search();
//            }
//        }
//    });
//}
//查看退回原因
MyOrder.viewModel.Reason = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/Order/" + model.Id, function (result) {
        Helper.ShowMessageDialog(result.CurrentOrderReview.ReturnComments, result.Code);
    });
}
//删除合同
MyOrder.viewModel.DeleteOrder = function () {
    var model = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: "delete",
                url: "/api/Order?Id=" + model.Id,
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        $("#editdialog").modal("hide");
                        MyOrder.viewModel.ClearSearch();
                        MyOrder.viewModel.Search();
                    }
                }
            });
        }
    });
};
//合同详细
MyOrder.ShowOrderDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/OrderDetail/GetByOrderId?id=" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.OrderModel.OrderDetailModels);
        $("#detaildialog").modal({
            show: true,
            backdrop: "static"
        });
    });
};
//收款详细
MyOrder.viewModel.ShowAmountDetail = function () {
    var model = ko.mapping.toJS(this);
    $.get("/api/FinanceByOrder/" + model.Id, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.FinanceDetailModels);
        $("#amountdetaildialog").modal({
            show: true,
            backdrop: "static"
        });
    });
}
$(function () {
    ko.applyBindings(MyOrder);
    MyOrder.viewModel.Search();

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
            MyOrder.viewModel.Page.CurrentPageIndex(num);
            MyOrder.viewModel.GotoPage();
        }

    });
});
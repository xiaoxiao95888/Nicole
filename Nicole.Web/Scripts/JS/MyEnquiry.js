var MyEnquiry = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            EnquiryModels: ko.observableArray()
        },
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
        OrderModel: {
            Id: ko.observable(),
            UnitPrice: ko.observable(),
            Qty: ko.observable(),
            Remark: ko.observable(),
            OrderDate: ko.observable(),
            EnquiryModel: ko.observable(),
            PayPeriodModel: ko.observable(),
            EstimatedDeliveryDate: ko.observable()
        },
        PayPeriodModels: ko.observableArray()
    }
};

MyEnquiry.viewModel.OrderModel.TotalPrice = ko.computed({
    read: function () {

        if (MyEnquiry.viewModel.OrderModel.Qty() != null && MyEnquiry.viewModel.OrderModel.UnitPrice() != null) {
            return MyEnquiry.viewModel.OrderModel.Qty() * 10000 * MyEnquiry.viewModel.OrderModel.UnitPrice() / 10000;
        }
        return null;
    },
    write: function (value) {
    }
});
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
MyEnquiry.viewModel.Print = function () {

};

MyEnquiry.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(MyEnquiry.viewModel.EnquiryModel);
    model.pageIndex = MyEnquiry.viewModel.Page.CurrentPageIndex();
    $.get("/api/MyEnquiry", model, function (result) {
        ko.mapping.fromJS(result, {}, MyEnquiry.viewModel.Page);
        MyEnquiry.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
MyEnquiry.viewModel.UpdatePagination = function () {
    var allPage = MyEnquiry.viewModel.Page.AllPage() == 0 ? 1 : MyEnquiry.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: MyEnquiry.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
MyEnquiry.viewModel.Search = function () {
    MyEnquiry.viewModel.Page.CurrentPageIndex(1);
    var model = {
        key: ko.mapping.toJS(MyEnquiry.viewModel.EnquiryModel)
    };
    $.get("/api/MyEnquiry", model, function (result) {
        ko.mapping.fromJS(result, {}, MyEnquiry.viewModel.Page);
        MyEnquiry.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//弹出搜索框
MyEnquiry.viewModel.ShowSearch = function () {
    MyEnquiry.viewModel.ClearSearch();
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
};

//清空搜索项
MyEnquiry.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(MyEnquiry.viewModel.EnquiryModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, MyEnquiry.viewModel.EnquiryModel);

};
//保存合同
MyEnquiry.viewModel.OrderSave = function () {
    var enquirymodel = ko.mapping.toJS(MyEnquiry.viewModel.EnquiryModel);
    var orderModel = ko.mapping.toJS(MyEnquiry.viewModel.OrderModel);
    orderModel.EnquiryModel = enquirymodel;
    orderModel.OrderDate = $("#orderdate").val();
    $.post("/api/Order", orderModel, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            $("#createorderdialog").modal("hide");
        }
    });
};
$(function () {
    ko.applyBindings(MyEnquiry);
    MyEnquiry.viewModel.Search();

    $("#orderdate").datetimepicker({
        locale: "zh-cn",
        format: "YYYY年MM月DD日"
    });
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
            MyEnquiry.viewModel.Page.CurrentPageIndex(num);
            MyEnquiry.viewModel.GotoPage();
        }

    });
});
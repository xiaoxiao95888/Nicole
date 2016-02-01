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
            UnitPrice: ko.observable(),
            Qty: ko.observable(),
            Remark: ko.observable(),
            State: ko.observable(),
            CanEdit:ko.observable(),
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
MyOrder.viewModel.UpdatePagination = function () {
    var allPage = MyOrder.viewModel.Page.AllPage() == 0 ? 1 : MyOrder.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: MyOrder.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
MyOrder.viewModel.Search = function () {
    MyOrder.viewModel.Page.CurrentPageIndex(1);
    var model = {
        key: ko.mapping.toJS(MyOrder.viewModel.OrderModel)
    };
    $.get("/api/Order", model, function (result) {
        ko.mapping.fromJS(result, {}, MyOrder.viewModel.Page);
        MyOrder.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
//弹出搜索框
MyOrder.viewModel.ShowSearch = function () {
    MyOrder.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//清空搜索项
MyOrder.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(MyOrder.viewModel.OrderModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, MyOrder.viewModel.OrderModel);

};
//编辑合同
MyOrder.viewModel.Edit = function() {
    alert("dsa");
};
$(function () {
    ko.applyBindings(MyOrder);
    MyOrder.viewModel.Search();
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
            MyOrder.viewModel.Page.CurrentPageIndex(num);
            MyOrder.viewModel.GotoPage();
        }

    });
});
var MyCustomer = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        CustomerModel: {
            Id: ko.observable(),
            Code: ko.observable(),
            Name: ko.observable(),
            Address: ko.observable(),
            Email: ko.observable(),
            ContactPerson: ko.observable(),
            TelNumber: ko.observable(),
            CustomerTypeModel: {
                Id: ko.observable(),
                Name: ko.observable()
            },
            Origin: ko.observable()
        },
        CustomerTypeModels: ko.observableArray(),
        ProductModel: {
            Id: ko.observable(),
            PartNumber: ko.observable(),
            ProductType: ko.observable(),
            Voltage: ko.observable(),
            Capacity: ko.observable(),
            Pitch: ko.observable(),
            Level: ko.observable(),
            SpecificDesign: ko.observable()
        },
        SelectedCustomerModel: ko.observable()
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

//更新页码
MyCustomer.viewModel.UpdatePagination = function () {
    var allPage = MyCustomer.viewModel.Page.AllPage() == 0 ? 1 : MyCustomer.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: MyCustomer.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
MyCustomer.viewModel.Search = function () {
    MyCustomer.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(MyCustomer.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/MyCustomer", model, function (result) {
        ko.mapping.fromJS(result, {}, MyCustomer.viewModel.Page);
        MyCustomer.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
MyCustomer.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(MyCustomer.viewModel.CustomerModel);
    model.pageIndex = MyCustomer.viewModel.Page.CurrentPageIndex();
    $.get("/api/MyCustomer", model, function (result) {
        ko.mapping.fromJS(result, {}, MyCustomer.viewModel.Page);
        MyCustomer.viewModel.UpdatePagination();
    });
};
//搜索
MyCustomer.viewModel.ShowSearch = function () {
    MyCustomer.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//询价
MyCustomer.viewModel.ShowEnquiry = function () {
    var model = ko.mapping.toJS(this);
    MyCustomer.viewModel.CustomerModel.Id(model.Id);
    MyCustomer.viewModel.CustomerModel.Name(model.Name);
    $('#enquirydialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//提交询价
MyCustomer.viewModel.SaveEnquiry = function () {
    var customer = ko.mapping.toJS(MyCustomer.viewModel.CustomerModel);
    var product = ko.mapping.toJS(MyCustomer.viewModel.ProductModel);
    var model = {
        CustomerModel: {
            Id: customer.Id
        },
        ProductModel: product
    };
    $.post('/api/MyEnquiry', model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            MyCustomer.viewModel.ClearSearch();
            $('#enquirydialog').modal('hide');
        }
    });
};
//清空搜索项
MyCustomer.viewModel.ClearSearch = function () {
    for (var index in MyCustomer.viewModel.CustomerModel) {
        if (ko.isObservable(MyCustomer.viewModel.CustomerModel[index])) {
            MyCustomer.viewModel.CustomerModel[index](null);
        }
    }
};
//根据料号搜索
MyCustomer.viewModel.SearchProduct = function () {
    var product = ko.mapping.toJS(MyCustomer.viewModel.ProductModel);
    $.get("/api/ProductSearch/" + product.PartNumber, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            ko.mapping.fromJS(result, {}, MyCustomer.viewModel.ProductModel);
        }
    });
};
$(function () {
    ko.applyBindings(MyCustomer);
    $.get('/api/CustomerType', function (result) {
        ko.mapping.fromJS(result, {}, MyCustomer.viewModel.CustomerTypeModels);
        MyCustomer.viewModel.Search();
    });

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
            MyCustomer.viewModel.Page.CurrentPageIndex(num);
            MyCustomer.viewModel.GotoPage();
        }

    });
});
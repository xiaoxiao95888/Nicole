﻿var CustomerSearch = {
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
        CustomerTypeModels: ko.observableArray()
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
CustomerSearch.viewModel.UpdatePagination = function () {
    var allPage = CustomerSearch.viewModel.Page.AllPage() == 0 ? 1 : CustomerSearch.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerSearch.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerSearch.viewModel.Search = function () {
    CustomerSearch.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(CustomerSearch.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/MyCustomer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerSearch.viewModel.Page);
        CustomerSearch.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerSearch.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(CustomerSearch.viewModel.CustomerModel);
    model.pageIndex = CustomerSearch.viewModel.Page.CurrentPageIndex();
    $.get("/api/MyCustomer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerSearch.viewModel.Page);
        CustomerSearch.viewModel.UpdatePagination();
    });
};
//搜索
CustomerSearch.viewModel.ShowSearch = function () {
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
$(function () {
    ko.applyBindings(CustomerSearch);
    $.get('/api/CustomerType', function (result) {
        ko.mapping.fromJS(result, {}, CustomerSearch.viewModel.CustomerTypeModels);
        CustomerSearch.viewModel.Search();
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
            CustomerSearch.viewModel.Page.CurrentPageIndex(num);
            CustomerSearch.viewModel.GotoPage();
        }

    });
});
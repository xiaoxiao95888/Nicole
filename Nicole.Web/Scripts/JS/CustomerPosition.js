var CustomerPosition = {
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
            CustomerType: ko.observable(),
            Origin: ko.observable()
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
CustomerPosition.viewModel.ShowCreate = function() {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//更新页码
CustomerPosition.viewModel.UpdatePagination = function () {
    var allPage = CustomerPosition.viewModel.Page.AllPage() == 0 ? 1 : CustomerPosition.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerPosition.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerPosition.viewModel.Search = function () {
    CustomerPosition.viewModel.Page.CurrentPageIndex(1);
    var data = ko.toJS(CustomerPosition.viewModel.CustomerModel);
    var model = {
        CodeKey: data.Code,
        NameKey: data.Name,
        AddressKey: data.Address,
        EmailKey: data.Email,
        ContactPersonKey: data.ContactPerson,
        TelNumberKey: data.TelNumber,
        CustomerTypeKey: data.CustomerType,
        OriginKey: data.Origin,
        pageIndex: CustomerPosition.viewModel.Page.CurrentPageIndex(),
    };
    $.get("/api/CustomerCreate", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerPosition.viewModel.Page);
        CustomerPosition.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerPosition.viewModel.GotoPage = function () {
    var model = {
        pageIndex: CustomerPosition.viewModel.Page.CurrentPageIndex(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val()
    };
    $.get("/api/CustomerCreate", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerPosition.viewModel.Page);
        CustomerPosition.viewModel.UpdatePagination();
    });
};
//搜索
CustomerPosition.viewModel.ShowSearch = function () {
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
$(function () {
    ko.applyBindings(CustomerPosition);
    CustomerPosition.viewModel.Search();
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
            CustomerPosition.viewModel.Page.CurrentPageIndex(num);
            CustomerPosition.viewModel.GotoPage();
        }

    });
});
var CustomerPosition = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },        
        PositionModels: ko.observableArray(),
        SelectedCustomerModel: {
            Id: ko.observable(),
            Name: ko.observable(),
            PositionModels: ko.observableArray()
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

//更新页码
CustomerPosition.viewModel.UpdatePagination = function () {
    var allPage = CustomerPosition.viewModel.Page.AllPage() === 0 ? 1 : CustomerPosition.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerPosition.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerPosition.viewModel.Search = function () {
    CustomerPosition.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(CustomerPosition.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/Customer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerPosition.viewModel.Page);
        CustomerPosition.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerPosition.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(CustomerPosition.viewModel.CustomerModel);
    model.pageIndex = CustomerPosition.viewModel.Page.CurrentPageIndex();
    $.get("/api/Customer", model, function (result) {
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
CustomerPosition.viewModel.ShowCreate = function () {
    var model = ko.mapping.toJS(this);
    //绑定选中的CustomerModel
    ko.mapping.fromJS(model, {}, CustomerPosition.viewModel.SelectedCustomerModel);
    $.get('/api/Position/', function (result) {
        ko.mapping.fromJS(result, {}, CustomerPosition.viewModel.PositionModels);
        $('#createdialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
};
//确定添加关系
CustomerPosition.viewModel.AddSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerPosition.viewModel.SelectedCustomerModel);
    var selectedPositionModel = ko.mapping.toJS(this);
    var positionCustomerModel = {
        PositionModel: {
            Id: selectedPositionModel.Id
        },
        CustomerModel: {
            Id: selectedCustomerModel.Id
        }
    };
    $.post('/api/PositionCustomer/', positionCustomerModel, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            CustomerPosition.viewModel.Search();
        }
    });

};
//解除关系
CustomerPosition.viewModel.Unfriend = function() {
    var model = ko.mapping.toJS(this);
    //绑定选中的CustomerModel
    ko.mapping.fromJS(model, {}, CustomerPosition.viewModel.SelectedCustomerModel);
    $('#unfrienddialog').modal({
        show: true,
        backdrop: 'static'
    });
};
CustomerPosition.viewModel.UnfriendSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerPosition.viewModel.SelectedCustomerModel);
    var selectedPositionModel = ko.mapping.toJS(this);
    var positionCustomerModel = {
        PositionModel: {
            Id: selectedPositionModel.Id
        },
        CustomerModel: {
            Id: selectedCustomerModel.Id
        }
    };
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/PositionCustomer/',
                contentType: 'application/json',
                dataType: "json",
                data:  JSON.stringify(positionCustomerModel),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        CustomerPosition.viewModel.Search();
                    }
                }
            });
        }
    });
};
$(function () {
    ko.applyBindings(CustomerPosition);
    $.get('/api/CustomerType', function (result) {
        ko.mapping.fromJS(result, {}, CustomerPosition.viewModel.CustomerTypeModels);
        CustomerPosition.viewModel.Search();
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
            CustomerPosition.viewModel.Page.CurrentPageIndex(num);
            CustomerPosition.viewModel.GotoPage();
        }

    });
});
var CustomerCreate = {
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
            CustomerTypeModel: ko.observable(),
            Origin: ko.observable()
        },
        CustomerTypeModels: ko.observableArray(),
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
        PositionModels: ko.observableArray()
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
CustomerCreate.viewModel.UpdatePagination = function () {
    var allPage = CustomerCreate.viewModel.Page.AllPage() == 0 ? 1 : CustomerCreate.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerCreate.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerCreate.viewModel.Search = function () {
    CustomerCreate.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/Customer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.Page);
        CustomerCreate.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerCreate.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    model.pageIndex = CustomerCreate.viewModel.Page.CurrentPageIndex();
    $.get("/api/Customer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.Page);
        CustomerCreate.viewModel.UpdatePagination();
    });
};
CustomerCreate.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//确定绑定关系
CustomerCreate.viewModel.AddSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
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
            CustomerCreate.viewModel.ClearSearch();
            CustomerCreate.viewModel.Search();
        }
    });

};
//解除绑定
CustomerCreate.viewModel.UnfriendSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    var selectedPositionModel = ko.mapping.toJS(CustomerCreate.viewModel.PositionModel);
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
                data: JSON.stringify(positionCustomerModel),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        CustomerCreate.viewModel.ClearSearch();
                        CustomerCreate.viewModel.Search();
                        $('#positiondetaildialog').modal('hide');
                    }
                }
            });
        }
    });
};
//保存新增
CustomerCreate.viewModel.CreateSave = function () {
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    $.post("/api/Customer", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            CustomerCreate.viewModel.ClearSearch();
            CustomerCreate.viewModel.Search();
            $('#createdialog').modal('hide');

        }
    });

};
CustomerCreate.viewModel.ShowEdit = function () {
    var model = ko.mapping.toJS(this);
    if (model.CustomerTypeModel != null) {
        ko.utils.arrayForEach(CustomerCreate.viewModel.CustomerTypeModels(), function (item) {
            if (item.Id() === model.CustomerTypeModel.Id) {
                CustomerCreate.viewModel.CustomerModel.CustomerTypeModel(item);
            }
        });
    }
    ko.mapping.fromJS(model, {}, CustomerCreate.viewModel.CustomerModel);
    $('#editdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存编辑
CustomerCreate.viewModel.EditSave = function () {
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    $.ajax({
        type: 'put',
        url: '/api/Customer',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $('#editdialog').modal('hide');
                CustomerCreate.viewModel.ClearSearch();
                CustomerCreate.viewModel.Search();
            }
        }
    });

};
//删除
CustomerCreate.viewModel.Delete = function () {
    var model = ko.mapping.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/Customer/' + model.Id,
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        CustomerCreate.viewModel.ClearSearch();
                        CustomerCreate.viewModel.Search();
                    }
                }
            });
        }
    });
};
//清空搜索项
CustomerCreate.viewModel.ClearSearch = function () {
    for (var index in CustomerCreate.viewModel.CustomerModel) {
        if (ko.isObservable(CustomerCreate.viewModel.CustomerModel[index])) {
            CustomerCreate.viewModel.CustomerModel[index](null);
        }
    }
};
//搜索
CustomerCreate.viewModel.ShowSearch = function () {
    CustomerCreate.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//显示职位详细
CustomerCreate.viewModel.ShowPositionDetail = function (customerModel,positionModel) {
    var position = ko.mapping.toJS(positionModel);
    var customer = ko.mapping.toJS(customerModel);
    ko.mapping.fromJS(position, {}, CustomerCreate.viewModel.PositionModel);
    ko.mapping.fromJS(customer, {}, CustomerCreate.viewModel.CustomerModel);
    $('#positiondetaildialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//弹出分配窗口
CustomerCreate.viewModel.ShowAllocation = function() {
    var model = ko.mapping.toJS(this);
    //绑定选中的CustomerModel
    ko.mapping.fromJS(model, {}, CustomerCreate.viewModel.CustomerModel);
    $.get('/api/Position/', function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.PositionModels);
        $('#allocationdialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
};
$(function () {
    ko.applyBindings(CustomerCreate);
    $.get('/api/CustomerType', function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.CustomerTypeModels);
        CustomerCreate.viewModel.Search();
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
            CustomerCreate.viewModel.Page.CurrentPageIndex(num);
            CustomerCreate.viewModel.GotoPage();
        }

    });
});
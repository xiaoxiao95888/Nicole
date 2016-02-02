var CustomerManager = {
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
CustomerManager.viewModel.UpdatePagination = function () {
    var allPage = CustomerManager.viewModel.Page.AllPage() == 0 ? 1 : CustomerManager.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerManager.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerManager.viewModel.Search = function () {
    CustomerManager.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/Customer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerManager.viewModel.Page);
        CustomerManager.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerManager.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
    model.pageIndex = CustomerManager.viewModel.Page.CurrentPageIndex();
    $.get("/api/Customer", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerManager.viewModel.Page);
        CustomerManager.viewModel.UpdatePagination();
    });
};
CustomerManager.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//确定绑定关系
CustomerManager.viewModel.AddSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
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
            CustomerManager.viewModel.ClearSearch();
            CustomerManager.viewModel.Search();
        }
    });

};
//解除绑定
CustomerManager.viewModel.UnfriendSave = function () {
    var selectedCustomerModel = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
    var selectedPositionModel = ko.mapping.toJS(CustomerManager.viewModel.PositionModel);
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
                        CustomerManager.viewModel.ClearSearch();
                        CustomerManager.viewModel.Search();
                        $('#positiondetaildialog').modal('hide');
                    }
                }
            });
        }
    });
};
//保存新增
CustomerManager.viewModel.CreateSave = function () {
    var model = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
    $.post("/api/Customer", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            CustomerManager.viewModel.ClearSearch();
            CustomerManager.viewModel.Search();
            $('#createdialog').modal('hide');

        }
    });

};
//编辑客户
CustomerManager.viewModel.ShowEdit = function () {
    var model = ko.mapping.toJS(this);
    if (model.CustomerTypeModel != null) {
        ko.utils.arrayForEach(CustomerManager.viewModel.CustomerTypeModels(), function (item) {
            if (item.Id() === model.CustomerTypeModel.Id) {
                CustomerManager.viewModel.CustomerModel.CustomerTypeModel(item);
            }
        });
    }
    ko.mapping.fromJS(model, {}, CustomerManager.viewModel.CustomerModel);
    $('#editdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存编辑
CustomerManager.viewModel.EditSave = function () {
    var model = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
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
                CustomerManager.viewModel.ClearSearch();
                CustomerManager.viewModel.Search();
            }
        }
    });

};
//删除
CustomerManager.viewModel.Delete = function () {
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
                        CustomerManager.viewModel.ClearSearch();
                        CustomerManager.viewModel.Search();
                    }
                }
            });
        }
    });
};
//清空搜索项
CustomerManager.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(CustomerManager.viewModel.CustomerModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, CustomerManager.viewModel.CustomerModel);
};
//搜索
CustomerManager.viewModel.ShowSearch = function () {
    CustomerManager.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//显示职位详细
CustomerManager.viewModel.ShowPositionDetail = function (customerModel, positionModel) {
    var position = ko.mapping.toJS(positionModel);
    var customer = ko.mapping.toJS(customerModel);
    ko.mapping.fromJS(position, {}, CustomerManager.viewModel.PositionModel);
    ko.mapping.fromJS(customer, {}, CustomerManager.viewModel.CustomerModel);
    $('#positiondetaildialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//弹出分配窗口
CustomerManager.viewModel.ShowAllocation = function () {
    var model = ko.mapping.toJS(this);
    //绑定选中的CustomerModel
    ko.mapping.fromJS(model, {}, CustomerManager.viewModel.CustomerModel);
    $.get('/api/Position/', function (result) {
        ko.mapping.fromJS(result, {}, CustomerManager.viewModel.PositionModels);
        $('#allocationdialog').modal({
            show: true,
            backdrop: 'static'
        });
    });
};
$(function () {
    ko.applyBindings(CustomerManager);
    $.get('/api/CustomerType', function (result) {
        ko.mapping.fromJS(result, {}, CustomerManager.viewModel.CustomerTypeModels);
        CustomerManager.viewModel.Search();
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
            CustomerManager.viewModel.Page.CurrentPageIndex(num);
            CustomerManager.viewModel.GotoPage();
        }

    });
});
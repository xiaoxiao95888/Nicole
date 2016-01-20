﻿var CustomerCreate = {
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
            Origin: ko.observable(),
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

//更新页码
CustomerCreate.viewModel.UpdatePagination = function () {
    var allPage = CustomerCreate.viewModel.Page.AllPage() == 0 ? 1 : CustomerCreate.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: CustomerCreate.viewModel.Page.CurrentPageIndex() });
};
//确定搜索
CustomerCreate.viewModel.Search = function () {
    CustomerCreate.viewModel.Page.CurrentPageIndex(1);
    var data = ko.toJS(CustomerCreate.viewModel.CustomerModel);
    var model = {
        CodeKey: data.Code,
        NameKey: data.Name,
        AddressKey: data.Address,
        EmailKey: data.Email,
        ContactPersonKey: data.ContactPerson,
        TelNumberKey: data.TelNumber,
        CustomerTypeKey: data.CustomerType,
        OriginKey: data.Origin,
        pageIndex: CustomerCreate.viewModel.Page.CurrentPageIndex(),
    };
    $.get("/api/CustomerCreate", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.Page);
        CustomerCreate.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
CustomerCreate.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存新增
CustomerCreate.viewModel.CreateSave = function () {
   
    var model = ko.toJS(CustomerCreate.viewModel.CustomerModel);    
    $.post("/api/CustomerCreate", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            CustomerCreate.viewModel.ClearSearch();
            Helper.ShowSuccessDialog(Messages.Success);
            CustomerCreate.viewModel.Search();
            $('#createdialog').modal('hide');
            
        }
    });

};
CustomerCreate.viewModel.ShowEdit = function () {
    var model = ko.toJS(this);
    ko.mapping.fromJS(model, {}, CustomerCreate.viewModel.CustomerModel);
    $('#editdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存编辑
CustomerCreate.viewModel.EditSave = function () {
    var model = ko.toJS(CustomerCreate.viewModel.CustomerModel);
    $.ajax({
        type: 'put',
        url: '/api/CustomerCreate',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                CustomerCreate.viewModel.ClearSearch();
                Helper.ShowSuccessDialog(Messages.Success);
                $('#editdialog').modal('hide');
                CustomerCreate.viewModel.Search();
            }
        }
    });
};
CustomerCreate.viewModel.GotoPage = function () {
    var model = {
        pageIndex: CustomerCreate.viewModel.Page.CurrentPageIndex(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val()
    };
    $.get("/api/CustomerCreate", model, function (result) {
        ko.mapping.fromJS(result, {}, CustomerCreate.viewModel.Page);
        CustomerCreate.viewModel.UpdatePagination();
    });
};
CustomerCreate.viewModel.Delete = function () {
    var model = ko.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/CustomerCreate/' + model.Id,
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        CustomerCreate.viewModel.Search();
                    }
                }
            });
        }
    });
};
//清空搜索项
CustomerCreate.viewModel.ClearSearch = function() {
    for (var index in CustomerCreate.viewModel.CustomerModel) {
        if (ko.isObservable(CustomerCreate.viewModel.CustomerModel[index])) {
            CustomerCreate.viewModel.CustomerModel[index](null);
        }
    }
    CustomerCreate.viewModel.CustomerModel.CustomerType('请选择');
};
//搜索
CustomerCreate.viewModel.ShowSearch = function () {
    CustomerCreate.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
$(function () {
    ko.applyBindings(CustomerCreate);    
    CustomerCreate.viewModel.Search();
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
var ProductSetting = {
    viewModel: {
        ProductSettingModels: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            ProductModels: ko.observableArray(),
        },
        ProductModel: {
            Id: ko.observable(),
            PartNumber: ko.observable(),
            ProductType: ko.observable(),
            Voltage: ko.observable(),
            Capacity: ko.observable(),
            Pitch: ko.observable(),
            Level: ko.observable(),
            SpecificDesign: ko.observable()
        }

    }
};
//显示特殊设计
ProductSetting.viewModel.ShowSpecificDesign = function () {
    var model = ko.toJS(this);
    Helper.ShowMessageDialog(model.SpecificDesign, model.PartNumber);
};
ProductSetting.viewModel.ShowEdit = function () {
    var model = ko.toJS(this);
    ko.mapping.fromJS(model, {}, ProductSetting.viewModel.ProductModel);
    $('#editdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//删除
ProductSetting.viewModel.Delete = function () {
    var model = ko.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/ProductSetting/' + model.Id,
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        ProductSetting.viewModel.Search();
                    }
                }
            });
        }
    });
};
//保存编辑
ProductSetting.viewModel.EditSave = function () {
    var model = ko.toJS(ProductSetting.viewModel.ProductModel);
    $.ajax({
        type: 'put',
        url: '/api/ProductSetting',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $('#editdialog').modal('hide');
                ProductSetting.viewModel.Search();
            }
        }
    });
};
ProductSetting.viewModel.GotoPage = function () {
    var model = {
        pageIndex: ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val()
    };
    $.get("/api/ProductSetting", model, function (result) {
        ko.mapping.fromJS(result, {}, ProductSetting.viewModel.ProductSettingModels);
        ProductSetting.viewModel.UpdatePagination();
    });
};
//更新页码
ProductSetting.viewModel.UpdatePagination = function () {
    var allPage = ProductSetting.viewModel.ProductSettingModels.AllPage() == 0 ? 1 : ProductSetting.viewModel.ProductSettingModels.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex() });
};
//弹出搜索框
ProductSetting.viewModel.ShowSearch = function() {
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//确定搜索
ProductSetting.viewModel.Search = function () {
    ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex(1);
    var model = {
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val(),
        pageIndex: ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex(),
    };
    $.get("/api/ProductSetting", model, function (result) {
        ko.mapping.fromJS(result, {}, ProductSetting.viewModel.ProductSettingModels);
        ProductSetting.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
ProductSetting.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//新增保存
ProductSetting.viewModel.CreateSave = function () {
    var model = ko.toJS(ProductSetting.viewModel.ProductModel);
    $.post("/api/ProductSetting", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            ProductSetting.viewModel.Search();
            $('#createdialog').modal('hide');
        }
    });

};
$(function () {
    ko.applyBindings(ProductSetting);
    ProductSetting.viewModel.Search();
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
            ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex(num);
            ProductSetting.viewModel.GotoPage();
        }

    });
});
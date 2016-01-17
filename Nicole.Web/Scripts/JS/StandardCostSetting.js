var StandardCostSetting = {
    viewModel: {
        StandardCostSettingModels: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            StandardCostModels: ko.observableArray()
        },
        StandardCostModel: {
            Id: ko.observable(),
            ProductModel: {
                Id: ko.observable(),
                PartNumber:ko.observable(),
            },
            Price: ko.observable(),
            QuotedTime: ko.observable(),
            Remark: ko.observable(),
            UpdateTime: ko.observable(),
            CreatedTime: ko.observable()
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
StandardCostSetting.viewModel.ShowRemark = function () {
    var model = ko.toJS(this);
    Helper.ShowMessageDialog(model.Remark, model.ProductModel.PartNumber);
};
//更新页码
StandardCostSetting.viewModel.UpdatePagination = function () {
    var allPage = StandardCostSetting.viewModel.StandardCostSettingModels.AllPage() == 0 ? 1 : StandardCostSetting.viewModel.StandardCostSettingModels.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: StandardCostSetting.viewModel.StandardCostSettingModels.CurrentPageIndex() });
};
//确定搜索
StandardCostSetting.viewModel.Search = function () {
    StandardCostSetting.viewModel.StandardCostSettingModels.CurrentPageIndex(1);
    var model = {
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val(),
        pageIndex: StandardCostSetting.viewModel.StandardCostSettingModels.CurrentPageIndex(),
    };
    $.get("/api/StandardCostSetting", model, function (result) {
        ko.mapping.fromJS(result, {}, StandardCostSetting.viewModel.StandardCostSettingModels);
        StandardCostSetting.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
StandardCostSetting.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存新增
StandardCostSetting.viewModel.CreateSave = function () {
    var model = ko.toJS(StandardCostSetting.viewModel.StandardCostModel);
    model.QuotedTime = $('#createdialog .date').data().date;
    $.post("/api/StandardCostSetting", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            StandardCostSetting.viewModel.Search();
            $('#createdialog').modal('hide');
        }
    });

};
StandardCostSetting.viewModel.ShowEdit = function () {
    var model = ko.toJS(this);
    ko.mapping.fromJS(model, {}, StandardCostSetting.viewModel.StandardCostModel);
    $('#editdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//保存编辑
StandardCostSetting.viewModel.EditSave = function () {
    var model = ko.toJS(StandardCostSetting.viewModel.StandardCostModel);
    
    $.ajax({
        type: 'put',
        url: '/api/StandardCostSetting',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $('#editdialog').modal('hide');
                StandardCostSetting.viewModel.Search();
            }
        }
    });
};
StandardCostSetting.viewModel.GotoPage = function () {
    var model = {
        pageIndex: StandardCostSetting.viewModel.StandardCostSettingModels.CurrentPageIndex(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val()
    };
    $.get("/api/StandardCostSetting", model, function (result) {
        ko.mapping.fromJS(result, {}, StandardCostSetting.viewModel.StandardCostSettingModels);
        StandardCostSetting.viewModel.UpdatePagination();
    });
};
//显示料号
StandardCostSetting.viewModel.ShowProductModel = function () {
    var model= ko.toJS(this);
    $.get("/api/ProductSetting/" + model.ProductModel.Id, function (result) {
        ko.mapping.fromJS(result, {}, StandardCostSetting.viewModel.ProductModel);
        $('#productdialog').modal({
            show: true,
            backdrop: 'static'
        });       
    });
}
StandardCostSetting.viewModel.Delete = function () {
    var model = ko.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/StandardCostSetting/' + model.Id,
                contentType: 'application/json',
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        StandardCostSetting.viewModel.Search();
                    }
                }
            });
        }
    });
};
//搜索
StandardCostSetting.viewModel.ShowSearch = function () {
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
$(function () {
    ko.applyBindings(StandardCostSetting);
    //初始化moment
    moment.locale('zh-cn');
    //新增选择日期初始化
    $('#createdialog .date').first().datetimepicker({
        locale: 'zh-cn',
        format: 'YYYY/MM/DD'
    });
    StandardCostSetting.viewModel.Search();
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
            StandardCostSetting.viewModel.StandardCostSettingModels.CurrentPageIndex(num);
            StandardCostSetting.viewModel.GotoPage();
        }

    });
});
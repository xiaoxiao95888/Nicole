var EnquiryManager = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            EnquiryModels: ko.observableArray(),
        },
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
            CustomerModel: {
                Code: ko.observable(),
                Name: ko.observable(),
            },

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
EnquiryManager.viewModel.Print = function () {

}
EnquiryManager.viewModel.CreateSave = function () {
    var model = ko.toJS(EnquiryManager.viewModel.EnquiryModel);
    $.post("/api/EnquiryManager", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            EnquiryManager.viewModel.Search();
            $('#createdialog').modal('hide');
        }
    });
}
//显示特殊设计
EnquiryManager.viewModel.ShowSpecificDesign = function () {
    var model = ko.toJS(this);
    Helper.ShowMessageDialog(model.SpecificDesign, model.PartNumber);
};
EnquiryManager.viewModel.GotoPage = function () {
    var model = {
        pageIndex: ProductSetting.viewModel.ProductSettingModels.CurrentPageIndex(),
        CustomerNameKey: $('#CustomerNameKey').val(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val(),
    };
    $.get("/api/EnquiryManager", model, function (result) {
        ko.mapping.fromJS(result, {}, EnquiryManager.viewModel.Page);
        EnquiryManager.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
EnquiryManager.viewModel.UpdatePagination = function () {
    var allPage = EnquiryManager.viewModel.Page.AllPage() == 0 ? 1 : EnquiryManager.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: EnquiryManager.viewModel.Page.CurrentPageIndex() });
}
//确定搜索
EnquiryManager.viewModel.Search = function () {
    EnquiryManager.viewModel.Page.CurrentPageIndex(1);
    var model = {
        CustomerNameKey: $('#CustomerNameKey').val(),
        ProductTypeKey: $('#ProductTypeKey').val(),
        PartNumberKey: $('#PartNumberKey').val(),
        VoltageKey: $('#VoltageKey').val(),
        CapacityKey: $('#CapacityKey').val(),
        PitchKey: $('#PitchKey').val(),
        LevelKey: $('#LevelKey').val(),
        SpecificDesignKey: $('#SpecificDesignKey').val(),
        pageIndex: EnquiryManager.viewModel.Page.CurrentPageIndex(),
    };
    $.get("/api/EnquiryManager", model, function (result) {
        ko.mapping.fromJS(result, {}, EnquiryManager.viewModel.Page);
        EnquiryManager.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
}
//弹出搜索框
EnquiryManager.viewModel.ShowSearch = function () {
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
}
EnquiryManager.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
}
$(function () {
    ko.applyBindings(EnquiryManager);
    EnquiryManager.viewModel.Search();
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
            EnquiryManager.viewModel.Page.CurrentPageIndex(num);
            EnquiryManager.viewModel.GotoPage();
        }

    });
});
var EnquirySetting = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            EnquiryModels: ko.observableArray()
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
            CustomerModel: {
                Id:ko.observable(),
                Code: ko.observable(),
                Name: ko.observable()
            }

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
EnquirySetting.viewModel.Print = function () {

}
EnquirySetting.viewModel.SearchForProduct = function() {
    var model = ko.mapping.toJS(EnquirySetting.viewModel.EnquiryModel.ProductModel);
    $.get('/api/ProductSearch', model, function(result) {
        ko.mapping.fromJS(result.ProductModels[0], {}, EnquirySetting.viewModel.EnquiryModel.ProductModel);
    });
};
EnquirySetting.viewModel.SearchForCustomer = function () {
    var model = ko.mapping.toJS(EnquirySetting.viewModel.EnquiryModel.CustomerModel);
    var mapping = {
        'ignore': ["PositionModels"]
    }
    $.get('/api/Customer', model, function (result) {
        // EnquirySetting.viewModel.EnquiryModel.CustomerModel
        
        var data = ko.mapping.fromJS(result.Models[0], mapping);
        ko.mapping.fromJS(data, {}, EnquirySetting.viewModel.EnquiryModel.CustomerModel);

    });
};
EnquirySetting.viewModel.SearchForEmployee = function () {
    var model = ko.mapping.toJS(EnquirySetting.viewModel.EnquiryModel.PositionModel.CurrentEmployeeModel);
    $.get('/api/Position', model, function (result) {
        ko.mapping.fromJS(result[0], {}, EnquirySetting.viewModel.EnquiryModel.PositionModel);
    });
};
//EnquirySetting.viewModel.CreateSave = function () {
//    var model = ko.mapping.toJS(EnquirySetting.viewModel.EnquiryModel);
//    $.post("/api/EnquirySetting", model, function (result) {
//        if (result.Error) {
//            Helper.ShowErrorDialog(result.Message);
//        } else {
//            Helper.ShowSuccessDialog(Messages.Success);
//            EnquirySetting.viewModel.Search();
//            $('#createdialog').modal('hide');
//        }
//    });
//}

EnquirySetting.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(EnquirySetting.viewModel.EnquiryModel);
    model.pageIndex = EnquirySetting.viewModel.Page.CurrentPageIndex();
    $.get("/api/EnquirySetting", model, function (result) {
        ko.mapping.fromJS(result, {}, EnquirySetting.viewModel.Page);
        EnquirySetting.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
EnquirySetting.viewModel.UpdatePagination = function () {
    var allPage = EnquirySetting.viewModel.Page.AllPage() == 0 ? 1 : EnquirySetting.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: EnquirySetting.viewModel.Page.CurrentPageIndex() });
}
//确定搜索
EnquirySetting.viewModel.Search = function () {
    EnquirySetting.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJSON(EnquirySetting.viewModel.EnquiryModel);
    model.pageIndex = 1;
    $.get("/api/EnquirySetting", model, function (result) {
        ko.mapping.fromJS(result, {}, EnquirySetting.viewModel.Page);
        EnquirySetting.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
}
//弹出搜索框
EnquirySetting.viewModel.ShowSearch = function () {
    EnquirySetting.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: "static"
    });
}
EnquirySetting.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
}
//清空搜索项
EnquirySetting.viewModel.ClearSearch = function () {
    for (var index in EnquirySetting.viewModel.EnquiryModel) {
        if (ko.isObservable(EnquirySetting.viewModel.EnquiryModel[index])) {
            EnquirySetting.viewModel.EnquiryModel[index](null);
        }
    }
};
$(function () {
    ko.applyBindings(EnquirySetting);
    EnquirySetting.viewModel.Search();
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
            EnquirySetting.viewModel.Page.CurrentPageIndex(num);
            EnquirySetting.viewModel.GotoPage();
        }

    });
});
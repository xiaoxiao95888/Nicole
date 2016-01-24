var MyEnquiry = {
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
                Id: ko.observable(),
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
MyEnquiry.viewModel.Print = function () {

}
//MyEnquiry.viewModel.CreateSave = function () {
//    var model = ko.mapping.toJS(MyEnquiry.viewModel.EnquiryModel);
//    $.post("/api/MyEnquiry", model, function (result) {
//        if (result.Error) {
//            Helper.ShowErrorDialog(result.Message);
//        } else {
//            Helper.ShowSuccessDialog(Messages.Success);
//            MyEnquiry.viewModel.Search();
//            $('#createdialog').modal('hide');
//        }
//    });
//}

MyEnquiry.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    model.pageIndex = CustomerCreate.viewModel.Page.CurrentPageIndex();
    $.get("/api/MyEnquiry", model, function (result) {
        ko.mapping.fromJS(result, {}, MyEnquiry.viewModel.Page);
        MyEnquiry.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};
MyEnquiry.viewModel.UpdatePagination = function () {
    var allPage = MyEnquiry.viewModel.Page.AllPage() == 0 ? 1 : MyEnquiry.viewModel.Page.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: MyEnquiry.viewModel.Page.CurrentPageIndex() });
}
//确定搜索
MyEnquiry.viewModel.Search = function () {
    MyEnquiry.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(CustomerCreate.viewModel.CustomerModel);
    model.pageIndex = 1;
    $.get("/api/MyEnquiry", model, function (result) {
        ko.mapping.fromJS(result, {}, MyEnquiry.viewModel.Page);
        MyEnquiry.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
}
//弹出搜索框
MyEnquiry.viewModel.ShowSearch = function () {
    MyEnquiry.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
}
MyEnquiry.viewModel.ShowCreate = function () {
    $('#createdialog').modal({
        show: true,
        backdrop: 'static'
    });
}
//清空搜索项
MyEnquiry.viewModel.ClearSearch = function () {
    for (var index in MyEnquiry.viewModel.EnquiryModel) {
        if (ko.isObservable(MyEnquiry.viewModel.EnquiryModel[index])) {
            MyEnquiry.viewModel.EnquiryModel[index](null);
        }
    }
};
$(function () {
    ko.applyBindings(MyEnquiry);
    MyEnquiry.viewModel.Search();
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
            MyEnquiry.viewModel.Page.CurrentPageIndex(num);
            MyEnquiry.viewModel.GotoPage();
        }

    });
});
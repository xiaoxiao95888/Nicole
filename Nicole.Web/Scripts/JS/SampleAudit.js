var SampleSettingModel = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        SampleModel: {
            Id: ko.observable(),
            IsApproved: ko.observable(),
            Code: ko.observable(),
            PositionModel: {
                Id: ko.observable(),
                Name: ko.observable()
            },
            CustomerModel: {
                Id: ko.observable(),
                Name: ko.observable(),
                Code: ko.observable()
            },
            ProductModel: {
                Id: ko.observable(),
                PartNumber: ko.observable()
            },
            Qty: ko.observable(),
            Remark: ko.observable(),
            CreatedTime: ko.observable()
        },
        SearchSampleModel: {
            //客户编号
            CustomerCode: ko.observable(),
            //客户编号名称
            CustomerName: ko.observable(),
            //料号
            PartNumber: ko.observable(),
            PositionId: ko.observable(),
            //是否通过审核
            IsApproved: ko.observable(),
            //申请编号
            Code: ko.observable()
        }
    }
};
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = {
            locale: "zh-cn",
            format: "YYYY年MM月DD日"
        };
        $(element).datetimepicker(options).on("dp.change", function (ev) {
            var observable = valueAccessor();
            observable(ev.date.toJSON());
        });
    }
};
ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || "YYYY/MM/DD";

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

SampleSettingModel.viewModel.UpdatePagination = function () {
    var allPage = SampleSettingModel.viewModel.Page.AllPage() === 0 ? 1 : SampleSettingModel.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: SampleSettingModel.viewModel.Page.CurrentPageIndex() });
};
SampleSettingModel.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(SampleSettingModel.viewModel.SearchSampleModel);
    model.pageIndex = SampleSettingModel.viewModel.Page.CurrentPageIndex();
    $.get("/api/SampleReview", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleSettingModel.viewModel.Page);
        SampleSettingModel.viewModel.UpdatePagination();
    });
};
SampleSettingModel.viewModel.ShowSearch = function () {
    $("#searchdialog").modal({
        show: true,
        backdrop: "static"
    });
}
//确定搜索
SampleSettingModel.viewModel.Search = function () {
    SampleSettingModel.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(SampleSettingModel.viewModel.SearchSampleModel);
    $.get("/api/SampleReview", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleSettingModel.viewModel.Page);
        SampleSettingModel.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};

//提交
SampleSettingModel.viewModel.Submit = function () {
    var model = ko.mapping.toJS(SampleSettingModel.viewModel.SampleModel);
    $.ajax({
        type: "post",
        url: "/api/Sample",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(model),
        success: function (result) {
            if (result.Error) {
                Helper.ShowErrorDialog(result.Message);
            } else {
                Helper.ShowSuccessDialog(Messages.Success);
                $("#createdialog").modal("hide");
                SampleSettingModel.viewModel.GotoPage();
            }
        }
    });

};
$(function () {
    ko.applyBindings(SampleSettingModel);
    SampleSettingModel.viewModel.Search();
    //初始化页码
    $("#page-selection").bootpag({
        total: 1,
        page: 1,
        maxVisible: 5,
        leaps: true,
        firstLastUse: true,
        first: "First",
        last: "Last",
        wrapClass: "pagination",
        activeClass: "active",
        disabledClass: "disabled",
        nextClass: "next",
        prevClass: "prev",
        lastClass: "last",
        firstClass: "first"
    }).on("page", function (event, num) {
        if (num != null) {
            SampleSettingModel.viewModel.Page.CurrentPageIndex(num);
            SampleSettingModel.viewModel.GotoPage();
        }
    });
});
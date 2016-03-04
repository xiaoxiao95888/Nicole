var SampleRecord = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        SearchSampleModel: {
            //客户编号
            CustomerCode: ko.observable(),
            //客户编号名称
            CustomerName: ko.observable(),
            //料号
            PositionId: ko.observable(),
            //是否通过审核
            IsApproved: ko.observable(),
            //申请编号
            Code: ko.observable()
        },
        CustomerModels: ko.observableArray(),
        ProductModels: ko.observableArray(),
        PostionModels: ko.observableArray(),
        SelectPositionModel: ko.observable()
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
SampleRecord.viewModel.UpdatePagination = function () {
    var allPage = SampleRecord.viewModel.Page.AllPage() === 0 ? 1 : SampleRecord.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: SampleRecord.viewModel.Page.CurrentPageIndex() });
};
SampleRecord.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(SampleRecord.viewModel.SearchSampleModel);
    var selectPositionModel = ko.mapping.toJS(SampleRecord.viewModel.SelectPositionModel);
    if (selectPositionModel != null) {
        model.PositionId = selectPositionModel.Id;
    }
    model.pageIndex = SampleRecord.viewModel.Page.CurrentPageIndex();
    $.get("/api/Sample", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleRecord.viewModel.Page);
        SampleRecord.viewModel.UpdatePagination();
    });
};
SampleRecord.viewModel.ShowSearch = function () {
    $.get("/api/Position/", function (result) {
        ko.mapping.fromJS(result, {}, SampleRecord.viewModel.PostionModels);
        $("#searchdialog").modal({
            show: true,
            backdrop: "static"
        });
    });
}
//确定搜索
SampleRecord.viewModel.Search = function () {
    SampleRecord.viewModel.Page.CurrentPageIndex(1);
    var model = ko.mapping.toJS(SampleRecord.viewModel.SearchSampleModel);
    var selectPositionModel = ko.mapping.toJS(SampleRecord.viewModel.SelectPositionModel);
    if (selectPositionModel != null) {
        model.PositionId = selectPositionModel.Id;
    }
    $.get("/api/SampleRecord", model, function (result) {
        ko.mapping.fromJS(result, {}, SampleRecord.viewModel.Page);
        SampleRecord.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
$(function () {
    ko.applyBindings(SampleRecord);
    SampleRecord.viewModel.Search();
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
            SampleRecord.viewModel.Page.CurrentPageIndex(num);
            SampleRecord.viewModel.GotoPage();
        }
    });
});
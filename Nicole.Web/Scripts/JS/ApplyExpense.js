var ApplyExpense = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        ApplyExpenseModel: {
            Id: ko.observable(),
            //报销日期
            Date: ko.observable(),
            //报销金额
            Amount: ko.observable(),
            //类别
            ApplyExpenseTypeModel: {
                Id: ko.observable(),
                Name: ko.observable()
            },
            //报销人
            ConcernedPositionModel: {
                Id: ko.observable(),
                Name: ko.observable()
            },
            //报销明细
            Detail: ko.observable(),
            IsApproved: ko.observable()
        },
        SearchApplyExpenseModel: {
            //报销日期
            DateRangeModel: {
                From: ko.observable(),
                To: ko.observable()
            },
            //类别
            ApplyExpenseTypeModel: ko.observable(),
            //报销人
            ConcernedPositionModel: ko.observable(),
            //报销明细
            Detail: ko.observable(),
            IsApproved: ko.observable()
        },
        ApplyExpenseTypeModels: ko.observableArray(),
        ConcernedPositionModels: ko.observableArray()
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

ApplyExpense.viewModel.UpdatePagination = function () {
    var allPage = ApplyExpense.viewModel.Page.AllPage() === 0 ? 1 : ApplyExpense.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: ApplyExpense.viewModel.Page.CurrentPageIndex() });
};
ApplyExpense.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(ApplyExpense.viewModel.SearchApplyExpenseModel);
    var dateRangeModel = {
        From: $("#datefrom").data().date,
        To: $("#dateto").data().date
    }
    model.DateRangeModel = dateRangeModel;
    model.pageIndex = ApplyExpense.viewModel.Page.CurrentPageIndex();
    $.get("/api/ApplyExpense", model, function (result) {
        ko.mapping.fromJS(result, {}, ApplyExpense.viewModel.Page);
        ApplyExpense.viewModel.UpdatePagination();
    });
};
ApplyExpense.viewModel.ShowSearch = function () {
    $.get("/api/ApplyExpenseType/", function (types) {
        ko.mapping.fromJS(types, {}, ApplyExpense.viewModel.ApplyExpenseTypeModels);
        $.get("/api/Position/", function (data) {
            ko.mapping.fromJS(data, {}, ApplyExpense.viewModel.ConcernedPositionModels);
            $("#searchdialog").modal({
                show: true,
                backdrop: "static"
            });
        });
    });
}
//确定搜索
ApplyExpense.viewModel.Search = function () {
    ApplyExpense.viewModel.Page.CurrentPageIndex(1);
    var dateRangeModel = {
        From: $("#datefrom").data().date,
        To: $("#dateto").data().date
    }
    var model = ko.mapping.toJS(ApplyExpense.viewModel.SearchApplyExpenseModel);
    model.DateRangeModel = dateRangeModel;
    $.get("/api/ApplyExpense", model, function (result) {
        ko.mapping.fromJS(result, {}, ApplyExpense.viewModel.Page);
        ApplyExpense.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//新增
ApplyExpense.viewModel.ShowCreate = function () {
    $.get("/api/ApplyExpenseType/", function (types) {
        ko.mapping.fromJS(types, {}, ApplyExpense.viewModel.ApplyExpenseTypeModels);
        $.get("/api/Position/", function (data) {
            ko.mapping.fromJS(data, {}, ApplyExpense.viewModel.ConcernedPositionModels);
            $("#createdialog").modal({
                show: true,
                backdrop: "static"
            });
        });
    });
};
//新增保存
ApplyExpense.viewModel.CreateSave = function () {
    var model = ko.mapping.toJS(ApplyExpense.viewModel.ApplyExpenseModel);
    $.post("/api/ApplyExpense/", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            ApplyExpense.viewModel.GotoPage();
            $("#createdialog").modal("hide");

        }
    });
};

$(function () {
    ko.applyBindings(ApplyExpense);
    ApplyExpense.viewModel.Search();
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
            ApplyExpense.viewModel.Page.CurrentPageIndex(num);
            ApplyExpense.viewModel.GotoPage();
        }

    });
    $("#datefrom").datetimepicker({
        locale: "zh-cn",
        format: "YYYY年MM月DD日"
    });
    $("#dateto").datetimepicker({
        locale: "zh-cn",
        format: "YYYY年MM月DD日",
        useCurrent: false //Important! See issue #1075
    });
    $("#datefrom").on("dp.change", function (e) {
        $("#dateto").data("DateTimePicker").minDate(e.date);
    });
    $("#dateto").on("dp.change", function (e) {
        $("#datefrom").data("DateTimePicker").maxDate(e.date);
    });
});
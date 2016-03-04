var ApplyExpenseAudit = {
    viewModel: {
        Page: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            Models: ko.observableArray()
        },
        ApplyExpenseAuditModel: {
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
                Name: ko.observable(),
                ConcernedPositionModel:ko.observable()
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

ApplyExpenseAudit.viewModel.UpdatePagination = function () {
    var allPage = ApplyExpenseAudit.viewModel.Page.AllPage() === 0 ? 1 : ApplyExpenseAudit.viewModel.Page.AllPage();
    $("#page-selection").bootpag({ total: allPage, maxVisible: 10, page: ApplyExpenseAudit.viewModel.Page.CurrentPageIndex() });
};
ApplyExpenseAudit.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(ApplyExpenseAudit.viewModel.SearchApplyExpenseModel);
    var dateRangeModel = {
        From: $("#datefrom").data().date,
        To: $("#dateto").data().date
    }
    model.DateRangeModel = dateRangeModel;
    model.pageIndex = ApplyExpenseAudit.viewModel.Page.CurrentPageIndex();
    $.get("/api/ApplyExpense", model, function (result) {
        ko.mapping.fromJS(result, {}, ApplyExpenseAudit.viewModel.Page);
        ApplyExpenseAudit.viewModel.UpdatePagination();
    });
};
ApplyExpenseAudit.viewModel.ShowSearch = function () {
    $.get("/api/ApplyExpenseType/", function (types) {
        ko.mapping.fromJS(types, {}, ApplyExpenseAudit.viewModel.ApplyExpenseTypeModels);
        $.get("/api/Position/", function (data) {
            ko.mapping.fromJS(data, {}, ApplyExpenseAudit.viewModel.ConcernedPositionModels);
            $("#searchdialog").modal({
                show: true,
                backdrop: "static"
            });
        });
    });
}
//确定搜索
ApplyExpenseAudit.viewModel.Search = function () {
    ApplyExpenseAudit.viewModel.Page.CurrentPageIndex(1);
    var dateRangeModel = {
        From: $("#datefrom").data().date,
        To: $("#dateto").data().date
    }
    var model = ko.mapping.toJS(ApplyExpenseAudit.viewModel.SearchApplyExpenseModel);
    model.DateRangeModel = dateRangeModel;
    $.get("/api/ApplyExpense", model, function (result) {
        ko.mapping.fromJS(result, {}, ApplyExpenseAudit.viewModel.Page);
        ApplyExpenseAudit.viewModel.UpdatePagination();
        $("#searchdialog").modal("hide");
    });
};
//通过审核
ApplyExpenseAudit.viewModel.Approved = function() {
    var model = ko.mapping.toJS(this);
    model.IsApproved = true;
    Helper.ShowConfirmationDialog({
        message: "是否确认通过?",
        confirmFunction: function () {
            $.ajax({
                type: "put",
                url: "/api/ApplyExpense",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(model),
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        ApplyExpenseAudit.viewModel.GotoPage();
                    }
                }
            });
        }
    });
};
$(function () {
    ko.applyBindings(ApplyExpenseAudit);
    ApplyExpenseAudit.viewModel.Search();
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
            ApplyExpenseAudit.viewModel.Page.CurrentPageIndex(num);
            ApplyExpenseAudit.viewModel.GotoPage();
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
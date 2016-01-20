var LeftNavigationSetting = {
    viewModel: {
        PositionModels: ko.observableArray(),
        LeftNavigationModels: ko.observableArray(),
        PositionLeftNavigationModels: ko.observableArray(),
        SelectedLeftNavigationModel: ko.observable(),
        SelectedPositionModel: ko.observable()
    }
};
LeftNavigationSetting.viewModel.loadpage = function () {
    $.get("/api/PositionLeftNavigation", function (result) {
        ko.mapping.fromJS(result, {}, LeftNavigationSetting.viewModel.PositionLeftNavigationModels);
    });
};

LeftNavigationSetting.viewModel.Delete = function () {
    var model = ko.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/PositionLeftNavigation/',
                contentType: 'application/json',
                data: JSON.stringify(model),
                dataType: "json",
                success: function (result) {
                    if (result.Error) {
                        Helper.ShowErrorDialog(result.Message);
                    } else {
                        Helper.ShowSuccessDialog(Messages.Success);
                        LeftNavigationSetting.viewModel.loadpage();
                    }
                }
            });
        }
    });
};
LeftNavigationSetting.viewModel.ShowCreate = function () {
    $.get("/api/Position", function (positions) {
        ko.mapping.fromJS(positions, {}, LeftNavigationSetting.viewModel.PositionModels);
        $.get("/api/LeftNavigation", function (leftNavigations) {
            ko.mapping.fromJS(leftNavigations, {}, LeftNavigationSetting.viewModel.LeftNavigationModels);
            $('#createdialog').modal({
                show: true,
                backdrop: 'static'
            });
        });
    });
};
LeftNavigationSetting.viewModel.CreateSave = function () {
    var model = {
        PositionModel: ko.toJS(LeftNavigationSetting.viewModel.SelectedPositionModel),
        LeftNavigationModel: ko.toJS(LeftNavigationSetting.viewModel.SelectedLeftNavigationModel)
    };
    model.LeftNavigationModel.SubModels = [];
    $.post("/api/PositionLeftNavigation", model, function (result) {
        if (result.Error) {
            Helper.ShowErrorDialog(result.Message);
        } else {
            Helper.ShowSuccessDialog(Messages.Success);
            LeftNavigationSetting.viewModel.loadpage();
            $('#createdialog').modal('hide');
        }
    });
};
$(function () {
    ko.applyBindings(LeftNavigationSetting);
    LeftNavigationSetting.viewModel.loadpage();
});
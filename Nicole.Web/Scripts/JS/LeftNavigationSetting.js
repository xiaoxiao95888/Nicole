var LeftNavigationSetting = {
    viewModel: {
        RoleModels: ko.observableArray(),
        LeftNavigationModels: ko.observableArray(),
        RoleLeftNavigationModels: ko.observableArray(),
        SelectedLeftNavigationModel: ko.observable(),
        SelectedRoleModel: ko.observable()
    }
};
LeftNavigationSetting.viewModel.loadpage = function () {
    $.get("/api/RoleLeftNavigation", function (result) {
        ko.mapping.fromJS(result, {}, LeftNavigationSetting.viewModel.RoleLeftNavigationModels);
    });
};

LeftNavigationSetting.viewModel.Delete = function () {
    var model = ko.toJS(this);
    Helper.ShowConfirmationDialog({
        message: "是否确认删除?",
        confirmFunction: function () {
            $.ajax({
                type: 'delete',
                url: '/api/RoleLeftNavigation/',
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
    $.get("/api/Role", function (positions) {
        ko.mapping.fromJS(positions, {}, LeftNavigationSetting.viewModel.RoleModels);
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
        RoleModel: ko.toJS(LeftNavigationSetting.viewModel.SelectedRoleModel),
        LeftNavigationModel: ko.toJS(LeftNavigationSetting.viewModel.SelectedLeftNavigationModel)
    };
    model.LeftNavigationModel.SubModels = [];
    $.post("/api/RoleLeftNavigation", model, function (result) {
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
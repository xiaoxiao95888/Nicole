var ProductSetting = {
    viewModel: {
        ProductTypes: ko.observableArray(),        
    }
};
ProductSetting.viewModel.load = function () {
    $.get("/api/ProductSetting", function (result) {
        ko.mapping.fromJS(result, {}, ProductSetting.viewModel.ProductTypes);
    });
};


$(function () {
    ko.applyBindings(ProductSetting);
    ProductSetting.viewModel.load();
});
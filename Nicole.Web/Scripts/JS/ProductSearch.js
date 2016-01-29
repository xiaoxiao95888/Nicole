var ProductSearch = {
    viewModel: {
        ProductSearchModels: {
            CurrentPageIndex: ko.observable(1),
            AllPage: ko.observable(1),
            ProductModels: ko.observableArray(),
        },
        ProductModel: {
            Id: ko.observable(),
            PartNumber: ko.observable(),
            ProductType: ko.observable(),
            Voltage: ko.observable(),
            Capacity: ko.observable(),
            Pitch: ko.observable(),
            Level: ko.observable(),
            SpecificDesign: ko.observable(),
            Price: ko.observable()
        }

    }
};
ProductSearch.viewModel.GotoPage = function () {
    var model = ko.mapping.toJS(ProductSearch.viewModel.ProductModel);
    model.pageIndex = ProductSearch.viewModel.ProductSearchModels.CurrentPageIndex();
    $.get("/api/ProductSearch", model, function (result) {
        ko.mapping.fromJS(result, {}, ProductSearch.viewModel.ProductSearchModels);
        ProductSearch.viewModel.UpdatePagination();
    });
};
//更新页码
ProductSearch.viewModel.UpdatePagination = function () {
    var allPage = ProductSearch.viewModel.ProductSearchModels.AllPage() === 0 ? 1 : ProductSearch.viewModel.ProductSearchModels.AllPage();
    $('#page-selection').bootpag({ total: allPage, maxVisible: 10, page: ProductSearch.viewModel.ProductSearchModels.CurrentPageIndex() });
};
//弹出搜索框
ProductSearch.viewModel.ShowSearch = function () {
    ProductSearch.viewModel.ClearSearch();
    $('#searchdialog').modal({
        show: true,
        backdrop: 'static'
    });
};
//确定搜索
ProductSearch.viewModel.Search = function () {
    ProductSearch.viewModel.ProductSearchModels.CurrentPageIndex(1);
    var model = ko.mapping.toJS(ProductSearch.viewModel.ProductModel);
    model.pageIndex = 1;
    $.get("/api/ProductSearch", model, function (result) {
        ko.mapping.fromJS(result, {}, ProductSearch.viewModel.ProductSearchModels);
        ProductSearch.viewModel.UpdatePagination();
        $('#searchdialog').modal('hide');
    });
};

//清空搜索项
ProductSearch.viewModel.ClearSearch = function () {
    var model = ko.mapping.toJS(ProductSearch.viewModel.ProductModel);
    Helper.ClearObject(model);
    ko.mapping.fromJS(model, {}, ProductSearch.viewModel.ProductModel);
};
$(function () {
    ko.applyBindings(ProductSearch);
    ProductSearch.viewModel.Search();
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
            ProductSearch.viewModel.ProductSearchModels.CurrentPageIndex(num);
            ProductSearch.viewModel.GotoPage();
        }

    });
});
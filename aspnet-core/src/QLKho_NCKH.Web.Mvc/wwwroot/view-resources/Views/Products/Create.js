(function ($) {
  var _productModal = $('#ProductCreateModal');
  var _addCategoryModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Categories/AddCategory',
    scriptUrl: abp.appPath + 'view-resources/Views/Categories/_AddCategoryModal.js',
    modalClass: 'AddCategoryModal'
  });

   //Bắt sự kiện click vào nút addCategory
  _productModal.on('click', '#addCategory', function () {
    console.log('Add Category button clicked'); // Kiểm tra trong Console
    _addCategoryModal.open({}, function (selectedCategory) {
      if (selectedCategory) {
        // Cập nhật giá trị vào input khi chọn category
        _productModal.find('input[name="CategoryId"]').val(selectedCategory.id);
        _productModal.find('input[name="CategoryId"]').attr('data-name', selectedCategory.name);
      }
    });
  });


  // Xử lý khi modal đóng
  _addCategoryModal.onClose(function () {
    // Có thể thêm logic xử lý khi modal đóng nếu cần
  });

})(jQuery);
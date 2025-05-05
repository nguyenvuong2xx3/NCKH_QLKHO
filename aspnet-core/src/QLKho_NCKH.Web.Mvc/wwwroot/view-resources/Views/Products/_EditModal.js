(function ($) {
  var _productService = abp.services.app.product,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#ProductEditModal'),
    _$form = _$modal.find('form[name=productEditForm]');

  // Modal Supplier
  var _addSupplierCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Suppliers/AddSupplier',
    scriptUrl: abp.appPath + 'view-resources/Views/Suppliers/_AddSupplierModal.js',
    modalClass: 'AddSupplierModal',
  });

  // Modal Category
  var _addCategoryCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Categories/AddCategory',
    scriptUrl: abp.appPath + 'view-resources/Views/Categories/_AddCategoryModal.js',
    modalClass: 'AddCategoryModal',
  });

  // Xử lý sự kiện tạo danh mục
  $('#EditCategoryBtn').on('click', function () {
    _addCategoryCreateModal.open({}, function (result) {
      if (result) {
        $('#CategoryDisplayEdit').val(result.categoryName.trim());
        $('#CategoryIdEdit').val(result.categoryId);
        _addCategoryEditModal.close(); // Đóng modal con
      }
    });
  });

  // Xử lý sự kiện thêm nhà cung cấp
  $('#EditSupplierBtn').on('click', function () {
    _addSupplierCreateModal.open({}, function (result) {
      if (result) {
        $('#SupplierDisplayEdit').val(result.supplierName.trim());
        $('#SupplierIdEdit').val(result.supplierId);
        _addSupplierEditModal.close(); // Đóng modal con
      }
    });
  });

  function save() {
    if (!_$form.valid()) {
      return;
    }

    var formData = new FormData(_$form[0]);
    var imageFile = $('#newImage')[0].files[0];

    if (imageFile) {
      formData.append('ImageFile', imageFile);
    }

    abp.ui.setBusy(_$modal);
    _productService.update(formData).done(function (result) {
      _$modal.modal('hide');
      abp.notify.info(l('SavedSuccessfully'));
      abp.event.trigger('product.edited', result);
    }).fail(function (error) {
      var errorMessage = error.message || "Có lỗi xảy ra khi cập nhật sản phẩm";
      if (error.details) {
        errorMessage += ": " + error.details;
      }
      abp.notify.error(errorMessage);
    }).always(function () {
      abp.ui.clearBusy(_$modal);
    });
  }

  // Xử lý sự kiện xóa ảnh sản phẩm
  $('#deleteImageBtn').on('click', function () {
    abp.message.confirm(
      "Bạn có chắc chắn muốn xóa ảnh này?",
      "Xác nhận",
      function (isConfirmed) {
        if (isConfirmed) {
          abp.ui.setBusy();
          $.ajax({
            url: abp.appPath + 'Products/DeleteImage',
            type: 'POST',
            data: { productId: $('input[name="Id"]').val() },
            success: function (response) {
              if (response.success) {
                $('#productImage').attr('src', '/img/products/default_image.png'); // Hiển thị ảnh mặc định
                abp.notify.info("Ảnh sản phẩm đã được xóa thành công."); // Cập nhật thông báo
                abp.event.trigger('product.edited', response);
              } else {
                abp.notify.error(response.message);
              }
            },
            error: function () {
              abp.notify.error("Đã có lỗi xảy ra, vui lòng thử lại.");
            },
            complete: function () {
              abp.ui.clearBusy();
            }
          });
        }
      }
    );
  });

  // Xem trước ảnh khi chọn file mới
  $('#newImage').on('change', function (event) {
    var file = event.target.files[0];
    if (file) {
      var reader = new FileReader();
      reader.onload = function (e) {
        $('#productImage').attr('src', e.target.result);
      };
      reader.readAsDataURL(file);
    }
  });

  // Xử lý sự kiện khi nhấn nút "Lưu"
  _$form.closest('div.modal-content').find(".save-button").click(function (e) {
    e.preventDefault();
    save();
  });

  // Xử lý sự kiện khi nhấn Enter trong form
  _$form.find('input').on('keypress', function (e) {
    if (e.which === 13) {
      e.preventDefault();
      save();
    }
  });

  // Tự động focus vào ô input đầu tiên khi mở modal
  _$modal.on('shown.bs.modal', function () {
    _$form.find('input[type=text]:first').focus();
  });

  // Đảm bảo cuộn nội dung trong modal
  $(document).on('hidden.bs.modal', '.modal', function () {
    if ($('.modal.show').length) {
      $('body').addClass('modal-open');
    }
  });

})(jQuery);
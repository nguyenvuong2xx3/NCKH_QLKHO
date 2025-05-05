(function ($) {
  var _productService = abp.services.app.product,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#ProductEditModal'),
    _$form = _$modal.find('form');

  // Modal Supplier
  var _addSupplierEditModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Suppliers/AddSupplier',
    scriptUrl: abp.appPath + 'view-resources/Views/Suppliers/_AddSupplierModal.js',
    modalClass: 'AddSupplierModal',
  });

  // Modal Category
  var _addCategoryEditModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Categories/AddCategory',
    scriptUrl: abp.appPath + 'view-resources/Views/Categories/_AddCategoryModal.js',
    modalClass: 'AddCategoryModal',
  });

  // Xử lý sự kiện tạo danh mục
  $('#EditCategoryBtn').on('click', function () {
    _addCategoryEditModal.open({}, function (result) {
      if (result) {
        $('#CategoryDisplayEdit').val(result.categoryName.trim());
        $('#CategoryIdEdit').val(result.categoryId);
        _addCategoryCreateModal.close(); // Đóng modal con
      }
    });
  });

  // Xử lý sự kiện thêm nhà cung cấp
  $('#EditSupplierBtn').on('click', function () {
    _addSupplierEditModal.open({}, function (result) {
      if (result) {
        $('#SupplierDisplayEdit').val(result.supplierName.trim());
        $('#SupplierIdEdit').val(result.supplierId);
        _addSupplierCreateModal.close(); // Đóng modal con
      }
    });
  });


  // Đảm bảo cuộn nội dung trong modal con khi modal con được mở
  $(document).on('hidden.bs.modal', '.modal', function () {
    if ($('.modal.show').length) {
      $('body').addClass('modal-open'); // Đảm bảo body không bị cuộn khi có modal khác đang mở
    }
  });


  // Khôi phục khả năng cuộn sau khi đóng modal con
  $(document).on('hidden.bs.modal', '.modal', function () {
    if ($('#ProductEditModal').hasClass('show')) {
      $('#ProductEditModal .modal-content').css('overflow-y', 'auto');
    }
  });


  

  function save() {
    if (!_$form.valid()) {
      return;
    }

    var formData = new FormData(_$form[0]);

    // Lấy thông tin về file ảnh
    var imageInput = document.getElementById('newImage');
    var imageFile = imageInput.files[0];

    // Thêm file ảnh vào FormData nếu đã chọn
    if (imageFile) {
      formData.append('ImagePath', imageFile);
    }

    abp.ui.setBusy(_$modal);
    $.ajax({
      url: abp.appPath + 'Products/EditAndUploadDeleteImage', // Đường dẫn đến phương thức trong controller
      type: 'POST',
      processData: false, // Quan trọng!
      contentType: false, // Quan trọng!
      data: formData,
      error: function (xhr, textStatus, errorThrown) {
        var errorMessage;
        if (xhr.responseJSON && xhr.responseJSON.errors && xhr.responseJSON.errors.length > 0) {
          errorMessage = xhr.responseJSON.errors.join("<br/>");
        } else {
          errorMessage = "Có lỗi xảy ra khi cập nhật sản phẩm (Có thể do upload ảnh không đúng định dạng .jpg, .jpeg, .png, .gif)";
        }
        $("#error-message").html(errorMessage).show();
      }
    }).done(function (customer) {
      _$modal.modal('hide');
      _$form[0].reset();
      abp.notify.info(l('Lưu thành công'));
      abp.event.trigger('product.edited', customer);
    }).always(function () {
      abp.ui.clearBusy(_$modal);

    });
  }

  // Xử lý sự kiện khi nhấn nút "Lưu"
  _$form.closest('div.modal-content').find(".save-button").click(function (e) {
    e.preventDefault(); // Ngăn chặn reload trang
    save(); // Gọi hàm save() để cập nhật sản phẩm
  });

  // Xử lý sự kiện khi nhấn Enter trong form
  _$form.find('input').on('keypress', function (e) {
    if (e.which === 13) { // Mã 13 là Enter
      e.preventDefault();
      save();
    }
  });

  // Khi mở modal, tự động focus vào ô input đầu tiên
  _$modal.on('shown.bs.modal', function () {
    _$form.find('input[type=text]:first').focus();
  });

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


  // Xem trước ảnh mới khi chọn file
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

})(jQuery);

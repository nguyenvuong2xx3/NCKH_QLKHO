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
        _addCategoryEditModal.close(); // Đóng modal con
      }
    });
  });

  // Xử lý sự kiện thêm nhà cung cấp
  $('#EditSupplierBtn').on('click', function () {
    _addSupplierEditModal.open({}, function (result) {
      if (result) {
        $('#SupplierDisplayEdit').val(result.supplierName.trim());
        $('#SupplierIdEdit').val(result.supplierId);
        _addSupplierEditModal.close(); // Đóng modal con
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

  // Phương thức kiểm tra tùy chỉnh cho số điện thoại (giữ nguyên)
  $.validator.addMethod("phoneNumber", function (value, element) {
    return this.optional(element) || /^[0-9\+\-\(\)\s]*$/.test(value);
  }, "Số điện thoại không hợp lệ.");

  // Phương thức kiểm tra tùy chỉnh cho mã vạch (barcode)
  $.validator.addMethod("barcode", function (value, element) {
    // Nếu trường không bắt buộc và không có giá trị, bỏ qua kiểm tra
    if (this.optional(element)) {
      return true;
    }

    // Kiểm tra mã vạch chỉ chứa số và có độ dài từ 8 đến 13 ký tự
    var regex = /^[0-9]{8,13}$/;
    return regex.test(value);
  }, "Mã vạch không hợp lệ. Chỉ cho phép từ 8 đến 13 ký tự số.");

  // Khởi tạo validate cho form sản phẩm
  _$form.validate({
    rules: {
      Name: {
        required: true,
        maxlength: 256
      },
      Code: {
        required: true,
        maxlength: 256
      },
      Description: {
        required: true,
        maxlength: 500
      },
      CategoryIdCreate: {
        required: true
      },
      SupplierIdCreate: {
        required: true
      },
      Barcode: {
        required: true,
        barcode: true,
        maxlength: 20
      },
      Unit: {
        required: true,
        maxlength: 50
      },
      Weight: {
        required: true,
        number: true,
        maxlength: 20
      },
      Volume: {
        required: true,
        number: true,
        maxlength: 20
      },
      Image: {
        required: true // Kiểm tra nếu có hình ảnh
      }
    },
    messages: {
      Name: {
        required: "Tên sản phẩm là bắt buộc.",
        maxlength: "Tên sản phẩm không được vượt quá 256 ký tự."
      },
      Code: {
        required: "Mã sản phẩm là bắt buộc.",
        maxlength: "Mã sản phẩm không được vượt quá 256 ký tự."
      },
      Description: {
        required: "Mô tả sản phẩm là bắt buộc.",
        maxlength: "Mô tả sản phẩm không được vượt quá 500 ký tự."
      },
      CategoryIdCreate: {
        required: "Danh mục sản phẩm là bắt buộc."
      },
      SupplierIdCreate: {
        required: "Nhà cung cấp là bắt buộc."
      },
      Barcode: {
        required: "Mã vạch là bắt buộc.",
        barcode: "Mã vạch không hợp lệ. Chỉ cho phép từ 8 đến 13 ký tự số."
      },
      Unit: {
        required: "Đơn vị tính là bắt buộc.",
        maxlength: "Đơn vị tính không được vượt quá 50 ký tự."
      },
      Weight: {
        required: "Trọng lượng là bắt buộc.",
        number: "Trọng lượng phải là một số hợp lệ.",
        maxlength: "Trọng lượng không được vượt quá 20 ký tự."
      },
      Volume: {
        required: "Thể tích là bắt buộc.",
        number: "Thể tích phải là một số hợp lệ.",
        maxlength: "Thể tích không được vượt quá 20 ký tự."
      },
      Image: {
        required: "Ảnh sản phẩm là bắt buộc."
      }
    },
    errorPlacement: function (error, element) {
      error.insertAfter(element);
    },
    highlight: function (element) {
      $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
      $(element).closest('.form-group').removeClass('has-error');
    }
  });

  function save() {
    if (!_$form.valid()) {
      return;
    }

    var formData = new FormData(_$form[0]);

    // Thêm file ảnh nếu có
    var imageFile = $('#newImage')[0].files[0];
    if (imageFile) {
      formData.append('ImageFile', imageFile);
    }

    abp.ui.setBusy(_$modal);
    $.ajax({
      url: abp.appPath + 'Products/EditAndUploadDeleteImage',
      type: 'POST',
      data: formData,
      processData: false,
      contentType: false,
      success: function (response) {
        if (response.success) {
          _$modal.modal('hide');
          abp.notify.info(l('SavedSuccessfully'));
          abp.event.trigger('product.edited', response);
        } else {
          abp.notify.error(response.message || 'Có lỗi xảy ra khi cập nhật');
        }
      },
      error: function (xhr) {
        var errorMessage = xhr.responseJSON && xhr.responseJSON.message
          ? xhr.responseJSON.message
          : "Có lỗi xảy ra khi cập nhật sản phẩm";
        abp.notify.error(errorMessage);
      },
      complete: function () {
        abp.ui.clearBusy(_$modal);
      }
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

  // Enter key handler
  _$form.find('input').on('keypress', function (e) {
    if (e.which === 13) {
      e.preventDefault();
      save();
    }
  });

  // Focus first input when modal shown
  _$modal.on('shown.bs.modal', function () {
    _$form.find('input[type=text]:first').focus();
  });

  // 1. Bind sự kiện click cho nút Save
  _$modal.on('click', '.save-button', function (e) {
    e.preventDefault();
    save();
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


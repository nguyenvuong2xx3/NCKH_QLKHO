(function ($) {
  // Khai báo dịch vụ và các biến
  var _productService = abp.services.app.product,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#ProductCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#ProductsTable');

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
  $('#CreateCategoryBtn').on('click', function () {
    _addCategoryCreateModal.open({}, function (result) {
      if (result) {
        $('#CategoryDisplay').val(result.categoryName.trim());
        $('#CategoryIdCreate').val(result.categoryId);
        _addCategoryCreateModal.close(); // Đóng modal con
      }
    });
  });

  // Xử lý sự kiện thêm nhà cung cấp
  $('#AddSupplierBtn').on('click', function () {
    _addSupplierCreateModal.open({}, function (result) {
      if (result) {
        $('#SupplierDisplay').val(result.supplierName.trim());
        $('#SupplierIdCreate').val(result.supplierId);
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
    if ($('#ProductCreateModal').hasClass('show')) {
      $('#ProductCreateModal .modal-content').css('overflow-y', 'auto');
    }
  });

  // Khởi tạo DataTable cho sản phẩm
  var _$productTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    listAction: {
      ajaxFunction: _productService.getAllProducts,
      inputFilter: function () {
        //return $('#ProductSearchForm').serializeFormToObject(true);
      }
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: function () {
          _$productTable.draw(false);
        }
      }
    ],
    responsive: {
      details: {
        type: 'column'
      }
    },
    columnDefs: [
      {
        targets: 0,
        data: 'code',
        sortable: false
      },
      {
        targets: 1,
        data: 'name',
        sortable: false
      },
      {
        targets: 2,
        data: 'image',
        sortable: false,
        render: function (data) {
          if (data) {
            return `<img src="${data}" alt="Ảnh sản phẩm" class="img-thumbnail d-block mx-auto" width="80" height="80" style="object-fit: cover;">`;
          }
          return '<span class="text-muted">Không có ảnh</span>';
        }
      },
      {
        targets: 3,
        data: 'description',
        sortable: false,
      },
      {
        targets: 4,
        data: 'categoryName',
        sortable: false
      },
      {
        targets: 5,
        data: 'barcode',
        sortable: false
      },
      {
        targets: 6,
        data: 'unit',
        sortable: false
      },
      {
        targets: 7,
        data: 'weight',
        sortable: false
      },
      {
        targets: 8,
        data: 'volume',
        sortable: false
      },
      {
        targets: 9,
        data: 'supplierName',
        sortable: false
      },
      {
        targets: 10,
        data: 'isActive',
        sortable: false,
        className: 'text-center',
        render: function (data, type, row) {
          return `<div class="d-flex justify-content-center align-items-center">
                                <input type="checkbox" class="form-check-input is-active-toggle" 
                                       data-id="${row.id}" ${data ? 'checked' : ''}>
                            </div>`;
        }
      },
      {
        targets: 11,
        data: null,
        sortable: false,
        autoWidth: true,
        render: function (data, type, row) {
          return [
            `<button type="button" class="btn btn-sm bg-secondary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#ProductEditModal">`,
            `    <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
            ' </button>',
            `<button type="button" class="btn btn-sm bg-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}">`,
            `    <i class="fas fa-trash"></i> ${l('Delete')}`,
            ' </button>',
            `<button type="button" class="btn btn-sm bg-info detail-product" data-product-id="${row.id}" data-toggle="modal">`,
            `    <i class="fas fa-eye"></i> ${l('Details')}`,
            ' </button>'
          ].join('');
        }
      }
    ]
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



  // Xử lý lưu sản phẩm
  _$form.find('.save-button').on('click', function (e) {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    var product = _$form.serializeFormToObject();
    var formData = new FormData(_$form[0]);
    abp.ui.setBusy(_$modal);

    $.ajax({
      url: abp.appPath + 'Products/Create',
      type: 'POST',
      processData: false,
      contentType: false,
      data: formData,
      error: function (xhr) {
        var errorMessage = xhr.responseJSON && xhr.responseJSON.errors && xhr.responseJSON.errors.length > 0
          ? xhr.responseJSON.errors.join("<br/>")
          : "Có lỗi xảy ra khi tạo mới sản phẩm (Có thể do upload ảnh không đúng định dạng)";
        $("#error-message").html(errorMessage).show();
      }
    }).done(function () {
      _$modal.modal('hide');
      _$form[0].reset();
      abp.notify.info(l('Lưu thành công'));
      _$productTable.ajax.reload();
    }).always(function () {
      abp.ui.clearBusy(_$modal);
    });
  });

  // Xử lý sự kiện chỉnh sửa sản phẩm
  $(document).on('click', '.edit-product', function (e) {
    var productId = $(this).attr("data-product-id");
    e.preventDefault();

    abp.ajax({
      url: abp.appPath + 'Products/EditModal?productId=' + productId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#ProductEditModal div.modal-content').html(content);
        //$('#ProductEditModal').modal('show'); // Show modal
      }
    });
  });

  abp.event.on('product.edited', (data) => {
    _$productTable.ajax.reload();
  });

  $(document).on('click', '.delete-product', function () {
    var productId = $(this).attr("data-product-id");
    var productName = $(this).attr('data-product-name');

    deleteProduct(productId, productName);
  });

  function deleteProduct(productId, productName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('Bạn có chắc chắn muốn xóa sản phẩm {0}'),
        productName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _productService.delete({
            id: productId
          }).done(() => {
            abp.notify.info(l('SuccessfullyDeleted'));
            _$productTable.ajax.reload();
          });
        }
      }
    );
  }

  // Tìm kiếm sản phẩm
  $('.btn-search').on('click', function () {
    _$productTable.ajax.reload();
  });

  $('.txt-search').on('keypress', function (e) {
    if (e.which === 13) {
      _$productTable.ajax.reload();
      return false;
    }
  });

  // Hiển thị ảnh xem trước khi tải lên
  document.getElementById('imageUpload').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const previewImage = document.getElementById('previewImage');

    if (file) {
      const reader = new FileReader();
      reader.onload = function (e) {
        previewImage.src = e.target.result;
        previewImage.style.display = 'block';
      };
      reader.readAsDataURL(file);
    } else {
      previewImage.src = '';
      previewImage.style.display = 'none';
    }
  });

  //$('#ExportExcelBtn').click(function () {
  //  var input = {
  //    filter: $('#ProductsTableFilter').val(),
  //    supplierId: $('#SupplierIdFilter').val(),
  //    categoryId: $('#CategoryIdFilter').val()
  //  };

  //  fetch(abp.appPath + 'Products/ExportToExcel', {
  //    method: 'POST',
  //    headers: {
  //      'Content-Type': 'application/json',
  //      'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
  //    },
  //    body: JSON.stringify(input)
  //  })
  //    .then(response => {
  //      if (!response.ok) {
  //        throw new Error('Xuất Excel thất bại: ' + response.statusText);
  //      }
  //      return response.blob();
  //    })
  //    .then(blob => {
  //      var link = document.createElement('a');
  //      link.href = window.URL.createObjectURL(blob);
  //      link.download = 'Danh_sach_san_pham.xlsx';
  //      link.click();
  //    })
  //    .catch(error => {
  //      abp.notify.error(error.message);
  //    });
  //});
  $('#ExportExcelBtn').click(function () {
    // Lấy các tham số filter từ form
    var input = {
      filter: $('#ProductsTableFilter').val(),
      supplierId: $('#SupplierIdFilter').val(),
      categoryId: $('#CategoryIdFilter').val(),
      // Thêm các tham số khác nếu cần
    };

    // Hiển thị loading
    abp.ui.setBusy();

    // Gọi API export
    $.ajax({
      url: abp.appPath + 'Products/ExportToExcel',
      type: 'POST',
      data: JSON.stringify(input),
      contentType: 'application/json',
      headers: {
        'RequestVerificationToken': abp.security.antiForgery.getToken()
      },
      xhrFields: {
        responseType: 'blob' // QUAN TRỌNG: để nhận dữ liệu kiểu file
      },
      success: function (blob) {
        var url = window.URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = url;
        a.download = 'Danh_sach_san_pham.xlsx';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        abp.notify.success('Xuất Excel thành công');
      },
      error: function (xhr) {
        abp.notify.error('Xuất Excel thất bại: ' + xhr.statusText);
      },
      complete: function () {
        abp.ui.clearBusy();
      }
    });

  });

  // Import Excel
  //$('#ImportExcelForm').submit(function (e) {
  //  e.preventDefault();
  //  var formData = new FormData(this);

  //  abp.ui.setBusy($('#ImportExcelModal'), abp.ajax({
  //    url: abp.appPath + 'Products/ImportFromExcel',
  //    type: 'POST',
  //    data: formData,
  //    processData: false,
  //    contentType: false
  //  }).done(function (result) {
  //    if (result.success) {
  //      abp.notify.success('Import thành công ' + result.results.length + ' sản phẩm');
  //      $('#ImportExcelModal').modal('hide');
  //      $('#ProductsTable').DataTable().ajax.reload();
  //    } else {
  //      var errorMsg = 'Import không thành công: <br>';
  //      result.results.forEach(function (item) {
  //        if (!item.IsSuccess) {
  //          errorMsg += `Dòng ${item.RowNumber}: ${item.Message}<br>`;
  //        }
  //      });
  //      abp.notify.error(errorMsg, { multiline: true });
  //    }
  //  }));
  //});
  $('#ImportExcelForm').submit(function (e) {
    e.preventDefault();
    var formData = new FormData(this);

    abp.ui.setBusy($('#ImportExcelModal'), abp.ajax({
      url: abp.appPath + 'Products/ImportFromExcel',
      type: 'POST',
      data: formData,
      processData: false,
      contentType: false
    }).done(function (result) {
      if (result.success) {
        var successCount = result.results.filter(r => r.IsSuccess).length;
        var errorCount = result.results.length - successCount;

        var message = `Import thành công ${successCount} sản phẩm`;
        if (errorCount > 0) {
          message += `, có ${errorCount} lỗi`;
        }

        abp.notify.success(message);
        $('#ImportExcelModal').modal('hide');
        $('#ProductsTable').DataTable().ajax.reload();
      } else {
        var errorMsg = 'Import không thành công: <br>';
        result.results.forEach(function (item) {
          if (!item.IsSuccess) {
            errorMsg += `Dòng ${item.RowNumber} (Mã: ${item.Code || 'N/A'}): ${item.Message}<br>`;
          }
        });
        abp.notify.error(errorMsg, { multiline: true });
      }
    }));
  });
})(jQuery);

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
        $('#ProductEditModal').modal('show'); // Show modal
      }
    });
  });

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
})(jQuery);

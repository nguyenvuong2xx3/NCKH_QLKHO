(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modalimport = $('#ImportModal'),
    _selectedProducts = []; // Thay đổi từ _selectedProductsId sang mảng object chứa đầy đủ thông tin
  _$form = _$modalimport.find('form'),
    _$table = $('#StockTransactionsTable');

  // Warehouse
  $('#CreateWarehouseImportBtn').on('click', function () {
    _addWarehouseCreateModal.open({}, function (result) {
      if (result) {
        $('#WarehouseImportDisplay').val(result.warehouseName.trim());
        $('#WarehouseIdImportCreate').val(result.warehouseId);
        loadStorageLocations(result.warehouseId); // Load vị trí lưu trữ khi chọn kho
      }
    });
  });

  var _addWarehouseCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });

  // Supplier
  $('#AddSupplierImportBtn').on('click', function () {
    _addSupplierCreateModal.open({}, function (result) {
      if (result) {
        $('#SupplierImportDisplay').val(result.supplierName.trim());
        $('#SupplierIdImportCreate').val(result.supplierId);
      }
    });
  });

  var _addSupplierCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Suppliers/AddSupplier',
    scriptUrl: abp.appPath + 'view-resources/Views/Suppliers/_AddSupplierModal.js',
    modalClass: 'AddSupplierModal',
  });

  // Products
  $('#AddProductsImportBtn').click(function () {
    const supplierId = $('#SupplierIdImportCreate').val();
    console.log(supplierId)
    if (!supplierId) {
      abp.notify.error('Vui lòng chọn nhà cung cấp trước khi thêm sản phẩm');
      return;
    }

    _addProductCreateModal.open({
      _selectedProducts: _selectedProducts,
      supplierId: supplierId // Truyền supplierId vào modal
    }, function (result) {
      if (!result || result.length === 0) {
        _selectedProducts = [];
        $('#ProductsImportDisplay').val('');
        $('#productImportTable tbody').empty();
        return;
      }

      _selectedProducts = result;
      const selectedNames = result.map(u => u.productName).join(', ');
      $('#ProductsImportDisplay').val(selectedNames);
      renderProductRows(_selectedProducts);
    });
  });

  var _addProductCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Products/AddProduct',
    scriptUrl: abp.appPath + 'view-resources/Views/Products/_AddProductsModal.js',
    modalClass: 'AddProductsModal',
  });

  // Hàm load vị trí lưu trữ theo kho
  function loadStorageLocations(warehouseId) {
    // gọi vào proxy của Application Service
    abp.services.app.storageLocation
      .getAll({ warehouseId: warehouseId })
      .done(function (result) {
        const locations = result.items; // giả định DTO trả về .Items
        const $select = $('.storage-location-select');

        $select.empty().append('<option value="">-- Chọn vị trí --</option>');

        locations.forEach(function (loc) {
          $select.append(
            `<option value="${loc.id}">${loc.code} (Còn ${loc.availableSpace})</option>`
          );
        });
      })
      .fail(function (err) {
        console.error('Không thể tải danh sách vị trí:', err);
        abp.message.error('Tải vị trí kho thất bại.');
      });
  }


  // Hàm render bảng sản phẩm
  function renderProductRows(products) {
    let $tbody = $('#productImportTable tbody');
    $tbody.empty();

    products.forEach((product, index) => {
      let row = `
                <tr data-product-id="${product.productId}">
                    <td>${product.productCode}</td>
                    <td>${product.productName}</td>
                    <td>
                        <input type="number" 
                               class="form-control import-quantity" 
                               min="1" 
                               value="1"
                               required>
                    </td>
                    <td>
                        <input type="number" 
                               class="form-control import-unit-price" 
                               min="0" 
                               step="1000"
                               required>
                    </td>
                    <td class="import-total-price">0</td>
                    <td>
                        <select class="form-control storage-location-select" required>
                            <option value="">-- Chọn vị trí --</option>
                        </select>
                    </td>
                    <td>
                        <button type="button" class="btn btn-danger btn-sm remove-product">
                            <i class="la la-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
      $tbody.append(row);
    });

    // Load vị trí lưu trữ nếu đã chọn kho
    if ($('#WarehouseIdImportCreate').val()) {
      loadStorageLocations($('#WarehouseIdImportCreate').val());
    }

    setupAutoCalculate();
  }

  // Tự động tính toán thành tiền
  function setupAutoCalculate() {
    $(document).off('input', '.import-quantity, .import-unit-price').on('input', '.import-quantity, .import-unit-price', function () {
      const $row = $(this).closest('tr');
      const quantity = parseInt($row.find('.import-quantity').val()) || 0;
      const unitPrice = parseInt($row.find('.import-unit-price').val()) || 0;
      const total = quantity * unitPrice;

      $row.find('.import-total-price').text(total.toLocaleString('vi-VN'));
      calculateGrandTotal();
    });
  }

  // Tính tổng tiền
  function calculateGrandTotal() {
    let grandTotal = 0;
    $('#productImportTable tbody tr').each(function () {
      const totalText = $(this).find('.import-total-price').text().replace(/\./g, '');
      grandTotal += parseInt(totalText) || 0;
    });
    $('#grandTotal').text(grandTotal.toLocaleString('vi-VN'));
  }

  // Xử lý xóa sản phẩm
  $(document).on('click', '.remove-product', function () {
    const productId = $(this).closest('tr').data('product-id');
    _selectedProducts = _selectedProducts.filter(p => p.productId !== productId);
    $(this).closest('tr').remove();
    calculateGrandTotal();

    // Cập nhật lại display
    $('#ProductsImportDisplay').val(_selectedProducts.map(p => p.productName).join(', '));
  });

  // Submit form
  _$form.find('.save-button').on('click', (e) => {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    // Validate ít nhất 1 sản phẩm
    if (_selectedProducts.length === 0) {
      abp.notify.error('Vui lòng chọn ít nhất 1 sản phẩm');
      return;
    }

    // Thu thập dữ liệu chi tiết
    let details = [];
    $('#productImportTable tbody tr').each(function () {
      const productId = $(this).data('product-id');
      details.push({
        productId: productId,
        quantity: parseInt($(this).find('.import-quantity').val()),
        unitPrice: parseFloat($(this).find('.import-unit-price').val()),
        storageLocationId: parseInt($(this).find('.storage-location-select').val())
      });
    });

    // Validate dữ liệu
    for (let detail of details) {
      if (!detail.quantity || detail.quantity <= 0) {
        abp.notify.error('Số lượng phải lớn hơn 0');
        return;
      }
      if (!detail.unitPrice || detail.unitPrice < 0) {
        abp.notify.error('Đơn giá không hợp lệ');
        return;
      }
      if (!detail.storageLocationId) {
        abp.notify.error('Vui lòng chọn vị trí lưu trữ cho tất cả sản phẩm');
        return;
      }
    }

    // Tạo object gửi lên server
    var importRequest = {
      warehouseId: parseInt($('#WarehouseIdImportCreate').val()),
      supplierId: parseInt($('#SupplierIdImportCreate').val()),
      importRequestDetails: details
    };

    abp.ui.setBusy(_$modalimport);
    _stockTransactionService
      .createImportRequest(importRequest)
      .done(function () {
        _$modalimport.modal('hide');
        _$form[0].reset();
        _selectedProducts = [];
        $('#productImportTable tbody').empty();
        abp.notify.info(l('Tạo phiếu nhập kho thành công'));
        _$table.DataTable().ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modalimport);
      });
  });

})(jQuery);
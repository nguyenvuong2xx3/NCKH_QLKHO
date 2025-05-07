(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modalexport = $('#ExportModal'),
    _selectedProducts = [];
  _$form = _$modalexport.find('form'),
    _$table = $('#StockTransactionsTable');

  // Add this to your script section or JS file
  var _addCustomerModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Customers/AddCustomer',
    scriptUrl: abp.appPath + 'view-resources/Views/Customers/_AddCustomersModal.js',
    modalClass: 'AddCustomersModal',
  });

  $('#SelectCustomerBtn').on('click', function () {
    _addCustomerModal.open({}, function (result) {
      if (result) {
        $('#CustomerDisplay').val(result.customerName.trim());
        $('#CustomerId').val(result.customerId);
      }
    });
  });

  // Warehouse
  $('#CreateWarehouseExportBtn').on('click', function () {
    _addWarehouseCreateModal.open({}, function (result) {
      if (result) {
        $('#WarehouseExportDisplay').val(result.warehouseName.trim());
        $('#WarehouseIdExportCreate').val(result.warehouseId);
        loadStorageLocations(result.warehouseId);
      }
    });
  });

  var _addWarehouseCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });

  // Supplier
  $('#AddSupplierExportBtn').on('click', function () {
    _addSupplierCreateModal.open({}, function (result) {
      if (result) {
        $('#SupplierExportDisplay').val(result.supplierName.trim());
        $('#SupplierIdExportCreate').val(result.supplierId);
      }
    });
  });

  var _addSupplierCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Suppliers/AddSupplier',
    scriptUrl: abp.appPath + 'view-resources/Views/Suppliers/_AddSupplierModal.js',
    modalClass: 'AddSupplierModal',
  });

  $('#AddProductsExportBtn').click(function () {
    const supplierId = $('#SupplierIdExportCreate').val();
    console.log(supplierId)
    if (!supplierId) {
      abp.notify.error('Vui lòng chọn nhà cung cấp trước khi thêm sản phẩm');
      return;
    }

    _addProductExportModal.open({
      _selectedProducts: _selectedProducts,
      supplierId: supplierId // Truyền supplierId vào modal
    }, function (result) {
      if (!result || result.length === 0) {
        _selectedProducts = [];
        $('#ProductsExportDisplay').val('');
        $('#productExportTable tbody').empty();
        return;
      }

      _selectedProducts = result;
      const selectedNames = result.map(u => u.productName).join(', ');
      $('#ProductsExportDisplay').val(selectedNames);
      renderProductRows(_selectedProducts);
    });
  });

  var _addProductExportModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Products/AddProduct',
    scriptUrl: abp.appPath + 'view-resources/Views/Products/_AddProductsModal.js',
    modalClass: 'AddProductsModal',
  });


  // Hàm load vị trí lưu trữ theo kho
  function loadStorageLocations(warehouseId) {
    abp.services.app.storageLocation
      .getAll({ warehouseId: warehouseId })
      .done(function (result) {
        const locations = result.items;
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
    let $tbody = $('#productExportTable tbody');
    $tbody.empty();

    products.forEach((product, index) => {
      let row = `
                <tr data-product-id="${product.productId}">
                    <td>${product.productCode}</td>
                    <td>${product.productName}</td>
                    <td>
                        <input type="number" 
                               class="form-control export-quantity" 
                               min="1" 
                               value="1"
                               required>
                    </td>
                    <td>
                        <input type="number" 
                               class="form-control export-unit-price" 
                               min="0" 
                               step="1000"
                               required>
                    </td>
                    <td class="export-total-price">0</td>
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

    if ($('#WarehouseIdExportCreate').val()) {
      loadStorageLocations($('#WarehouseIdExportCreate').val());
    }

    setupAutoCalculate();
  }

  function setupAutoCalculate() {
    $(document).off('input', '.export-quantity, .export-unit-price').on('input', '.export-quantity, .export-unit-price', function () {
      const $row = $(this).closest('tr');
      const quantity = parseInt($row.find('.export-quantity').val()) || 0;
      const unitPrice = parseInt($row.find('.export-unit-price').val()) || 0;
      const total = quantity * unitPrice;

      $row.find('.export-total-price').text(total.toLocaleString('vi-VN'));
      calculateGrandTotal();
    });
  }

  function calculateGrandTotal() {
    let grandTotal = 0;
    $('#productExportTable tbody tr').each(function () {
      const totalText = $(this).find('.export-total-price').text().replace(/\./g, '');
      grandTotal += parseInt(totalText) || 0;
    });
    $('#grandTotal').text(grandTotal.toLocaleString('vi-VN'));
  }

  $(document).on('click', '.remove-product', function () {
    const productId = $(this).closest('tr').data('product-id');
    _selectedProducts = _selectedProducts.filter(p => p.productId !== productId);
    $(this).closest('tr').remove();
    calculateGrandTotal();

    $('#ProductsExportDisplay').val(_selectedProducts.map(p => p.productName).join(', '));
  });

  _$form.find('.save-button').on('click', (e) => {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    if (_selectedProducts.length === 0) {
      abp.notify.error('Vui lòng chọn ít nhất 1 sản phẩm');
      return;
    }

    let details = [];
    $('#productExportTable tbody tr').each(function () {
      const productId = $(this).data('product-id');
      details.push({
        productId: productId,
        quantity: parseInt($(this).find('.export-quantity').val()),
        unitPrice: parseFloat($(this).find('.export-unit-price').val()),
        storageLocationId: parseInt($(this).find('.storage-location-select').val())
      });
    });

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

    var exportRequest = {
      transactionCode: $('#TransactionCodeImport').val(),
      fromWarehouseId: parseInt($('#WarehouseIdExportCreate').val()),
      customerId: parseInt($('#CustomerIdExportCreate').val()),
      exportRequestDetails: details
    };

    abp.ui.setBusy(_$modalexport);
    _stockTransactionService
      .createExportRequest(exportRequest)
      .done(function () {
        _$modalexport.modal('hide');
        _$form[0].reset();
        _selectedProducts = [];
        $('#productExportTable tbody').empty();
        abp.notify.info(l('Tạo phiếu xuất kho thành công'));
        _$table.DataTable().ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modalexport);
      });
  });

  // Handle modal scrolling
  $(document).on('hidden.bs.modal', '.modal', function () {
    if ($('.modal.show').length) {
      $('body').addClass('modal-open');
    }
  });

  $(document).on('hidden.bs.modal', '.modal', function () {
    if ($('#ExportModal').hasClass('show')) {
      $('#ExportModal .modal-content').css('overflow-y', 'auto');
    }
  });

})(jQuery);

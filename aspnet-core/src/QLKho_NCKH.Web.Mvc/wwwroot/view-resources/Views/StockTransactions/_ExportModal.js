(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#ExportModal'),
    _$form = _$modal.find('form'),
    _$table = $('#StockTransactionsTable'),
    _selectedProducts = [];
  _warehouseId = null;

  // Customer Modal
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

  // Warehouse Modal
  var _addWarehouseModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });

  $('#CreateWarehouseExportBtn').on('click', function () {
    _addWarehouseModal.open({}, function (result) {
      if (result) {
        $('#WarehouseExportDisplay').val(result.warehouseName.trim());
        $('#WarehouseIdExportCreate').val(result.warehouseId);
        _warehouseId = result.warehouseId;
        loadStorageLocations(result.warehouseId);
      }
    });
  });

  // Products Modal
  var _addInventoryItemModal = new app.ModalManager({
    viewUrl: abp.appPath + 'InventoryItems/AddInventory',
    scriptUrl: abp.appPath + 'view-resources/Views/InventoryItems/_AddInventoryItemModal.js',
    modalClass: 'AddInventoryItemModal',
  });

  $('#AddInventoryItemsBtn').click(function () {
    const warehouseId = $('#WarehouseIdExportCreate').val();
    if (!warehouseId) {
      abp.notify.error('Vui lòng chọn kho trước khi thêm sản phẩm');
      return;
    }

    _addInventoryItemModal.open({
      warehouseId: warehouseId,
      _selectedItems: _selectedProducts
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

  // Load storage locations
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
      });
  }

  // Render product rows
  function renderProductRows(products) {
    let $tbody = $('#productExportTable tbody');
    $tbody.empty();

    products.forEach((product, index) => {
      let row = `
        <tr data-product-id="${product.productId}">
          <td>${product.productCode}</td>
          <td>${product.productBarcode}</td>
          <td>${product.productName}</td>
          <td>${product.quantity || 0}</td>
          <td>
            <input type="number" class="form-control export-quantity" 
                   min="1" max="${product.quantity || 1}" 
                   value="1" required>
          </td>
          <td>
            <input type="number" class="form-control export-unit-price" 
                   min="0" step="1000" value="${product.price || 0}" required>
          </td>
          <td class="export-total-price">${product.price || 0}</td>
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

  // Auto calculate totals
  function setupAutoCalculate() {
    $(document).off('input', '.export-quantity, .export-unit-price').on('input', '.export-quantity, .export-unit-price', function () {
      const $row = $(this).closest('tr');
      const quantity = parseInt($row.find('.export-quantity').val()) || 0;
      const unitPrice = parseFloat($row.find('.export-unit-price').val()) || 0;
      const total = quantity * unitPrice;

      $row.find('.export-total-price').text(total.toLocaleString('vi-VN'));
      calculateGrandTotal();
    });
  }

  // Calculate grand total
  function calculateGrandTotal() {
    let grandTotal = 0;
    $('#productExportTable tbody tr').each(function () {
      const totalText = $(this).find('.export-total-price').text().replace(/\./g, '');
      grandTotal += parseInt(totalText) || 0;
    });
    $('#grandTotal').text(grandTotal.toLocaleString('vi-VN'));
  }

  // Remove product
  $(document).on('click', '.remove-product', function () {
    const productId = $(this).closest('tr').data('product-id');
    _selectedProducts = _selectedProducts.filter(p => p.productId !== productId);
    $(this).closest('tr').remove();
    calculateGrandTotal();
    $('#ProductsExportDisplay').val(_selectedProducts.map(p => p.productName).join(', '));
  });

  // Submit form
  _$form.find('.save-button').on('click', function (e) {
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

    // Validate details
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
      transactionCode: $('#TransactionCodeExport').val(),
      fromWarehouseId: parseInt($('#WarehouseIdExportCreate').val()),
      customerId: parseInt($('#CustomerId').val()),
      note: $('#NoteExport').val(),
      referenceNumber: $('#ReferenceNumberExport').val(),
      exportRequestDetails: details
    };

    abp.ui.setBusy(_$modal);
    _stockTransactionService
      .createExportRequest(exportRequest)
      .done(function () {
        _$modal.modal('hide');
        _$form[0].reset();
        _selectedProducts = [];
        $('#productExportTable tbody').empty();
        abp.notify.info(l('Tạo phiếu xuất kho thành công'));
        _$table.DataTable().ajax.reload();
      })
      .fail(function (error) {
        abp.notify.error(error.message || 'Có lỗi xảy ra khi tạo phiếu xuất kho');
      })
      .always(function () {
        abp.ui.clearBusy(_$modal);
      });
  });
})(jQuery);
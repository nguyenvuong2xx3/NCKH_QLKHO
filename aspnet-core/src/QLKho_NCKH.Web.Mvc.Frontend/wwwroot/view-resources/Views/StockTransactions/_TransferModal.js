(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modalTransfer = $('#TransferModal'),
    _selectedProducts = [],
    _$form = _$modalTransfer.find('form'),
    _$table = $('#StockTransactionsTable');

  // From Warehouse Selection
  $('#SelectFromWarehouseBtn').on('click', function () {
    _addWarehouseCreateModal.open({}, function (result) {
      if (result) {
        $('#FromWarehouseDisplay').val(result.warehouseName.trim());
        $('#FromWarehouseId').val(result.warehouseId);
        loadStorageLocations(result.warehouseId, 'from');

        // Clear products if warehouse changes
        _selectedProducts = [];
        $('#ProductsTransferDisplay').val('');
        $('#productTransferTable tbody').empty();
      }
    });
  });

  // To Warehouse Selection
  $('#SelectToWarehouseBtn').on('click', function () {
    _addWarehouseCreateModal.open({}, function (result) {
      if (result) {
        // Validate that to warehouse is different from from warehouse
        const fromWarehouseId = $('#FromWarehouseId').val();
        if (fromWarehouseId && fromWarehouseId == result.warehouseId) {
          abp.notify.error(l('FromAndToWarehouseCannotBeSame'));
          return;
        }

        $('#ToWarehouseDisplay').val(result.warehouseName.trim());
        $('#ToWarehouseId').val(result.warehouseId);
        loadStorageLocations(result.warehouseId, 'to');
      }
    });
  });

  var _addWarehouseCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });

  // Product Selection
  $('#AddProductsTransferBtn').click(function () {
    const fromWarehouseId = $('#FromWarehouseId').val();

    if (!fromWarehouseId) {
      abp.notify.error(l('PleaseSelectFromWarehouseFirst'));
      return;
    }

    _addProductTransferModal.open({
      _selectedProducts: _selectedProducts,
      warehouseId: fromWarehouseId // Pass warehouseId to filter available products
    }, function (result) {
      if (!result || result.length === 0) {
        _selectedProducts = [];
        $('#ProductsTransferDisplay').val('');
        $('#productTransferTable tbody').empty();
        return;
      }

      _selectedProducts = result;
      const selectedNames = result.map(u => u.productName).join(', ');
      $('#ProductsTransferDisplay').val(selectedNames);
      renderProductRows(_selectedProducts);
    });
  });

  var _addProductTransferModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Products/AddProduct',
    scriptUrl: abp.appPath + 'view-resources/Views/Products/_AddProductsModal.js',
    modalClass: 'AddProductsModal',
  });

  // Load storage locations for either from or to warehouse
  function loadStorageLocations(warehouseId, type) {
    abp.services.app.storageLocation
      .getAll({ warehouseId: warehouseId })
      .done(function (result) {
        const locations = result.items;
        const $select = $(`.storage-location-select-${type}`);

        $select.empty().append('<option value="">-- ' + l('SelectLocation') + ' --</option>');

        locations.forEach(function (loc) {
          $select.append(
            `<option value="${loc.id}">${loc.code} (${l('Available')}: ${loc.availableSpace})</option>`
          );
        });
      })
      .fail(function (err) {
        console.error('Could not load location list:', err);
        abp.message.error(l('FailedToLoadLocations'));
      });
  }

  // Render product rows for transfer
  function renderProductRows(products) {
    let $tbody = $('#productTransferTable tbody');
    $tbody.empty();

    products.forEach((product, index) => {
      let row = `
                <tr data-product-id="${product.productId}">
                    <td>${product.productCode}</td>
                    <td>${product.productName}</td>
                    <td>
                        <input type="number" 
                               class="form-control transfer-quantity" 
                               min="1" 
                               max="${product.availableQuantity || ''}"
                               value="1"
                               required>
                        <small class="text-muted">${l('Available')}: ${product.availableQuantity || 0}</small>
                    </td>
                    <td>
                        <select class="form-control storage-location-select-from" required>
                            <option value="">-- ${l('CurrentLocation')} --</option>
                        </select>
                    </td>
                    <td>
                        <select class="form-control storage-location-select-to" required>
                            <option value="">-- ${l('NewLocation')} --</option>
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

      // Load locations for the from warehouse (current location)
      if ($('#FromWarehouseId').val()) {
        loadStorageLocations($('#FromWarehouseId').val(), 'from');
      }

      // Load locations for the to warehouse (new location)
      if ($('#ToWarehouseId').val()) {
        loadStorageLocations($('#ToWarehouseId').val(), 'to');
      }
    });
  }

  // Remove product from transfer list
  $(document).on('click', '.remove-product', function () {
    const productId = $(this).closest('tr').data('product-id');
    _selectedProducts = _selectedProducts.filter(p => p.productId !== productId);
    $(this).closest('tr').remove();
    $('#ProductsTransferDisplay').val(_selectedProducts.map(p => p.productName).join(', '));
  });

  // Save transfer
  _$form.find('.save-button').on('click', (e) => {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    const fromWarehouseId = $('#FromWarehouseId').val();
    const toWarehouseId = $('#ToWarehouseId').val();

    // Validate warehouses
    if (!fromWarehouseId) {
      abp.notify.error(l('PleaseSelectFromWarehouse'));
      return;
    }

    if (!toWarehouseId) {
      abp.notify.error(l('PleaseSelectToWarehouse'));
      return;
    }

    if (fromWarehouseId === toWarehouseId) {
      abp.notify.error(l('FromAndToWarehouseCannotBeSame'));
      return;
    }

    // Validate products
    if (_selectedProducts.length === 0) {
      abp.notify.error(l('PleaseSelectAtLeastOneProduct'));
      return;
    }

    let details = [];
    $('#productTransferTable tbody tr').each(function () {
      const productId = $(this).data('product-id');
      const quantity = parseInt($(this).find('.transfer-quantity').val());
      const maxQuantity = parseInt($(this).find('.transfer-quantity').attr('max'));

      if (quantity > maxQuantity) {
        abp.notify.error(l('TransferQuantityExceedsAvailable', $('td:nth-child(2)', this).text()));
        return false; // Break the each loop
      }

      details.push({
        productId: productId,
        quantity: quantity,
        fromLocationId: parseInt($(this).find('.storage-location-select-from').val()),
        toLocationId: parseInt($(this).find('.storage-location-select-to').val())
      });
    });

    // If we broke out of the loop due to validation error
    if (details.length !== $('#productTransferTable tbody tr').length) {
      return;
    }

    // Validate all locations are selected
    for (let detail of details) {
      if (!detail.fromLocationId) {
        abp.notify.error(l('PleaseSelectCurrentLocationForAllProducts'));
        return;
      }
      if (!detail.toLocationId) {
        abp.notify.error(l('PleaseSelectNewLocationForAllProducts'));
        return;
      }
    }

    var transferRequest = {
      fromWarehouseId: parseInt(fromWarehouseId),
      toWarehouseId: parseInt(toWarehouseId),
      transferDate: $('#TransferDate').val(),
      notes: $('#Notes').val(),
      transferDetails: details
    };

    abp.ui.setBusy(_$modalTransfer);
    _stockTransactionService
      .createTransferRequest(transferRequest)
      .done(function () {
        _$modalTransfer.modal('hide');
        _$form[0].reset();
        _selectedProducts = [];
        $('#productTransferTable tbody').empty();
        abp.notify.info(l('TransferCreatedSuccessfully'));
        _$table.DataTable().ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modalTransfer);
      });
  });

})(jQuery);
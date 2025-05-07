(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modalExport = $('#ExportModal'),
    _selectedProducts = []; // Thay đổi từ _selectedProductsId sang mảng object chứa đầy đủ thông tin
  _$form = _$modalExport.find('form'),

  // Warehouse
  $('#CreateWarehouseExportBtn').on('click', function () {
    _addWarehouseCreateModal.open({}, function (result) {
      if (result) {
        $('#WarehouseExportDisplay').val(result.warehouseName.trim());
        $('#WarehouseIdExportCreate').val(result.warehouseId);
      }
    });
  });

  var _addWarehouseCreateModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });


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
        storageLocationId: $(this).find('.storage-location-select').val()
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
      warehouseId: $('#WarehouseIdExportCreate').val(),
      details: details
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
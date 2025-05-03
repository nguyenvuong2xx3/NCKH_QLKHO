(function ($) {
  var _storageLocationService = abp.services.app.storageLocation,
        l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#StorageLocationEditModal'),
        _$form = _$modal.find('form');

  $('#EditWarehouseBtn').on('click', function () {
    _addWarehouseEditModal.open({}, function (result) {
      if (result) {
				//console.log(result);
        $('#WarehouseDisplayEdit').val(result.warehouseName.trim());
        $('#WarehouseIdEdit').val(result.warehouseId);
      }
    });
  });

  var _addWarehouseEditModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
    scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
    modalClass: 'AddWarehousesModal',
  });

    function save() {
        if (!_$form.valid()) {
            return;
        }
      var storageLocation = _$form.serializeFormToObject();
        abp.ui.setBusy(_$form);
      _storageLocationService.update(storageLocation).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
        abp.event.trigger('storageLocation.edited', storageLocation);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").on("click", function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]').first().trigger('focus');
    });
})(jQuery);

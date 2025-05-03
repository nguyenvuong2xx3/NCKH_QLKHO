(function ($) {
  var _warehouseService = abp.services.app.warehouse,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#WarehouseEditModal'),
    _$form = _$modal.find('form');

  function save() {
    if (!_$form.valid()) {
      return;
    }

    var warehouse = _$form.serializeFormToObject();

    abp.ui.setBusy(_$form);
    _warehouseService.update(warehouse).done(function () {
      _$modal.modal('hide');
      abp.notify.info(l('SavedSuccessfully'));
      abp.event.trigger('warehouse.edited', warehouse);
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

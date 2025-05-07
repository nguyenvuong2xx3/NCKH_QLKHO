(function ($) {
  var _customerService = abp.services.app.customer,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#CustomerEditModal'),
    _$form = _$modal.find('form');

  function save() {
    if (!_$form.valid()) {
      return;
    }

    var customer = _$form.serializeFormToObject();

    abp.ui.setBusy(_$form);
    _customerService.update(customer)
      .done(function () {
        _$modal.modal('hide');
        abp.notify.info(l('CustomerUpdatedSuccessfully'));
        abp.event.trigger('customer.edited', customer);
      })
      .always(function () {
        abp.ui.clearBusy(_$form);
      });
  }

  // Save button click handler
  _$form.closest('div.modal-content').find(".save-button").click(function (e) {
    e.preventDefault();
    save();
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

})(jQuery);
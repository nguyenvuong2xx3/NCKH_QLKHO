(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#StockTransactionEditModal'),
    _$form = _$modal.find('form');
  _Id = $('input[name="Id"]').val();

  var _customerDetailModal = new app.ModalManager({
    viewUrl: abp.appPath + 'StockTransactions/GetCustomerByStockTransactions',
    //scriptUrl: abp.appPath + 'view-resources/Views/Customers/_AddCustomersModal.js',
    //modalClass: 'AddCustomersModal',
  });

  $('#CustomerDetailBtn').on('click', function () {
    _customerDetailModal.open({ id: _Id });
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


  function save() {
    if (!_$form.valid()) {
      return;
    }

    var storageLocation = _$form.serializeFormToObject();
    console.log(storageLocation)
    parseInt(storageLocation.id)
    if (storageLocation.TransactionType == "Export") {
      abp.ui.setBusy(_$modal);
      _stockTransactionService
        .updateExportStockTransactions(storageLocation)
        .done(function () {
          _$modal.modal('hide');
          _$form[0].reset();
          abp.notify.info(l('SavedSuccessfully'));
        })
        .always(function () {
          abp.ui.clearBusy(_$modal);
        });
    }
    if (storageLocation.TransactionType == "Import") {
      abp.ui.setBusy(_$form);
      $.ajax({
        url: '/StockTransactions/UpdateImportStockTransactions',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(storageLocation),
        success: function (response) {
          if (response.success) {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('stockTransaction.edited', response.data);
            _$modal.DataTable().ajax.reload();
          } else {
            abp.notify.error(response.message || l('AnErrorOccurred'));
          }
        },
        error: function () {
          abp.notify.error(l('AnErrorOccurred'));
        },
        complete: function () {
          abp.ui.clearBusy(_$form);
        }
      });
    }
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

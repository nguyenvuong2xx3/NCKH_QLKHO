(function ($) {
  var _supplierService = abp.services.app.supplier,
        l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#SupplierEditModal'),
    _$form = _$modal.find('form');

  // Phương thức kiểm tra tùy chỉnh cho số điện thoại
  $.validator.addMethod("phoneNumber", function (value, element) {
    return this.optional(element) || /^[0-9\+\-\(\)\s]*$/.test(value);
  }, "Số điện thoại không hợp lệ.");

  // Khởi tạo validate
  _$form.validate({
    rules: {
      Name: {
        required: true,
        maxlength: 256
      },
      Code: {
        required: true,
        maxlength: 256
      },
      Address: {
        required: true,
        maxlength: 500
      },
      PhoneNumber: {
        required: true,
        phoneNumber: true,
        maxlength: 20
      },
      Email: {
        email: true,
        maxlength: 256
      },
      TaxCode: {
        required: true,
        maxlength: 50
      }
    },
    messages: {
      Name: {
        required: "Tên nhà cung cấp là bắt buộc.",
        maxlength: "Tên nhà cung cấp không được vượt quá 256 ký tự."
      },
      Code: {
        required: "Mã nhà cung cấp là bắt buộc.",
        maxlength: "Mã nhà cung cấp không được vượt quá 256 ký tự."
      },
      Address: {
        required: "Địa chỉ là bắt buộc.",
        maxlength: "Địa chỉ không được vượt quá 500 ký tự."
      },
      PhoneNumber: {
        required: "Số điện thoại là bắt buộc.",
        phoneNumber: "Số điện thoại không hợp lệ.",
        maxlength: "Số điện thoại không được vượt quá 20 ký tự."
      },
      Email: {
        email: "Địa chỉ email không hợp lệ.",
        maxlength: "Email không được vượt quá 256 ký tự."
      },
      TaxCode: {
        required: "Mã số thuế là bắt buộc.",
        maxlength: "Mã số thuế không được vượt quá 50 ký tự."
      }
    },
    errorPlacement: function (error, element) {
      error.insertAfter(element);
    },
    highlight: function (element) {
      $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
      $(element).closest('.form-group').removeClass('has-error');
    }
  });

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var supplier = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _supplierService.update(supplier).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('supplier.edited', supplier);
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

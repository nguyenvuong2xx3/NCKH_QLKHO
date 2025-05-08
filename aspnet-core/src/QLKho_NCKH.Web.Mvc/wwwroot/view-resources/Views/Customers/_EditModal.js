(function ($) {
  var _customerService = abp.services.app.customer,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#CustomerEditModal'),
    _$form = _$modal.find('form');
  // Phương thức kiểm tra tùy chỉnh
  $.validator.addMethod("phoneNumber", function (value, element) {
    return this.optional(element) || /^[0-9\+\-\(\)\s]*$/.test(value);
  }, "Số điện thoại không hợp lệ.");

  // Khởi tạo validate cho biểu mẫu CustomerEditForm
  $("form[name='CustomerEditForm']").validate({
    rules: {
      Code: {
        required: true,
        maxlength: 50
      },
      Name: {
        required: true,
        maxlength: 100
      },
      Address: {
        maxlength: 200
      },
      PhoneNumber: {
        phoneNumber: true,
        maxlength: 20
      },
      Email: {
        email: true,
        maxlength: 100
      },
      TaxCode: {
        maxlength: 20
      },
      IsActive: {
        required: true
      }
    },
    messages: {
      Code: {
        required: "Mã khách hàng là bắt buộc.",
        maxlength: "Mã khách hàng không được vượt quá 50 ký tự."
      },
      Name: {
        required: "Tên khách hàng là bắt buộc.",
        maxlength: "Tên khách hàng không được vượt quá 100 ký tự."
      },
      Address: {
        maxlength: "Địa chỉ không được vượt quá 200 ký tự."
      },
      PhoneNumber: {
        phoneNumber: "Số điện thoại không đúng định dạng.",
        maxlength: "Số điện thoại không được vượt quá 20 ký tự."
      },
      Email: {
        email: "Địa chỉ email không đúng định dạng.",
        maxlength: "Email không được vượt quá 100 ký tự."
      },
      TaxCode: {
        maxlength: "Mã số thuế không được vượt quá 20 ký tự."
      },
      IsActive: {
        required: "Trạng thái là bắt buộc."
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
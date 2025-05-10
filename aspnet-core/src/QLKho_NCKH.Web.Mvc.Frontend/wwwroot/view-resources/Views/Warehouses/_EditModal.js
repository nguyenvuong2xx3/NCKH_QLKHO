(function ($) {
  var _warehouseService = abp.services.app.warehouse,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#WarehouseEditModal'),
    _$form = _$modal.find('form');

  // Phương thức kiểm tra tùy chỉnh
  $.validator.addMethod("validArea", function (value, element) {
    return this.optional(element) || /^[0-9]/.test(value);
  }, "Diện tích không hợp lệ. Vui lòng nhập theo định dạng, ví dụ: 500m2.");

  // Khởi tạo validate cho biểu mẫu tạo mới kho
  _$form.validate({
    rules: {
      Code: {
        required: true,
        maxlength: 256
      },
      Name: {
        required: true,
        maxlength: 256
      },
      Location: {
        required: true,
        maxlength: 256
      },
      TotalArea: {
        required: true,
        validArea: true,
        maxlength: 20
      }
    },
    messages: {
      Code: {
        required: "Mã kho là bắt buộc.",
        maxlength: "Mã kho không được vượt quá 256 ký tự."
      },
      Name: {
        required: "Tên kho là bắt buộc.",
        maxlength: "Tên kho không được vượt quá 256 ký tự."
      },
      Location: {
        required: "Địa chỉ kho là bắt buộc.",
        maxlength: "Địa chỉ kho không được vượt quá 256 ký tự."
      },
      TotalArea: {
        required: "Diện tích kho là bắt buộc.",
        validArea: "Diện tích không hợp lệ. Vui lòng nhập theo định dạng, ví dụ: 500m2.",
        maxlength: "Diện tích không được vượt quá 20 ký tự."
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

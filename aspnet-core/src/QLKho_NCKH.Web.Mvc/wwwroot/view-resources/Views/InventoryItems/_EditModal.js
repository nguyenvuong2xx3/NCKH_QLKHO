(function ($) {
  app.modals.EditInventoryItemModal = function () {
    var _modalManager;
    var _inventoryItemService = abp.services.app.inventoryItem;
    var _$form;
    var l = abp.localization.getSource('QLKho_NCKH');

    this.init = function (modalManager) {
      _modalManager = modalManager;
      _$form = _modalManager.getModal().find('form[name="InventoryItemEditForm"]');

      console.log("Edit modal initialized");
      console.log("Form element:", _$form.length);

      // Initialize form validation
      //_$form.validate({
      //  rules: {
      //    Quantity: {
      //      required: true,
      //      min: 0
      //    },
      //    UnitPrice: {
      //      min: 0
      //    }
      //  },
      //  messages: {
      //    Quantity: {
      //      required: "Số lượng tồn kho là bắt buộc",
      //      min: "Số lượng không thể âm"
      //    },
      //    UnitPrice: {
      //      min: "Đơn giá không thể âm"
      //    }
      //  },
      //  errorPlacement: function (error, element) {
      //    error.insertAfter(element);
      //  },
      //  highlight: function (element) {
      //    $(element).closest('.form-group').addClass('has-error');
      //  },
      //  unhighlight: function (element) {
      //    $(element).closest('.form-group').removeClass('has-error');
      //  }
      //});

      // Custom validation for reserved quantity
      //$.validator.addMethod("reservedQuantityMax", function (value, element) {
      //  var quantity = parseFloat(_$form.find('input[name="Quantity"]').val());
      //  return parseFloat(value) <= quantity;
      //}, "Số lượng đặt trước không thể lớn hơn số lượng tồn kho");

      //// Apply custom validation
      //_$form.find('input[name="ReservedQuantity"]').rules('add', {
      //  reservedQuantityMax: true
      //});

      // Update max reserved quantity when quantity changes
      _$form.find('input[name="Quantity"]').on('change', function () {
        var newQuantity = parseFloat($(this).val());
        _$form.find('input[name="ReservedQuantity"]').attr('max', newQuantity);
        if (parseFloat(_$form.find('input[name="ReservedQuantity"]').val()) > newQuantity) {
          _$form.find('input[name="ReservedQuantity"]').valid();
        }
      });

      // Focus first input - Sử dụng sự kiện modal shown của Bootstrap
      _modalManager.getModal().on('shown.bs.modal', function () {
        _$form.find('input[type="number"]').first().trigger('focus');
      });
    };
    function parseDecimal(value) {
      // Chuyển đổi chuỗi sang số với độ chính xác decimal
      value = value.toString().replace(/\./g, '').replace(',', '.');
      return parseFloat(value) || 0;
    }
    this.save = function () {
      event.preventDefault();  // Ngăn reload trang
      if (!_$form.valid()) {
        console.log("Form validation failed");
        return;
      }

      var inventoryItem = _$form.serializeFormToObject();
      console.log("Saving inventory item:", inventoryItem);

      // Convert numeric fields
      inventoryItem.Quantity = parseInt(inventoryItem.Quantity);
      //inventoryItem.ReservedQuantity = parseInt(inventoryItem.ReservedQuantity);
      inventoryItem.UnitPrice = inventoryItem.UnitPrice
        ? parseDecimal(inventoryItem.UnitPrice)
        : null;

      abp.ui.setBusy(_modalManager.getModal());
      _inventoryItemService.updateInventoryItem(inventoryItem)
        .done(function () {
          abp.notify.success(l('SavedSuccessfully'));
          _modalManager.setResult(inventoryItem);
          _modalManager.close();
        })
        .always(function () {
          abp.ui.clearBusy(_modalManager.getModal());
        });
    };
  };
})(jQuery);
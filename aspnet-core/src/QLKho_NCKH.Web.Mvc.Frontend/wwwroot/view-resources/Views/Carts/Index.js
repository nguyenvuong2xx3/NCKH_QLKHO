(function ($) {
  var _cartService = abp.services.app.cart;
  var _stockTransactionService = abp.services.app.stockTransaction;

  $('.btn-reduce').on('click', function (e) {
    e.preventDefault();

    var productId = $(this).data('id');
    var cartItem = $(this).closest('.cart-item');
    var quantityInput = cartItem.find('.quantity-input');
    var priceElement = cartItem.find('.product-price');

    var unitPriceText = priceElement.attr('data-unit-price');
    if (!unitPriceText) {
      console.error("Không tìm thấy data-unit-price!");
      return;
    }

    var unitPrice = parseFloat(unitPriceText.replace(/,/g, "")) || 0;
    var currentQuantity = parseInt(quantityInput.val()) || 0;

    if (currentQuantity > 1) {
      var bool = false;
      _cartService.addToCart(productId, 1, bool).done(function () {
        abp.notify.success("Giảm số lượng sản phẩm thành công");
      });

      var newQuantity = currentQuantity - 1;
      quantityInput.val(newQuantity);
      var newPrice = unitPrice * newQuantity;
      priceElement.text(newPrice.toLocaleString('vi-VN') + " đ");
      updateTotalPrice();
    } else {
      $('#confirmDeleteModal').modal('show');

      $('#confirmDeleteBtn').off('click').on('click', function () {
        _cartService.deleteCart(productId).done(function () {
          abp.notify.success("Xóa sản phẩm thành công");
          location.reload();
        });
      });

      $('#confirmDeleteModal').off('hidden.bs.modal').on('hidden.bs.modal', function () {
        quantityInput.val(1);
        var newPrice = unitPrice * 1;
        priceElement.text(newPrice.toLocaleString('vi-VN') + " đ");
        updateTotalPrice();
      });
    }
  });

  $('.btn-increase').on('click', function (e) {
    e.preventDefault();
    var productId = $(this).data('id');
    var cartItem = $(this).closest('.cart-item');
    var quantityInput = cartItem.find('.quantity-input');
    var priceElement = cartItem.find('.product-price');

    var unitPriceText = priceElement.attr('data-unit-price');
    var unitPrice = parseFloat(unitPriceText.replace(/,/g, "")) || 0;
    var currentQuantity = parseInt(quantityInput.val()) || 0;

    if (currentQuantity >= 10) {
      abp.notify.error("Số lượng sản phẩm không được vượt quá 10");
      return;
    }

    var bool = true;
    _cartService.addToCart(productId, 1, bool).done(function () {
      abp.notify.success("Thêm vào giỏ hàng thành công");
    });

    var newQuantity = currentQuantity + 1;
    quantityInput.val(newQuantity);
    var newPrice = unitPrice * newQuantity;
    priceElement.text(newPrice.toLocaleString('vi-VN') + " đ");
    updateTotalPrice();
  });

  function updateTotalPrice() {
    var total = 0;

    $(".cart-item").each(function () {
      var priceText = $(this).find(".product-price").text().replace(/[^\d]/g, "");
      var price = parseFloat(priceText) || 0;
      total += price;
    });

    $("#totalPrice").text(total.toLocaleString('vi-VN') + " đ");
  }

  $('.btn-add-detail').on('click', function (e) {
    e.preventDefault();
    var productId = $(this).data('id');
    var quantityInput = parseInt($('#quantity-add-detail').val()) || 1;

    var bool = true;
    _cartService.addToCart(productId, quantityInput, bool).done(function () {
      abp.notify.success("Thêm vào giỏ hàng thành công");
    });
  });

  let currentProductIdToDelete = null;

  $(".btn-delete").on("click", function () {
    currentProductIdToDelete = $(this).data("id");
    $("#confirmDeleteModal").modal("show");
  });

  $("#confirmDeleteBtn").on("click", function () {
    if (currentProductIdToDelete) {
      _cartService.clearProduct(currentProductIdToDelete).done(function () {
        abp.notify.success("Xóa sản phẩm thành công!");
        location.reload();
      });
    }
  });

  $('.quantity-input').on('change', function (e) {
    var productId = $(this).data('id');
    var quantity = parseInt($(this).val());
    var bool = true;

    _cartService.updateCart(productId, quantity).done(function () {
      location.reload();
      updateTotalPrice();
    });
  });

  $("#btnCheckout").on("click", function (e) {
    e.preventDefault();

    const userId = $(this).data("userid");
    const nameUser = $(this).data("nameuser");

    //let orderItems = [];
    //$(".cart-item").each(function () {
    //  const productId = $(this).find(".btn-reduce").data("id");
    //  const quantity = parseInt($(this).find(".quantity-input").val()) || 0;
    //  const unitPriceText = $(this).find(".product-price").data("unit-price");
    //  const unitPrice = parseFloat(unitPriceText.replace(/,/g, "")) || 0;

    //  orderItems.push({
    //    ProductId: productId,
    //    Quantity: quantity,
    //    UnitPrice: unitPrice,
    //    DiscountPrice: 0
    //  });
    //});

    //const totalAmount = orderItems.reduce((sum, item) => sum + (item.UnitPrice * item.Quantity), 0);

    //const orderData = {
    //  UserId: userId,
    //  NameUser: nameUser,
    //  TotalAmount: totalAmount,
    //  DiscountAmount: 0,
    //  PaymentMethod: 0,
    //  Items: orderItems
    //};

    //console.log("Dữ liệu gửi đi:", orderData);

    let details = [];
    $(".cart-item").each(function () {
      details.push({
        productId: $(this).find('.btn-delete').data('id'),
        quantity: parseInt($(this).find('.quantity-input').val()),
        unitPrice: parseFloat($(this).find('.product-price').data('unit-price')),
        storageLocationId: 0 // Cần xác định storageLocationId phù hợp
      });
    });
		console.log("Chi tiết đơn hàng:", details);
    function generateTransactionCode() {
      const now = new Date();
      const pad = (n) => n.toString().padStart(2, '0');

      // Format: NK_DDMMYY_HHMMSS (18 ký tự)
      return `NK${pad(now.getDate())}${pad(now.getMonth() + 1)}${now.getFullYear().toString().slice(-2)}_${pad(now.getHours())}${pad(now.getMinutes())}${pad(now.getSeconds())}`;

      // Hoặc format ngắn hơn nếu cần (NK_DDMM_HHMM - 11 ký tự)
      // return `NK${pad(now.getDate())}${pad(now.getMonth() + 1)}_${pad(now.getHours())}${pad(now.getMinutes())}`;
    }

    var exportRequest = {
      transactionCode: generateTransactionCode(),
      customerId: userId,
      note: `Đơn đặt hàng của khách ${nameUser}`,
      referenceNumber: "",
      exportRequestDetails: details
    };

    abp.ui.setBusy($('#checkoutModal'));
    _stockTransactionService.createExportRequest(exportRequest)
      .done(function () {
        abp.notify.info('Tạo phiếu xuất kho thành công');
        window.location.href = "/Order/Success";
      })
      .fail(function (error) {
        abp.notify.error(error.message || 'Có lỗi xảy ra khi tạo phiếu xuất kho');
      })
      .always(function () {
        abp.ui.clearBusy($('#checkoutModal'));
      });
  });

  $(document).ready(function () {
    updateTotalPrice();
  });

  $(".btn-reduce, .btn-increase").on("click", function () {
    setTimeout(updateTotalPrice, 200);
  });
})(jQuery);
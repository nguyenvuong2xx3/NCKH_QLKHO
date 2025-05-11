(function ($) {
  $('#searchForm').on('submit', function (e) {
    e.preventDefault();
    var keyword = $('#searchInput').val().trim();

    if (keyword) {
      window.location.href = "/Home/SearchProducts?keyword=" + encodeURIComponent(keyword);
    } else {
      window.location.href = "/";
    }
  });

  $('.btn-view-detail').on('click', function (e) {
    e.preventDefault();
    var productId = $(this).data('id');

    if (productId) {
      window.location.href = "/Home/GetDetailProduct?Id=" + productId;
    } else {
      console.log("Không tìm thấy sản phẩm!");
    }
  });

  $('.cart').on('click', function (e) {
    e.preventDefault();
    window.location.href = "/Carts/Index";
  });

  $('.btn-add-cart').on('click', function (e) {
		console.log("click!!!");
    var _cartService = abp.services.app.cart;
    e.preventDefault();

    var productId = $(this).data('id');
    var bool = true;

    _cartService.addToCart(
      productId,
      1,
      bool
    ).done(function () {
      abp.notify.info("Thêm sản phẩm vào giỏ hàng thành công");
      window.location.href = "/Carts/Index";
    });
  });

  $('.btn-all-product').on('click', function (e) {
    window.location.href = "/Home/PageAllProduct";
  });

  $('.btn-all-product-byId').on('click', function (e) {
    e.preventDefault();
    var categoryId = $(this).data('id');
    window.location.href = "/Home/PageAllProduct?categoryId=" + categoryId;
  });

  $(document).ready(function () {
    let currentIndex = 0;
    const pageSize = 5;
    const totalProducts = $(".product-card").length;
    let animating = false;

    function updateView(direction) {
      if (animating) return;
      animating = true;

      let start = currentIndex;
      let end = currentIndex + pageSize;

      let oldProducts = $(".product-card.active");
      let newProducts = $(".product-card").slice(start, end);

      newProducts.css({
        display: "block",
        opacity: 0,
        position: "relative",
        left: direction === "next" ? "200px" : "-200px"
      });

      oldProducts.animate(
        {
          left: direction === "next" ? "-200px" : "200px",
          opacity: 0.6
        },
        500,
        function () {
          $(this).removeClass("active").hide();
        }
      );

      setTimeout(function () {
        newProducts.addClass("active").animate(
          {
            left: "0",
            opacity: 1
          },
          500,
          function () {
            animating = false;
          }
        );

        $("#prevBtn").prop("disabled", currentIndex === 0);
        $("#nextBtn").prop("disabled", currentIndex + pageSize >= totalProducts);
      }, 500);
    }

    $("#nextBtn").click(function () {
      if (currentIndex + pageSize < totalProducts) {
        currentIndex += pageSize;
        updateView("next");
      }
    });

    $("#prevBtn").click(function () {
      if (currentIndex - pageSize >= 0) {
        currentIndex -= pageSize;
        updateView("prev");
      }
    });

    updateView("next");
  });
})(jQuery);
(function ($) {
  $(document).ready(function () {
    $(document).on('click', '.view-details', function () {
      var transactionId = $(this).data('id');

      // Gọi đến service thay vì controller
      abp.services.app.stockTransaction.getStockTransaction(transactionId)
        .done(function (data) {
          if (data) {
            // Hiển thị dữ liệu lên giao diện
            $('#orderCode').text(data.transactionCode || 'N/A');
            $('#orderDate').text(new Date(data.transactionDate).toLocaleString());
            var totalAmount = Array.isArray(data.detailProduct)
              ? data.detailProduct.reduce((sum, p) => sum + (p.totalPrice || 0), 0)
              : 0;
            $('#orderTotal').text(totalAmount.toLocaleString() + ' đ');

            var statusText = '';
            switch (data.status) {
              case 0: statusText = '<span class="badge badge-info">Chờ duyệt</span>'; break;
              case 1: statusText = '<span class="badge badge-success">Đã duyệt</span>'; break;
              case 2: statusText = '<span class="badge badge-primary">Hoàn thành</span>'; break;
              case 3: statusText = '<span class="badge badge-danger">Đã hủy</span>'; break;
              default: statusText = '<span class="badge badge-secondary">' + data.status + '</span>';
            }
            $('#orderStatus').html(statusText);

            var typeText = data.transactionType === 0
              ? '<span class="badge badge-success">Nhập kho</span>'
              : '<span class="badge badge-warning">Xuất kho</span>';
            $('#orderType').html(typeText);

            $('#productList').empty();
            if (Array.isArray(data.detailProduct)) {
              data.detailProduct.forEach((product, index) => {
                $('#productList').append(`
                  <tr>
                    <td>${index + 1}</td>
                    <td>${product.productCode || 'N/A'}</td>
                    <td>${product.productName || 'N/A'}</td>
                    <td>${product.quantity || 0}</td>
                    <td>${(product.unitPrice || 0).toLocaleString()} đ</td>
                    <td>${(product.totalPrice || 0).toLocaleString()} đ</td>
                  </tr>
                `);
              });
            } else {
              $('#productList').html('<tr><td colspan="6">Không có sản phẩm nào.</td></tr>');
            }

            //$('#orderDetailsModal').modal('show');
            var modal = new bootstrap.Modal(document.getElementById('orderDetailsModal'));
            modal.show();
          } else {
            alert('Dữ liệu đơn hàng không hợp lệ.');
          }
        })
        .fail(function (error) {
          abp.notify.error(error.message || 'Không thể lấy chi tiết đơn hàng.');
        });
    });
  });
})(jQuery);

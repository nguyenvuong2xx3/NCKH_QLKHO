(function ($) {
  var _stockTransactionService = abp.services.app.stockTransaction,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$table = $('#StockTransactionsTable');

  var _$stockTransactionTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    listAction: {
      ajaxFunction: _stockTransactionService.getStockTransactions,
      inputFilter: function () {
        return $('#StockTransactionSearchForm').serializeFormToObject(true);
      }
    },
    columnDefs: [
      {
        targets: 0,
        data: 'transactionCode',
        className: 'dt-center',
        orderable: false
      },
      {
        targets: 1,
        data: 'transactionType',
        className: 'dt-center',
        render: function (data) {
          return data === 0 ? 'Nhập kho' : data === 1 ? 'Xuất kho' : 'Chuyển kho';
        }
      },
      {
        targets: 2,
        data: 'fromWarehouseName',
        className: 'dt-center',
        render: function (data, type, row) {
          return data ? data : ''; // hoặc 'N/A'
        }
      },
      {
        targets: 3,
        data: 'toWarehouseName',
        className: 'dt-center',
        render: function (data, type, row) {
          return data ? data : '';
        }
      },
      {
        targets: 4,
        data: 'referenceNumber',
        className: 'dt-center'
      },
      {
        targets: 5,
        data: 'status',
        className: 'dt-center',
        render: function (data) {
          var badgeClass = data === 1 ? 'bg-success' : 'bg-warning';
          return `<span class="badge ${badgeClass}">${getStatusText(data)}</span>`;
        }
      },
      {
        targets: 6,
        data: 'creationTime',
        render: function (data, type, row) {
          if (data) {
            var date = new Date(data);
            var day = date.getDate().toString().padStart(2, '0'); // Lấy ngày, đảm bảo có 2 chữ số
            var month = (date.getMonth() + 1).toString().padStart(2, '0'); // Lấy tháng, đảm bảo có 2 chữ số
            var year = date.getFullYear(); // Lấy năm
            return day + '/' + month + '/' + year; // Định dạng dd/mm/yyyy
          }
          return ''; // Trả về chuỗi rỗng nếu không có dữ liệu
        }
      },
      {
        targets: 7,
        data: null,
        sortable: false,
        autoWidth: false,
        defaultContent: '',
        render: (data, type, row, meta) => {
          return [
            `   <button type="button" class="btn btn-sm bg-secondary edit-stockTransaction" data-stockTransaction-id="${row.id}" data-toggle="modal" data-target="#StockTransactionEditModal">`,
            `       <i class="fas fa-pencil-alt"></i> ${l('Duyệt')}`,
            '   </button>',
            `   <button type="button" class="btn btn-sm bg-danger delete-stockTransaction" data-stockTransaction-id="${row.id}" data-stockTransaction-name="${row.name}">`,
            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
            '   </button>',
          ].join('');
        }
      }
    ]
  });

  $(document).on('click', '.edit-stockTransaction', function (e) {
    var stockTransactionId = $(this).attr("data-stockTransaction-id");

    e.preventDefault();
    abp.ajax({
      url: abp.appPath + 'StockTransactions/Edit?StockTransactionId=' + stockTransactionId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#StockTransactionEditModal div.modal-content').html(content);
      },
      error: function (e) {
      }
    })
  });
  function getStatusText(status) {
    switch (status) {
      case 0: return 'Chờ duyệt';
      case 1: return 'Đã duyệt';
      //case 2: return 'Đã duyệt';
      //case 3: return 'Hoàn thành';
      default: return 'Đã hủy';
    }
  }

  // Xử lý tìm kiếm
  $('#SearchButton').click(function () {
    _$stockTransactionTable.ajax.reload();
  });

  // Xử lý xóa
  $(document).on('click', '.delete-stockTransaction', function () {
    var stockTransactionId = $(this).attr("data-stockTransaction-id");
    var stockTransactionName = $(this).attr('data-stockTransaction-name');

    deleteSupplier(stockTransactionId, stockTransactionName);
  });
  function deleteSupplier(stockTransactionId, stockTransactionName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('AreYouSureWantToDelete'),
        stockTransactionName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _stockTransactionService.deleteStockTransaction(
            stockTransactionId
          ).done(() => {
            abp.notify.info(l('SuccessfullyDeleted'));
            _$stockTransactionTable.ajax.reload();
          });
        }
      }
    );
  }

})(jQuery);
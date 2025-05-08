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
          var badgeClass = data === 2 ? 'bg-success' : 'bg-warning';
          return `<span class="badge ${badgeClass}">${getStatusText(data)}</span>`;
        }
      },
      {
        targets: 6,
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
      //case 1: return 'Chờ duyệt';
      case 2: return 'Đã duyệt';
      case 3: return 'Hoàn thành';
      default: return 'Đã hủy';
    }
  }

  // Xử lý tìm kiếm
  $('#SearchButton').click(function () {
    _$stockTransactionTable.ajax.reload();
  });

  // Xử lý xóa
  $(document).on('click', '.delete-btn', function () {
    var id = $(this).data('id');
    abp.message.confirm(
      l('AreYouSureWantToDelete'),
      l('DeleteConfirmation'),
      function (confirmed) {
        if (confirmed) {
          _stockTransactionService.delete(id).done(function () {
            _$stockTransactionTable.ajax.reload();
            abp.notify.success(l('SuccessfullyDeleted'));
          });
        }
      }
    );
  });

})(jQuery);
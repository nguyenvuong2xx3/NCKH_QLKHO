(function ($) {
	var _stockTransactionService = abp.services.app.stockTransaction,
		l = abp.localization.getSource('QLKho_NCKH'),
		_$table = $('#StockTransactionsTable');

	// Khai báo biến toàn cục để lưu giá trị ngày
	var _selectedDateRange = {
		StartTime: null,
		EndTime: null
	};

	// Khởi tạo DateRangePicker
	$('#StartEndRange').daterangepicker({
		autoUpdateInput: false,
		opens: 'left',
		locale: {
			format: 'DD/MM/YYYY',
			applyLabel: 'Áp dụng',
			cancelLabel: 'Hủy bỏ',
			fromLabel: 'Từ',
			toLabel: 'Đến',
			customRangeLabel: 'Tùy chỉnh',
			daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
			monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
				'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
			firstDay: 1
		}
	});

	// Xử lý khi áp dụng date range
	$('#StartEndRange').on('apply.daterangepicker', function (ev, picker) {
		$(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
		_selectedDateRange.StartTime = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
		_selectedDateRange.EndTime = picker.endDate.format('YYYY-MM-DDT23:59:59Z');
	});

	// Xử lý khi hủy chọn date range
	$('#StartEndRange').on('cancel.daterangepicker', function (ev, picker) {
		$(this).val('');
		_selectedDateRange.StartTime = null;
		_selectedDateRange.EndTime = null;
	});

	var _$stockTransactionTable = _$table.DataTable({
		paging: true,
		serverSide: true,
		listAction: {
			ajaxFunction: _stockTransactionService.getStockTransactions,
			inputFilter: function () {
				var formData = $('#StockTransactionSearchForm').serializeFormToObject(true);
				// Thêm startTime và endTime từ biến _selectedDateRange vào formData
				if (_selectedDateRange.StartTime && _selectedDateRange.EndTime) {
					formData.startTime = _selectedDateRange.StartTime;
					formData.endTime = _selectedDateRange.EndTime;
				}
				return formData;
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
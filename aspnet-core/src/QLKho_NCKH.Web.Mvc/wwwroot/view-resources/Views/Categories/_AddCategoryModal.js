(function ($) {
	var _categoryService = abp.services.app.category,

		l = abp.localization.getSource('QLKho_NCKH'),
		_$modal = $('#AddCategoryModal'),
		_$form = _$modal.find('form'),
		_$table = $('#addCategoryModalTable');

	var _$categoryTable = _$table.DataTable({
		paging: true,
		serverSide: true,
		listAction: {
			ajaxFunction: _categoryService.getAll,
			inputFilter: function () {
				//return $('#CategorySearchForm').serializeFormToObject(true);
			}
		},
		buttons: [
			{
				name: 'refresh',
				text: '<i class="fas fa-redo-alt"></i>',
				action: () => _$categoryTable.draw(false)
			}
		],
		responsive: {
			details: {
				type: 'column'
			}
		},
		columnDefs: [
			{
				targets: 0,
				data: null,
				sortable: false,
				defaultContent: '',
				render: function (data) {
					return (
						'<label for="radio_' +
						data.id +
						'" class="radio form-check">' +
						'<input type="radio" name="selectedUser" id="radio_' +
						data.id +
						'" class="form-check-input" />&nbsp;' +
						'<span class="form-check-label"></span>' +
						'</label>'
					);
				},
			},
			{
				targets: 1,
				data: 'name',
				sortable: false
			},
		]
	});

	// Thêm sự kiện khi chọn radio button
	_$table.on('change', 'input[type="radio"]', function () {
		var rowData = _$categoryTable.row($(this).closest('tr')).data();
		_$modal.data('selectedCategory', rowData); // Lưu category được chọn vào modal
	});

	// Xử lý khi nhấn nút Save trong modal
	_$modal.find('.save-button').click(function () {
		var selectedCategory = _$modal.data('selectedCategory');
		if (!selectedCategory) {
			abp.message.warn(l('PleaseSelectACategory'));
			return;
		}

		// Đóng modal và trả về category được chọn
		_addCategoryModal.close(selectedCategory);
	});




	let timeOut = null;
	$("#Filter").on("keyup", function () {
		clearTimeout(timeOut);
		timeOut = setTimeout(function () {
			_$categoryTable.ajax.reload();
		}, 300);
	});



})(jQuery);



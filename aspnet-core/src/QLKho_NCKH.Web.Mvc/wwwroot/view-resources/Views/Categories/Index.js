(function ($) {
	var _categoryService = abp.services.app.category,

		l = abp.localization.getSource('QLKho_NCKH'),
		_$modal = $('#CategoryCreateModal'),
		_$form = _$modal.find('form'),
		_$table = $('#CategoriesTable');

	var _$categoryTable = _$table.DataTable({
		paging: true,
		serverSide: true,
		listAction: {
			ajaxFunction: _categoryService.getAll,
			inputFilter: function () {
				return $('#CategorySearchForm').serializeFormToObject(true);
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
				data: 'name',
				sortable: false
			},
			{
				targets: 1,
				data: 'description',
				sortable: false
			},
			//{
			//	targets: 2,
			//	data: 'parentId',
			//	sortable: false,
			//	render: function (data, type, row) {
			//		if (data == null) {
			//			return '<span class="text-success">Danh mục gốc</span>';
			//		} else {
			//			return '<span class="text-secondary">Danh mục con</span>';
			//		}
			//	}
			//},
			{
				targets: 2,
				data: null,
				sortable: false,
				autoWidth: false,
				defaultContent: '',
				render: (data, type, row, meta) => {
					return [
						`   <button type="button" class="btn btn-sm bg-secondary edit-category" data-category-id="${row.id}" data-toggle="modal" data-target="#CategoryEditModal">`,
						`       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
						'   </button>',
						`   <button type="button" class="btn btn-sm bg-danger delete-category" data-category-id="${row.id}" data-category-name="${row.name}">`,
						`       <i class="fas fa-trash"></i> ${l('Delete')}`,
						'   </button>',
						`   <button type="button" class="btn btn-sm bg-info detail-category" data-category-id="${row.id}" data-toggle="modal" data-target="#CategoryDetailModal">`,
						`       <i class="fas fa-eye"></i> ${l('Details')}`,
						'   </button>'
					].join('');
				}
			}
		]
	});

	_$form.validate({
		rules: {
			Name: {
				required: true,
				maxlength: 80,
			},
			Description: {
				required: true,
				maxlength: 500,
			},
		},
		messages: {
			Name: {
				required: "Vui lòng nhập tên danh mục.",
				maxlength: "Tên danh mục không được quá dài",
			},
			Description: {
				required: "Vui lòng nhập mô tả.",
				maxlength: "Mô tả không được vượt quá 500 ký tự.",
			},
		},
		errorClass: "error",
		errorPlacement: function (error, element) {
			error.insertAfter(element);
		},
		highlight: function (element) {
			$(element).closest(".form-group").addClass("has-error");
		},
		unhighlight: function (element) {
			$(element).closest(".form-group").removeClass("has-error");
		},
	});



	_$form.find('.save-button').on('click', (e) => {
		e.preventDefault();

		if (!_$form.valid()) {
			return;
		}

		var category = _$form.serializeFormToObject(); // Chuyển dữ liệu form thành object
		console.log(category);

		abp.ui.setBusy(_$modal); // Hiển thị trạng thái bận

		_categoryService.create(category).done(function () {
			_$modal.modal('hide'); // Đóng modal sau khi lưu thành công
			_$form[0].reset(); // Xóa dữ liệu trong form
			abp.notify.info(l('SavedSuccessfully')); // Hiển thị thông báo
			_$categoryTable.ajax.reload(); // Làm mới bảng sản phẩm
		}).always(function () {
			abp.ui.clearBusy(_$modal); // Ẩn trạng thái bận
		});
	});

	$(document).on('click', '.delete-category', function () {
		var categoryId = $(this).attr("data-category-id");
		var categoryName = $(this).attr('data-category-name');
		console.log(categoryId)
		deleteCategory(categoryId, categoryName);
	});


	function deleteCategory(categoryId, categoryName) {
		abp.message.confirm(
			abp.utils.formatString(
				l('Bạn có chắc chắn xóa danh mục {0}'),
				categoryName),
			null,
			(isConfirmed) => {
				if (isConfirmed) {
					_categoryService.delete(
						categoryId
					).done(() => {
						abp.notify.info(l('SuccessfullyDeleted'));
						_$categoryTable.ajax.reload();
					});
				}
			}
		);
	}



	$(document).on('click', '.edit-category', function (e) {
		var categoryId = $(this).attr("data-category-id");
		console.log(categoryId);
		e.preventDefault();
		abp.ajax({
			url: abp.appPath + 'Categories/EditModal?categoryId=' + categoryId,
			type: 'POST',
			dataType: 'html',
			success: function (content) {
				$('#CategoryEditModal div.modal-content').html(content);
			},
			error: function (e) {
			}
		});
	});

	$(document).on('click', 'a[data-target="#CategoryCreateModal"]', (e) => {
		$('.nav-tabs a[href="#user-details"]').tab('show')
	});

	abp.event.on('category.edited', (data) => {
		_$categoryTable.ajax.reload();
	});

	_$modal.on('shown.bs.modal', () => {
		_$modal.find('input:not([type=hidden]):first').focus();
	}).on('hidden.bs.modal', () => {
		_$form.clearForm();
	});

	$('.btn-search').on('click', (e) => {
		_$categoryTable.ajax.reload();
	});

	$('.txt-search').on('keypress', (e) => {
		if (e.which == 13) {
			_$categoryTable.ajax.reload();
			return false;
		}
	});


	//$(document).on("click", ".detail-category", function () {
	//	var categoryId = $(this).attr("data-category-id");
	//	window.location.href = "/Categories/Detail?categoryId=" + categoryId;
	//});

	$(document).on('click', '.detail-category', function (e) {
		var categoryId = $(this).attr("data-category-id");
		console.log(categoryId);
		e.preventDefault();
		abp.ajax({
			url: abp.appPath + 'Categories/DetailModal?categoryId=' + categoryId,
			type: 'POST',
			dataType: 'html',
			success: function (content) {
				$('#CategoryDetailModal div.modal-content').html(content);
			},
			error: function (e) {
			}
		});
	});




	let timeOut = null;
	$("#Filter").on("keyup", function () {
		clearTimeout(timeOut);
		timeOut = setTimeout(function () {
			_$categoryTable.ajax.reload();
		}, 300);
	});



})(jQuery);



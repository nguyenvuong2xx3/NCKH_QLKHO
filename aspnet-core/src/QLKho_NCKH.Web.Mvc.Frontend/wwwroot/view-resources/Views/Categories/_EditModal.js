(function ($) {
	var _categoryService = abp.services.app.category,
		l = abp.localization.getSource('QLKho_NCKH'),
		_$modal = $('#CategoryEditModal'),
		_$form = _$modal.find('form');

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

	function save() {
		if (!_$form.valid()) {
			return;
		}

		var category = _$form.serializeFormToObject(); // chuyển form thành object để gửi API

		abp.ui.setBusy(_$form); // hiển thị loading (disable form)
		_categoryService.update(category).done(function () {
			_$modal.modal('hide'); // đóng modal sau khi lưu thành công
			abp.notify.info(l('SavedSuccessfully')); // hiển thị thông báo lưu thành công
			abp.event.trigger('category.edited', category); // kích hoạt sự kiện product.edited
		}).always(function () {
			abp.ui.clearBusy(_$form); // xóa trạng thái loading
		});

		//alway() sau khi API chạy xong( thành công hay thất bại) ==> xóa trạng thái loading
	}

	_$form.closest('div.modal-content').find(".save-button").click(function (e) {
		e.preventDefault(); // ngăn chặn reload trang
		save(); // gọi hàm save() để cập nhật sản phẩm khi bấm lưu
	});

	_$form.find('input').on('keypress', function (e) {
		if (e.which === 13) { // nếu nhấn enter mã 13
			e.preventDefault();
			save();
		}
	});

	_$modal.on('shown.bs.modal', function () {
		_$form.find('input[type=text]:first').focus();
	});
})(jQuery);

(function ($) {
	var _stockTransactionService = abp.services.app.stockTransaction,
		l = abp.localization.getSource('QLKho_NCKH'),
		_$modalimport = $('#ImportModal'),
		_selectedProductsId = null;
		//_$modalexport = $('#ExportModal'),
		//_$modaltransfer = $('#TransferModal'),
		_$form = _$modalimport.find('form'),
		_$table = $('#StockTransactionsTable');

	//warehouse
	$('#CreateWarehouseImportBtn').on('click', function () {
		_addWarehouseCreateModal.open({}, function (result) {
			if (result) {
				$('#WarehouseImportDisplay').val(result.warehouseName.trim());
				$('#WarehouseIdImportCreate').val(result.warehouseId);
			}
		});
	});

	var _addWarehouseCreateModal = new app.ModalManager({
		viewUrl: abp.appPath + 'Warehouses/AddWarehoses',
		scriptUrl: abp.appPath + 'view-resources/Views/Warehouses/_AddWarehousesModal.js',
		modalClass: 'AddWarehousesModal',
	});
	//suplier
	$('#AddSupplierImportBtn').on('click', function () {
		_addSupplierCreateModal.open({}, function (result) {
			if (result) {
				$('#SupplierImportDisplay').val(result.supplierName.trim());
				$('#SupplierIdImportCreate').val(result.supplierId);
			}
		});
	});

	var _addSupplierCreateModal = new app.ModalManager({
		viewUrl: abp.appPath + 'Suppliers/AddSupplier',
		scriptUrl: abp.appPath + 'view-resources/Views/Suppliers/_AddSupplierModal.js',
		modalClass: 'AddSupplierModal',
	});
	// products
	$('#AddProductsImportBtn').click(function () {
		_addProductCreateModal.open({ _selectedProductsId }, function (result) {
			// Nếu không chọn ai => reset input và danh sách user đã chọn
			if (!result || result.length === 0) {
				_selectedProductsId = [];
				$('#ProductsImportDisplay').val('');
				return;
			}

			// Nếu có chọn => cập nhật danh sách
			_selectedProductsId = result.map(u => u.productId);
			// Show tên user đã chọn ra UI
			const selectedNames = result.map(u => u.productName).join(', ');
			$('#ProductsImportDisplay').val(selectedNames);
		});
	});
	var _addProductCreateModal = new app.ModalManager({
		viewUrl: abp.appPath + 'Products/AddProduct',
		scriptUrl: abp.appPath + 'view-resources/Views/Products/_AddProductsModal.js',
		modalClass: 'AddProductsModal',
	});

	//var _$stockTransactionTable = _$table.DataTable({
	//	paging: true,
	//	serverSide: true,
	//	listAction: {
	//		ajaxFunction: _storageLocationService.getAll,
	//		inputFilter: function () {
	//			return $('#StorageLocationServiceSearchForm').serializeFormToObject(true);
	//		}
	//	},
	//	buttons: [
	//		{
	//			name: 'refresh',
	//			text: '<i class="fas fa-redo-alt"></i>',
	//			action: () => _$rolesTable.draw(false)
	//		}
	//	],
	//	responsive: {
	//		details: {
	//			type: 'column'
	//		}
	//	},
	//	columnDefs: [
	//		{ targets: 0, data: 'code', className: 'dt-center', orderable: false },
	//		{ targets: 1, data: 'warehouseName', className: 'dt-center', orderable: false },
	//		{ targets: 2, data: 'capacity', className: 'dt-center', orderable: false },
	//		{ targets: 3, data: 'currentVolume', className: 'dt-center', orderable: false },
	//		{
	//			targets: 4,
	//			data: 'isAvailable',
	//			className: 'dt-center',
	//			orderable: false,
	//			render: function (data) {
	//				return data ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-danger">Inactive</span>';
	//			}
	//		},
	//		{
	//			targets: 5,
	//			data: null,
	//			sortable: false,
	//			autoWidth: false,
	//			defaultContent: '',
	//			render: (data, type, row, meta) => {
	//				return [
	//					`   <button type="button" class="btn btn-sm bg-secondary edit-storageLocation" data-storageLocation-id="${row.id}" data-toggle="modal" data-target="#StorageLocationEditModal">`,
	//					`       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
	//					'   </button>',
	//					`   <button type="button" class="btn btn-sm bg-danger delete-storageLocation" data-storageLocation-id="${row.id}" data-storageLocation-name="${row.name}">`,
	//					`       <i class="fas fa-trash"></i> ${l('Delete')}`,
	//					'   </button>',
	//				].join('');
	//			}
	//		}
	//	]
	//});
	_$form.find('.save-button').on('click', (e) => {
		e.preventDefault();

		if (!_$form.valid()) {
			return;
		}

		var storageLocation = _$form.serializeFormToObject();
		console.log(storageLocation);
		abp.ui.setBusy(_$modalimport);
		_storageLocationService
			.create(storageLocation)
			.done(function () {
				_$modalimport.modal('hide');
				_$form[0].reset();
				abp.notify.info(l('SavedSuccessfully'));
				_$storageLocationTable.ajax.reload();
			})
			.always(function () {
				abp.ui.clearBusy(_$modalimport);
			});
	});

	$(document).on('click', '.delete-storageLocation', function () {
		var storageLocationId = $(this).attr("data-storageLocation-id");
		var storageLocationName = $(this).attr('data-storageLocation-name');

		deleteSupplier(storageLocationId, storageLocationName);
	});
	function deleteSupplier(storageLocationId, storageLocationName) {
		abp.message.confirm(
			abp.utils.formatString(
				l('AreYouSureWantToDelete'),
				storageLocationName),
			null,
			(isConfirmed) => {
				if (isConfirmed) {
					_storageLocationService.delete(
						storageLocationId
					).done(() => {
						abp.notify.info(l('SuccessfullyDeleted'));
						_$storageLocationTable.ajax.reload();
					});
				}
			}
		);
	}
	abp.event.on('storageLocation.edited', (data) => {
		_$storageLocationTable.ajax.reload();
	});

	$('.btn-search').on('click', (e) => {
		_$storageLocationTable.ajax.reload();
	});

	$(document).on('click', '.edit-storageLocation', function (e) {
		var storageLocationId = $(this).attr("data-storageLocation-id");

		e.preventDefault();
		abp.ajax({
			url: abp.appPath + 'StorageLocations/Edit?StorageLocationId=' + storageLocationId,
			type: 'POST',
			dataType: 'html',
			success: function (content) {
				$('#StorageLocationEditModal div.modal-content').html(content);
			},
			error: function (e) {
			}
		})
	});

})(jQuery);

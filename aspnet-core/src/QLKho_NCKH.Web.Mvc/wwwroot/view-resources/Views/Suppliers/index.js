(function ($) {
  var _supplierService = abp.services.app.supplier,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#SupplierCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#SuppliersTable');

  var _$supplierTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    listAction: {
      ajaxFunction: _supplierService.getAll,
      inputFilter: function () {
        return $('#SupplierSearchForm').serializeFormToObject(true);
      }
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: () => _$rolesTable.draw(false)
      }
    ],
    responsive: {
      details: {
        type: 'column'
      }
    },
    columnDefs: [
      { targets: 0, data: 'code', className: 'dt-center', orderable: false },
      { targets: 1, data: 'name', className: 'dt-center', orderable: false },
      { targets: 2, data: 'address', className: 'dt-center', orderable: false },
      { targets: 3, data: 'phoneNumber', className: 'dt-center', orderable: false },
      { targets: 4, data: 'email', className: 'dt-center', orderable: false },
      { targets: 5, data: 'taxCode', className: 'dt-center', orderable: false },
      {
        targets: 6,
        data: 'isActive',
        className: 'dt-center',
        orderable: false,
        render: function (data) {
          return data ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-danger">Inactive</span>';
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
            `   <button type="button" class="btn btn-sm bg-secondary edit-supplier" data-supplier-id="${row.id}" data-toggle="modal" data-target="#SupplierEditModal">`,
            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
            '   </button>',
            `   <button type="button" class="btn btn-sm bg-danger delete-supplier" data-supplier-id="${row.id}" data-supplier-name="${row.name}">`,
            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
            '   </button>',
          ].join('');
        }
      }
    ]
  });
  _$form.find('.save-button').on('click', (e) => {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    var supplier = _$form.serializeFormToObject();

    abp.ui.setBusy(_$modal);
    _supplierService
      .create(supplier)
      .done(function () {
        _$modal.modal('hide');
        _$form[0].reset();
        abp.notify.info(l('SavedSuccessfully'));
        _$supplierTable.ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modal);
      });
  });

  $(document).on('click', '.delete-supplier', function () {
    var supplierId = $(this).attr("data-supplier-id");
    var supplierName = $(this).attr('data-supplier-name');

    deleteSupplier(supplierId, supplierName);
  });
  function deleteSupplier(supplierId, supplierName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('AreYouSureWantToDelete'),
        supplierName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _supplierService.delete(
          supplierId
          ).done(() => {
            abp.notify.info(l('SuccessfullyDeleted'));
            _$supplierTable.ajax.reload();
          });
        }
      }
    );
  }
  abp.event.on('supplier.edited', (data) => {
    _$supplierTable.ajax.reload();
  });

  $('.btn-search').on('click', (e) => {
    _$supplierTable.ajax.reload();
  });

  $(document).on('click', '.edit-supplier', function (e) {
    var supplierId = $(this).attr("data-supplier-id");

    e.preventDefault();
    abp.ajax({
      url: abp.appPath + 'Suppliers/Edit?SupplierId=' + supplierId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#SupplierEditModal div.modal-content').html(content);
      },
      error: function (e) {
      }
    })
  });
	
})(jQuery);

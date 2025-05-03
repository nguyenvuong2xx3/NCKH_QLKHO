(function ($) {
  var _warehouseService = abp.services.app.warehouse,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#WarehouseCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#WarehousesTable');

  var _$warehousesTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    listAction: {
      ajaxFunction: _warehouseService.getAll,
      inputFilter: function () {
        return $('#WarehouseSearchForm').serializeFormToObject(true);
      }
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: () => _$warehousesTable.draw(false)
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
      { targets: 2, data: 'location', className: 'dt-center', orderable: false },
      { targets: 3, data: 'totalArea', className: 'dt-center', orderable: false },
      {
        targets: 4,
        data: 'isActive',
        className: 'dt-center',
        orderable: false,
        render: function (data) {
          return data ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-danger">Inactive</span>';
        }
      },
      {
        targets: 5,
        data: null,
        sortable: false,
        autoWidth: false,
        defaultContent: '',
        render: (data, type, row, meta) => {
          return [
            `   <button type="button" class="btn btn-sm bg-secondary edit-warehouse" data-warehouse-id="${row.id}" data-toggle="modal" data-target="#WarehouseEditModal">`,
            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
            '   </button>',
            `   <button type="button" class="btn btn-sm bg-danger delete-warehouse" data-warehouse-id="${row.id}" data-warehouse-name="${row.name}">`,
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

    var warehouse = _$form.serializeFormToObject();

    abp.ui.setBusy(_$modal);
    _warehouseService
      .create(warehouse)
      .done(function () {
        _$modal.modal('hide');
        _$form[0].reset();
        abp.notify.info(l('SavedSuccessfully'));
        _$warehousesTable.ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modal);
      });
  });

  $(document).on('click', '.delete-warehouse', function () {
    var warehouseId = $(this).attr("data-warehouse-id");
    var warehouseName = $(this).attr('data-warehouse-name');

    deleteWarehouse(warehouseId, warehouseName);
  });

  function deleteWarehouse(warehouseId, warehouseName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('AreYouSureWantToDelete'),
        warehouseName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _warehouseService.delete(
            warehouseId
          ).done(() => {
            abp.notify.info(l('SuccessfullyDeleted'));
            _$warehousesTable.ajax.reload();
          });
        }
      }
    );
  }

  abp.event.on('warehouse.edited', (data) => {
    _$warehousesTable.ajax.reload();
  });

  $('.btn-search').on('click', (e) => {
    _$warehousesTable.ajax.reload();
  });

  $(document).on('click', '.edit-warehouse', function (e) {
    var warehouseId = $(this).attr("data-warehouse-id");

    e.preventDefault();
    abp.ajax({
      url: abp.appPath + 'Warehouses/Edit?WarehouseId=' + warehouseId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#WarehouseEditModal div.modal-content').html(content);
      },
      error: function (e) {
      }
    })
  });
})(jQuery);
(function () {
  app.modals.AddWarehousesModal = function () {
    var _modalManager;
    var _warehouseService = abp.services.app.warehouse;
    var _$table;
    var _$filterInput;
    var dataTable;


		console.log('AddWarehousesModal.js loaded');
    //// Hàm lấy giá trị filter
    //var getFilter = function () {
    //  let dataFilter = {};
    //  dataFilter.filter = $('#WarehouseFilter').val();
    //  return dataFilter;
    //}

    //// Làm mới bảng
    //function refreshTable() {
    //  dataTable.ajax.reload();
    //}

    //// Cập nhật trạng thái nút Save
    //function updateSaveButtonState() {
    //  if (!_$table || typeof _$table.find !== 'function') {
    //    return;
    //  }

    //  var selectedWarehouse = _$table.find('input[name="selectedWarehouse"]:checked');
    //  var $saveButton = _modalManager.getModal().find('.save-button');

    //  if (selectedWarehouse.length > 0) {
    //    $saveButton.removeAttr('disabled');
    //  } else {
    //    $saveButton.attr('disabled', 'disabled');
    //  }
    //}

    this.init = function (modalManager) {
      _modalManager = modalManager;

      // Lấy các phần tử từ modal
      _$table = $('#addWarehouseModalTable');
      _$filterInput = _modalManager.getModal().find('#WarehouseFilter');

      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        info: true,
        listAction: {
          ajaxFunction: _warehouseService.getAll,
          inputFilter: getFilter
        },
        columnDefs: [
          {
            targets: 0,
            data: null,
            orderable: false,
            defaultContent: '',
            render: function (data) {
              return (
                '<label for="radio_' +
                data.id +
                '" class="radio form-check">' +
                '<input type="radio" name="selectedWarehouse" id="radio_' +
                data.id +
                '" class="form-check-input" />&nbsp;' +
                '<span class="form-check-label"></span>' +
                '</label>'
              );
            },
          },
          {
            targets: 1,
            data: "code",
            orderable: false,
            render: function (data) {
              return data;
            }
          },
          {
            targets: 2,
            data: "name",
            orderable: false,
            render: function (data) {
              return data;
            }
          },
          {
            targets: 3,
            data: "location",
            orderable: false,
            render: function (data) {
              return data;
            }
          },
          {
            targets: 4,
            data: "id",
            visible: false
          }
        ]
      });

      // Sự kiện change cho radio button
      _$table.on('change', 'input[name="selectedWarehouse"]', function () {
        updateSaveButtonState();
      });

      dataTable.on('draw', function () {
        updateSaveButtonState();
      });

      updateSaveButtonState();

      // Sự kiện tìm kiếm
      _modalManager.getModal().find('.add-warehouse-filter-button').click(function () {
        refreshTable();
      });

      _$filterInput.keydown(function (e) {
        if (e.which === 13) {
          e.preventDefault();
          refreshTable();
        }
      });

      // Debounce tìm kiếm
      let timeOut = null;
      $("#WarehouseFilter").on("keyup", function () {
        clearTimeout(timeOut);
        timeOut = setTimeout(function () {
          refreshTable();
        }, 300);
      });
    };

    this.save = function () {
      var selectedRow = dataTable.row($('input[name="selectedWarehouse"]:checked').closest('tr')).data();

      _modalManager.setResult({
        warehouseId: selectedRow.id,
        warehouseCode: selectedRow.code,
        warehouseName: selectedRow.name,
        warehouseLocation: selectedRow.location
      });

      _modalManager.close();
    };
  };
})();
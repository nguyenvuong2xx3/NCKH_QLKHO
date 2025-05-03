(function () {
  app.modals.AddSupplierModal = function () {
    var _modalManager;
    var _supplierService = abp.services.app.supplier;
    var _$table;
    var _$filterInput;
    var dataTable;

    var getFilter = function () {
      return {
        filter: _$filterInput.val()
      };
    };

    // Làm mới bảng
    function refreshTable() {
      dataTable.ajax.reload();
    }

    // Cập nhật trạng thái nút Save
    function updateSaveButtonState() {
      if (!_$table || typeof _$table.find !== 'function') {
        return;
      }

      var selectedSupplier = _$table.find('input[name="selectedSupplier"]:checked');
      var $saveButton = _modalManager.getModal().find('.save-button');

      if (selectedSupplier.length > 0) {
        $saveButton.removeAttr('disabled');
      } else {
        $saveButton.attr('disabled', 'disabled');
      }
    }

    this.init = function (modalManager) {
      _modalManager = modalManager;

      // Lấy các phần tử từ modal
      _$table = _modalManager.getModal().find('#addSupplierModalTable');
      _$filterInput = _modalManager.getModal().find('#SupplierFilter');

      // Khởi tạo DataTable
      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
          ajaxFunction: _supplierService.getAll,
          inputFilter: getFilter
        },
        columns: [
          {
            data: null,
            orderable: false,
            defaultContent: '',
            render: function (data) {
              return (
                '<label for="radio_' +
                data.id +
                '" class="radio form-check">' +
                '<input type="radio" name="selectedSupplier" id="radio_' +
                data.id +
                '" class="form-check-input" />&nbsp;' +
                '<span class="form-check-label"></span>' +
                '</label>'
              );
            }
          },
          { data: "name", orderable: false },
          { data: "address", orderable: false },
          { data: "phoneNumber", orderable: false },
          {
            data: "isActive",
            orderable: false,
            render: function (data) {
              return data ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-danger">Inactive</span>';
            }
          }
        ],
        buttons: [
          {
            name: 'refresh',
            text: '<i class="fas fa-redo-alt"></i>',
            action: function () {
              dataTable.draw(false);
            }
          }
        ],
        responsive: {
          details: {
            type: 'column'
          }
        }
      });

      // Sự kiện change cho radio button
      _$table.on('change', 'input[name="selectedSupplier"]', function () {
        updateSaveButtonState();
      });

      dataTable.on('draw', function () {
        updateSaveButtonState();
      });

      updateSaveButtonState();

      // Sự kiện tìm kiếm
      _modalManager.getModal().find('.add-supplier-filter-button').click(function () {
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
      _$filterInput.on("keyup", function () {
        clearTimeout(timeOut);
        timeOut = setTimeout(function () {
          refreshTable();
        }, 300);
      });
    };

    this.save = function () {
      var selectedRow = dataTable.row($('input[name="selectedSupplier"]:checked').closest('tr')).data();

      if (!selectedRow) {
        abp.message.warn(abp.localization.localize('PleaseSelectASupplier', 'QLKho_NCKH'));
        return;
      }

      _modalManager.setResult({
        supplierId: selectedRow.id,
        supplierName: selectedRow.name
      });

      _modalManager.close();
    };
  };
})();

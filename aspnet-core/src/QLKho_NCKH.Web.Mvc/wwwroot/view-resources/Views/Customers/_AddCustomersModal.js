(function () {
  app.modals.AddCustomersModal = function () {
    var _modalManager;
    var _customerService = abp.services.app.customer;
    var _$table;
    var _$filterInput;
    var dataTable;

    var getFilter = function () {
      return {
        filter: _$filterInput.val()
      };
    }

    function refreshTable() {
      dataTable.ajax.reload();
    }

    function updateSaveButtonState() {
      if (!_$table || typeof _$table.find !== 'function') {
        return;
      }

      var selectedCustomer = _$table.find('input[name="selectedCustomer"]:checked');
      var $saveButton = _modalManager.getModal().find('.save-button');

      if (selectedCustomer.length > 0) {
        $saveButton.removeAttr('disabled');
      } else {
        $saveButton.attr('disabled', 'disabled');
      }
    }

    this.init = function (modalManager) {
      _modalManager = modalManager;

      _$table = _modalManager.getModal().find('#addCustomerModalTable');
      _$filterInput = _modalManager.getModal().find('#CustomerFilter');

      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
          ajaxFunction: _customerService.getAllCustomers,
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
                '<input type="radio" name="selectedCustomer" id="radio_' +
                data.id +
                '" class="form-check-input" />&nbsp;' +
                '<span class="form-check-label"></span>' +
                '</label>'
              );
            }
          },
          { data: "code", orderable: false },
          { data: "name", orderable: false },
          { data: "phoneNumber", orderable: false },
          { data: "email", orderable: false },
          { data: "id", visible: false }
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

      _$table.on('change', 'input[name="selectedCustomer"]', function () {
        updateSaveButtonState();
      });

      dataTable.on('draw', function () {
        updateSaveButtonState();
      });

      updateSaveButtonState();

      _modalManager.getModal().find('.add-customer-filter-button').click(function () {
        refreshTable();
      });

      _$filterInput.keydown(function (e) {
        if (e.which === 13) {
          e.preventDefault();
          refreshTable();
        }
      });

      let timeOut = null;
      _$filterInput.on("keyup", function () {
        clearTimeout(timeOut);
        timeOut = setTimeout(function () {
          refreshTable();
        }, 300);
      });
    };

    this.save = function () {
      var selectedRow = dataTable.row($('input[name="selectedCustomer"]:checked').closest('tr')).data();

      _modalManager.setResult({
        customerId: selectedRow.id,
        customerCode: selectedRow.code,
        customerName: selectedRow.name,
        customerPhone: selectedRow.phoneNumber,
        customerEmail: selectedRow.email
      });

      _modalManager.close();
    };
  };
})();
(function () {
  app.modals.AddInventoryItemModal = function () {
    var _modalManager;
    var _inventoryItemService = abp.services.app.inventoryItem;
    var _$table;
    var _$filterInput;
    var dataTable;
    var selectedItems = [];
    var initialSelectedIds = [];

    function getFilter() {
      return {
        filter: _$filterInput.val(),
        warehouseId: _modalManager.getArgs().warehouseId
      };
    }

    function refreshTable() {
      dataTable.ajax.reload();
    }

    function updateSaveButtonState() {
      var $saveButton = _modalManager.getModal().find('.save-button');
      $saveButton.prop('disabled', selectedItems.length === 0);
    }

    this.init = function (modalManager) {
      _modalManager = modalManager;
      const args = _modalManager.getArgs();
      console.log("Modal args:", args);

      initialSelectedIds = Array.isArray(args._selectedItems) ?
        args._selectedItems.map(item => item.productId) : [];

      _$table = _modalManager.getModal().find('#addInventoryItemModalTable');
      _$filterInput = _modalManager.getModal().find('#InventoryItemFilter');

      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        info: true,
        destroy: true,
        listAction: {
          ajaxFunction: _inventoryItemService.getInventoryItems,
          inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columns: [
          {
            data: null,
            orderable: false,
            render: function (data) {
              const isChecked = selectedItems.some(item => item.productId === data.productId);
              return `
                <label class="checkbox form-check">
                    <input type="checkbox" 
                           class="form-check-input inventory-item-checkbox" 
                           data-product-id="${data.productId}"
                           ${isChecked ? 'checked' : ''}>
                    <span class="form-check-label"></span>
                </label>`;
            }
          },
          {
            data: 'productCode',
            title: 'Mã sản phẩm'
          },
          {
            data: 'productBarcode',
            title: 'Mã vạch'
          },
          {
            data: 'productName',
            title: 'Tên sản phẩm'
          },
          {
            data: 'quantity',
            title: 'Số lượng tồn',
            className: 'text-right'
          },
          {
            data: 'productId',
            visible: false
          }
        ],
        createdRow: function (row, data) {
          $(row).attr('data-product-id', data.productId);

          if (initialSelectedIds.includes(data.productId)) {
            selectedItems.push({
              productId: data.productId,
              productCode: data.productCode,
              productBarCode: data.productBarCode,
              productName: data.productName,
              quantity: data.quantity
            });
            $(row).find('.inventory-item-checkbox').prop('checked', true);
          }
        },
        drawCallback: function () {
          $('.inventory-item-checkbox').each(function () {
            const productId = $(this).data('product-id');
            $(this).prop('checked',
              selectedItems.some(item => item.productId === productId));
          });
          updateSaveButtonState();
        }
      });

      _$table.on('change', '.inventory-item-checkbox', function () {
        const productId = $(this).data('product-id');
        const row = dataTable.row($(this).closest('tr'));
        const itemData = row.data();

        if ($(this).is(':checked')) {
          selectedItems.push({
            productId: productId,
            productCode: itemData.productCode,
            productBarCode: itemData.productBarCode,
            productName: itemData.productName,
            quantity: itemData.quantity
          });
        } else {
          selectedItems = selectedItems.filter(item => item.productId !== productId);
        }
        updateSaveButtonState();
      });

      _$filterInput.on('keyup', function () {
        refreshTable();
      });

      _modalManager.getModal().find('.add-inventory-item-filter-button').click(function () {
        refreshTable();
      });
    };

    this.save = function () {
      $('.inventory-item-checkbox').each(function () {
        const productId = $(this).data('product-id');
        const row = dataTable.row($(this).closest('tr'));
        const itemData = row.data();

        if ($(this).is(':checked')) {
          if (!selectedItems.some(item => item.productId === productId)) {
            selectedItems.push({
              productId: productId,
              productCode: itemData.productCode,
              productBarCode: itemData.productBarcode,
              productName: itemData.productName,
              quantity: itemData.quantity
            });
          }
        }
      });
      _modalManager.setResult(selectedItems);
      _modalManager.close();
    };
  };
})();
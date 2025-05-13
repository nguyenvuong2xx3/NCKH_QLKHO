(function () {
  var _inventoryItemService = abp.services.app.inventoryItem;
  var _$inventoryItemTable = $('#InventoryItemsTable');
  var _$filterInput = $('#InventoryItemTableFilter');
  var dataTable;

  function getFilter() {
    return {
      filter: _$filterInput.val(),
      warehouseId: $('#WarehouseIdFilter').val(),
      categoryId: $('#CategoryIdFilter').val()
    };
  }

  function refreshTable() {
    dataTable.ajax.reload();
  }

  function initTable() {
    dataTable = _$inventoryItemTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      info: true,
      destroy: true,
      listAction: {
        ajaxFunction: _inventoryItemService.getAllInventoryItems,
        inputFilter: getFilter
      },
      order: [[1, 'asc']],
      columns: [
        {
          data: 'productImage',
          title: '',
          orderable: false,
          render: function (data) {
            return data
              ? `<img src="${data}" class="product-thumbnail" / style="max-width: 60px;">`
              : '<div class="no-image-placeholder"></div>';
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
          data: 'warehouseName',
          title: 'Kho'
        },
        {
          data: 'storageLocationCode',
          title: 'Vị trí'
        },
        {
          data: 'quantity',
          title: 'Số lượng tồn',
        },
        {
          data: 'unitPrice',
          title: 'Đơn giá',
          className: 'text-right',
          render: function (data) {
            return data ? data.toLocaleString() + ' đ' : '';
          }
        },
        {
          data: null,
          title: 'Hành động',
          orderable: false,
          render: function (data) {
            return `
                            <button class="btn btn-sm btn-primary edit-item" data-product-id="${data.productId}" data-location-id="${data.storageLocationId}">
                                <i class="fa fa-edit"></i> Sửa
                            </button>
                            <button class="btn btn-sm btn-danger delete-item" data-produtc-name="${data.productName}"  data-product-id="${data.productId}" data-location-id="${data.storageLocationId}">
                                <i class="fa fa-trash"></i> Xóa
                            </button>
            `;
          }
        }
      ],
      drawCallback: function () {
        // Initialize tooltips
        $('[data-toggle="tooltip"]').tooltip();

        // Handle edit button click
        $('.edit-item').click(function () {
          var productId = $(this).data('product-id');
          var locationId = $(this).data('location-id');
          // Open edit modal
          // You'll need to implement this based on your modal system
        });
        // Edit button handler (update your existing code)
        //$('.edit-item').click(function () {
        //  var productId = $(this).data('product-id');
        //  var locationId = $(this).data('location-id');

        //  // Open edit modal with current data
        //  var row = $(this).closest('tr');
        //  var rowData = dataTable.row(row).data();

        //  var modal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Inventory/EditInventoryItemModal',
        //    scriptUrl: abp.appPath + 'view-resources/Areas/Inventory/Views/Inventory/_EditInventoryItemModal.js',
        //    modalClass: 'EditInventoryItemModal'
        //  });

        //  modal.open({
        //    productId: productId,
        //    storageLocationId: locationId,
        //    currentQuantity: rowData.quantity,
        //    currentReservedQuantity: rowData.reservedQuantity,
        //    currentUnitPrice: rowData.unitPrice
        //    // Pass any other data you need
        //  }, function (result) {
        //    if (result) {
        //      abp.notify.success('Cập nhật tồn kho thành công!');
        //      refreshTable();
        //    }
        //  });
        //});
        // In your index page JavaScript file
        $('.edit-item').click(function () {
          var productId = $(this).data('product-id');
          var locationId = $(this).data('location-id');

          console.log("Opening edit modal for", productId, locationId); // Debug log

          var modal = new app.ModalManager({
            viewUrl: abp.appPath + 'InventoryItems/EditInventoryItemModal',
            scriptUrl: abp.appPath + 'view-resources/Views/InventoryItems/_EditModal.js',
            modalClass: 'EditInventoryItemModal'
          });

          modal.open({
            productId: productId,
            storageLocationId: locationId
          }
            ,
            function (result) {
            if (result) {
              abp.notify.success('Cập nhật tồn kho thành công!');
              refreshTable();
            }
          });
        });

        // Delete button handler (already implemented in previous code)
        // No changes needed as it already uses the correct service
        // Handle delete button click
        $('.delete-item').click(function () {
          var productId = $(this).data('product-id');
          var locationId = $(this).data('location-id');
          var productName = $(this).data('produtc-name');


          abp.message.confirm(
            `Bạn có chắc chắn muốn xóa tồn kho của sản phẩm '${productName}'?`,
            'Xác nhận xóa',
            function (confirmed) {
              if (confirmed) {
                abp.ui.setBusy();
                _inventoryItemService.deleteInventoryItem({
                  productId: productId,
                  storageLocationId: locationId
                }).done(function () {
                  abp.notify.success('Xóa tồn kho thành công!');
                  refreshTable();
                }).always(function () {
                  abp.ui.clearBusy();
                });
              }
            }
          );
        });
      }
    });
  }

  $(document).ready(function () {
    initTable();

    // Initialize filter controls
    _$filterInput.on('keyup', function (e) {
      if (e.keyCode === 13) { // Enter key
        refreshTable();
      }
    });

    $('#SearchButton').click(function () {
      refreshTable();
    });

    $('#WarehouseIdFilter, #CategoryIdFilter').change(function () {
      refreshTable();
    });

    // Initialize create new button
    $('#CreateNewInventoryItemButton').click(function () {
      // Open create modal
      // You'll need to implement this based on your modal system
    });
  });
})();
(function () {
  app.modals.AddProductsModal = function () {
    var _modalManager;
    var _productService = abp.services.app.product;
    var _$table;
    var _$filterInput;
    var dataTable;
    var selectedProducts = []; // Lưu trữ tất cả product đã chọn
    var initialSelectedIds = []; // ID đã được chọn ban đầu

    // Lấy giá trị filter từ input
    function getFilter() {
      return {
        filter: _$filterInput.val(),
        supplierId: _modalManager.getArgs().supplierId, // Lấy supplierId từ modal args
				warehouseId: _modalManager.getArgs().warehouseId // Lấy warehouseId từ modal args
      };
    }
    // Làm mới bảng

    //var get = getFilter()
    //console.log(get)
    function refreshTable() {
      dataTable.ajax.reload();
    }
  //  var fil = getFilter();
		//console.log(fil)
    // Cập nhật trạng thái nút Save
    function updateSaveButtonState() {
      var $saveButton = _modalManager.getModal().find('.save-button');
      $saveButton.prop('disabled', selectedProducts.length === 0);
    }

    this.init = function (modalManager) {
      _modalManager = modalManager;
      const args = _modalManager.getArgs();
			console.log(args);
      // Khởi tạo danh sách ID đã chọn ban đầu
      initialSelectedIds = Array.isArray(args) ? [...args] :
        (args._selectedProductIds ? [...args._selectedProductIds] : []);

      // Lấy các phần tử từ modal
      _$table = _modalManager.getModal().find('#addProductModalTable');
      _$filterInput = _modalManager.getModal().find('#ProductFilter');

      // Khởi tạo DataTable
      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        processing: true,
        info: true,
        destroy: true,
        listAction: {
          ajaxFunction: _productService.getAllProducts,
          inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columns: [
          {
            data: null,
            orderable: false,
            render: function (data) {
              const isChecked = selectedProducts.some(p => p.productId === data.id);
              return `
                                <label class="checkbox form-check">
                                    <input type="checkbox" 
                                           class="form-check-input product-checkbox" 
                                           data-product-id="${data.id}"
                                           ${isChecked ? 'checked' : ''}>
                                    <span class="form-check-label"></span>
                                </label>`;
            }
          },
          { data: 'code', title: 'Code' },
          { data: 'name', title: 'Name' },
          { data: 'unit', title: 'Unit' },
          { data: 'barcode', title: 'Barcode' },
          { data: 'id', visible: false }
        ],
        createdRow: function (row, data) {
          $(row).attr('data-id', data.id);

          // Nếu là lần đầu mở modal
          if (initialSelectedIds.includes(data.id) &&
            !selectedProducts.some(p => p.productId === data.id)) {
            selectedProducts.push({
              productId: data.id,
              productCode: data.code,
              productName: data.name,
              unit: data.unit,
              barcode: data.barcode
            });
            $(row).find('.product-checkbox').prop('checked', true);
          }
        },
        drawCallback: function () {
          $('.product-checkbox').each(function () {
            const productId = $(this).data('product-id');
            $(this).prop('checked',
              selectedProducts.some(p => p.productId === productId));
          });
          updateSaveButtonState();
        }
      });

      // Xử lý click checkbox
      _$table.on('change', '.product-checkbox', function () {
        const productId = $(this).data('product-id');
        const row = dataTable.row($(this).closest('tr'));
        const productData = row.data();

        if ($(this).is(':checked')) {
          if (!selectedProducts.some(p => p.productId === productId)) {
            selectedProducts.push({
              productId: productId,
              productCode: productData.code,
              productName: productData.name,
              unit: productData.unit,
              barcode: productData.barcode
            });
          }
        } else {
          selectedProducts = selectedProducts.filter(p => p.productId !== productId);
        }
        updateSaveButtonState();
      });

      // Sự kiện ô tìm kiếm
      _$filterInput.on('keyup', function () {
        dataTable.search($(this).val()).draw();
      });

      _modalManager.getModal().find('.add-product-filter-button').click(function () {
        refreshTable();
      });
    };

    this.save = function () {
      // Đồng bộ các checkbox đang hiển thị vào selectedProducts
      $('.product-checkbox').each(function () {
        const productId = $(this).data('product-id');
        const row = dataTable.row($(this).closest('tr'));
        const productData = row.data();

        if ($(this).is(':checked')) {
          if (!selectedProducts.some(p => p.productId === productId)) {
            selectedProducts.push({
              productId: productId,
              productCode: productData.code,
              productName: productData.name,
              unit: productData.unit,
              barcode: productData.barcode
            });
          }
        } else {
          selectedProducts = selectedProducts.filter(p => p.productId !== productId);
        }
      });

      _modalManager.setResult(selectedProducts);
      _modalManager.close();
    };
  };
})();
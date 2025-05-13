(function () {
  app.modals.AddCategoryModal = function () {
    var _modalManager;
    var _categoryService = abp.services.app.category;
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

      var selectedCategory = _$table.find('input[name="selectedCategory"]:checked');
      var $saveButton = _modalManager.getModal().find('.save-button');

      if (selectedCategory.length > 0) {
        $saveButton.removeAttr('disabled');
      } else {
        $saveButton.attr('disabled', 'disabled');
      }
    }

    this.init = function (modalManager) {
      _modalManager = modalManager;

      // Lấy các phần tử từ modal
      _$table = _modalManager.getModal().find('#addCategoryModalTable');
      _$filterInput = _modalManager.getModal().find('#CategoryFilter');

      // Khởi tạo DataTable
      dataTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
          ajaxFunction: _categoryService.getAll,
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
                '<input type="radio" name="selectedCategory" id="radio_' +
                data.id +
                '" class="form-check-input" />&nbsp;' +
                '<span class="form-check-label"></span>' +
                '</label>'
              );
            }
          },
          { data: "name", orderable: false },
          { data: "description", orderable: false }
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
      _$table.on('change', 'input[name="selectedCategory"]', function () {
        updateSaveButtonState();
      });

      dataTable.on('draw', function () {
        updateSaveButtonState();
      });

      updateSaveButtonState();

      // Sự kiện tìm kiếm
      _modalManager.getModal().find('.add-category-filter-button').click(function () {
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
      var selectedRow = dataTable.row($('input[name="selectedCategory"]:checked').closest('tr')).data();

      if (!selectedRow) {
        abp.message.warn(abp.localization.localize('PleaseSelectACategory', 'QLKho_NCKH'));
        return;
      }

      _modalManager.setResult({
        categoryId: selectedRow.id,
        categoryName: selectedRow.name
      });

      _modalManager.close();
    };
  };
})();

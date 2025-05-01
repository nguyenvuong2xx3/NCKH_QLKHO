(function () {
  $(function () {
    var _productService = abp.services.app.product,  
      _$table = $('#productList');



    //var _createModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'Hrm/ShiftPeriods/CreateShiftPeriodModal',
    //  scriptUrl: abp.appPath + 'view-resources/Hrm/ShiftPeriods/_CreateShiftPeriodModal.js',
    //  modalClass: 'CreateShiftPeriodModal',
    //});

    //$('#CreateNewButtonShiftPeriods').click(function () {
    //  _createModal.open();
    //});

    //var _editModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'Hrm/ShiftPeriods/EditShiftPeriodModal',
    //  scriptUrl: abp.appPath + 'view-resources/Hrm/ShiftPeriods/_EditShiftPeriodModal.js',
    //  modalClass: 'EditShiftPeriodModal',
    //});
    //// detail
    //var _detailModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'Hrm/ShiftPeriods/DetailShiftPeriodModal',
    //  scriptUrl: abp.appPath + 'view-resources/Hrm/ShiftPeriods/_DetailShiftPeriodModal.js',
    //  modalClass: 'DetailShiftPeriodModal',
    //  modalType: 'modal-xl'
    //});

    ////export
    //var _exportShiftPeriodModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'Hrm/ShiftPeriods/ExportDataModal',
    //  scriptUrl: abp.appPath + 'view-resources/Hrm/ShiftPeriods/_ExportShiftperiodDataModal.js',
    //  modalClass: 'ExportShiftperiodDataModal',
    //  modalSize: 'modal-xl'
    //});

    //$('#ExportShiftPeriodData').click(function (e) {
    //  e.preventDefault();

    //  var filterData = getFilter(); // Lấy search từ view chính
    //  _exportShiftPeriodModal.open({
    //    filter: filterData
    //  });
    //});

    ////import
    //var _importShiftPeriodModal = new app.ModalManager({
    //  viewUrl: abp.appPath + 'Hrm/ShiftPeriods/ImportDataModal',
    //  scriptUrl: abp.appPath + 'view-resources/Hrm/ShiftPeriods/_ImportShiftperiodDataModal.js',
    //  modalClass: 'ImportShiftperiodDataModal',
    //  modalSize: 'modal-xl'
    //});

    //$('#ImportShiftPeriodData').click(function (e) {
    //  e.preventDefault();
    //  _importShiftPeriodModal.open();
    //});

    ///
    //function deleteShiftPeriod(data) {
    //  abp.message.confirm(
    //    app.localize('Delete'),
    //    app.localize('AreYouSure'),
    //    function (isConfirmed) {
    //      if (isConfirmed) {
    //        _shiftPeriodService.deleteShiftPeriod(
    //          data.record.id
    //        ).done(function () {
    //          getShiftPeriods(true);
    //          abp.notify.success(app.localize('SuccessfullyDeleted'));
    //        });
    //      }
    //    },
    //    { confirmButtonText: 'Xóa', confirmButtonColor: '#dc3545', cancelButtonText: 'Hủy' }
    //  );
    //}

    //let dataFilter = {};

    //if (_selectedCalcDateRange?.startDate) {
    //  dataFilter.calcDateStart = _selectedCalcDateRange.startDate.format('YYYY-MM-DD');
    //}
    //if (_selectedCalcDateRange?.endDate) {
    //  dataFilter.calcDateEnd = _selectedCalcDateRange.endDate.format('YYYY-MM-DD');
    //}

    //if (_selectedUserIds?.length > 0) {
    //  dataFilter.userIds = _selectedUserIds;
    //}

    //return dataFilter;
    //function getFilter() {
    //  let dataFilter = {};
    //  dataFilter.filter = $('#SearchTerm').val()
    //  //return {
    //  //  search: $('#SearchTerm').val() // Lấy giá trị từ ô input tìm kiếm
    //  //};
    //  return dataFilter;

    //}



    var dataTable = _$table.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      info: true,
      listAction: {
        ajaxFunction: _productService.getAllProducts, // Gọi AppService
        inputFilter: function () {
          return getFilter(); // Hàm filter tuỳ bạn định nghĩa
        },
      },
      columnDefs: [
        {
          targets: 0,
          data: 'code',
          className: 'dt-center',
          orderable: false
        },
        {
          targets: 1,
          data: 'name',
          className: 'dt-left',
          orderable: false
        },
        {
          targets: 2,
          data: 'category.name',
          className: 'dt-center',
          orderable: false
        },
        {
          targets: 3,
          data: 'barcode',
          className: 'dt-center',
          orderable: false
        },
        {
          targets: 4,
          data: 'unit',
          className: 'dt-center',
          orderable: false
        },
        {
          targets: 5,
          data: 'weight',
          className: 'dt-right',
          orderable: false,
          render: $.fn.dataTable.render.number(',', '.', 2, '', ' kg')
        },
        {
          targets: 6,
          data: 'volume',
          className: 'dt-right',
          orderable: false,
          render: $.fn.dataTable.render.number(',', '.', 2, '', ' m³')
        },
        {
          targets: 7,
          data: 'supplier.name',
          className: 'dt-center',
          orderable: false
        },
        {
          targets: 8,
          data: 'isActive',
          className: 'dt-center',
          orderable: false,
          render: function (isActive) {
            return isActive ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-secondary">Inactive</span>';
          }
        },
        {
          targets: 9,
          data: null,
          className: 'dt-center',
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text:
              '<i class="fa fa-cog"></i> <span class="d-none d-md-inline-block">' +
              app.localize('Actions') +
              '</span>',
            items: [
              {
                text: app.localize('Edit'),
                action: function (data) {
                  _editModal.open({ id: data.record.id });
                },
              },
              {
                text: app.localize('Detail'),
                action: function (data) {
                  _detailModal.open({ id: data.record.id });
                },
              },
              {
                text: app.localize('Delete'),
                action: function (data) {
                  deleteProduct(data.record); // Viết hàm deleteProduct riêng
                },
              },
            ],
          },
        },
      ]
    });


    //$('#shiftPeriod').on('click', '.edit-action', function (e) {
    //  e.preventDefault();
    //  var id = $(this).data('id');
    //  _editModal.open({ id: id });
    //});

    //$('#shiftPeriod').on('click', '.delete-action', function (e) {
    //  e.preventDefault();
    //  var id = $(this).data('id');
    //  deleteShiftPeriod({ record: { id: id } });
    //});

    //function getShiftPeriods() {
    //  dataTable.ajax.reload();
    //}
    //abp.event.on('app.updateShiftPeriodModalSaved', function () {
    //  getShiftPeriods(true);
    //});

    //$('#Search').click(function (e) {
    //  e.preventDefault();
    //  getShiftPeriods();
    //});

    //$('#SearchTerm').on('keydown', function (e) {
    //  if (e.keyCode !== 13) {
    //    return;
    //  }
    //  e.preventDefault();
    //  getShiftPeriods();
    //});

    //abp.event.on('app.reloadDocTable', function () {
    //  getDocs();
    //});


    //StatusShow = {
    //  _inputcheckbox: null,
    //  init: function () {
    //    StatusShow._inputcheckbox = $('#PollVoteTable tbody input[name="checkshow"]'),
    //      StatusShow._inputcheckbox.change(function () {
    //        var inputchange = $(this);
    //        var data = {}
    //        data.id = inputchange.attr('data-id');
    //        if (inputchange.is(':checked')) {
    //          data.status = 1;
    //        }
    //        else {
    //          data.status = 0;
    //        }
    //        _pollVoteService.activeStatus(data).done(function () {
    //          abp.notify.success(app.localize('Cập nhật thành công'));
    //          getDocs(true);
    //        })
    //      })
    //  }
    //}


    //$('#ShowAdvancedFiltersSpan').click(function () {
    //  $('#ShowAdvancedFiltersSpan').hide();
    //  $('#HideAdvancedFiltersSpan').show();
    //  $('#AdvacedAuditFiltersArea').slideDown();
    //});

    //$('#HideAdvancedFiltersSpan').click(function () {
    //  $('#HideAdvancedFiltersSpan').hide();
    //  $('#ShowAdvancedFiltersSpan').show();
    //  $('#AdvacedAuditFiltersArea').slideUp();
    //});

    //function getDocs() {
    //  dataTable.ajax.reload();
    //}

    //jQuery(document).ready(function () {
    //  $("#SearchTerm").focus();
    //});

  });
})();

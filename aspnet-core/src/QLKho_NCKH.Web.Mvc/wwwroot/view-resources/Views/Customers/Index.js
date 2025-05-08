(function ($) {
  // Declare services and variables
  var _customerService = abp.services.app.customer,
    l = abp.localization.getSource('QLKho_NCKH'),
    _$modal = $('#CustomerCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#CustomersTable');

  // Initialize DataTable for customers
  var _$customerTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    listAction: {
      ajaxFunction: _customerService.getAllCustomers,
      inputFilter: function () {
        // return $('#CustomerSearchForm').serializeFormToObject(true);
      }
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: function () {
          _$customerTable.draw(false);
        }
      }
    ],
    responsive: {
      details: {
        type: 'column'
      }
    },
    columnDefs: [
      {
        targets: 0,
        data: 'code',
        sortable: false
      },
      {
        targets: 1,
        data: 'name',
        sortable: false
      },
      {
        targets: 2,
        data: 'address',
        sortable: false,
        render: function (data) {
          return data || '<span class="text-muted">N/A</span>';
        }
      },
      {
        targets: 3,
        data: 'phoneNumber',
        sortable: false,
        render: function (data) {
          return data || '<span class="text-muted">N/A</span>';
        }
      },
      {
        targets: 4,
        data: 'email',
        sortable: false,
        render: function (data) {
          return data || '<span class="text-muted">N/A</span>';
        }
      },
      {
        targets: 5,
        data: 'taxCode',
        sortable: false,
        render: function (data) {
          return data || '<span class="text-muted">N/A</span>';
        }
      },
      {
        targets: 6,
        data: 'isActive',
        sortable: false,
        className: 'text-center',
        render: function (data, type, row) {
          return `<div class="d-flex justify-content-center align-items-center">
                                <input type="checkbox" class="form-check-input is-active-toggle" 
                                       data-id="${row.id}" ${data ? 'checked' : ''}>
                            </div>`;
        }
      },
      {
        targets: 7,
        data: null,
        sortable: false,
        autoWidth: true,
        render: function (data, type, row) {
          return [
            `<button type="button" class="btn btn-sm bg-secondary edit-customer" data-customer-id="${row.id}" data-toggle="modal" data-target="#CustomerEditModal">`,
            `    <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
            ' </button>',
            `<button type="button" class="btn btn-sm bg-danger delete-customer" data-customer-id="${row.id}" data-customer-name="${row.name}">`,
            `    <i class="fas fa-trash"></i> ${l('Delete')}`,
            ' </button>'
          ].join('');
        }
      }
    ]
  });

  // Phương thức kiểm tra tùy chỉnh
  $.validator.addMethod("phoneNumber", function (value, element) {
    return this.optional(element) || /^[0-9\+\-\(\)\s]*$/.test(value);
  }, "Số điện thoại không hợp lệ.");

  // Khởi tạo validate cho biểu mẫu CustomerCreateForm
  $("form[name='customerCreateForm']").validate({
    rules: {
      Code: {
        required: true,
        maxlength: 50
      },
      Name: {
        required: true,
        maxlength: 100
      },
      Address: {
        required: true,
        maxlength: 200
      },
      PhoneNumber: {
        required: true,
        phoneNumber: true,
        maxlength: 20
      },
      Email: {
        email: true,
        maxlength: 100
      },
      TaxCode: {
        required: true,
        maxlength: 20
      },
      IsActive: {
        required: true
      }
    },
    messages: {
      Code: {
        required: "Mã khách hàng là bắt buộc.",
        maxlength: "Mã khách hàng không được vượt quá 50 ký tự."
      },
      Name: {
        required: "Tên khách hàng là bắt buộc.",
        maxlength: "Tên khách hàng không được vượt quá 100 ký tự."
      },
      Address: {
        required: "Địa chỉ là bắt buộc.",
        maxlength: "Địa chỉ không được vượt quá 200 ký tự."
      },
      PhoneNumber: {
        required: "Số điện thoại là bắt buộc.",
        phoneNumber: "Số điện thoại không đúng định dạng.",
        maxlength: "Số điện thoại không được vượt quá 20 ký tự."
      },
      Email: {
        email: "Địa chỉ email không đúng định dạng.",
        maxlength: "Email không được vượt quá 100 ký tự."
      },
      TaxCode: {
        required: "Mã số thuế là bắt buộc.",
        maxlength: "Mã số thuế không được vượt quá 20 ký tự."
      },
      IsActive: {
        required: "Trạng thái là bắt buộc."
      }
    },
    errorPlacement: function (error, element) {
      error.insertAfter(element);
    },
    highlight: function (element) {
      $(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
      $(element).closest('.form-group').removeClass('has-error');
    }
  });



  // Handle save customer
  _$form.find('.save-button').on('click', function (e) {
    e.preventDefault();

    if (!_$form.valid()) {
      return;
    }

    var customer = _$form.serializeFormToObject();
    abp.ui.setBusy(_$modal);

    _customerService.create(customer)
      .done(function () {
        _$modal.modal('hide');
        _$form[0].reset();
        abp.notify.info(l('CustomerCreatedSuccessfully'));
        _$customerTable.ajax.reload();
      })
      .always(function () {
        abp.ui.clearBusy(_$modal);
      });
  });

  // Handle edit customer
  $(document).on('click', '.edit-customer', function (e) {
    var customerId = $(this).attr("data-customer-id");
    e.preventDefault();

    abp.ajax({
      url: abp.appPath + 'Customers/EditModal?customerId=' + customerId,
      type: 'POST',
      dataType: 'html',
      success: function (content) {
        $('#CustomerEditModal div.modal-content').html(content);
        $('#CustomerEditModal').modal('show');
      }
    });
  });

  // Handle customer edited event
  abp.event.on('customer.edited', (data) => {
    _$customerTable.ajax.reload();
  });

  // Handle delete customer
  $(document).on('click', '.delete-customer', function () {
    var customerId = $(this).attr("data-customer-id");
    var customerName = $(this).attr('data-customer-name');

    deleteCustomer(customerId, customerName);
  });

  function deleteCustomer(customerId, customerName) {
    abp.message.confirm(
      abp.utils.formatString(
        l('AreYouSureToDeleteCustomer'),
        customerName),
      null,
      (isConfirmed) => {
        if (isConfirmed) {
          _customerService.delete(
            customerId
          ).done(() => {
            abp.notify.info(l('CustomerDeletedSuccessfully'));
            _$customerTable.ajax.reload();
          });
        }
      }
    );
  }

  // Handle isActive toggle
  $(document).on('change', '.is-active-toggle', function () {
    var customerId = $(this).data('id');
    var isActive = $(this).is(':checked');

    _customerService.setActive({
      id: customerId,
      isActive: isActive
    }).done(function () {
      abp.notify.info(l('StatusUpdatedSuccessfully'));
    });
  });

  // Search functionality (if needed)
  $('.btn-search').on('click', function () {
    _$customerTable.ajax.reload();
  });

  $('.txt-search').on('keypress', function (e) {
    if (e.which === 13) {
      _$customerTable.ajax.reload();
      return false;
    }
  });

})(jQuery);
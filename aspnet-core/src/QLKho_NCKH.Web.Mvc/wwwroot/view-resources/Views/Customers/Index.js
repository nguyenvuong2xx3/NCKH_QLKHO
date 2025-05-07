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
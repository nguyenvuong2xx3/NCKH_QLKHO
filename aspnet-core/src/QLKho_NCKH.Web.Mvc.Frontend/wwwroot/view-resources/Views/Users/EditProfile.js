$(function () {
  var _userService = abp.services.app.user;
  var _$form = $('#EditProfileForm');

  //_$form.submit(function (e) {
  //  e.preventDefault();

  //  if (!_$form.valid()) {
  //    return;
  //  }

  //  var profile = _$form.serializeFormToObject();

  //  abp.ui.setBusy(_$form);
  //  _userService.editProfile(profile)
  //    .done(function () {
  //      abp.notify.success('Profile updated successfully');
  //    })
  //    .always(function () {
  //      abp.ui.clearBusy(_$form);
  //    });
  //});

  $('#SaveProfileButton').on('click', function (e) {
		e.preventDefault();
		if (!_$form.valid()) {
			return;
		}
		var profile = _$form.serializeFormToObject();
		abp.ui.setBusy(_$form);
		_userService.editProfile(profile)
			.done(function () {
        abp.notify.success('Profile updated successfully');
				setTimeout(() => {
          window.location.href = "/Users/InforUser";
				}, 1000);
			})
			.always(function () {
				abp.ui.clearBusy(_$form);
			});
  })

  // Initialize form validation
  _$form.validate({
    rules: {
      EmailAddress: {
        required: true,
        email: true
      },
      Name: {
        required: true
      }
    },
    messages: {
      EmailAddress: {
        required: "Email is required",
        email: "Please enter a valid email address"
      },
      Name: {
        required: "Name is required"
      }
    }
  });
});
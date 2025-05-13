namespace QLKho_NCKH.Web.Models.Users
{
	public class UserViewModel
	{
		public long Id { get; set; }
		public string FullName { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string UserName { get; set; }
		public string Address { get; set; }
		public string Avatar { get; set; }
		public bool IsActive { get; set; }
		public string CreationTime { get; set; }

	}
}

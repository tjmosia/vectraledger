namespace VectraBooks.Areas.Identity.Models.Authorization
{
	public class AuthorizationModels
	{
		public class RoleRequest
		{
			public string? Role { get; set; }
			public string? Value { get; set; }
		}

		public class ClaimRequest
		{
			public string? ClaimType { get; set; }
			public string? Value { get; set; }
		}
	}
}

using Microsoft.AspNetCore.Identity;

namespace VectraBooks.Areas.Identity.Services
{
	public class IdentityErrorDescriberExtension : IdentityErrorDescriber
	{
		public IdentityError UnVerifiedEmail ()
			=> new()
			{
				Code = nameof(UnVerifiedEmail),
				Description = "Email is not verified."
			};

		public override IdentityError InvalidEmail (string? email = null)
		{
			return new IdentityError
			{
				Code = nameof(InvalidEmail),
				Description = $"Email not registered."
			};
		}

		public IdentityError Duplicate (string? description)
		{
			return new IdentityError
			{
				Code = nameof(Duplicate),
				Description = $"{description}"
			};
		}

		public IdentityError InvalidRole (string? description)
		{
			return new IdentityError
			{
				Code = nameof(InvalidRole),
				Description = $"{description}"
			};
		}
	}
}

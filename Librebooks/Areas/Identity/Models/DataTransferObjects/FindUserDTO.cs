using VectraBooks.Models.Entity.IdentitySpace;
namespace VectraBooks.Areas.Identity.Models.DataTransferObjects;

public readonly struct FindUserDTO (User user)
{
	public readonly string? Email = user.Email;
	public readonly string? FirstName = user.Name;
	public readonly string? LastName = user.Surname;
	public readonly string? Photo = user.Photo;
}

using VectraBooks.Data;
using VectraBooks.Models.Entity.GeneralSpace;
using VectraBooks.Providers.Stores;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Providers;

public class VerificationStore (AppDbContext context)
	: StoreBase(context)
{
	public async Task<VerificationRequest?> FindAsync (string email, string reason, CancellationToken cancellationToken = default)
	{
		return await context.VerificationRequests!
			.Where(p => p.Email == email && p.Reason == reason)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<VerificationRequest?> UpdateAsync (VerificationRequest request)
	{
		try
		{
			var result = context.VerificationRequests!.Update(request);
			await context.SaveChangesAsync();

			return result.Entity;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<bool> DeleteAsync (VerificationRequest request)
	{
		try
		{
			context.VerificationRequests!.Remove(request);
			await context.SaveChangesAsync();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public async Task<VerificationRequest?> CreateAsync (VerificationRequest request)
	{
		var oldRequest = await FindAsync(request.Email!, request.Reason!);

		if (oldRequest != null)
			await DeleteAsync(oldRequest);

		try
		{
			var result = await context.VerificationRequests!.AddAsync(request);
			await context.SaveChangesAsync();
			return result.Entity;
		}
		catch
		{
			return null;
		}
	}
}

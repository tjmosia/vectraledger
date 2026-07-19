using VectraBooks.Core.EFCore;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.DocumentSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Providers.Stores;

public class DocumentSetupStore (AppDbContext context, ILogger<DocumentSetupStore> logger)
{
	private readonly AppDbContext context = context;
	private readonly ILogger<DocumentSetupStore> logger = logger;
	public async Task<IList<DocumentSetup>> FindAllAsync (Company? company = null, CancellationToken cancellationToken = default)
	{
		if (company == null)
			return await context.DocumentSetups!.Where(p => p.System).ToListAsync(cancellationToken);

		return await context.DocumentSetups!.Where(p => p.CompanyId == company.Id).ToListAsync(cancellationToken);
	}

	public async Task<DocumentSetup?> FindByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context.DocumentSetups!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult<DocumentSetup>> UpdateAsync (DocumentSetup documentSetup)
	{
		try
		{
			var result = context.DocumentSetups!.Update(documentSetup);
			await context.SaveChangesAsync();
			return TransactionResult<DocumentSetup>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<DocumentSetup>.Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateAsync), logger));
		}
	}

	public async Task<TransactionResult> DeleteAsync (params DocumentSetup[] documentSetups)
	{
		try
		{
			context.RemoveRange(documentSetups);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			if (ex is DbUpdateConcurrencyException)
				return TransactionResult.Failure(new TransactionError(nameof(AppErrorDescriber.Concurrency), ""));

			return TransactionResult.Failure(new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), ""));
		}
	}
}

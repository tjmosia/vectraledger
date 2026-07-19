using VectraBooks.Core.EFCore;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Areas.Accounting.Providers;

public class AccountsStore (AppDbContext context, ILogger<AccountsStore> logger) : DbStoreBase(context), IAccountsStore
{
	private readonly ILogger<AccountsStore> logger = logger;


	/**********************************************************
	 * LEDGER ACCOUNTS
	 **********************************************************/

	public async Task<TransactionResult<LedgerAccount>> CreateAsync (LedgerAccount ledgerAccount, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context.LedgerAccounts!.AddAsync(ledgerAccount, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<LedgerAccount>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Exception occured while creating a ledger account with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<LedgerAccount>.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError("One or more dependencies were not found.");
				return GeneralError;
			});
		}
	}

	public async Task<LedgerAccount?> FindByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context.LedgerAccounts!.FindAsync([id], cancellationToken);

	public async Task<IList<LedgerAccount>> FindBySysAsync (CancellationToken cancellationToken = default)
		=> await context.LedgerAccounts!.Where(p => p.System).ToListAsync(cancellationToken);

	public async Task<TransactionResult<LedgerAccount>> UpdateAsync (LedgerAccount ledgerAccount, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context.LedgerAccounts!.Update(ledgerAccount);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<LedgerAccount>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<LedgerAccount>
				.Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateAsync), logger));
		}
	}

	public async Task<TransactionResult> DeleteAsync (params LedgerAccount[] accounts)
	{
		try
		{
			context.LedgerAccounts!.RemoveRange(accounts);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("Exception occured while delete a ledger account with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), accounts.Length > 1 ? "One or more accounts are currently in use." : "account is currently in use.");
				return GeneralError;
			});
		}

	}


	/**********************************************************
	 * LEDGER ACCOUNT CATEGORIES
	 **********************************************************/

	public async Task<LedgerAccountCategory?> FindCategoryByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context.LedgerAccountCategories!.FindAsync([id], cancellationToken);

	public async Task<IList<LedgerAccountCategory>> FindCategoriesAsync (CancellationToken cancellationToken = default)
		=> await context.LedgerAccountCategories!.ToListAsync(cancellationToken);

	public async Task<TransactionResult<LedgerAccountCategory>> CreateOrUpdateCategoryAsync (LedgerAccountCategory category, CancellationToken cancellationToken = default)
	{
		try
		{
			if (category.Id > 0)
			{

				var update = context.LedgerAccountCategories!.Update(category);
				await context.SaveChangesAsync(cancellationToken);
				return TransactionResult<LedgerAccountCategory>.Success(update.Entity);
			}

			var add = await context.LedgerAccountCategories!.AddAsync(category, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<LedgerAccountCategory>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Exception occured while creating/updating a ledger account category with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<LedgerAccountCategory>.Failure(() =>
			{
				if (IsUniqueConstaint(ex))
					return new TransactionError(nameof(AppErrorDescriber.DuplicateKey), "Category with similar name already exists.");

				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.DuplicateKey), "Cashflow type does not exist.");

				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult> DeleteCategoryAsync (params LedgerAccountCategory[] categories)
	{
		try
		{
			context.LedgerAccountCategories!.RemoveRange(categories);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("Exception occured while deleting a ledger account category with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), $"Delete Failed: {(categories.Length > 1 ? "One of more categories are in use." : "Category is currently in use.")}");
				return GeneralError;
			});
		}
	}


	/**********************************************************
	 * CASHFLOW CLASS
	 **********************************************************/
	public async Task<IList<LedgerAccountCashFlowType>> FindCashFlowTypesAsync (CancellationToken cancellationToken = default)
	{
		return await context.LedgerAccountCashFlowTypes!.ToListAsync(cancellationToken);
	}

	public async Task<LedgerAccountCashFlowType?> FindCashflowTypeByIdAsync (int id, CancellationToken cancellationToken = default)
	{
		return await context.LedgerAccountCashFlowTypes!.FindAsync([id], cancellationToken);
	}

	public async Task<TransactionResult<LedgerAccountCashFlowType>> CreateorUpdateCashFlowTypeAsync (LedgerAccountCashFlowType cashFlowType, CancellationToken cancellationToken = default)
	{
		try
		{
			if (cashFlowType.Id > 0)
			{
				var update = context.LedgerAccountCashFlowTypes!.Update(cashFlowType);
				await context.SaveChangesAsync(cancellationToken);
				return TransactionResult<LedgerAccountCashFlowType>.Success(update.Entity);
			}

			var add = await context.LedgerAccountCashFlowTypes!.AddAsync(cashFlowType);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<LedgerAccountCashFlowType>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Exception occured while creating/updating cash flow type with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<LedgerAccountCashFlowType>.Failure(() =>
			{
				if (IsUniqueConstaint(ex))
					return new TransactionError(nameof(AppErrorDescriber.DuplicateKey), "Cash flow type with similar name already exists.");
				return GeneralError;
			});
		}
	}

	/**********************************************************
	 * JOURNAL ENTRIES
	 **********************************************************/
	public async Task<IList<JournalLine>> FindJournalEntriesAsync (Company company, CancellationToken cancellationToken = default)
	{
		return await context.JournalEntries!.Where(p => p.CompanyId == company.Id).ToListAsync(cancellationToken);
		throw new NotImplementedException();
	}

	public async Task<JournalLine?> FindJournalEntryByIdAsync (Company company, int id, CancellationToken cancellationToken = default)
	{
		return await context.JournalEntries!
			.Where(p => p.CompanyId == company.Id && p.Id == id)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<TransactionResult<JournalLine>> CreateJournalEntryAsync (JournalLine entry)
	{
		try
		{
			var add = await context.JournalEntries!.AddAsync(entry);
			await context.SaveChangesAsync();
			return TransactionResult<JournalLine>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<JournalLine>.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), "One or more dependencies were not found.");
				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult<JournalLine>> UpdateJournalEntryAsync (JournalLine entry)
	{
		try
		{
			var add = context.JournalEntries!.Update(entry);
			await context.SaveChangesAsync();
			return TransactionResult<JournalLine>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<JournalLine>.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), "One or more dependencies were not found.");
				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult> DeleteJournalEntriesAsync (params JournalLine[] entries)
	{
		try
		{
			context.JournalEntries!.RemoveRange(entries);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception)
		{
			return TransactionResult.Failure(GeneralError);
		}
	}

	public async Task<TransactionResult> CreateJournalEntriesAsync (params JournalLine[] entries)
	{
		try
		{
			await context.JournalEntries!.AddRangeAsync(entries);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			return TransactionResult.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), "One or more dependencies were not found.");
				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult> UpdateJournalEntriesAsync (params JournalLine[] entries)
	{
		try
		{
			context.JournalEntries!.UpdateRange(entries);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			return TransactionResult.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), "One or more dependencies were not found.");
				return GeneralError;
			});
		}
	}



}

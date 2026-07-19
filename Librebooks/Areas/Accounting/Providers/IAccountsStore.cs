using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CompanySpace;

namespace VectraBooks.Areas.Accounting.Providers;

public interface IAccountsStore
{
	/**********************************************************
	 * LEDGER ACCOUNTS
	 **********************************************************/
	Task<LedgerAccount?> FindByIdAsync (int id, CancellationToken cancellationToken = default);
	Task<IList<LedgerAccount>> FindBySysAsync (CancellationToken cancellationToken = default);
	Task<TransactionResult<LedgerAccount>> UpdateAsync (LedgerAccount ledgerAccount, CancellationToken cancellationToken = default);
	Task<TransactionResult<LedgerAccount>> CreateAsync (LedgerAccount ledgerAccount, CancellationToken cancellationToken = default);
	Task<TransactionResult> DeleteAsync (params LedgerAccount[] accounts);

	/**********************************************************
	 * LEDGER ACCOUNTS
	 **********************************************************/
	Task<TransactionResult<LedgerAccountCategory>> CreateOrUpdateCategoryAsync (LedgerAccountCategory category, CancellationToken cancellationToken = default);
	Task<LedgerAccountCategory?> FindCategoryByIdAsync (int id, CancellationToken cancellationToken = default);
	Task<IList<LedgerAccountCategory>> FindCategoriesAsync (CancellationToken cancellationToken = default);
	Task<TransactionResult> DeleteCategoryAsync (params LedgerAccountCategory[] categories);


	/**********************************************************
	 * CASHFLOW CLASS
	 **********************************************************/
	Task<IList<LedgerAccountCashFlowType>> FindCashFlowTypesAsync (CancellationToken cancellationToken = default);
	Task<LedgerAccountCashFlowType?> FindCashflowTypeByIdAsync (int id, CancellationToken cancellationToken = default);
	Task<TransactionResult<LedgerAccountCashFlowType>> CreateorUpdateCashFlowTypeAsync (LedgerAccountCashFlowType cashFlowType, CancellationToken cancellationToken = default);

	/**********************************************************
	 * JOURNAL ENTRIES
	 **********************************************************/
	Task<IList<JournalLine>> FindJournalEntriesAsync (Company company, CancellationToken cancellationToken = default);
	Task<JournalLine?> FindJournalEntryByIdAsync (Company company, int id, CancellationToken cancellationToken = default);
	Task<TransactionResult<JournalLine>> CreateJournalEntryAsync (JournalLine entry);
	Task<TransactionResult> CreateJournalEntriesAsync (params JournalLine[] entry);
	Task<TransactionResult<JournalLine>> UpdateJournalEntryAsync (JournalLine entry);
	Task<TransactionResult> DeleteJournalEntriesAsync (params JournalLine[] entries);
}

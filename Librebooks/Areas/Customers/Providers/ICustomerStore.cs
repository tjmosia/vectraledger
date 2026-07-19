using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.GeneralSpace;

namespace VectraBooks.Areas.Customers.Providers;

public interface ICustomerStore
{
	/*******************************************
     * CUSTOMER
     ******************************************/
	Task<TransactionResult<Customer>> AddAsync (Company company, Customer customer);
	Task<TransactionResult<Customer>> UpdateAsync (Customer customer);
	Task<TransactionResult> DeleteAsync (Customer customer);
	Task<IList<Customer>> GetAsync (Company company, CancellationToken cancellationToken = default);
	Task<Customer?> FindByIdAsync (Company company, int id, CancellationToken cancellationToken = default);

	/*******************************************
     * CUSTOMER CATEGORIES
     ******************************************/
	Task<CustomerCategory?> FindCategoryByIdAsync (Company company, int categoryId, CancellationToken cancellationToken = default);
	Task<IList<CustomerCategory>> GetCategoriesAsync (Company company, CancellationToken cancellationToken = default);
	Task<TransactionResult<CustomerCategory>> AddCategoryAsync (Company company, CustomerCategory category);
	Task<TransactionResult<CustomerCategory>> UpdateCategoryAsync (CustomerCategory category);
	Task<TransactionResult> DeleteCategoriesAsync (params CustomerCategory[] categories);

	/*******************************************
     * CONTACTS CATEGORIES
     ******************************************/
	Task<IList<Contact>> GetContactsAsync (Customer customer, CancellationToken cancellationToken = default);
	Task<Contact?> FindContactByIdAsync (Customer customer, int contactId, CancellationToken cancellationToken = default);
	Task<TransactionResult<Contact>> AddContactAsync (Customer customer, Contact contact);
	Task<TransactionResult<Contact>> UpdateContactAsync (Contact contact);
	Task<TransactionResult> DeleteContactsAsync (params Contact[] contacts);
	Task<TransactionResult> AddAccountsContact (Customer customer, Contact contact);

	/*******************************************
     * NOTES CATEGORIES
     ******************************************/
	Task<Note?> FindNoteByIdAsync (Customer customer, int noteId, CancellationToken cancellationToken = default);
	Task<IList<Note>> GetNotesAsync (Customer customer, CancellationToken cancellationToken = default);
	Task<TransactionResult<Note>> AddNoteAsync (Customer customer, Note note);
	Task<TransactionResult<Note>> UpdateNoteAsync (Note note);
	Task<TransactionResult> DeleteNotesAsync (params Note[] notes);

	/*******************************************
     * NOTES CATEGORIES
     ******************************************/
	Task<TransactionResult<CustomerAdjustment>> AddAdjustmentAsync (Customer customer, CustomerAdjustment adjustment);
	Task<TransactionResult<CustomerAdjustment>> UpdateAdjustmentAsync (CustomerAdjustment adjustment);
	Task<CustomerAdjustment?> FindAdjustmentByIdAsync (Customer customer, int id, CancellationToken cancellationToken = default);
	Task<IList<CustomerAdjustment>> GetAdjustmentsAsync (Customer customer, CancellationToken cancellationToken = default);
	Task<TransactionResult> DeleteAdjustmentAsync (CustomerAdjustment adjustment);
}

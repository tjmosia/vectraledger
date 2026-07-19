using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.BankingSpace;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.DocumentSpace;
using VectraBooks.Models.Entity.GeneralSpace;
using VectraBooks.Models.Entity.IdentitySpace;
using VectraBooks.Models.Entity.InventorySpace;
using VectraBooks.Models.Entity.SalesSpace;
using VectraBooks.Models.Entity.SupplierSpace;
using VectraBooks.Models.Entity.SystemSpace;

namespace VectraBooks.Areas.Suppliers.Services
{
    public interface ISupplierStore
    {
        /*******************************************
         * Supplier
         ******************************************/
        public Task<TransactionResult<Supplier>> AddAsync(Company company, Supplier Supplier);
        public Task<TransactionResult<Supplier>> UpdateAsync(Supplier Supplier);
        Task<TransactionResult> DeleteAsync(Supplier Supplier);
        public Task<IList<Supplier>> GetAsync(Company company, CancellationToken cancellationToken = default);
        public Task<Supplier?> FindByIdAsync(Company company, int id, CancellationToken cancellationToken = default);

        /*******************************************
         * Supplier CATEGORIES
         ******************************************/
        Task<SupplierCategory?> FindCategoryByIdAsync(Company company, int categoryId, CancellationToken cancellationToken = default);
        Task<IList<SupplierCategory>> GetCategoriesAsync(Company company, CancellationToken cancellationToken = default);
        Task<TransactionResult<SupplierCategory>> AddCategoryAsync(Company company, SupplierCategory category);
        Task<TransactionResult<SupplierCategory>> UpdateCategoryAsync(SupplierCategory category);
        Task<TransactionResult> DeleteCategoriesAsync(params SupplierCategory[] categories);

        /*******************************************
         * CONTACTS CATEGORIES
         ******************************************/
        Task<IList<Contact>> GetContactsAsync(Supplier Supplier, CancellationToken cancellationToken = default);
        Task<Contact?> FindContactByIdAsync(Supplier Supplier, int contactId, CancellationToken cancellationToken = default);
        Task<TransactionResult<Contact>> AddContactAsync(Supplier Supplier, Contact contact);
        Task<TransactionResult<Contact>> UpdateContactAsync(Contact contact);
        Task<TransactionResult> DeleteContactsAsync(params Contact[] contacts);
        Task<TransactionResult> AddAccountsContact(Supplier Supplier, Contact contact);

        /*******************************************
         * NOTES CATEGORIES
         ******************************************/
        Task<Note?> FindNoteByIdAsync(Supplier Supplier, int noteId, CancellationToken cancellationToken = default);
        Task<IList<Note>> GetNotesAsync(Supplier Supplier, CancellationToken cancellationToken = default);
        Task<TransactionResult<Note>> AddNoteAsync(Supplier Supplier, Note note);
        Task<TransactionResult<Note>> UpdateNoteAsync(Note note);
        Task<TransactionResult> DeleteNotesAsync(params Note[] notes);


        /*******************************************
         * NOTES CATEGORIES
         ******************************************/
        Task<TransactionResult<SupplierAdjustment>> AddAdjustmentAsync(Supplier Supplier, SupplierAdjustment adjustment);
        Task<TransactionResult<SupplierAdjustment>> UpdateAdjustmentAsync(SupplierAdjustment adjustment);
        Task<SupplierAdjustment?> FindAdjustmentByIdAsync(Supplier Supplier, int id, CancellationToken cancellationToken = default);
        Task<IList<SupplierAdjustment>> GetAdjustmentsAsync(Supplier Supplier, CancellationToken cancellationToken = default);
        Task<TransactionResult> DeleteAdjustmentAsync(SupplierAdjustment adjustment);
    }
}

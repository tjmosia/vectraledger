using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SupplierSpace;

namespace VectraBooks.Areas.Suppliers.Services
{
    public interface ISupplierManager
    {
        Task<TransactionResult> AllocateReturnToInvoiceAsync (PurchaseInvoice invoice, PurchasesReturn purchaseReturn);
        Task<TransactionResult> AllocateReceiptToInvoiceAsync (PurchasePayment receipt, PurchaseInvoice invoice);

        Task<IList<Supplier>> GetAllAsync (Company company);
        Task<TransactionResult<Supplier>> CreateAsync (Company company, Supplier supplier);

        Task<TransactionResult<PurchaseOrder>> AddOrderAsync (Supplier supplier, PurchaseOrder order);
        Task<TransactionResult<PurchaseInvoice>> AddInvoiceAsync (Supplier supplier, PurchaseOrder order);
        Task<TransactionResult<PurchasePayment>> AddReceiptAsync (Supplier supplier, PurchaseOrder order);
        Task<TransactionResult<PurchasesReturn>> AddReturnAsync (Supplier supplier, PurchasePayment purchaseReturn);

        Task<Supplier> FindByIdAsync (Company company, string supplierId);
        Task<Supplier> FindByVendorNumberAsync (Company company, string vendorNumber);
        Task<PurchaseInvoice> FindInvoiceByNumberAsync (Supplier supplier, string invoiceNumber);
        Task<PurchaseInvoice> FindOrderByNumberAsync (Supplier supplier, string orderNumber);
        Task<PurchaseInvoice> FindReturnByNumberAsync (Supplier supplier, string orderNumber);
        Task<PurchasePayment> FindReceiptByNumberAsync (Supplier supplier, string receiptNumber);

        Task<IList<PurchaseOrder>> GetOrdersAsync (Supplier supplier);
        Task<IList<PurchaseInvoice>> GetInvoicesAsync (Supplier supplier);
        Task<IList<PurchasePayment>> GetReceiptsAsync (Supplier supplier);
        Task<IList<PurchasesReturn>> GetReturnAsync (Supplier supplier);

        Task<TransactionResult<Supplier>> UpdateAsync (Supplier supplier);

        Task<TransactionResult> DeleteAsync (Supplier supplier);
    }
}

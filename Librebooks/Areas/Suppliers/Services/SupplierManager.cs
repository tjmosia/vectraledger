
using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SupplierSpace;

namespace VectraBooks.Areas.Suppliers.Services
{
	public class SupplierManager : ISupplierManager
	{
		public Task<TransactionResult> AllocateReturnToInvoiceAsync (PurchaseInvoice invoice, PurchasesReturn purchaseReturn)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult> AllocateReceiptToInvoiceAsync (PurchasePayment receipt, PurchaseInvoice invoice)
		{
			throw new NotImplementedException();
		}

		public Task<IList<Supplier>> GetAllAsync (Company company)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<Supplier>> CreateAsync (Company company, Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<PurchaseOrder>> AddOrderAsync (Supplier supplier, PurchaseOrder order)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<PurchaseInvoice>> AddInvoiceAsync (Supplier supplier, PurchaseOrder order)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<PurchasePayment>> AddReceiptAsync (Supplier supplier, PurchaseOrder order)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<PurchasesReturn>> AddReturnAsync (Supplier supplier, PurchasePayment purchaseReturn)
		{
			throw new NotImplementedException();
		}

		public Task<Supplier> FindByIdAsync (Company company, string supplierId)
		{
			throw new NotImplementedException();
		}

		public Task<Supplier> FindByVendorNumberAsync (Company company, string vendorNumber)
		{
			throw new NotImplementedException();
		}

		public Task<PurchaseInvoice> FindInvoiceByNumberAsync (Supplier supplier, string invoiceNumber)
		{
			throw new NotImplementedException();
		}

		public Task<PurchaseInvoice> FindOrderByNumberAsync (Supplier supplier, string orderNumber)
		{
			throw new NotImplementedException();
		}

		public Task<PurchaseInvoice> FindReturnByNumberAsync (Supplier supplier, string orderNumber)
		{
			throw new NotImplementedException();
		}

		public Task<PurchasePayment> FindReceiptByNumberAsync (Supplier supplier, string receiptNumber)
		{
			throw new NotImplementedException();
		}

		public Task<IList<PurchaseOrder>> GetOrdersAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<IList<PurchaseInvoice>> GetInvoicesAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<IList<PurchasePayment>> GetReceiptsAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<IList<PurchasesReturn>> GetReturnAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult<Supplier>> UpdateAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionResult> DeleteAsync (Supplier supplier)
		{
			throw new NotImplementedException();
		}
	}
}

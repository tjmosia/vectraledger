using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.SalesSpace;

namespace VectraBooks.Areas.Customers.Providers;

public interface ISalesStore
{
	Task<TransactionResult<SalesDocument>> CreateDocumentAsync (Company company, Customer customer, SalesDocument document);
	Task<TransactionResult<SalesDocument>> UpdateDocumentAsync (Company company, Customer customer, SalesDocument document);

	/**************************************************************************
     * SALES QUOTES
     *************************************************************************/

	Task<TransactionResult> CreateQuoteAsync (Company company, Customer customer, SalesDocument quoteDocument);
	Task<TransactionResult> UpdateQuoteAsync (SalesDocument quoteDocument);
	Task<IList<SalesQuote>> GetQuotesAsync (Company company, CancellationToken cancellationToken = default);
	Task<SalesDocument?> GetQuotesAsync (Company company, Customer customer, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindQuoteByIdASync (Company company, int quoteId, CancellationToken cancellationToken = default);
	Task<SalesDocument> FindQuoteByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> DeleteQuoteAsync (SalesQuote quote);


	/**************************************************************************
     * SALES ORDERS
     *************************************************************************/
	Task<IList<SalesDocument>> GetOrdersAsync (Company company, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindOrderByIdAsync (Company company, int orderId, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindOrderByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> CreateOrderAsync (Company company, Customer customer, SalesDocument orderDocument);
	Task<TransactionResult> UpdateOrderAsync (SalesDocument orderDocument);
	Task<TransactionResult> DeleteOrderAsync (SalesDocument orderDocument);


	/**************************************************************************
     * SALES INVOICES
     *************************************************************************/
	Task<IList<SalesDocument>> GetInvoicesAsync (Company company, CancellationToken cancellationToken = default);
	Task<IList<SalesDocument>> GetInvoicesAsync (Company company, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
	Task<IList<SalesDocument>> GetInvoicesAsync (Company company, Customer customer, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindInvoiceByIdAsync (Company company, int invoiceId, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindInvoiceByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> CreateInvoiceAsync (Company company, Customer customer, SalesDocument invoiceDocument);
	Task<TransactionResult> UpdateInvoiceAsync (SalesDocument invoiceDocument);
	Task<TransactionResult> DeleteInvoiceAsync (SalesDocument invoiceDocument);

	/**************************************************************************
     * SALES CREDIT NOTES
     *************************************************************************/
	Task<IList<SalesDocument>> GetCreditNotesAsync (Company company, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindCreditNoteByIdAsync (Company company, int creditNoteId, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindCreditNoteByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> CreateCreditNoteAsync (Company company, Customer customer, SalesDocument creditNoteDocument);
	Task<TransactionResult> UpdateCreditNoteAsync (SalesDocument creditNoteDocument);
	Task<TransactionResult> DeleteCreditNoteAsync (SalesDocument creditNoteDocument);
	Task<TransactionResult> AllocateCreditToInvoiceAsync (SalesInvoice invoice, SalesReceipt receiptDocument, decimal amount);

	/**************************************************************************
     * SALES PRO FORMA INVOICES
     *************************************************************************/
	Task<IList<SalesDocument>> GetProFormaInvoicesAsync (Company company, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindProFormaInvoiceByIdAsync (Company company, int proFormaInvoiceId, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindProFormaInvoiceByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> CreateProFormaInvoiceAsync (Company company, Customer customer, SalesDocument proFormaInvoiceDocument);
	Task<TransactionResult> UpdateProFormaInvoiceAsync (SalesDocument proFormaInvoiceDocument);
	Task<TransactionResult> DeleteProFormaInvoiceAsync (SalesDocument proFormaInvoiceDocument);


	/**************************************************************************
     * SALES RECEIPTS
     *************************************************************************/
	Task<IList<SalesDocument>> GetReceiptsAsync (Company company, CancellationToken cancellationToken = default);
	Task<IList<SalesDocument>> GetReceiptsAsync (Company company, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindReceiptByIdAsync (Company company, int receiptId, CancellationToken cancellationToken = default);
	Task<SalesDocument?> FindReceiptByNumberAsync (Company company, string number, CancellationToken cancellationToken = default);
	Task<TransactionResult> CreateReceiptAsync (Company company, Customer customer, SalesDocument receipt);
	Task<TransactionResult> UpdateReceiptAsync (SalesDocument receipt);
	Task<TransactionResult> DeleteReceiptAsync (SalesDocument receipt);
	Task<TransactionResult> AllocateReceiptToInvoiceAsync (SalesDocument invoice, SalesReceipt receipt, decimal amount);
	Task<TransactionResult> AllocateWriteOffToInvoiceAsync (SalesInvoice invoice, CustomerWriteOff writeOff, decimal amount);
}

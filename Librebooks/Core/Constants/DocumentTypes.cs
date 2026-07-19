namespace Librebooks.Core.Types.PurchasesAndSales;

public readonly struct DocumentTypes
{
	public static readonly (string Name, int Value)
		/***************************************************
		 * CUSTOMERS DOCUMENTS
		 **************************************************/
		CustomerQuote = (Name: "Quote", Value: 1),
		CustomerSalesOrder = (Name: "Sales Order", Value: 2),
		CustomerProForma = (Name: "Pro Forma", Value: 3),
		CustomerInvoice = (Name: "Tax Invoice", Value: 4),
		CustomerGoodsReturnNote = (Name: "Goods Return Note", Value: 5),
		CustomerGoodsIssuedNote = (Name: "Goods Issue Note", Value: 18),
		CustomerCreditNote = (Name: "Credit Note", Value: 6),
		CustomerReceipt = (Name: "Customer Receipt", Value: 7),
		CustomerAdjustment = (Name: "Customer Adjustment", Value: 8),
		CustomerWriteOff = (Name: "Customer Write Off", Value: 9),

		/***************************************************
		 * SUPPLIER DOCUMENTS
		 **************************************************/
		SupplierQuote = (Name: "Request For Quote", Value: 10),
		SupplierPurchaseOrder = (Name: "Purchase Order", Value: 11),
		SupplierInvoice = (Name: "Supplier Invoice", Value: 12),
		SupplierReturn = (Name: "Supplier Return", Value: 13),
		SupplierCreditNote = (Name: "Supplier Credit Note", Value: 14),
		SupplierPayment = (Name: "Supplier Payment", Value: 15),
		SupplierAdjustment = (Name: "Supplier Adjustment", Value: 16),


		/***************************************************
		 * SUPPLIER DOCUMENTS
		 **************************************************/
		InventoryTransfer = (Name: "Stock Transfer", Value: 17),
		GoodsIssue = (Name: "", Value: 19),
		GoodsReceipt = (Name: "", Value: 20);
}

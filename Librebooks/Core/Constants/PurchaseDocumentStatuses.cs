namespace Librebooks.Core.Types.PurchasesAndSales;

public readonly struct PurchaseDocumentStatuses
{
	public readonly struct Orders
	{
		public const int ACTIVE = 1,
			DELIVERED = 3,
			CANCELLED = 4;
	}

	public readonly struct Invoices
	{
		public const int UNPAID = 1,
			PAID = 3,
			RETURNED = 4;
	}
}

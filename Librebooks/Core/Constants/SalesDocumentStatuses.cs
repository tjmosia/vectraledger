namespace Librebooks.Core.Types.PurchasesAndSales;

public readonly struct SalesDocumentStatuses
{
	public readonly struct Quotes
	{
		public const int ACTIVE = 1,
			EXPIRED = 2,
			CLOSED = 3,
			APPROVED = 4;
	}

	public readonly struct Orders
	{
		public const int PENDING = 1,
			OVERDUE = 2,
			DELIVERED = 3,
			CANCELLED = 4;
	}

	public readonly struct Invoices
	{
		public const int UNPAID = 1,
			CREDITED = 3,
			PAID = 4;
	}
}

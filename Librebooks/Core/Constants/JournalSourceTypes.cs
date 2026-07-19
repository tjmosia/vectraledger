namespace Librebooks.Core.Constants;

public readonly struct JournalSourceTypes
{
	public static readonly int SalesDocument = 1,
		PurchaseDocument = 2,
		CustomerAdjustment = 3,
		SupplierAdjustment = 4,
		CustomerWriteOff = 5;
}

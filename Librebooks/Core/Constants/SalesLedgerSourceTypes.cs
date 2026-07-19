namespace Librebooks.Core.Types.PurchasesAndSales;

public readonly struct SalesLedgerSourceTypes
{
	public const int Invoice = 1,
		Credit = 2,
		Receipt = 3,
		WriteOff = 4,
		Adjustment = 5;
}

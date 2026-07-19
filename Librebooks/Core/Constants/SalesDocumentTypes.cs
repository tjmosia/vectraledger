namespace Librebooks.Core.Types.PurchasesAndSales;

public readonly struct SalesDocumentTypes
{
	public const int Quote = 1,
		ProForma = 2,
		Order = 3,
		Invoice = 4,
		Credit = 5,
		Receipt = 6;
}

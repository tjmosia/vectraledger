namespace VectraBooks.Core.Constants;

public class ApplicationTypes
{
	public readonly struct TaxTypes
	{
		public const string ValueAdded = "types/tax/value-added";
		public const string Income = "types/tax/income";
		public const string CapitalGains = "types/tax/income";
	}

	public readonly struct SalesDocumentTypes
	{
		public const string Quote = "types/sales/document/quote";
		public const string Invoice = "types/sales/document/invoice";
		public const string Credit = "types/sales/document/credit";
		public const string Order = "types/sales/document/order";
	}
}

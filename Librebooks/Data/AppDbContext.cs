using Librebooks.Models.Entity.AccountingSpace;
using Librebooks.Models.Entity.BankingSpace;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;
using Librebooks.Models.Entity.DocumentSpace;
using Librebooks.Models.Entity.GeneralSpace;
using Librebooks.Models.Entity.IdentitySpace;
using Librebooks.Models.Entity.InventorySpace;
using Librebooks.Models.Entity.PurchasesSpace;
using Librebooks.Models.Entity.SalesSpace;
using Librebooks.Models.Entity.SupplierSpace;
using Librebooks.Models.Entity.SystemSpace;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Data;

public class AppDbContext :
	IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
	public AppDbContext (DbContextOptions<AppDbContext> options)
		: base(options) { }

	public AppDbContext () { }

	/************************************************************************************************
         * Company Space
         ************************************************************************************************/
	public DbSet<Company>? Companies { get; set; }
	public DbSet<CompanyUser>? CompanyUsers { get; set; }
	public DbSet<CompanyBankAccount>? CompanyDefaultBankAccounts { get; set; }
	public DbSet<CompanyTax>? CompanyTaxes { get; set; }
	public DbSet<CompanyMailSetup>? CompanyMailSettings { get; set; }
	public DbSet<CompanyLogo>? CompanyLogos { get; set; }
	public DbSet<CompanyImage>? CompanyImages { get; set; }
	public DbSet<CompanySetup>? CompanySetups { get; set; }
	public DbSet<CompanyRegionalSetup>? CompanyRegionalSettings { get; set; }
	public DbSet<CompanyBuyer>? CompanyBuyers { get; set; }

	/************************************************************************************************
         * Customer Space
         ************************************************************************************************/
	public DbSet<Customer>? Customers { get; set; }
	public DbSet<CustomerAdjustment>? CustomerAdjustments { get; set; }
	public DbSet<CustomerCategory>? CustomerCategories { get; set; }
	public DbSet<CustomerContact>? CustomerContacts { get; set; }
	public DbSet<CustomerNote>? CustomerNotes { get; set; }
	public DbSet<CustomerSetup>? CustomerSetups { get; set; }

	/************************************************************************************************
         * Sales Space
         ************************************************************************************************/
	public DbSet<CompanySalesRep>? SalesPeople { get; set; }
	public DbSet<SalesDocument>? SalesDocuments { get; set; }
	public DbSet<SalesDocumentLine>? SalesDocumentLines { get; set; }
	public DbSet<SalesInvoiceReceipt>? SalesInvoiceReceipts { get; set; }
	public DbSet<SalesReceipt>? SalesReceipts { get; set; }
	public DbSet<SalesDocumentLine>? SalesLines { get; set; }
	public DbSet<SalesDocumentCustomerInfo>? SalesDocumentCustomerDetails { get; set; }
	public DbSet<SalesInvoiceCredit>? SalesInvoiceCredits { get; set; }
	public DbSet<SalesInvoiceWriteoff>? SalesInvoiceWriteoffs { get; set; }
	public DbSet<SalesLedger>? SalesLedgers { get; set; }
	public DbSet<SalesInvoice>? SalesInvoices{ get; set; }
	public DbSet<SalesProforma>? SalesProformas { get; set; }
	public DbSet<SalesOrder>? SalesOrders { get; set; }
	public DbSet<SalesQuote>? SalesQuotes { get; set; }

	/************************************************************************************************
         * Inventory Space
         ************************************************************************************************/
	public DbSet<Item>? Items { get; set; }
	public DbSet<ItemSetup>? ItemSetups { get; set; }
	public DbSet<InventoryAdjustment>? InventoryAdjustments { get; set; }
	public DbSet<ItemCategory>? ItemCategories { get; set; }
	public DbSet<Inventory>? Inventories { get; set; }
	public DbSet<ItemDetail>? ItemDetails { get; set; }
	public DbSet<Warehouse> Warehouses { get; set; }
	public DbSet<WarehouseBin> WarehouseBins { get; set; }
	public DbSet<WarehouseBay> WarehouseColumns { get; set; }	
	public DbSet<WarehouseRow> WarehouseRows { get; set; }
	public DbSet<WarehouseShelve> WarehouseShelves { get; set; }
	public DbSet<WarehouseZone> WarehouseZones { get; set; }
	public DbSet<InventoryTransfer> InventoryTransfers { get; set; }
	public DbSet<ItemPriceHistory> ItemPriceHistory { get; set; }


	/************************************************************************************************
         * Accounting Space
         ************************************************************************************************/
	public DbSet<LedgerAccount>? LedgerAccounts { get; set; }
	public DbSet<LedgerAccountCategory>? LedgerAccountCategories { get; set; }
	public DbSet<JournalLine>? JournalEntries { get; set; }
	public DbSet<LedgerAccountCashFlowType>? LedgerAccountCashFlowTypes { get; set; }
	public DbSet<CompanyLedgerAccount>? CompanyLedgerAccounts { get; set; }

	/************************************************************************************************
         * Banking Space
         ************************************************************************************************/
	public DbSet<BankAccount>? BankAccounts { get; set; }
	public DbSet<BankAccountCategory>? BankAccountCategories { get; set; }

	/************************************************************************************************
         * Document Space
         ************************************************************************************************/
	public DbSet<DocumentSetup>? DocumentSetups { get; set; }
	public DbSet<DocumentStatus>? DocumentStatuses { get; set; }
	public DbSet<DocumentPrintTemplate>? DocumentPrintTemplates { get; set; }
	public DbSet<DocumentCompanyInfo>? DocumentCompanyDetails { get; set; }
	public DbSet<DocumentType>? DocumentTypes { get; set; }

	/************************************************************************************************
         * System Space
         ************************************************************************************************/
	public DbSet<ShippingTerm>? ShippingTerms { get; set; }
	public DbSet<ShippingMethod>? ShippingMethods { get; set; }
	public DbSet<Country>? Countries { get; set; }
	public DbSet<Currency>? Currencies { get; set; }
	public DbSet<DateFormat>? DateFormats { get; set; }
	public DbSet<Tax>? Taxes { get; set; }
	public DbSet<PaymentMethod>? PaymentMethods { get; set; }
	public DbSet<PaymentTerm>? PaymentTerms { get; set; }
	public DbSet<BusinessSector>? BusinessSectors { get; set; }

	/************************************************************************************************
         * Supplier Space
         ************************************************************************************************/
	public DbSet<Supplier>? Suppliers { get; set; }
	public DbSet<SupplierNote>? SupplierNotes { get; set; }
	public DbSet<SupplierAccountsContact>? SupplierAccountsContacts { get; set; }
	public DbSet<SupplierAdjustment>? SupplierAdjustments { get; set; }
	public DbSet<SupplierCategory>? SupplierCategories { get; set; }
	public DbSet<SupplierContact>? SupplierContacts { get; set; }
	public DbSet<SupplierSetup>? SupplierSetups { get; set; }

	/************************************************************************************************
         * Purchases Space
         ************************************************************************************************/
	public DbSet<PurchaseDocument>? PurchaseDocuments { get; set; }
	public DbSet<PurchaseDocumentLine>? PurchaseDocumentLines { get; set; }
	public DbSet<PurchaseCreditInvoice>? PurchaseCreditInvoices { get; set; }
	public DbSet<PurchaseLine>? PurchaseLines { get; set; }
	public DbSet<PurchaseInvoiceReturn>? PurchaseReturnInvoices { get; set; }
	public DbSet<PurchasePayment>? PurchaseReceipts { get; set; }
	public DbSet<PurchaseOrderInvoice>? PurchaseOrderInvoices { get; set; }
	public DbSet<PurchaseInvoicePayment>? PurchaseInvoiceReceipts { get; set; }
	public DbSet<DocumentSupplierDetail>? DocumentSupplierDetails { get; set; }
	public DbSet<PurchaseLedger>? PurchaseLedgers { get; set; }
	public DbSet<PurchaseLedgerJournal>? PurchaseLedgerJournals { get; set; }

	/************************************************************************************************
         * GENERAL SPACE
         ************************************************************************************************/
	public DbSet<Contact>? Contacts { get; set; }
	public DbSet<Note>? Notes { get; set; }
	public DbSet<VerificationRequest>? VerificationRequests { get; set; }
	public DbSet<Address>? Addresses { get; set; }

	protected override void OnModelCreating (ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		/************************************************************************************************
             * User Space
             ************************************************************************************************/
		User.OnModelCreating(builder);
		Role.OnModelCreating(builder);
		RoleClaim.OnModelCreating(builder);
		UserRole.OnModelCreating(builder);
		UserToken.OnModelCreating(builder);
		UserLogin.OnModelCreating(builder);
		UserClaim.OnModelCreating(builder);

		/************************************************************************************************
             * Inventory Space
             ************************************************************************************************/
		Item.OnModelCreating(builder);
		ItemCategory.OnModelCreating(builder);
		Inventory.OnModelCreating(builder);
		InventoryAdjustment.OnModelCreating(builder);
		ItemSetup.OnModelCreating(builder);
		ItemDetail.OnModelCreating(builder);
		Warehouse.OnModelCreating(builder);
		WarehouseBin.OnModelCreating (builder);
		WarehouseBay.OnModelCreating(builder);
		WarehouseRow.OnModelCreating(builder);
		WarehouseZone.OnModelCreating(builder);
		WarehouseShelve.OnModelCreating(builder);
		InventoryTransfer.OnModelCreating(builder);

		/************************************************************************************************
             * Document Space
             ************************************************************************************************/
		DocumentCompanyInfo.OnModelCreating(builder);
		DocumentPrintTemplate.OnModelCreating(builder);
		DocumentSetup.OnModelCreating(builder);
		DocumentStatus.OnModelCreating(builder);
		DocumentType.OnModelCreating(builder);

		/************************************************************************************************
             * Sales Space
             ************************************************************************************************/
		SalesDocumentCustomerInfo.OnModelCreating(builder);
		SalesDocument.OnModelCreating(builder);
		SalesDocumentLine.OnModelCreating(builder);
		SalesInvoiceCredit.OnModelCreating(builder);
		SalesInvoiceReceipt.OnModelCreating(builder);
		SalesInvoiceWriteoff.OnModelCreating(builder);
		SalesDocumentLine.OnModelCreating(builder);
		CompanySalesRep.OnModelCreating(builder);
		SalesReceipt.OnModelCreating(builder);
		SalesLedger.OnModelCreating(builder);
		SalesInvoice.OnModelCreating(builder);
		SalesProforma.OnModelCreating(builder);
		SalesQuote.OnModelCreating(builder);
		SalesQuoteInvoice.OnModelCreating(builder);

		/************************************************************************************************
             * Company Space
             ************************************************************************************************/
		Company.OnModelCreating(builder);
		CompanyBankAccount.OnModelCreating(builder);
		CompanyImage.OnModelCreating(builder);
		CompanyLogo.OnModelCreating(builder);
		CompanyMailSetup.OnModelCreating(builder);
		CompanyRegionalSetup.OnModelCreating(builder);
		CompanySetup.OnModelCreating(builder);
		CompanyTax.BuildModel(builder);
		CompanyUser.OnModelCreating(builder);

		/************************************************************************************************
             * System Space
             ************************************************************************************************/
		Country.OnModelCreating(builder);
		Currency.OnModelCreating(builder);
		DateFormat.OnModelCreating(builder);
		Tax.OnModelCreating(builder);
		ShippingMethod.OnModelCreating(builder);
		ShippingTerm.OnModelCreating(builder);
		PaymentMethod.OnModelCreating(builder);
		BusinessSector.OnModelCreating(builder);

		/************************************************************************************************
             * Customer Space
             ************************************************************************************************/
		Customer.OnModelCreating(builder);
		CustomerAdjustment.OnModelCreating(builder);
		CustomerCategory.OnModelCreating(builder);
		CustomerContact.BuildModel(builder);
		CustomerNote.OnModelCreating(builder);
		CustomerSetup.OnModelCreating(builder);

		/************************************************************************************************
             * Accounting Space
             ************************************************************************************************/
		LedgerAccount.OnModelCreating(builder);
		LedgerAccountCategory.OnModelCreating(builder);
		LedgerAccountCashFlowType.OnModelCreating(builder);
		Journal.OnModelCreating(builder);
		JournalLine.OnModelCreating(builder);
		CompanyLedgerAccount.OnModelCreating(builder);

		/************************************************************************************************
             * Supplier Space
             ************************************************************************************************/
		Supplier.OnModelCreating(builder);
		SupplierNote.OnModelCreating(builder);
		SupplierAdjustment.OnModelCreating(builder);
		SupplierContact.OnModelCreating(builder);
		SupplierAccountsContact.OnModelCreating(builder);
		SupplierCategory.OnModelCreating(builder);
		SupplierSetup.OnModelCreating(builder);

		/************************************************************************************************
             * Purchasing Space
             ************************************************************************************************/
		DocumentSupplierDetail.OnModelCreating(builder);
		CompanyBuyer.OnModelCreating(builder);
		PurchaseDocument.OnModelCreating(builder);
		PurchaseDocumentLine.OnModelCreating(builder);
		PurchaseCreditInvoice.OnModelCreating(builder);
		PurchaseInvoicePayment.OnModelCreating(builder);
		PurchaseInvoiceReturn.OnModelCreating(builder);
		PurchaseLine.OnModelCreating(builder);
		PurchaseOrderInvoice.OnModelCreating(builder);
		PurchasePayment.OnModelCreating(builder);
		PurchaseLedger.OnModelCreating(builder);
		PurchaseLedgerJournal.OnModelCreating(builder);

		/************************************************************************************************
             * Banking Space
             ************************************************************************************************/
		BankAccount.OnModelCreating(builder);
		BankAccountCategory.OnModelCreating(builder);

		/************************************************************************************************
             * General Space
             ************************************************************************************************/
		Contact.OnModelCreating(builder);
		Note.OnModelCreating(builder);
		VerificationRequest.OnModelCreating(builder);
		Address.OnModelCreating(builder);
	}
}

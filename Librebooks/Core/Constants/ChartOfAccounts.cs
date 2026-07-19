using Microsoft.Identity.Client;

namespace VectraBooks.Core.Constants;

public readonly struct ChartOfAccountsnamespace 
{
    // ==========================================
    // ASSETS (1000 - 1999)
    // ==========================================
    public static readonly (string Name, int Value) BankMainOperationalAccount = ("Bank Main Operational Account", 1000);
    public static readonly (string Name, int Value) PettyCashFloat = ("Petty Cash Float", 1050);
    public static readonly (string Name, int Value) AccountsReceivable = ("Accounts Receivable", 1100);
    public static readonly (string Name, int Value) InventoryOrDirectMaterials = ("Inventory Or Direct Materials", 1200);
    public static readonly (string Name, int Value) PrepaymentsAndDeposits = ("Prepayments And Deposits", 1300); // Rent deposits, prepaid insurance
    public static readonly (string Name, int Value) PropertyPlantAndEquipment = ("Property Plant And Equipment", 1500); // Core fixed assets
    public static readonly (string Name, int Value) AccumulatedDepreciation = ("Accumulated Depreciation", 1550);

    // ==========================================
    // LIABILITIES (2000 - 2999)
    // ==========================================
    public static readonly (string Name, int Value) AccountsPayable = ("Accounts Payable", 2000);
    public static readonly (string Name, int Value) VatSalesTaxPayable = ("Vat Sales Tax Payable", 2100);
    public static readonly (string Name, int Value) PayrollLiabilities = ("Payroll Liabilities", 2200);
    public static readonly (string Name, int Value) IncomeReceivedInAdvance = ("Income Received In Advance", 2300); // Deferred revenue / customer deposits
    public static readonly (string Name, int Value) ShareholderOrDirectorLoanAccount = ("Shareholder Or Director Loan Account", 2500);
    public static readonly (string Name, int Value) LongTermBorrowings = ("Long Term Borrowings", 2600);

    // ==========================================
    // EQUITY (3000 - 3999)
    // ==========================================
    public static readonly (string Name, int Value) ShareCapital = ("Share Capital", 3000);
    public static readonly (string Name, int Value) RetainedEarnings = ("Retained Earnings", 3100);
    public static readonly (string Name, int Value) CurrentYearEarnings = ("Current Year Earnings", 3200);

    // ==========================================
    // REVENUE / INCOME (4000 - 4999)
    // ==========================================
    public static readonly (string Name, int Value) PrimaryOperatingRevenue = ("Primary Operating Revenue", 4000); // Main sales or service income
    public static readonly (string Name, int Value) SecondaryOperatingRevenue = ("Secondary Operating Revenue", 4100); // Ancillary business income
    public static readonly (string Name, int Value) OtherIncome = ("Other Income", 4200); // Interest earned, asset disposal gains

    // ==========================================
    // COST OF SALES / DIRECT COSTS (5000 - 5499)
    // ==========================================
    public static readonly (string Name, int Value) CostOfGoodsSoldOrServices = ("Cost Of Goods Sold Or Services", 5000); // Core direct expense to fulfill revenue
    public static readonly (string Name, int Value) DirectLabor = ("Direct Labor", 5100); // Wages explicitly tied to production or service delivery
    public static readonly (string Name, int Value) OtherDirectCosts = ("Other Direct Costs", 5200); // Subcontractors, direct software/tools, or job-specific freight

    // ==========================================
    // OPERATING EXPENSES (5500 - 6999)
    // ==========================================
    public static readonly (string Name, int Value) RentAndFacilities = ("Rent And Facilities", 5500);
    public static readonly (string Name, int Value) UtilitiesAndCommunications = ("Utilities And Communications", 5600);
    public static readonly (string Name, int Value) SalariesWagesAdministrative = ("Salaries Wages Administrative", 5700);
    public static readonly (string Name, int Value) AdvertisingAndMarketing = ("Advertising And Marketing", 5750);
    public static readonly (string Name, int Value) SoftwareIctInfrastructure = ("Software Ict Infrastructure", 5800);
    public static readonly (string Name, int Value) ProfessionalFees = ("Professional Fees", 5850); // Legal, accounting, compliance consulting
    public static readonly (string Name, int Value) GeneralRepairsAndMaintenance = ("General Repairs And Maintenance", 5900);
    public static readonly (string Name, int Value) BankChargesAndInterestPaid = ("Bank Charges And Interest Paid", 6000);
}

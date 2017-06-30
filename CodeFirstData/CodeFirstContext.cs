using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CodeFirstEntities;
using System.Web;

namespace CodeFirstData
{
    public class CodeFirstContext : DbContext
    {
        public static string DatabaseName
        {
            get { return ((dynamic)HttpContext.Current.ApplicationInstance).DynamicDatabase; }
            set { ((dynamic)HttpContext.Current.ApplicationInstance).DynamicDatabase = value; }
        }
        private static string CompanyName
        {
            get { return (string)HttpContext.Current.Session["CompanyName"]; }
        }
        private static string FinancialYear
        {
            get { return (string)HttpContext.Current.Session["FinancialYear"]; }
        }
        public CodeFirstContext()
            : base(SetConnectionString())
        { }

        public static string SetConnectionString()
        {
            if (CompanyName == "A2ZRetail" && FinancialYear == "A2ZRetail")
            {
                DatabaseName = "A2ZRetail";
            }
            if (CompanyName == "A2ZRetail" && FinancialYear == "A2ZRetail")
            {
                DatabaseName = "RetailManagement1";
            }

            else if (CompanyName == "A To Z DADAR T.T." && FinancialYear == "01-04-2015 To 31-03-2016")
            {
                DatabaseName = "RetailManagement1";
            }
            else if (DatabaseName == "")
                DatabaseName = CompanyName + " " + FinancialYear;
            //return ("Data Source=180.149.243.22;User ID=sa;Password=hfyrh$5f6$#D4;Initial Catalog=" + DatabaseName + ";persist security info=True;MultipleActiveResultSets=true;App=EntityFramework");
            //return ("Data Source=LENOVO-PC\\SQLEXPRESS;Initial Catalog="+ DatabaseName +";Integrated Security=True");
            return ("Data Source=DEVELOPER-PC\\SQLEXPRESS;Initial Catalog=" + DatabaseName + ";Integrated Security=True");
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<BankName> BankNames { get; set; }
        public DbSet<BankMaster> BankMasters { get; set; }
        public DbSet<ClientMaster> ClientMasters { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<MonthExperience> MonthExperiences { get; set; }
        public DbSet<YearExperience> YearExperiences { get; set; }
        public DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public DbSet<TypeOfSupplier> TypeOfSuppliers { get; set; }

        public DbSet<GenerateItemCode> GenerateItemCodes { get; set; }
        public DbSet<LabMaster> labMasters { get; set; }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UnitMaster> UnitMasters { get; set; }
        public DbSet<BrandMaster> BrandMasters { get; set; }
        public DbSet<SupplierMaster> SupplierMasters { get; set; }

        public DbSet<DesignMaster> DesignMasters { get; set; }
        public DbSet<OpeningStockMaster> OpeningStockMasters { get; set; }

        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }
        //public DbSet<Barcode> Barcode { get; set; }
        //public DbSet<logindetail> logindetails { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemSubCategory> ItemSubCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemName> ItemNames { get; set; }
        public DbSet<itemTempDetail> itemTempDetails { get; set; }
        public DbSet<CostCodeCreation> CostCodeCreations { get; set; }
        public DbSet<ColorCode> ColorCodes { get; set; }
        public DbSet<TypeOfMaterial> TypeOfMaterials { get; set; }
        public DbSet<PurchaseItemDetail> PurchaseItemDetails { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<SupplierBankDetail> SupplierBankDetails { get; set; }
        public DbSet<ClientBankDetail> ClientBankDetails { get; set; }
        public DbSet<TransportMaster> TransportMasters { get; set; }
        public DbSet<TransportBankDetail> TransportBankDetails { get; set; }
        public DbSet<GodownMaster> GodownMasters { get; set; }
        public DbSet<GodownAddress> GodownAddresses { get; set; }
        public DbSet<SalesIncentiveMaster> SalesIncentiveMasters { get; set; }
        public DbSet<DesignationMaster> DesignationMasters { get; set; }
        public DbSet<companybankdetail> companybankdetails { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<InwardFromSupplier> InwardFromSuppliers { get; set; }
        public DbSet<InwardFromGodown> InwardFromGodowns { get; set; }
        public DbSet<InwardItemFromGodown> InwardItemFromGodowns { get; set; }
        public DbSet<InwardItemsFromSupplier> InwardItemFromSuppliers { get; set; }
        public DbSet<BarcodeNumber> BarcodeNumbers { get; set; }
        public DbSet<BarcodeTempDetail> BarcodeTempDetails { get; set; }
        public DbSet<ItemTruncate> ItemTruncate { get; set; }
        public DbSet<ListOfItemCode> ListOfItemCode { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<EntryStockMaster> EntryStockMasters { get; set; }
        public DbSet<EntryStockItem> EntryStockItems { get; set; }
        public DbSet<PurchaseItemTaxMaster> PurchaseItemTaxMasters { get; set; }
        public DbSet<ItemTaxMaster> ItemTaxMasters { get; set; }
        public DbSet<ShopRequisitionGodown> ShopRequisitionGodowns { get; set; }
        public DbSet<ShopRequisitionGodownItem> ShopRequisitionGodownItems { get; set; }

        public DbSet<ShopRequisitionGodownsalesbill> ShopRequisitionGodownsalesbills { get; set; }
        public DbSet<ShopRequisitionGodownitemsalesbill> ShopRequisitionGodownitemsalesbills { get; set; }

        public DbSet<OutwardStockDistribution> OutwardStockDistributions { get; set; }
        public DbSet<OutwardItemStockDistribution> OutwardItemStockDistributions { get; set; }
        public DbSet<OutwardToShop> OutwardToShops { get; set; }
        public DbSet<OutwardItemToShop> OutwardItemGodownToShops { get; set; }
        public DbSet<OutwardShopToGodown> OutwardShopToGodowns { get; set; }
        public DbSet<OutwardItemShopToGodown> OutwardItemShopToGodowns { get; set; }
        public DbSet<OutwardToClient> OutwardToClients { get; set; }
        public DbSet<OutwardItemToClient> OutwardItemToClients { get; set; }
        public DbSet<OutwardInterGodown> OutwardInterGodowns { get; set; }
        public DbSet<OutwardItemInterGodown> OutwardItemInterGodowns { get; set; }

        public DbSet<MainTaxMaster> MainTaxMasters { get; set; }
        public DbSet<SubTaxMaster> SubTaxMaster { get; set; }
        public DbSet<RetailBill> RetailBills { get; set; }
        public DbSet<RetailBillItem> RetailBillItems { get; set; }
        public DbSet<RetailBillAdjAmtDetail> RetailBillAdjAmtDetails { get; set; }

        public DbSet<TemporaryCashMemo> TemporaryCashMemos { get; set; }
        public DbSet<TemporaryCashMemoItem> TemporaryCashMemoItems { get; set; }
        public DbSet<TemporaryCashMemoAdjAmtDetail> TemporaryCashMemoAdjAmtDetails { get; set; }

        public DbSet<CustomerDetail> CustomerDetails { get; set; }

        public DbSet<ShopMaster> ShopMasters { get; set; }
        public DbSet<StockDistribution> StockDistributions { get; set; }
        public DbSet<StockItemDistribution> StockItemDistributions { get; set; }
        public DbSet<ShopStock> ShopStocks { get; set; }
        public DbSet<GodownStock> GodownStocks { get; set; }

        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationItem> QuotationItems { get; set; }
        public DbSet<InventoryTax> InventoryTaxes { get; set; }
        public DbSet<PurchaseInventoryTax> PurchaseInventoryTaxes { get; set; }
        public DbSet<DeliveryChallan> DeliveryChallans { get; set; }
        public DbSet<DeliveryChallanItem> DeliveryChallanItems { get; set; }

        public DbSet<CashHandover> CashHandovers { get; set; }
        public DbSet<CardChequeHandover> CardChequeHandovers { get; set; }
        public DbSet<BalanceCarryForward> BalanceCarryForwards { get; set; }

        public DbSet<CashierReceivable> CashierReceivables { get; set; }
        public DbSet<CashierSalesOrder> CashierSalesOrders { get; set; }
        public DbSet<CashierSalesOrderItem> CashierSalesOrderItems { get; set; }
        public DbSet<CashierRetailBill> CashierRetailBills { get; set; }
        public DbSet<CashierRetailBillItem> CashierRetailBillItems { get; set; }
        public DbSet<CashierSalesBill> CashierSalesBills { get; set; }
        public DbSet<CashierSalesBillItem> CashierSalesBillItems { get; set; }
        public DbSet<CashierTemporaryCashMemo> CashierTemporaryCashMemoes { get; set; }
        public DbSet<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoItems { get; set; }

        public DbSet<GetCashRowsBySO> GetCashRowsBySO { get; set; }

        public DbSet<CashierPayable> CashierPayables { get; set; }
        public DbSet<CashierRefundOrder> CashierRefundOrders { get; set; }
        public DbSet<CashierRefundOrderItem> CashierRefundOrderItems { get; set; }
        public DbSet<BillPayment> BillPayments { get; set; }
        public DbSet<BillPaymentItem> BillPaymentItems { get; set; }
        public DbSet<GetPOListByPaymentStatusAndSupplier> GetPOListByPaymentStatusAndSupplier { get; set; }

        public DbSet<SalesBill> SalesBills { get; set; }
        public DbSet<SalesBillItem> SalesBillItems { get; set; }
        public DbSet<SalesBillAdjAmtDetail> SalesBillAdjAmtDetails { get; set; }

        public DbSet<InwardFromStockDistribution> InwardFromStockDistributions { get; set; }
        public DbSet<InwardItemFromStockDistribution> InwardItemFromStockDistributions { get; set; }

        public DbSet<CreateDynamicDb> CreateDynamicDb { get; set; }
        public DbSet<BackUpDatabase> BackUpDatabase { get; set; }
        public DbSet<UpdateDynamicDb> UpdateDynamicDb { get; set; }
        public DbSet<ValidationCompany> ValidationCompany { get; set; }
        public DbSet<EmployeesCompany> EmployeesCompanies { get; set; }
        public DbSet<NonInventoryItem> NonInventoryItems { get; set; }
        public DbSet<GetUsersEmail> GetUsersEmail { get; set; }
        public DbSet<GetAllItemsFromStkDistrbtn> GetAllItemsFromStkDistrbtn { get; set; }
        public DbSet<GetItemCodesByQuotAndItemCode> GetItemCodesByQuotAndItemCode { get; set; }
        public DbSet<GetItemsByQuotOrOrderNo> GetItemsByQuotOrOrderNo { get; set; }

        public DbSet<RequisitionForShop> RequisitionForShops { get; set; }
        public DbSet<RequisitionForGodown> RequisitionForGodowns { get; set; }
        public DbSet<RequisitionofNewItemsForShop> RequisitionofNewItemsForShops { get; set; }
        public DbSet<RequisitionOfPo> RequisitionOfPoes { get; set; }

        public DbSet<InwardFromShopToGodown> InwardFromShopToGodowns { get; set; }
        public DbSet<InwardItemFromShopToGodown> InwardItemFromShopToGodowns { get; set; }

        public DbSet<OutwardInterShop> OutwardInterShops { get; set; }
        public DbSet<OutwardItemInterShop> OutwardItemInterShops { get; set; }

        public DbSet<SalesReturn> SalesReturns { get; set; }
        public DbSet<SalesReturnItem> SalesReturnItems { get; set; }

        public DbSet<CommissionMaster> CommissionMasters { get; set; }
        public DbSet<CommissionProduct> CommissionProducts { get; set; }

        public DbSet<JobWorkItem> JobWorkerItems { get; set; }
        public DbSet<JobWorkType> JobWorkTypes { get; set; }
        public DbSet<JobWorker> JobWorkers { get; set; }
        public DbSet<JobWorkPayment> JobWorkPayments { get; set; }
        public DbSet<JobWorkStock> JobWorkStocks { get; set; }
        public DbSet<OutwardToTailor> OutwardToTailors { get; set; }
        public DbSet<OutwardToTailorItem> OutwardToTailorItems { get; set; }
        public DbSet<InwardFromTailor> InwardFromTailors { get; set; }
        public DbSet<InwardFromTailorItem> InwardFromTailorItems { get; set; }
        public DbSet<InwardToTailor> InwardToTailors { get; set; }
        public DbSet<InwardToTailorItem> InwardToTailorItems { get; set; }
        public DbSet<JobWorkOutwardToClient> JobWorkOutwardToClients { get; set; }

        public DbSet<InwardInterGodown> InwardInterGodowns { get; set; }
        public DbSet<InwardInterGodownItem> InwardInterGodownItems { get; set; }

        public DbSet<InwardInterShop> InwardInterShops { get; set; }
        public DbSet<InwardInterShopItem> InwardInterShopItems { get; set; }

        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnItem> PurchaseReturnItems { get; set; }

        public DbSet<IncomeExpenseVoucher> IncomeExchangeVouchers { get; set; }
       
        public DbSet<RetailBillCreditNote> CreditNotes { get; set; }
        public DbSet<RetailBillCreditNoteItem> CreditNoteItems { get; set; }

        public DbSet<SalesBillCreditNote> SalesBillCreditNotes { get; set; }
        public DbSet<SalesBillCreditNoteItem> SalesBillCreditNoteItems { get; set; }

        public DbSet<PhysicalStockTaking> PhysicalStockTakings { get; set; }

        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteItem> DebitNoteItems { get; set; }

        public DbSet<TruncateTable> TruncateTable { get; set; }
        public DbSet<DiscountMaster> DiscountMasters { get; set; }
        public DbSet<DiscountMasterItem> DiscountMasterItems { get; set; }

        public DbSet<ResetRetailBill> ResetRetailBills { get; set; }
        public DbSet<ResetRetailBillItem> ResetRetailBillItems { get; set; }
        public DbSet<ResetRetailBillAdjAmtDetail> ResetRetailBillAdjAmtDetails { get; set; }

        public DbSet<ResetSalesBill> ResetSalesBills { get; set; }
        public DbSet<ResetSalesBillItem> ResetSalesBillItems { get; set; }
        public DbSet<ResetSalesBillAdjAmtDetail> ResetSalesBillAdjAmtDetails { get; set; }

        public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<Catalogue> Catalogues { get; set; }

        public DbSet<UniversalBarcodeNumber> UniversalBarcodeNumbers { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}

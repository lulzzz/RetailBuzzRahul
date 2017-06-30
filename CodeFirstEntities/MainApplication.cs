using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstEntities
{
    [Serializable]
    public class MainApplication
    {
        [Key]
        public int mainApp { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string FinancialYear { get; set; }

        public EmployeeMaster EmployeeDetails { get; set; }
        public IEnumerable<EmployeeMaster> EmpList { get; set; }
        public IEnumerable<BloodGroup> BloodGroups { get; set; }
        public IEnumerable<YearExperience> totalExpYears { get; set; }
        public IEnumerable<MonthExperience> totalExpmonths { get; set; }
        public IEnumerable<TypeOfSupplier> TypeOfSupplierList { get; set; }

        public Company companydetails { get; set; }
        public IEnumerable<Company> CmpList { get; set; }
        public companybankdetail compbankdetails { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<companybankdetail> CompanyBankList { get; set; }

        public IEnumerable<Item> ItemList { get; set; }
        public Item ItemDetails { get; set; }
        public IEnumerable<ItemName> ItemNameList { get; set; }
        public ItemName ItemNameDetails { get; set; }
        public IEnumerable<City> CityList { get; set; }
        public string itemCount { get; set; }
        public State statedetails { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public ItemCategory ItemCategoryDetails { get; set; }
        public IEnumerable<ItemCategory> ItemCategoryList { get; set; }
        public string itemcategoryname { get; set; }
        public ItemSubCategory ItemSubCategoryDetails { get; set; }
        public IEnumerable<ItemSubCategory> ItemSubCategoryList { get; set; }
        public IEnumerable<CostCodeCreation> CostCodeCreationList { get; set; }
        public CostCodeCreation CostCodeCreationDetails { get; set; }
        public BrandMaster BrandMasterDetails { get; set; }
        public IEnumerable<BrandMaster> BrandMasterList { get; set; }
        public SupplierMaster SupplierDetails { get; set; }
        public IEnumerable<SupplierMaster> SupplierList { get; set; }
        public SupplierBankDetail SupplierBankDetails { get; set; }
        public IEnumerable<SupplierBankDetail> SupplierBankDetailList { get; set; }
        public string suppliercode { get; set; }
        public string typeOfSuppilerData { get; set; }

        public itemTempDetail itemTempDetailData { get; set; }
        public IEnumerable<itemTempDetail> itemTempDetailList { get; set; }
        public GodownMaster GodownDetails { get; set; }
        public IEnumerable<GodownMaster> GodownMasterList { get; set; }
        public GodownAddress GodownAddressDetails { get; set; }
        public IEnumerable<GodownAddress> GodownAddressList { get; set; }
        public IEnumerable<TypeOfMaterial> typeOfMaterialList { get; set; }
        public ColorCode ColorCodeDetails { get; set; }
        public IEnumerable<ColorCode> ColorCodeList { get; set; }
        public TypeOfMaterial TypeOfMaterialData { get; set; }

        public PurchaseOrderDetail PurchaseOrderData { get; set; }
        public IEnumerable<PurchaseOrderDetail> PurchaseOrderList { get; set; }
        public PurchaseItemDetail PurchaseItemData { get; set; }
        public string PoCode { get; set; }
        public string Subcat { get; set; }
        public IEnumerable<PurchaseItemDetail> PurchaseItemList { get; set; }

        public ClientMaster ClientDetails { get; set; }
        public IEnumerable<ClientMaster> ClientList { get; set; }
        public ClientBankDetail ClientBankDetails { get; set; }
        public IEnumerable<ClientBankDetail> ClientBankDetailList { get; set; }
        public string membershipCardType { get; set; }
        public string clientcode { get; set; }

        public UnitMaster UnitDetails { get; set; }
        public IEnumerable<UnitMaster> UnitList { get; set; }

        public TransportMaster TransportDetails { get; set; }
        public IEnumerable<TransportMaster> TransportList { get; set; }
        public TransportBankDetail TransportBankDetails { get; set; }
        public IEnumerable<TransportBankDetail> TransportBankDetailList { get; set; }
        public string transportcode { get; set; }
        public string typeOfTransportData { get; set; }

        public SalesIncentiveMaster SalesIncentiveDetails { get; set; }
        public IEnumerable<SalesIncentiveMaster> SalesIncentiveList { get; set; }

        public BankMaster BankDetails { get; set; }
        public IEnumerable<BankMaster> BankList { get; set; }

        public BankName BankNameDetails { get; set; }
        public IEnumerable<BankName> BankNameList { get; set; }

        public Department DepartmentDetails { get; set; }
        public IEnumerable<Department> DepartmentList { get; set; }

        public DesignationMaster DesignationDetails { get; set; }
        public IEnumerable<DesignationMaster> DesignationList { get; set; }

        public IEnumerable<DesignMaster> DesignList { get; set; }
        public DesignMaster DesignDetails { get; set; }

        public IEnumerable<TypeOfMaterial> MaterialList { get; set; }
        public TypeOfMaterial MaterialDetails { get; set; }

        public MainTaxMaster MainTaxMasters { get; set; }
        public SubTaxMaster SubTaxMasters { get; set; }
        public IEnumerable<SubTaxMaster> DynamicList { get; set; }
        public int stateid { get; set; }

        public InwardFromSupplier InwardFromSupplier { get; set; }
        public string InwardCode { get; set; }

        public OpeningStockMaster OpeningStockDetails { get; set; }
        public IEnumerable<OpeningStockMaster> OpeningStockList { get; set; }

        public User user { get; set; }

        public IEnumerable<Module> modulelist { get; set; }
        public Module module { get; set; }
        public UserCredential usercredential { get; set; }

        public IEnumerable<UserCredential> userCredentialList { get; set; }
        public IEnumerable<UserCredential> UpdatedUserCredentailList { get; set; }

        public EntryStockMaster EntryStockDetails { get; set; }
        public IEnumerable<EntryStockMaster> EntryStockList { get; set; }
        public EntryStockItem EntryStockItemDetails { get; set; }
        public IEnumerable<EntryStockItem> EntryStockItemList { get; set; }

        public InwardFromSupplier InwardFromSupplierDetails { get; set; }
        public IEnumerable<InwardFromSupplier> InwardFromSupplierList { get; set; }
        public InwardItemsFromSupplier InwardItemsFromSupplierDetails { get; set; }
        public IEnumerable<InwardItemsFromSupplier> InwardItemsFromSupplierList { get; set; }
        public string PerformaCode { get; set; }

        public InwardFromGodown InwardFromGodownDetails { get; set; }
        public IEnumerable<InwardFromGodown> InwardFromGodownList { get; set; }
        public InwardItemFromGodown InwardItemFromGodownDetails { get; set; }
        public IEnumerable<InwardItemFromGodown> InwardItemFromGodownlist { get; set; }

        public ItemTaxMaster ItemTaxDetails { get; set; }
        public IEnumerable<ItemTaxMaster> ItemTaxList { get; set; }

        public PurchaseItemTaxMaster PurchaseItemTaxDetails { get; set; }
        public IEnumerable<PurchaseItemTaxMaster> PurchaseItemTaxList { get; set; }

        public OutwardToShop OutwardToShopDetails { get; set; }
        public IEnumerable<OutwardToShop> OutwardToShopList { get; set; }

        public OutwardItemToShop OutwardItemToShopDetails { get; set; }
        public IEnumerable<OutwardItemToShop> OutwardItemToShopList { get; set; }

        public OutwardInterGodown OutwardInterGodownDetails { get; set; }
        public IEnumerable<OutwardInterGodown> OutwardInterGodownList { get; set; }

        public OutwardItemInterGodown OutwardItemInterGodownDetails { get; set; }
        public IEnumerable<OutwardItemInterGodown> OutwardItemInterGodownList { get; set; }

        public ShopMaster ShopDetails { get; set; }
        public IEnumerable<ShopMaster> ShopList { get; set; }

        public CustomerDetail CustomerDetails { get; set; }
        public IEnumerable<CustomerDetail> CustomerList { get; set; }
        public RetailBillItem RetailBillItemDetails { get; set; }
        public IEnumerable<RetailBillItem> RetailBillItemList { get; set; }
        public RetailBill RetailBillDetails { get; set; }
        public IEnumerable<RetailBill> RetailBillList { get; set; }
        public RetailBillAdjAmtDetail RetailBillAdjAmtDetails { get; set; }
        public IEnumerable<RetailBillAdjAmtDetail> RetailBillAdjAmtDetailList { get; set; }

        public TemporaryCashMemo TemporaryCashMemoDetails { get; set; }
        public IEnumerable<TemporaryCashMemo> TemporaryCashMemoList { get; set; }
        public TemporaryCashMemoItem TemporaryCashMemoItemDetails { get; set; }
        public IEnumerable<TemporaryCashMemoItem> TemporaryCashMemoItemList { get; set; }
        public TemporaryCashMemoAdjAmtDetail TemporaryCashMemoAdjAmtDetails { get; set; }
        public IEnumerable<TemporaryCashMemoAdjAmtDetail> TemporaryCashMemoAdjAmtDetailList { get; set; }

        public IEnumerable<InwardFromSupplier> Performalist { get; set; }

        public StockDistribution StockDistributionDetails { get; set; }
        public IEnumerable<StockDistribution> StockDistributionList { get; set; }

        public StockItemDistribution StockItemDistributionDetails { get; set; }
        public IEnumerable<StockItemDistribution> StockItemDistributionList { get; set; }

        public ShopStock ShopStockDetails { get; set; }
        public IEnumerable<ShopStock> ShopStockList { get; set; }

        public GodownStock GodownStockDetails { get; set; }
        public IEnumerable<GodownStock> GodownStockList { get; set; }

        public OutwardToClient OutwardToClientDetails { get; set; }
        public IEnumerable<OutwardToClient> OutwardToClientList { get; set; }

        public OutwardItemToClient OutwardItemToClientDetails { get; set; }
        public IEnumerable<OutwardItemToClient> OutwardItemToClientList { get; set; }

        public SalesOrder SalesOrderDetails { get; set; }
        public IEnumerable<SalesOrder> SalesOrderList { get; set; }
        public SalesOrderItem SalesOrderItemDetails { get; set; }
        public IEnumerable<SalesOrderItem> SalesOrderItemList { get; set; }

        public Quotation QuotationDetails { get; set; }
        public IEnumerable<Quotation> QuotationList { get; set; }
        public QuotationItem QuotationItemDetails { get; set; }
        public IEnumerable<QuotationItem> QuotationItemList { get; set; }

        public InventoryTax InventoryTaxDetails { get; set; }
        public IEnumerable<InventoryTax> InventoryTaxList { get; set; }

        public PurchaseInventoryTax PurchaseInventoryTaxDetails { get; set; }
        public IEnumerable<PurchaseInventoryTax> PurchaseInventoryTaxList { get; set; }

        public OutwardStockDistribution OutwardStkDisDetails { get; set; }
        public IEnumerable<OutwardStockDistribution> OutwardStkDisList { get; set; }

        public OutwardItemStockDistribution OutwardItemStkDisDetails { get; set; }
        public IEnumerable<OutwardItemStockDistribution> OutwardItemStkDisList { get; set; }

        public OutwardShopToGodown OutwardShopToGodownDetails { get; set; }
        public IEnumerable<OutwardShopToGodown> OutwardShopToGodownList { get; set; }

        public OutwardItemShopToGodown OutwardItemShopToGodownDetails { get; set; }
        public IEnumerable<OutwardItemShopToGodown> OutwardItemShopToGodownList { get; set; }

        public string CashierCode { get; set; }
        public string RefundAmount { get; set; }
        public CashierReceivable CashierReceivableDetails { get; set; }
        public IEnumerable<CashierReceivable> CashierReceivableList { get; set; }
        public CashierSalesOrder CashierSalesOrderDetails { get; set; }
        public IEnumerable<CashierSalesOrder> CashierSalesOrderList { get; set; }
        public CashierSalesOrderItem CashierSalesOrderItemDetails { get; set; }
        public IEnumerable<CashierSalesOrderItem> CashierSalesOrderItemList { get; set; }
        public CashierSalesBill CashierSalesBillDetails { get; set; }
        public IEnumerable<CashierSalesBill> CashierSalesBillList { get; set; }
        public CashierSalesBillItem CashierSalesBillItemDetails { get; set; }
        public IEnumerable<CashierSalesBillItem> CashierSalesBillItemList { get; set; }
        public CashierRetailBill CashierRetailBillDetails { get; set; }
        public IEnumerable<CashierRetailBill> CashierRetailBillList { get; set; }
        public CashierRetailBillItem CashierRetailBillItemDetails { get; set; }
        public IEnumerable<CashierRetailBillItem> CashierRetailBillItemList { get; set; }
        public CashierTemporaryCashMemo CashierTemporaryCashMemoDetails { get; set; }
        public IEnumerable<CashierTemporaryCashMemo> CashierTemporaryCashMemoList { get; set; }
        public CashierTemporaryCashMemoItem CashierTemporaryCashMemoItemDetails { get; set; }
        public IEnumerable<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoItemItemList { get; set; }

        public CashierPayable CashierPayableDetails { get; set; }
        public CashierRefundOrder CashierRefundOrderDetails { get; set; }
        public IEnumerable<CashierRefundOrder> CashierRefundOrderList { get; set; }
        public CashierRefundOrderItem CashierRefundOrderItemDetails { get; set; }
        public IEnumerable<CashierRefundOrderItem> CashierRefundOrderItemList { get; set; }
        public BillPayment BillPaymentDetails { get; set; }
        public IEnumerable<BillPayment> BillPaymentList { get; set; }
        public BillPaymentItem BillPaymentItemDetails { get; set; }
        public IEnumerable<BillPaymentItem> BillPaymentItemList { get; set; }

        public CashHandover CashHandoverDetails { get; set; }
        public IEnumerable<CashHandover> CashHandoverList { get; set; }
        public CardChequeHandover CardChequeHandoverDetails { get; set; }
        public IEnumerable<CardChequeHandover> CardChequeHandoverList { get; set; }
        public BalanceCarryForward BalanceCarryForwardDetails { get; set; }
        public IEnumerable<BalanceCarryForward> BalanceCarryForwardList { get; set; }


        public string ChallanCode { get; set; }
        public DeliveryChallan DeliveryChallanDetails { get; set; }
        public IEnumerable<DeliveryChallan> DeliveryChallanList { get; set; }
        public DeliveryChallanItem DeliveryChallanItemDetails { get; set; }
        public IEnumerable<DeliveryChallanItem> DeliveryChallanItemList { get; set; }

        public string SalesBillCode { get; set; }
        public SalesBill SalesBillDetails { get; set; }
        public IEnumerable<SalesBill> SalesBillList { get; set; }
        public SalesBillItem SalesBillItemDetails { get; set; }
        public IEnumerable<SalesBillItem> SalesBillItemList { get; set; }
        public SalesBillAdjAmtDetail SalesBillAdjAmtDetails { get; set; }
        public IEnumerable<SalesBillAdjAmtDetail> SalesBillAdjAmtDetailList { get; set; }

        public InwardFromStockDistribution InwardFromStockDistributionDetails { get; set; }
        public IEnumerable<InwardFromStockDistribution> InwardFromStockDistributionList { get; set; }
        public InwardItemFromStockDistribution InwardItemFromStockDistributionDetails { get; set; }
        public IEnumerable<InwardItemFromStockDistribution> InwardItemFromStockDistributionList { get; set; }

        public IEnumerable<EmployeesCompany> EmployeeCompanyList { get; set; }

        public NonInventoryItem NonInventoryItemDetails { get; set; }
        public IEnumerable<NonInventoryItem> NonInventoryItemList { get; set; }


        public ShopRequisitionGodown ShopRequisitionGodown { get; set; }
        public IEnumerable<ShopRequisitionGodown> ShopRequisitionGodownList { get; set; }
        public string REcode { get; set; }

        public ShopRequisitionGodownItem ShopRequisitionGodownItemDetails { get; set; }

        public IEnumerable<ShopRequisitionGodownItem> ShopRequisitionGodownItemList { get; set; }
        public IEnumerable<ShopRequisitionGodownItem> ShopRequisitionGodownItemListForNull { get; set; }
        public IEnumerable<ShopRequisitionGodownItem> ShopRequisitionGodownItemListForSession { get; set; }

        public ShopRequisitionGodownsalesbill ShopRequisitionGodownsalesbillDetails { get; set; }
        public ShopRequisitionGodownitemsalesbill ShopRequisitionGodownitemsalesbillDetails { get; set; }

        public IEnumerable<ShopRequisitionGodownsalesbill> ShopRequisitionGodownsalesbillList { get; set; }

        public IEnumerable<ShopRequisitionGodownitemsalesbill> ShopRequisitionGodownitemsalesbillList { get; set; }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> ShopRequisitionGodownitemsalesbillListForNull { get; set; }
        public IEnumerable<ShopRequisitionGodownitemsalesbill> ShopRequisitionGodownitemsalesbillListForSession { get; set; }

        public RequisitionForShop RequisitionForShopDetails { get; set; }
        public RequisitionForGodown RequisitionForGodownDetails { get; set; }
        public IEnumerable<RequisitionForShop> RequisitionForShopList { get; set; }
        public IEnumerable<RequisitionForShop> RequisitionForShopListForNull { get; set; }
        public IEnumerable<RequisitionForGodown> Requisitionforgodownlist { get; set; }
        public IEnumerable<RequisitionForGodown> RequisitionforgodownlistNull { get; set; }

        //  public IEnumerable<ShopRequisitionGodownsalesbill> ShopRequisitionGodownsalesbillList { get; set; }
        public RequisitionofNewItemsForShop RequisitionofNewItemsForShopDetails { get; set; }
        public IEnumerable<RequisitionofNewItemsForShop> RequisitionofNewItemsForShopenum { get; set; }
        public IEnumerable<GetItemsFromItemMaster> GetItemsFromItemMasterEnum { get; set; }

        public InwardFromShopToGodown InwardFromShopToGodownDetails { get; set; }
        public IEnumerable<InwardFromShopToGodown> InwardFromShopToGodownList { get; set; }

        public InwardItemFromShopToGodown InwardItemFromShopToGodownDetails { get; set; }
        public IEnumerable<InwardItemFromShopToGodown> InwardItemFromShopToGodownList { get; set; }

        public RequisitionOfPo RequisitionOfPoDetails { get; set; }

        public OutwardInterShop OutwardInterShopDetails { get; set; }
        public IEnumerable<OutwardInterShop> OutwardInterShopList { get; set; }
        public OutwardItemInterShop OutwardItemInterShopdetails { get; set; }
        public IEnumerable<OutwardItemInterShop> OutwardItemInterShopList { get; set; }

        public SalesReturn SalesReturnDetails { get; set; }
        public IEnumerable<SalesReturn> SalesReturnList { get; set; }
        public string SalesReturnNo { get; set; }

        public SalesReturnItem SalesReturnItemDetails { get; set; }
        public IEnumerable<SalesReturnItem> SalesReturnItemList { get; set; }

        public CommissionMaster CommissionMasterDetails { get; set; }
        public IEnumerable<CommissionMaster> CommissionMasterList { get; set; }

        public BarcodeNumber BarcodeDetails { get; set; }
        public IEnumerable<BarcodeNumber> BarcodeList { get; set; }

        public CommissionProduct CommissionProductDetails { get; set; }
        public IEnumerable<CommissionProduct> CommissionProductList { get; set; }

        public JobWorkItem JobWorkItemDetails { get; set; }
        public IEnumerable<JobWorkItem> JobWorkItemList { get; set; }

        public JobWorkType JobWorkTypeDetails { get; set; }
        public IEnumerable<JobWorkType> JobWorkTypeList { get; set; }

        public JobWorker JobWorkerDetails { get; set; }
        public IEnumerable<JobWorker> JobWorkerList { get; set; }

        public JobWorkPayment JobWorkPaymentDetails { get; set; }
        public IEnumerable<JobWorkPayment> JobWorkPaymentList { get; set; }

        public JobWorkStock JobWorkStockDetails { get; set; }
        public IEnumerable<JobWorkStock> JobWorkStockList { get; set; }

        public JobWorkOutwardToClient JobWorkOutwardToClientDetails { get; set; }
        public IEnumerable<JobWorkOutwardToClient> JobWorkOutwardToClientList { get; set; }

        public InwardInterGodown InwardInterGodownDetails { get; set; }
        public IEnumerable<InwardInterGodown> InwardInterGodownList { get; set; }

        public InwardInterGodownItem InwardInterGodownItemDetails { get; set; }
        public IEnumerable<InwardInterGodownItem> InwardInterGodownItemList { get; set; }

        public InwardInterShop InwardInterShopDetails { get; set; }
        public IEnumerable<InwardInterShop> InwardInterShopList { get; set; }

        public InwardInterShopItem InwardInterShopItemDetails { get; set; }
        public IEnumerable<InwardInterShopItem> InwardInterShopItemList { get; set; }

        public PurchaseReturn PurchaseReturnDetails { get; set; }
        public IEnumerable<PurchaseReturn> PurchaseReturnList { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string DebitNoteNo { get; set; }

        public PurchaseReturnItem PurchaseReturnItemDetails { get; set; }
        public IEnumerable<PurchaseReturnItem> PurchaseReturnItemList { get; set; }

        public OutwardToTailor OutwardToTailorDetails { get; set; }
        public IEnumerable<OutwardToTailor> OutwardToTailorList { get; set; }

        public OutwardToTailorItem OutwardToTailorItemDetails { get; set; }
        public IEnumerable<OutwardToTailorItem> OutwardToTailorItemList { get; set; }

        public InwardFromTailor InwardFromTailorDetails { get; set; }
        public IEnumerable<InwardFromTailor> InwardFromTailorList { get; set; }
        public InwardFromTailorItem InwardFromTailorItemDetails { get; set; }
        public IEnumerable<InwardFromTailorItem> InwardFromTailorItemList { get; set; }

        public InwardToTailor InwardToTailorDetails { get; set; }
        public IEnumerable<InwardToTailor> InwardToTailorList { get; set; }
        public InwardToTailorItem InwardToTailorItemDetails { get; set; }
        public IEnumerable<InwardToTailorItem> InwardToTailorItemList { get; set; }

        public IncomeExpenseVoucher IncomeExpenseVoucherDetails { get; set; }
        public IEnumerable<IncomeExpenseVoucher> IncomeExpenseVoucherList { get; set; }

        public PhysicalStockTaking PhysicalStockTakingDetails { get; set; }
        public IEnumerable<PhysicalStockTaking> PhysicalStockTakingList { get; set; }

        public RetailBillCreditNote RetailBillCreditNoteDetails { get; set; }
        public IEnumerable<RetailBillCreditNote> RetailBillCreditNoteList { get; set; }
        public RetailBillCreditNoteItem RetailBillCreditNoteItemDetails { get; set; }
        public IEnumerable<RetailBillCreditNoteItem> RetailBillCreditNoteItemList { get; set; }

        public SalesBillCreditNote SalesBillCreditNoteDetails { get; set; }
        public IEnumerable<SalesBillCreditNote> SalesBillCreditNoteList { get; set; }
        public SalesBillCreditNoteItem SalesBillCreditNoteItemDetails { get; set; }
        public IEnumerable<SalesBillCreditNoteItem> SalesBillCreditNoteItemList { get; set; }

        public DebitNote DebitNoteDetails { get; set; }
        public IEnumerable<DebitNote> DebitNoteList { get; set; }
        public DebitNoteItem DebitNoteItemDetails { get; set; }
        public IEnumerable<DebitNoteItem> DebitNoteItemList { get; set; }

        public DiscountMaster DiscountMasterDetails { get; set; }
        public IEnumerable<DiscountMaster> DiscountMasterList { get; set; }
        public DiscountMasterItem DiscountMasterItemDetails { get; set; }
        public IEnumerable<DiscountMasterItem> DiscountMasterItemList { get; set; }

        public ResetRetailBillItem ResetRetailBillItemDetails { get; set; }
        public IEnumerable<ResetRetailBillItem> ResetRetailBillItemList { get; set; }
        public ResetRetailBill ResetRetailBillDetails { get; set; }
        public IEnumerable<ResetRetailBill> ResetRetailBillList { get; set; }
        public ResetRetailBillAdjAmtDetail ResetRetailBillAdjAmtDetails { get; set; }
        public IEnumerable<ResetRetailBillAdjAmtDetail> ResetRetailBillAdjAmtDetailList { get; set; }

        public ResetSalesBill ResetSalesBillDetails { get; set; }
        public IEnumerable<ResetSalesBill> ResetSalesBillList { get; set; }
        public ResetSalesBillItem ResetSalesBillItemDetails { get; set; }
        public IEnumerable<ResetSalesBillItem> ResetSalesBillItemList { get; set; }
        public ResetSalesBillAdjAmtDetail ResetSalesBillAdjAmtDetails { get; set; }
        public IEnumerable<ResetSalesBillAdjAmtDetail> ResetSalesBillAdjAmtDetailList { get; set; }

        public Catalogue CatalogueDetails { get; set; }
        public IEnumerable<Catalogue> CatalogueList { get; set; }

        public UniversalBarcodeNumber UniversalBarcodeNumberDetails { get; set; }
        public IEnumerable<UniversalBarcodeNumber> UniversalBarcodeNumberList { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MvcRetailApp.IoC;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;

namespace MvcRetailApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static int LoginTimes;
        public static string CompanyCode;
        public static string CompanyName;
        public static string FinancialYear;
        public string DynamicDatabase = string.Empty;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
            .RegisterType<IDBFactory, DBFactory>(new HttpContextLifetimeManager<IDBFactory>())
            .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>())
            .RegisterType<ICompanyService, CompanyService>(new HttpContextLifetimeManager<ICompanyService>())
            .RegisterType<ICompanyRepository, CompanyRepository>(new HttpContextLifetimeManager<ICompanyRepository>())
            .RegisterType<IBankNameService, BankNameService>(new HttpContextLifetimeManager<IBankNameService>())
            .RegisterType<IBankNameRepository, BankNameRepository>(new HttpContextLifetimeManager<IBankNameRepository>())
            .RegisterType<IBankAccountTypeService, BankAccountTypeService>(new HttpContextLifetimeManager<IBankAccountTypeService>())
            .RegisterType<IBankAccountTypeRepository, BankAccountTypeRepository>(new HttpContextLifetimeManager<IBankAccountTypeRepository>())
            .RegisterType<IBankService, BankService>(new HttpContextLifetimeManager<IBankService>())
            .RegisterType<IBankRepository, BankRepository>(new HttpContextLifetimeManager<IBankRepository>())
            .RegisterType<IClientMasterService, ClientMasterService>(new HttpContextLifetimeManager<IClientMasterService>())
            .RegisterType<IClientMasterRepository, ClientMasterRepository>(new HttpContextLifetimeManager<IClientMasterRepository>())
            .RegisterType<IClientBankDetailRepository, ClientBankDetailReposirtory>(new HttpContextLifetimeManager<IClientBankDetailRepository>())
            .RegisterType<IClientBankDetailService, ClientBankDetailService>(new HttpContextLifetimeManager<IClientBankDetailService>())
            .RegisterType<IBloodGroupService, BloodGroupService>(new HttpContextLifetimeManager<IBloodGroupService>())
            .RegisterType<IBloodGroupRepository, BloodGroupRepository>(new HttpContextLifetimeManager<IBloodGroupRepository>())
            .RegisterType<IMonthExperienceService, MonthExperienceService>(new HttpContextLifetimeManager<IMonthExperienceService>())
            .RegisterType<IMonthExperienceRepository, MonthExperienceRepository>(new HttpContextLifetimeManager<IMonthExperienceRepository>())
            .RegisterType<IYearExperienceService, YearExperienceService>(new HttpContextLifetimeManager<IYearExperienceService>())
            .RegisterType<IYearExperienceRepository, YearExperienceRepository>(new HttpContextLifetimeManager<IYearExperienceRepository>())
            .RegisterType<ITransportService, TranportService>(new HttpContextLifetimeManager<ITransportService>())
            .RegisterType<ITransportMasterRepository, TransportMasterRepository>(new HttpContextLifetimeManager<ITransportMasterRepository>())
            .RegisterType<ITransportBankDetailService, TransportBankDetailService>(new HttpContextLifetimeManager<ITransportBankDetailService>())
            .RegisterType<ITransportBankDetailRepository, TransportBankDetailRepository>(new HttpContextLifetimeManager<ITransportBankDetailRepository>())
            .RegisterType<ISuppliersMasterRepository, SuppliersMasterRepository>(new HttpContextLifetimeManager<ISuppliersMasterRepository>())
            .RegisterType<ISuppliersMasterService, SuppliersMasterService>(new HttpContextLifetimeManager<ISuppliersMasterService>())
            .RegisterType<ISupplierBankDetailRepository, SupplierBankDetailRepository>(new HttpContextLifetimeManager<ISupplierBankDetailRepository>())
            .RegisterType<ISupplierBankDetailService, SupplierBankDetailService>(new HttpContextLifetimeManager<ISupplierBankDetailService>())
            .RegisterType<ITypeOfSupplierRepository, TypeOfSupplierRepository>(new HttpContextLifetimeManager<ITypeOfSupplierRepository>())
            .RegisterType<ITypeOfSupplierService, TypeOfSupplierService>(new HttpContextLifetimeManager<ITypeOfSupplierService>())
            .RegisterType<IUtilityService, UtilityService>(new HttpContextLifetimeManager<IUtilityService>())

            .RegisterType<IGenerateItemCodeRepository, GenerateItemCodeRepository>(new HttpContextLifetimeManager<IGenerateItemCodeRepository>())
            .RegisterType<IGenerateItemCodeService, GenerateItemCodeService>(new HttpContextLifetimeManager<IGenerateItemCodeService>())
            .RegisterType<ILabRepository, LabRepository>(new HttpContextLifetimeManager<ILabRepository>())
            .RegisterType<ILabMasterService, LabMasterService>(new HttpContextLifetimeManager<ILabMasterService>())
            .RegisterType<ICurrencyRepository, CurrencyRepository>(new HttpContextLifetimeManager<ICurrencyRepository>())
            .RegisterType<ICurrencyService, CurrencyService>(new HttpContextLifetimeManager<ICurrencyService>())
            .RegisterType<IUnitRepository, UnitRepository>(new HttpContextLifetimeManager<IUnitRepository>())
            .RegisterType<IUnitService, UnitService>(new HttpContextLifetimeManager<IUnitService>())
            .RegisterType<IBrandMasterRepository, BrandMasterRepository>(new HttpContextLifetimeManager<IBrandMasterRepository>())
            .RegisterType<IBrandMasterService, BrandMasterService>(new HttpContextLifetimeManager<IBrandMasterService>())
            .RegisterType<IDesignReposirory, DesignRepository>(new HttpContextLifetimeManager<IDesignReposirory>())
            .RegisterType<IDesignService, DesignService>(new HttpContextLifetimeManager<IDesignService>())
            .RegisterType<IOpeningStockRepository, OpeningStockRepository>(new HttpContextLifetimeManager<IOpeningStockRepository>())
            .RegisterType<IOpeningStockService, OpeningStockService>(new HttpContextLifetimeManager<IOpeningStockService>())
            .RegisterType<ISuppliersMasterRepository, SuppliersMasterRepository>(new HttpContextLifetimeManager<ISuppliersMasterRepository>())
            .RegisterType<ISuppliersMasterService, SuppliersMasterService>(new HttpContextLifetimeManager<ISuppliersMasterService>())
            .RegisterType<IStateRepository, StateRepository>(new HttpContextLifetimeManager<IStateRepository>())
            .RegisterType<IStateService, StateService>(new HttpContextLifetimeManager<IStateService>())
            .RegisterType<ICityRepository, CityRepository>(new HttpContextLifetimeManager<ICityRepository>())
            .RegisterType<ICityService, CityService>(new HttpContextLifetimeManager<ICityService>())
            .RegisterType<IEmployeeMasterService, EmployeeMasterService>(new HttpContextLifetimeManager<IEmployeeMasterService>())
            .RegisterType<IEmployeeMasterRepository, EmployeeMasterRepository>(new HttpContextLifetimeManager<IEmployeeMasterRepository>())
            .RegisterType<ICountryRepository, CountryRepository>(new HttpContextLifetimeManager<ICountryRepository>())
            .RegisterType<ICountryService, CountryService>(new HttpContextLifetimeManager<ICountryService>())
            .RegisterType<IlogindetailRepository, logindetailRepository>(new HttpContextLifetimeManager<IlogindetailRepository>())
            .RegisterType<IlogindetailService, logindetailService>(new HttpContextLifetimeManager<IlogindetailService>())
            .RegisterType<IDistrictRepository, DistrictRepository>(new HttpContextLifetimeManager<IDistrictRepository>())
            .RegisterType<IDistrictService, DistrictService>(new HttpContextLifetimeManager<IDistrictService>())
            .RegisterType<IItemCategoryRepository, ItemCategoryRepository>(new HttpContextLifetimeManager<IItemCategoryRepository>())
            .RegisterType<IItemCategoryService, ItemCategoryService>(new HttpContextLifetimeManager<IItemCategoryService>())
            .RegisterType<IItemSubCategoryRepository, ItemSubCategoryRepository>(new HttpContextLifetimeManager<IItemSubCategoryRepository>())
            .RegisterType<IItemSubCategoryService, ItemSubCategoryService>(new HttpContextLifetimeManager<IItemSubCategoryService>())
            .RegisterType<IItemRepository, ItemRepository>(new HttpContextLifetimeManager<IItemRepository>())
            .RegisterType<IItemService, ItemService>(new HttpContextLifetimeManager<IItemService>())
            .RegisterType<IItemNameRepository, ItemNameRepository>(new HttpContextLifetimeManager<IItemNameRepository>())
            .RegisterType<IItemNameService, ItemNameService>(new HttpContextLifetimeManager<IItemNameService>())
            .RegisterType<IItemTempDetailRepository, ItemTempDetailRepository>(new HttpContextLifetimeManager<IItemTempDetailRepository>())
            .RegisterType<IItemTempDetailService, ItemTempDetailService>(new HttpContextLifetimeManager<IItemTempDetailService>())
            .RegisterType<ICostCodeCreationRepository, CostCodeCreationRepository>(new HttpContextLifetimeManager<ICostCodeCreationRepository>())
            .RegisterType<ICostCodeCreationService, CostCodeCreationService>(new HttpContextLifetimeManager<ICostCodeCreationService>())
            .RegisterType<IColorCodeRepository, ColorCodeRepository>(new HttpContextLifetimeManager<IColorCodeRepository>())
            .RegisterType<IColorCodeService, ColorCodeService>(new HttpContextLifetimeManager<IColorCodeService>())
            .RegisterType<ITypeOfMaterialRepository, TypeOfMaterialRepository>(new HttpContextLifetimeManager<ITypeOfMaterialRepository>())
            .RegisterType<ITypeOfMaterialService, TypeOfMaterialService>(new HttpContextLifetimeManager<ITypeOfMaterialService>())
            .RegisterType<IGodownRepository, GodownRepository>(new HttpContextLifetimeManager<IGodownRepository>())
            .RegisterType<IGodownService, GodownMasterService>(new HttpContextLifetimeManager<IGodownService>())
            .RegisterType<IGodownAddressRepository, GodownAddressRepository>(new HttpContextLifetimeManager<IGodownAddressRepository>())
            .RegisterType<IGodownAddressService, GodownAddressService>(new HttpContextLifetimeManager<IGodownAddressService>())
            .RegisterType<ISalesIncentiveRepository, SalesIncentiveRepository>(new HttpContextLifetimeManager<ISalesIncentiveRepository>())
            .RegisterType<ISalesIncentiveService, SalesIncentiveService>(new HttpContextLifetimeManager<ISalesIncentiveService>())
            .RegisterType<IEmployeeMasterService, EmployeeMasterService>(new HttpContextLifetimeManager<IEmployeeMasterService>())
            .RegisterType<IEmployeeMasterRepository, EmployeeMasterRepository>(new HttpContextLifetimeManager<IEmployeeMasterRepository>())
            .RegisterType<IPurchaseItemDetailRepository, PurchaseItemDetailRepository>(new HttpContextLifetimeManager<IPurchaseItemDetailRepository>())
            .RegisterType<IPurchaseItemDetailService, PurchaseItemDetailService>(new HttpContextLifetimeManager<IPurchaseItemDetailService>())
            .RegisterType<IPurchaseOrderDetailRepository, PurchaseOrderDetailRepository>(new HttpContextLifetimeManager<IPurchaseOrderDetailRepository>())
            .RegisterType<IPurchaseOrderDetailService, PurchaseOrderDetailService>(new HttpContextLifetimeManager<IPurchaseOrderDetailService>())
            .RegisterType<IItemTaxService, ItemTaxService>(new HttpContextLifetimeManager<IItemTaxService>())
            .RegisterType<IItemTaxRepository, ItemTaxRepository>(new HttpContextLifetimeManager<IItemTaxRepository>())
             .RegisterType<IPurchaseItemTaxService, PurchaseItemTaxService>(new HttpContextLifetimeManager<IPurchaseItemTaxService>())
            .RegisterType<IPurchaseItemTaxRepository, PurchaseItemTaxRepository>(new HttpContextLifetimeManager<IPurchaseItemTaxRepository>())
            .RegisterType<ISubTaxMasterRepository, SubTaxMasterRepository>(new HttpContextLifetimeManager<ISubTaxMasterRepository>())
            .RegisterType<ISubTaxMasterService, SubTaxMasterService>(new HttpContextLifetimeManager<ISubTaxMasterService>())
            .RegisterType<IDesignationMasterRepository, DesignationMasterRepository>(new HttpContextLifetimeManager<IDesignationMasterRepository>())
            .RegisterType<IDesignationMasterService, DesignationMasterService>(new HttpContextLifetimeManager<IDesignationMasterService>())
            .RegisterType<IComapnyBankDetailsRepository, CompanyBankDetailsRepository>(new HttpContextLifetimeManager<IComapnyBankDetailsRepository>())
            .RegisterType<ICompanyBankDetailsService, CompanyBankDetailService>(new HttpContextLifetimeManager<ICompanyBankDetailsService>())
            .RegisterType<IDepartmentRepository, DepertmentRepository>(new HttpContextLifetimeManager<IDepartmentRepository>())
            .RegisterType<IDepartmentService, DepartmentService>(new HttpContextLifetimeManager<IDepartmentService>())
            .RegisterType<IInwardFromSupplierRepository, InwardFromSupplierRepository>(new HttpContextLifetimeManager<IInwardFromSupplierRepository>())
            .RegisterType<IInwardItemsFromSupplierRepository, InwardItemsFromSupplierRepository>(new HttpContextLifetimeManager<IInwardItemsFromSupplierRepository>())
            .RegisterType<IBarcodeNumberRepository, BarcodeNumberRepository>(new HttpContextLifetimeManager<IBarcodeNumberRepository>())
            .RegisterType<IBarcodeNumberService, BarcodeNumberService>(new HttpContextLifetimeManager<IBarcodeNumberService>())
            .RegisterType<IBarcodeTempDetailRepository, BarcodeTempDetailRepository>(new HttpContextLifetimeManager<IBarcodeTempDetailRepository>())
            .RegisterType<IBarcodeTempDetailService, BarcodeTempDetailService>(new HttpContextLifetimeManager<IBarcodeTempDetailService>())
            .RegisterType<IItemTruncateRepository, ItemTruncateRepository>(new HttpContextLifetimeManager<ItemTruncateRepository>())
            .RegisterType<IInwardFromSupplierService, InwardfromSupplierService>(new HttpContextLifetimeManager<IInwardFromSupplierService>())
            .RegisterType<IInwardItemFromSupplierService, InwardItemFromSupplierService>(new HttpContextLifetimeManager<IInwardItemFromSupplierService>())
            .RegisterType<IUserRepository, UserRepository>(new HttpContextLifetimeManager<IUserRepository>())
            .RegisterType<IUserService, UserService>(new HttpContextLifetimeManager<IUserService>())
            .RegisterType<IUserCredentialRepository, UserCredentialRepository>(new HttpContextLifetimeManager<IUserCredentialRepository>())
            .RegisterType<IUserCredentialService, UserCredentialService>(new HttpContextLifetimeManager<IUserCredentialService>())
            .RegisterType<IModuleRepository, ModuleRepository>(new HttpContextLifetimeManager<IModuleRepository>())
            .RegisterType<IModuleService, ModuleService>(new HttpContextLifetimeManager<IModuleService>())

            .RegisterType<IEntryStockRepository, EntryStockRepository>(new HttpContextLifetimeManager<IEntryStockRepository>())
            .RegisterType<IEntryStockService, EntryStockService>(new HttpContextLifetimeManager<IEntryStockService>())
            .RegisterType<IEntryStockItemRepository, EntryStockItemRepository>(new HttpContextLifetimeManager<IEntryStockItemRepository>())
            .RegisterType<IEntryStockItemService, EntryStockItemService>(new HttpContextLifetimeManager<IEntryStockItemService>())

            .RegisterType<IPurchaseInventoryTaxRepository, PurchaseInventoryTaxRepository>(new HttpContextLifetimeManager<IPurchaseInventoryTaxRepository>())
            .RegisterType<IPurchaseInventoryTaxService, PurchaseInventoryTaxService>(new HttpContextLifetimeManager<PurchaseInventoryTaxService>())

            .RegisterType<IItemTaxRepository, ItemTaxRepository>(new HttpContextLifetimeManager<IItemTaxRepository>())
            .RegisterType<IItemTaxService, ItemTaxService>(new HttpContextLifetimeManager<IItemTaxService>())
            .RegisterType<IShopRequisitionGodownItemRepository, ShopRequisitionGodownItemRepository>(new HttpContextLifetimeManager<IShopRequisitionGodownItemRepository>())
            .RegisterType<IShopRequisitionGodownItemService, ShopRequisitionGodownItemService>(new HttpContextLifetimeManager<IShopRequisitionGodownItemService>())
            .RegisterType<IShopRequisitionGodownRepository, ShopRequisitionGodownRepository>(new HttpContextLifetimeManager<IShopRequisitionGodownRepository>())
            .RegisterType<IShopRequisitionGodownService, ShopRequisitionService>(new HttpContextLifetimeManager<IShopRequisitionGodownService>())


            .RegisterType<IShopRequisitionGodownSalesBillRepository, ShopRequisitionGodownSalesBillRepository>(new HttpContextLifetimeManager<IShopRequisitionGodownSalesBillRepository>())
            .RegisterType<IShopRequisitionGodownSalesBillService, ShopRequisitiongodownSalesbillService>(new HttpContextLifetimeManager<IShopRequisitionGodownSalesBillService>())
            .RegisterType<IShopRequisitionGodownItemSalesBillRepository, ShopRequisitionGodownItemSalesBillRepository>(new HttpContextLifetimeManager<IShopRequisitionGodownItemSalesBillRepository>())
            .RegisterType<IShopRequisitionGodownItemSalesBillService, ShopRequsitionGodownItemSalesBillService>(new HttpContextLifetimeManager<IShopRequisitionGodownItemSalesBillService>())
            .RegisterType<IRequisitionForNewItemRepository, RequisitionForNewItemRepository>(new HttpContextLifetimeManager<IRequisitionForNewItemRepository>())
            .RegisterType<IRequisitionForNewItemService, RequisitionForNewItemService>(new HttpContextLifetimeManager<IRequisitionForNewItemService>())

            .RegisterType<ICustomerDetailRepository, CustomerDetailRepository>(new HttpContextLifetimeManager<ICustomerDetailRepository>())
            .RegisterType<ICustomerDetailService, CustomerDetailService>(new HttpContextLifetimeManager<ICustomerDetailService>())
            .RegisterType<IRetailBillItemRepository, RetailBillItemRepository>(new HttpContextLifetimeManager<IRetailBillItemRepository>())
            .RegisterType<IRetailBillItemService, RetailBillItemService>(new HttpContextLifetimeManager<IRetailBillItemService>())
            .RegisterType<IRetailBillRepository, RetailBillRepository>(new HttpContextLifetimeManager<IRetailBillRepository>())
            .RegisterType<IRetailBillService, RetailBillService>(new HttpContextLifetimeManager<IRetailBillService>())

            .RegisterType<IRetailBillAdjAmtDetailRepository, RetailBillAdjAmtDetailRepository>(new HttpContextLifetimeManager<IRetailBillAdjAmtDetailRepository>())
            .RegisterType<IRetailBillAdjAmtDetailService, RetailBillAdjAmtDetailService>(new HttpContextLifetimeManager<IRetailBillAdjAmtDetailService>())

            .RegisterType<ITemporaryCashMemoRepository, TemporaryCashMemoRepository>(new HttpContextLifetimeManager<ITemporaryCashMemoRepository>())
            .RegisterType<ITemporaryCashMemoService, TemporaryCashMemoService>(new HttpContextLifetimeManager<ITemporaryCashMemoService>())
            .RegisterType<ITemporaryCashMemoItemRepository, TemporaryCashMemoItemRepository>(new HttpContextLifetimeManager<ITemporaryCashMemoItemRepository>())
            .RegisterType<ITemporaryCashMemoItemService, TemporaryCashMemoItemService>(new HttpContextLifetimeManager<ITemporaryCashMemoItemService>())
            .RegisterType<ITemporaryCashMemoAdjAmtDetailRepository, TemporaryCashMemoAdjAmtDetailRepository>(new HttpContextLifetimeManager<ITemporaryCashMemoAdjAmtDetailRepository>())
            .RegisterType<ITemporaryCashMemoAdjAmtDetailService, TemporaryCashMemoAdjAmtDetailService>(new HttpContextLifetimeManager<ITemporaryCashMemoAdjAmtDetailService>())

            .RegisterType<IListOfItemCodeRepository, ListOfItemCodeRepository>(new HttpContextLifetimeManager<IListOfItemCodeRepository>())
            .RegisterType<ITaxMasterRepository, TaxMasterRepository>(new HttpContextLifetimeManager<ITaxMasterRepository>())
            .RegisterType<ITaxService, TaxService>(new HttpContextLifetimeManager<ITaxService>())
            .RegisterType<ISubTaxMasterRepository, SubTaxMasterRepository>(new HttpContextLifetimeManager<ISubTaxMasterRepository>())
            .RegisterType<ISubTaxMasterService, SubTaxMasterService>(new HttpContextLifetimeManager<ISubTaxMasterService>())

            .RegisterType<IInwardFromGodownRepository, InwardFromGodownRepository>(new HttpContextLifetimeManager<IInwardFromGodownRepository>())
            .RegisterType<IInwardFromGodownService, InwardFromGodownService>(new HttpContextLifetimeManager<IInwardFromGodownService>())
            .RegisterType<IInwardItemFromGodownRepository, InwardItemFromGodownRepository>(new HttpContextLifetimeManager<IInwardItemFromGodownRepository>())
            .RegisterType<IInwardItemFromGodownService, InwardItemFromGodownService>(new HttpContextLifetimeManager<IInwardItemFromGodownService>())

            .RegisterType<IShopService, ShopService>(new HttpContextLifetimeManager<IShopService>())
            .RegisterType<IShopRepository, ShopRepository>(new HttpContextLifetimeManager<IShopRepository>())

            .RegisterType<IStockDistributionRepository, StockDistributionRepository>(new HttpContextLifetimeManager<IStockDistributionRepository>())
            .RegisterType<IStockDistributionService, StockDistributionService>(new HttpContextLifetimeManager<IStockDistributionService>())
            .RegisterType<IStockItemDistributionRepository, StockItemDistributionRepository>(new HttpContextLifetimeManager<IStockItemDistributionRepository>())
            .RegisterType<IStockItemDistributionService, StockItemDistributionService>(new HttpContextLifetimeManager<IStockItemDistributionService>())

            .RegisterType<IShopStockRepository, ShopStockRepository>(new HttpContextLifetimeManager<IShopStockRepository>())
            .RegisterType<IShopStockService, ShopStockService>(new HttpContextLifetimeManager<IShopStockService>())
            .RegisterType<IGodownStockRepository, GodownStockRepository>(new HttpContextLifetimeManager<IGodownStockRepository>())
            .RegisterType<IGodownStockService, GodownStockService>(new HttpContextLifetimeManager<IGodownStockService>())
            .RegisterType<IGodownStockService, GodownStockService>(new HttpContextLifetimeManager<IGodownStockService>())

            .RegisterType<IOutwardToShopRepository, OutwardToShopRepository>(new HttpContextLifetimeManager<IOutwardToShopRepository>())
            .RegisterType<IOutwardToShopService, OutwardToShopService>(new HttpContextLifetimeManager<IOutwardToShopService>())
            .RegisterType<IOutwardItemToShopRepository, OutwardItemToShopRepository>(new HttpContextLifetimeManager<IOutwardItemToShopRepository>())
            .RegisterType<IOutwardItemToShopService, OutwardItemToShopService>(new HttpContextLifetimeManager<IOutwardItemToShopService>())

            .RegisterType<IOutwardShopToGodownService, OutwardShopToGodownService>(new HttpContextLifetimeManager<IOutwardShopToGodownService>())
            .RegisterType<IOutwardShopToGodownRepository, OutwardShopToGodownRepository>(new HttpContextLifetimeManager<IOutwardShopToGodownRepository>())
            .RegisterType<IOutwardItemShopToGodownService, OutwardItemShopToGodownService>(new HttpContextLifetimeManager<IOutwardItemShopToGodownService>())
            .RegisterType<IOutwardItemShopToGodownRepository, OutwardItemShopToGodownRepository>(new HttpContextLifetimeManager<IOutwardItemShopToGodownRepository>())

            .RegisterType<IOutwardStockDistributionRepository, OutwardStockDistributionRepository>(new HttpContextLifetimeManager<IOutwardStockDistributionRepository>())
            .RegisterType<IOutwardStockDistributionService, OutwardStockDistributionService>(new HttpContextLifetimeManager<IOutwardStockDistributionService>())
            .RegisterType<IOutwardItemStockDistributionRepository, OutwardItemStockDistributionRepository>(new HttpContextLifetimeManager<IOutwardItemStockDistributionRepository>())
            .RegisterType<IOutwardItemStockDistributionService, OutwardItemStockDistributionService>(new HttpContextLifetimeManager<IOutwardItemStockDistributionService>())

            .RegisterType<IOutwardToClientRepository, OutwardToClientRepository>(new HttpContextLifetimeManager<IOutwardToClientRepository>())
            .RegisterType<IOutwardToClientService, OutwardToClientService>(new HttpContextLifetimeManager<IOutwardToClientService>())
            .RegisterType<IOutwardItemToClientRepository, OutwardItemToClientRepository>(new HttpContextLifetimeManager<IOutwardItemToClientRepository>())
            .RegisterType<IOutwardItemToClientService, OutwardItemToClientService>(new HttpContextLifetimeManager<IOutwardItemToClientService>())

            .RegisterType<IOutwardInterGodownRepository, OutwardInterGodownRepository>(new HttpContextLifetimeManager<IOutwardInterGodownRepository>())
            .RegisterType<IOutwardInterGodownService, OutwardInterGodownService>(new HttpContextLifetimeManager<IOutwardInterGodownService>())
            .RegisterType<IOutwardItemInterGodownRepository, OutwardItemInterGodownRepository>(new HttpContextLifetimeManager<IOutwardItemInterGodownRepository>())
            .RegisterType<IOutwardItemInterGodownService, OutwardItemInterGodownService>(new HttpContextLifetimeManager<IOutwardItemInterGodownService>())

            .RegisterType<ISalesOrderRepository, SalesOrderRepository>(new HttpContextLifetimeManager<ISalesOrderRepository>())
            .RegisterType<ISalesOrderService, SalesOrderService>(new HttpContextLifetimeManager<ISalesOrderService>())
            .RegisterType<ISalesOrderItemRepository, SalesOrderItemRepository>(new HttpContextLifetimeManager<ISalesOrderItemRepository>())
            .RegisterType<ISalesOrderItemService, SalesOrderItemService>(new HttpContextLifetimeManager<ISalesOrderItemService>())

            .RegisterType<IQuotationRepository, QuotationRepository>(new HttpContextLifetimeManager<IQuotationRepository>())
            .RegisterType<IQuotationService, QuotationService>(new HttpContextLifetimeManager<IQuotationService>())
            .RegisterType<IQuotationItemRepository, QuotationItemRepository>(new HttpContextLifetimeManager<IQuotationItemRepository>())
            .RegisterType<IQuotationItemService, QuotationItemService>(new HttpContextLifetimeManager<IQuotationItemService>())

            .RegisterType<IInventoryTaxRepository, InventoryTaxRepository>(new HttpContextLifetimeManager<IInventoryTaxRepository>())
            .RegisterType<IInventoryTaxService, InventoryTaxService>(new HttpContextLifetimeManager<IInventoryTaxService>())

            .RegisterType<IDeliveryChallanRepository, DeliveryChallanRepository>(new HttpContextLifetimeManager<IDeliveryChallanRepository>())
            .RegisterType<IDeliveryChallanService, DeliveryChallanService>(new HttpContextLifetimeManager<IDeliveryChallanService>())
            .RegisterType<IDeliveryChallanItemRepository, DeliveryChallanItemRepository>(new HttpContextLifetimeManager<IDeliveryChallanItemRepository>())
            .RegisterType<IDeliveryChallanItemService, DeliveryChallanItemService>(new HttpContextLifetimeManager<IDeliveryChallanItemService>())

            .RegisterType<ICashHandoverRepository, CashHandoverRepository>(new HttpContextLifetimeManager<ICashHandoverRepository>())
            .RegisterType<ICashHandoverService, CashHandoverService>(new HttpContextLifetimeManager<ICashHandoverService>())

            .RegisterType<ICardChequeHandoverRepository, CardChequeHandoverRepository>(new HttpContextLifetimeManager<ICardChequeHandoverRepository>())
            .RegisterType<ICardChequeHandoverService, CardChequeHandoverService>(new HttpContextLifetimeManager<ICardChequeHandoverService>())


            .RegisterType<IBalanceCarryForwardRepository, BalanceCarryForwardRepository>(new HttpContextLifetimeManager<IBalanceCarryForwardRepository>())
            .RegisterType<IBalanceCarryForwardService, BalanceCarryForwardService>(new HttpContextLifetimeManager<IBalanceCarryForwardService>())

            .RegisterType<ICashierReceivableRepository, CashierReceivableRepository>(new HttpContextLifetimeManager<ICashierReceivableRepository>())
            .RegisterType<ICashierReceivableService, CashierReceivableService>(new HttpContextLifetimeManager<ICashierReceivableService>())

            .RegisterType<ICashierSalesOrderRepository, CashierSalesOrderRepository>(new HttpContextLifetimeManager<ICashierSalesOrderRepository>())
            .RegisterType<ICashierSalesOrderService, CashierSalesOrderService>(new HttpContextLifetimeManager<ICashierSalesOrderService>())
            .RegisterType<ICashierSalesOrderItemRepository, CashierSalesOrderItemRepository>(new HttpContextLifetimeManager<ICashierSalesOrderItemRepository>())
            .RegisterType<ICashierSalesOrderItemService, CashierSalesOrderItemService>(new HttpContextLifetimeManager<ICashierSalesOrderItemService>())

            .RegisterType<ICashierSalesBillRepository, CashierSalesBillRepository>(new HttpContextLifetimeManager<ICashierSalesBillRepository>())
            .RegisterType<ICashierSalesBillService, CashierSalesBillService>(new HttpContextLifetimeManager<ICashierSalesBillService>())
            .RegisterType<ICashierSalesBillItemRepository, CashierSalesBillItemRepository>(new HttpContextLifetimeManager<ICashierSalesBillItemRepository>())
            .RegisterType<ICashierSalesBillItemService, CashierSalesBillItemService>(new HttpContextLifetimeManager<ICashierSalesBillItemService>())

            .RegisterType<ICashierRetailBillRepository, CashierRetailBillRepository>(new HttpContextLifetimeManager<ICashierRetailBillRepository>())
            .RegisterType<ICashierRetailBillService, CashierRetailBillService>(new HttpContextLifetimeManager<ICashierRetailBillService>())
            .RegisterType<ICashierRetailBillItemRepository, CashierRetailBillItemRepository>(new HttpContextLifetimeManager<ICashierRetailBillItemRepository>())
            .RegisterType<ICashierRetailBillItemService, CashierRetailBillItemService>(new HttpContextLifetimeManager<ICashierRetailBillItemService>())

            .RegisterType<ICashierTemporaryCashMemoRepository, CashierTemporaryCashMemoRepository>(new HttpContextLifetimeManager<ICashierTemporaryCashMemoRepository>())
            .RegisterType<ICashierTemporaryCashMemoService, CashierTemporaryCashMemoService>(new HttpContextLifetimeManager<ICashierTemporaryCashMemoService>())
            .RegisterType<ICashierTemporaryCashMemoItemRepository, CashierTemporaryCashMemoItemRepository>(new HttpContextLifetimeManager<ICashierTemporaryCashMemoItemRepository>())
            .RegisterType<ICashierTemporaryCashMemoItemService, CashierTemporaryCashMemoItemService>(new HttpContextLifetimeManager<ICashierTemporaryCashMemoItemService>())

            .RegisterType<ICashierPayableRepository, CashierPayableRepository>(new HttpContextLifetimeManager<ICashierPayableRepository>())
            .RegisterType<ICashierPayableService, CashierPayableService>(new HttpContextLifetimeManager<ICashierPayableService>())

            .RegisterType<IGetCashRowsBySORepository, GetCashRowsBySORepository>(new HttpContextLifetimeManager<IGetCashRowsBySORepository>())

            .RegisterType<ICashierRefundOrderRepository, CashierRefundOrderRepository>(new HttpContextLifetimeManager<ICashierRefundOrderRepository>())
            .RegisterType<ICashierRefundOrderService, CashierRefundOrderService>(new HttpContextLifetimeManager<ICashierRefundOrderService>())
            .RegisterType<ICashierRefundOrderItemRepository, CashierRefundOrderItemRepository>(new HttpContextLifetimeManager<ICashierRefundOrderItemRepository>())
            .RegisterType<ICashierRefundOrderItemService, CashierRefundOrderItemService>(new HttpContextLifetimeManager<ICashierRefundOrderItemService>())

            .RegisterType<ISalesBillRepository, SalesBillRepository>(new HttpContextLifetimeManager<ISalesBillRepository>())
            .RegisterType<ISalesBillService, SalesBillService>(new HttpContextLifetimeManager<ISalesBillService>())
            .RegisterType<ISalesBillItemRepository, SalesBillItemRepository>(new HttpContextLifetimeManager<ISalesBillItemRepository>())
            .RegisterType<ISalesBillItemService, SalesBillItemService>(new HttpContextLifetimeManager<ISalesBillItemService>())

            .RegisterType<ISalesBillAdjAmtDetailRepository, SalesBillAdjAmtDetailRepository>(new HttpContextLifetimeManager<ISalesBillAdjAmtDetailRepository>())
            .RegisterType<ISalesBillAdjAmtDetailService, SalesBillAdjAmtDetailService>(new HttpContextLifetimeManager<ISalesBillAdjAmtDetailService>())

            .RegisterType<IInwardFromStkDistRepository, InwardFromStkDistRepository>(new HttpContextLifetimeManager<IInwardFromStkDistRepository>())
            .RegisterType<IInwardFromStkDistService, InwardFromStkDistService>(new HttpContextLifetimeManager<IInwardFromStkDistService>())

            .RegisterType<IInwardItemFromStockDistributionRepository, InwardItemFromStockDistributionRepository>(new HttpContextLifetimeManager<IInwardItemFromStockDistributionRepository>())
            .RegisterType<IInwardItemFromStockDistributionService, InwardItemFromStockDistributionService>(new HttpContextLifetimeManager<IInwardItemFromStockDistributionService>())

            .RegisterType<ICreateDynamicDbRepository, CreateDynamicDbRepository>(new HttpContextLifetimeManager<ICreateDynamicDbRepository>())
            .RegisterType<IUpdateDynamicDbRepository, UpdateDynamicDbRepository>(new HttpContextLifetimeManager<IUpdateDynamicDbRepository>())
            .RegisterType<IBackUpDatabaseRepository, BackUpDatabaseRepository>(new HttpContextLifetimeManager<IBackUpDatabaseRepository>())

            .RegisterType<IEmployeesCompanyRepository, EmployeesCompanyRepository>(new HttpContextLifetimeManager<IEmployeesCompanyRepository>())
            .RegisterType<IEmployeesCompanyService, EmployeesCompanyService>(new HttpContextLifetimeManager<IEmployeesCompanyService>())

            .RegisterType<INonInventoryItemService, NonInventoryItemService>(new HttpContextLifetimeManager<INonInventoryItemService>())
            .RegisterType<INonInventoryItemRepository, NonInventoryItemRepository>(new HttpContextLifetimeManager<INonInventoryItemRepository>())

            .RegisterType<IGetUsersEmailRepository, GetUsersEmailRepository>(new HttpContextLifetimeManager<IGetUsersEmailRepository>())
            .RegisterType<IGetAllItemsFromStkDistrbtnRepository, GetAllItemsFromStkDistrbtnRepository>(new HttpContextLifetimeManager<IGetAllItemsFromStkDistrbtnRepository>())

            .RegisterType<IGetItemCodesByQuotAndItemCodeRepository, GetItemCodesByQuotAndItemCodeRepository>(new HttpContextLifetimeManager<IGetItemCodesByQuotAndItemCodeRepository>())
            .RegisterType<IGetItemsByQuotOrOrderNoRepository, GetItemsByQuotOrOrderNoRepository>(new HttpContextLifetimeManager<IGetItemsByQuotOrOrderNoRepository>())

            .RegisterType<IRequisitionForShopService, RequisitionForShopService>(new HttpContextLifetimeManager<IRequisitionForShopService>())
            .RegisterType<IRequisitionForShopRepository, RequisitionForShopRepository>(new HttpContextLifetimeManager<IRequisitionForShopRepository>())
            .RegisterType<IRequisitionForGodownService, RequisitionForGodownService>(new HttpContextLifetimeManager<IRequisitionForGodownService>())
            .RegisterType<IRequisitionForGodownRepository, RequisitionForGodownRepository>(new HttpContextLifetimeManager<IRequisitionForGodownRepository>())
            .RegisterType<IRequisitionOfPoRepository, RequisitionOfPoRepository>(new HttpContextLifetimeManager<IRequisitionOfPoRepository>())
            .RegisterType<IRequisitionOfPoService, RequisitionOfPoService>(new HttpContextLifetimeManager<IRequisitionOfPoService>())
            .RegisterType<IGetItemsFromItemMasterRepository, GetItemsFromItemMasterRepository>(new HttpContextLifetimeManager<IGetItemsFromItemMasterRepository>())


            .RegisterType<IInwardFromShopToGodownRepository, InwardFromShopToGodownRepository>(new HttpContextLifetimeManager<IInwardFromShopToGodownRepository>())
            .RegisterType<IInwardFromShopToGodownService, InwardFromShopToGodownService>(new HttpContextLifetimeManager<IInwardFromShopToGodownService>())

            .RegisterType<IInwardItemFromShopToGodownRepository, InwardItemFromShopToGodownRepository>(new HttpContextLifetimeManager<IInwardItemFromShopToGodownRepository>())
            .RegisterType<IInwardItemFromShopToGodownService, InwardItemFromShopToGodownService>(new HttpContextLifetimeManager<IInwardItemFromShopToGodownService>())

            .RegisterType<IOutwardInterShopRepository, OutwardInterShopRepository>(new HttpContextLifetimeManager<IOutwardInterShopRepository>())
            .RegisterType<IOutwardInterShopService, OutwardInterShopservice>(new HttpContextLifetimeManager<IOutwardInterShopService>())

            .RegisterType<IOutwardItemInterShopRepository, OutwardItemInterShopRepository>(new HttpContextLifetimeManager<IOutwardItemInterShopRepository>())
            .RegisterType<IOutwardItemInterShopservice, OutwardItemInterShopservice>(new HttpContextLifetimeManager<IOutwardItemInterShopservice>())

            .RegisterType<ISalesReturnRepository, SalesReturnRepository>(new HttpContextLifetimeManager<ISalesReturnRepository>())
            .RegisterType<ISalesReturnService, SalesReturnService>(new HttpContextLifetimeManager<ISalesReturnService>())

            .RegisterType<ISalesReturnItemRepository, SalesReturnItemRepository>(new HttpContextLifetimeManager<ISalesReturnItemRepository>())
            .RegisterType<ISalesReturnItemService, SalesReturnItemService>(new HttpContextLifetimeManager<ISalesReturnItemService>())

            .RegisterType<ICommissionRepository, CommissionRepository>(new HttpContextLifetimeManager<ICommissionRepository>())
            .RegisterType<ICommissionService, CommissionService>(new HttpContextLifetimeManager<ICommissionService>())

            .RegisterType<ICommissionProductRepository, CommissionProductRepository>(new HttpContextLifetimeManager<ICommissionProductRepository>())
            .RegisterType<ICommissionProductService, CommissionProductService>(new HttpContextLifetimeManager<ICommissionProductService>())

            .RegisterType<IJobWorkTypeRepository, JobWorkTypeRepository>(new HttpContextLifetimeManager<IJobWorkTypeRepository>())
            .RegisterType<IJobWorkTypeService, JobWorkTypeService>(new HttpContextLifetimeManager<IJobWorkTypeService>())

            .RegisterType<IJobWorkItemRepository, JobWorkItemRepository>(new HttpContextLifetimeManager<IJobWorkItemRepository>())
            .RegisterType<IJobWorkItemService, JobWorkItemService>(new HttpContextLifetimeManager<IJobWorkItemService>())

            .RegisterType<IJobWorkerRepository, JobWorkerRepository>(new HttpContextLifetimeManager<IJobWorkerRepository>())
            .RegisterType<IJobWorkerService, JobWorkerService>(new HttpContextLifetimeManager<IJobWorkerService>())

            .RegisterType<IJobWorkPaymentRepository, JobWorkPaymentRepository>(new HttpContextLifetimeManager<IJobWorkPaymentRepository>())
            .RegisterType<IJobWorkPaymentService, JobWorkPaymentService>(new HttpContextLifetimeManager<IJobWorkPaymentService>())

            .RegisterType<IJobWorkStockRepository, JobWorkStockRepository>(new HttpContextLifetimeManager<IJobWorkStockRepository>())
            .RegisterType<IJobWorkStockService, JobWorkStockService>(new HttpContextLifetimeManager<IJobWorkStockService>())

            .RegisterType<IOutwardToTailorRepository, OutwardToTailorRepository>(new HttpContextLifetimeManager<IOutwardToTailorRepository>())
            .RegisterType<IOutwardToTailorService, OutwardToTailorService>(new HttpContextLifetimeManager<IOutwardToTailorService>())

            .RegisterType<IOutwardToTailorItemRepository, OutwardToTailorItemRepository>(new HttpContextLifetimeManager<IOutwardToTailorItemRepository>())
            .RegisterType<IOutwardToTailorItemService, OutwardToTailorItemService>(new HttpContextLifetimeManager<IOutwardToTailorItemService>())

            .RegisterType<IInwardFromTailorRepository, InwardFromTailorRepository>(new HttpContextLifetimeManager<IInwardFromTailorRepository>())
            .RegisterType<IInwardFromTailorService, InwardFromTailorService>(new HttpContextLifetimeManager<IInwardFromTailorService>())
            .RegisterType<IInwardFromTailorItemRepository, InwardFromTailorItemRepository>(new HttpContextLifetimeManager<IInwardFromTailorItemRepository>())
            .RegisterType<IInwardFromTailorItemService, InwardFromTailorItemService>(new HttpContextLifetimeManager<IInwardFromTailorItemService>())

            .RegisterType<IInwardToTailorRepository, InwardToTailorRepository>(new HttpContextLifetimeManager<IInwardToTailorRepository>())
            .RegisterType<IInwardToTailorService, InwardToTailorService>(new HttpContextLifetimeManager<IInwardToTailorService>())
            .RegisterType<IInwardToTailorItemRepository, InwardToTailorItemRepository>(new HttpContextLifetimeManager<IInwardToTailorItemRepository>())
            .RegisterType<IInwardToTailorItemService, InwardToTailorItemService>(new HttpContextLifetimeManager<IInwardToTailorItemService>())

            .RegisterType<IInwardInterGodownRepository, InwardInterGodownRepository>(new HttpContextLifetimeManager<IInwardInterGodownRepository>())
            .RegisterType<IInwardInterGodownService, InwardInterGodownService>(new HttpContextLifetimeManager<IInwardInterGodownService>())

            .RegisterType<IInwardInterGodownItemRepository, InwardInterGodownItemRepository>(new HttpContextLifetimeManager<IInwardInterGodownItemRepository>())
            .RegisterType<IInwardInterGodownItemService, InwardInterGodownItemService>(new HttpContextLifetimeManager<IInwardInterGodownItemService>())

            .RegisterType<IInwardInterShopRepository, InwardInterShopRepository>(new HttpContextLifetimeManager<IInwardInterShopRepository>())
            .RegisterType<IInwardInterShopService, InwardInterShopService>(new HttpContextLifetimeManager<IInwardInterShopService>())

            .RegisterType<IInwardInterShopItemRepository, InwardInterShopItemRepository>(new HttpContextLifetimeManager<IInwardInterShopItemRepository>())
            .RegisterType<IInwardInterShopItemService, InwardInterShopItemService>(new HttpContextLifetimeManager<IInwardInterShopItemService>())

            .RegisterType<IPurchaseReturnRepository, PurchaseReturnRepository>(new HttpContextLifetimeManager<IPurchaseReturnRepository>())
            .RegisterType<IPurchaseReturnService, PurchaseReturnService>(new HttpContextLifetimeManager<IPurchaseReturnService>())

            .RegisterType<IPurchaseReturnItemRepository, PurchaseReturnItemRepository>(new HttpContextLifetimeManager<IPurchaseReturnItemRepository>())
            .RegisterType<IPurchaseReturnItemService, PurchaseReturnItemService>(new HttpContextLifetimeManager<IPurchaseReturnItemService>())

            .RegisterType<IIncomeExpenseVoucherRepository, IncomeExpenseVoucherRepository>(new HttpContextLifetimeManager<IIncomeExpenseVoucherRepository>())
            .RegisterType<IIncomeExpenseVoucherService, IncomeExpenseVoucherService>(new HttpContextLifetimeManager<IIncomeExpenseVoucherService>())

            .RegisterType<IRetailBillCreditNoteRepository, RetailBillCreditNoteRepository>(new HttpContextLifetimeManager<IRetailBillCreditNoteRepository>())
            .RegisterType<IRetailBillCreditNoteService, RetailBillCreditNoteService>(new HttpContextLifetimeManager<IRetailBillCreditNoteService>())

            .RegisterType<IRetailBillCreditNoteItemRepository, RetailBillCreditNoteItemRepository>(new HttpContextLifetimeManager<IRetailBillCreditNoteItemRepository>())
            .RegisterType<IRetailBillCreditNoteItemService, RetailBillCreditNoteItemService>(new HttpContextLifetimeManager<IRetailBillCreditNoteItemService>())

            .RegisterType<ISalesBillCreditNoteRepository, SalesBillCreditNoteRepository>(new HttpContextLifetimeManager<ISalesBillCreditNoteRepository>())
            .RegisterType<ISalesBillCreditNoteService, SalesBillCreditNoteService>(new HttpContextLifetimeManager<ISalesBillCreditNoteService>())

            .RegisterType<ISalesBillCreditNoteItemRepository, SalesBillCreditNoteItemRepository>(new HttpContextLifetimeManager<ISalesBillCreditNoteItemRepository>())
            .RegisterType<ISalesBillCreditNoteItemService, SalesBillCreditNoteItemService>(new HttpContextLifetimeManager<ISalesBillCreditNoteItemService>())

            .RegisterType<IDebitNoteRepository, DebitNoteRepository>(new HttpContextLifetimeManager<IDebitNoteRepository>())
            .RegisterType<IDebitNoteService, DebitNoteService>(new HttpContextLifetimeManager<IDebitNoteService>())

            .RegisterType<IDebitNoteItemRepository, DebitNoteItemRepository>(new HttpContextLifetimeManager<IDebitNoteItemRepository>())
            .RegisterType<IDebitNoteItemService, DebitNoteItemService>(new HttpContextLifetimeManager<IDebitNoteItemService>())

            .RegisterType<IPhysicalStockTakingRepository, PhysicalStockTakingRepository>(new HttpContextLifetimeManager<IPhysicalStockTakingRepository>())
            .RegisterType<IPhysicalStockTakingService, PhysicalStockTakingService>(new HttpContextLifetimeManager<IPhysicalStockTakingService>())

            .RegisterType<IUnitConversionRepository, UnitConversionRepository>(new HttpContextLifetimeManager<IUnitConversionRepository>())
            .RegisterType<IUnitConversionService, UnitConversionService>(new HttpContextLifetimeManager<IUnitConversionService>())

            .RegisterType<ITruncateTableRepository, TruncateTableRepository>(new HttpContextLifetimeManager<ITruncateTableRepository>())

            .RegisterType<IValidationCompanyRepository, ValidationCompanyRepository>(new HttpContextLifetimeManager<IValidationCompanyRepository>())

            .RegisterType<IBillPaymentRepository, BillPaymentRepository>(new HttpContextLifetimeManager<IBillPaymentRepository>())
            .RegisterType<IBillPaymentService, BillPaymentService>(new HttpContextLifetimeManager<IBillPaymentService>())
            .RegisterType<IBillPaymentItemRepository, BillPaymentItemRepository>(new HttpContextLifetimeManager<IBillPaymentItemRepository>())
            .RegisterType<IBillPaymentItemService, BillPaymentItemService>(new HttpContextLifetimeManager<IBillPaymentItemService>())
            .RegisterType<IGetPOListByPaymentStatusAndSupplierRepository, GetPOListByPaymentStatusAndSupplierRepository>(new HttpContextLifetimeManager<IGetPOListByPaymentStatusAndSupplierRepository>())

            .RegisterType<IDiscountMasterRepository, DiscountMasterRepository>(new HttpContextLifetimeManager<IDiscountMasterRepository>())
            .RegisterType<IDiscountMasterService, DiscountMasterService>(new HttpContextLifetimeManager<IDiscountMasterService>())
            .RegisterType<IDiscountMasterItemRepository, DiscountMasterItemRepository>(new HttpContextLifetimeManager<IDiscountMasterItemRepository>())
            .RegisterType<IDiscountMasterItemService, DiscountMasterItemService>(new HttpContextLifetimeManager<IDiscountMasterItemService>())
            .RegisterType<IJobWorkOutwardToClientRepository, JobWorkOutwardToClientRepository>(new HttpContextLifetimeManager<JobWorkOutwardToClientRepository>())
            .RegisterType<IJobWorkOutwardToClientService, JobWorkOutwardToClientService>(new HttpContextLifetimeManager<IJobWorkOutwardToClientService>())

            .RegisterType<IResetRetailBillItemRepository, ResetRetailBillItemRepository>(new HttpContextLifetimeManager<IResetRetailBillItemRepository>())
            .RegisterType<IResetRetailBillItemService, ResetRetailBillItemService>(new HttpContextLifetimeManager<IResetRetailBillItemService>())
            .RegisterType<IResetRetailBillRepository, ResetRetailBillRepository>(new HttpContextLifetimeManager<IResetRetailBillRepository>())
            .RegisterType<IResetRetailBillService, ResetRetailBillService>(new HttpContextLifetimeManager<IResetRetailBillService>())
            .RegisterType<IResetRetailBillAdjAmtDetailRepository, ResetRetailBillAdjAmtDetailRepository>(new HttpContextLifetimeManager<IResetRetailBillAdjAmtDetailRepository>())
            .RegisterType<IResetRetailBillAdjAmtDetailService, ResetRetailBillAdjAmtDetailService>(new HttpContextLifetimeManager<IResetRetailBillAdjAmtDetailService>())

            .RegisterType<IResetSalesBillItemRepository, ResetSalesBillItemRepository>(new HttpContextLifetimeManager<IResetSalesBillItemRepository>())
            .RegisterType<IResetSalesBillItemService, ResetSalesBillItemService>(new HttpContextLifetimeManager<IResetSalesBillItemService>())
            .RegisterType<IResetSalesBillRepository, ResetSalesBillRepository>(new HttpContextLifetimeManager<IResetSalesBillRepository>())
            .RegisterType<IResetSalesBillService, ResetSalesBillService>(new HttpContextLifetimeManager<IResetSalesBillService>())
            .RegisterType<IResetSalesBillAdjAmtDetailRepository, ResetSalesBillAdjAmtDetailRepository>(new HttpContextLifetimeManager<IResetSalesBillAdjAmtDetailRepository>())
            .RegisterType<IResetSalesBillAdjAmtDetailService, ResetSalesBillAdjAmtDetailService>(new HttpContextLifetimeManager<IResetSalesBillAdjAmtDetailService>())

            .RegisterType<ICatalogueRepository, CatalogueRepository>(new HttpContextLifetimeManager<ICatalogueRepository>())
            .RegisterType<ICatalogueService, CatalogueService>(new HttpContextLifetimeManager<ICatalogueService>())

            .RegisterType<IUniversalBarcodeNumberRepository, UniversalBarcodeNumberRepository>(new HttpContextLifetimeManager<IUniversalBarcodeNumberRepository>())
            .RegisterType<IUniversalBarcodeNumberService, UniversalBarcodeNumberService>(new HttpContextLifetimeManager<IUniversalBarcodeNumberService>());

            return container;
        }
    }
}


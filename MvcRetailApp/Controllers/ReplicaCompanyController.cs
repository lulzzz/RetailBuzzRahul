using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class ReplicaCompanyController : Controller
    {
        private string UserEmail
        {
            get { return (string)HttpContext.Session["UserEmail"]; }
            set { HttpContext.Session["UserEmail"] = value; }
        }
        private string CompanyCode
        {
            get { return (string)HttpContext.Session["CompanyCode"]; }
            set { HttpContext.Session["CompanyCode"] = value; }
        }
        private string CompanyName
        {
            get { return (string)HttpContext.Session["CompanyName"]; }
            set { HttpContext.Session["CompanyName"] = value; }
        }
        private string FinancialYear
        {
            get { return (string)HttpContext.Session["FinancialYear"]; }
            set { HttpContext.Session["FinancialYear"] = value; }
        }
        private static string DatabaseName
        {
            set { ((dynamic)System.Web.HttpContext.Current.ApplicationInstance).DynamicDatabase = value; }
        }

        private readonly ICompanyService _CompanyService;
        private readonly ICompanyBankDetailsService _CompanyBankDetailsService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly IBankService _BankService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IClientBankDetailService _ClientBankDetailService;
        private readonly IDesignService _DesignService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IGodownService _GodownService;
        private readonly IGodownAddressService _GodownAddressService;
        private readonly IItemCategoryService _ItemCategoryService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IItemService _ItemService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IJobWorkerService _JobWorkerService;
        private readonly IJobWorkItemService _JobWorkItemService;
        private readonly IJobWorkTypeService _JobWorkTypeService;
        private readonly ISalesIncentiveService _SalesIncentiveService;
        private readonly IShopService _ShopService;
        private readonly ITypeOfMaterialService _TypeOfMaterialService;
        private readonly IUnitService _UnitService;
        private readonly ISuppliersMasterService _SuppliersMasterService;
        private readonly ISupplierBankDetailService _SupplierBankDetailService;
        private readonly ITransportService _TransportService;
        private readonly ITransportBankDetailService _TransportBankDetailService;
        private readonly IUserService _UserService;
        private readonly IEmployeesCompanyService _EmployeesCompanyService;
        private readonly ICommissionService _CommissionService;
        private readonly IBrandMasterService _BrandMasterService;
        private readonly IDesignationMasterService _DesignationMasterService;

        public ReplicaCompanyController(ICompanyService CompanyService, ICompanyBankDetailsService CompanyBankDetailsService, IUserCredentialService UserCredentialService, IModuleService ModuleService, IBankService BankService,
            IClientMasterService ClientMasterService, IClientBankDetailService ClientBankDetailService, IDesignService DesignService, IEmployeeMasterService EmployeeMasterService, IGodownService GodownService, IGodownAddressService GodownAddressService,
            IItemCategoryService ItemCategoryService, IItemSubCategoryService ItemSubCategoryService, IItemService ItemService, IJobWorkerService JobWorkerService, IJobWorkItemService JobWorkItemService, IJobWorkTypeService JobWorkTypeService,
            ISalesIncentiveService SalesIncentiveService, IShopService ShopService, ITypeOfMaterialService TypeOfMaterialService, IUnitService UnitService, ISuppliersMasterService SuppliersMasterService, ISupplierBankDetailService SupplierBankDetailService,
            ITransportService TransportService, ITransportBankDetailService TransportBankDetailService, INonInventoryItemService NonInventoryItemService, IUserService UserService, IEmployeesCompanyService EmployeesCompanyService, ICommissionService CommissionService,
            IBarcodeNumberService BarcodeNumberService, IBrandMasterService BrandMasterService, IDesignationMasterService DesignationMasterService)
        {
            this._CompanyService = CompanyService;
            this._CompanyBankDetailsService = CompanyBankDetailsService;
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
            this._BankService = BankService;
            this._ClientMasterService = ClientMasterService;
            this._ClientBankDetailService = ClientBankDetailService;
            this._DesignService = DesignService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._GodownService = GodownService;
            this._GodownAddressService = GodownAddressService;
            this._ItemCategoryService = ItemCategoryService;
            this._ItemSubCategoryService = ItemSubCategoryService;
            this._ItemService = ItemService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._JobWorkerService = JobWorkerService;
            this._JobWorkItemService = JobWorkItemService;
            this._JobWorkTypeService = JobWorkTypeService;
            this._SalesIncentiveService = SalesIncentiveService;
            this._ShopService = ShopService;
            this._TypeOfMaterialService = TypeOfMaterialService;
            this._UnitService = UnitService;
            this._SuppliersMasterService = SuppliersMasterService;
            this._SupplierBankDetailService = SupplierBankDetailService;
            this._TransportService = TransportService;
            this._TransportBankDetailService = TransportBankDetailService;
            this._UserService = UserService;
            this._EmployeesCompanyService = EmployeesCompanyService;
            this._CommissionService = CommissionService;
            this._BrandMasterService = BrandMasterService;
            this._DesignationMasterService = DesignationMasterService;
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
                            filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                              RouteValueDictionary(new { controller = "User", action = "Login", area = "" }));
                }
            }
        }

        [HttpGet]
        public ActionResult ReplicaCompany()
        {
            MainApplication model = new MainApplication();
            model.CmpList = new List<Company>();
            model.CmpList = Session["CompanyList"] as IEnumerable<Company>;
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');

            string FromFinYr = yr[0] + "/" + yr[1] + "/" + yr[2];
            string ToFinYr = yr[4] + "/" + yr[5] + "/" + yr[6];
            DateTime LoginFromFinYrDate = DateTime.ParseExact(FromFinYr, "dd/MM/yyyy", null).Date;
            DateTime LoginToFinYrDate = DateTime.ParseExact(ToFinYr, "dd/MM/yyyy", null).Date;
            string[] CompanyNames = new string[model.CmpList.Count()];
            string[] CompanyCodes = new string[model.CmpList.Count()];
            int count = 0;
            foreach (var data in model.CmpList)
            {
                if (CompanyName.ToLower() == data.companyName.ToLower() & LoginFromFinYrDate.Date < Convert.ToDateTime(data.FinancialYearFrom).Date & LoginToFinYrDate.Date < Convert.ToDateTime(data.FinancialYearTo).Date & data.Replicated == "No")
                {
                    string CompanyWithFinancial = string.Empty;
                    string Name = data.companyName;
                    CompanyWithFinancial = Name + " " + Convert.ToDateTime(data.FinancialYearFrom).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(data.FinancialYearTo).ToString("dd-MM-yyyy");
                    CompanyNames[count] = CompanyWithFinancial;
                    CompanyCodes[count] = data.CompanyCode;
                    count++;
                }
            }
            TempData["NewCompanyNames"] = CompanyNames;
            TempData["NewCompanyCodes"] = CompanyCodes;
            TempData["ArrayCount"] = count;
            return View(model);
        }

        [HttpPost]
        public ActionResult ReplicaCompany(FormCollection frmcol)
        {
            string Company = frmcol["CompanyNameWithFinancial"];
            CompanyCode = Company.Substring(0, 9);
            DatabaseName = Company.Substring(10);
            TempData["LogedInCompanyCode"] = CompanyCode;
            TempData["BankMasters"] = _BankService.getAllBanks();
            TempData["ClientMasters"] = _ClientMasterService.GetAllClients();
            TempData["ClientBanks"] = _ClientBankDetailService.GetAllClientBanks();
            TempData["DesignationMasters"] = _DesignationMasterService.getAllDesignation();
            TempData["DesignMasters"] = _DesignService.GetAll();
            TempData["BrandMasters"] = _BrandMasterService.GetAllBrands();
            TempData["EmployeeMasters"] = _EmployeeMasterService.getAllemployees();
            TempData["GodownMasters"] = _GodownService.GetGodownNames();
            TempData["GodownAddresses"] = _GodownAddressService.GetAllGodownAddresses();
            TempData["ItemCategory"] = _ItemCategoryService.GetItemCategories();
            TempData["ItemSubCategory"] = _ItemSubCategoryService.GetItemSubCategories();
            TempData["ItemMasters"] = _ItemService.getAllItems();
            TempData["NonInventoryItems"] = _NonInventoryItemService.GetAll();
            TempData["BarcodeNumbers"] = _BarcodeNumberService.GetAllBarcodeNumbers();
            TempData["JobWorkers"] = _JobWorkerService.GetAll();
            TempData["JobWorkerTypes"] = _JobWorkTypeService.GetAll();
            //TempData["SalesIncentives"] = _SalesIncentiveService.GetAllSI();
            TempData["ShopMasters"] = _ShopService.GetAll();
            TempData["TypeOfMaterials"] = _TypeOfMaterialService.GetMaterialList();
            TempData["UnitMasters"] = _UnitService.getallsize();
            TempData["SupplierMasters"] = _SuppliersMasterService.getAllSuppliers();
            TempData["SupplierBanks"] = _SupplierBankDetailService.GetAllSupplierBanks();
            TempData["TransportMasters"] = _TransportService.GetAllTransport();
            TempData["TransportBanks"] = _TransportBankDetailService.GetAllTransportBanks();
            TempData["Users"] = _UserService.getAllusers();
            TempData["CommissionMasters"] = _CommissionService.GetAll();
            TempData["Modules"] = _ModuleService.getAllModules();
            TempData["UserCredentials"] = _UserCredentialService.GetAllUserCredentials();
            TempData["EmployeeCompanies"] = _EmployeesCompanyService.GetAllEmployeeCompanies();
            return RedirectToAction("ReplicateTheValues");
        }

        [HttpGet]
        public ActionResult ReplicateTheValues()
        {
            string procname = "TruncateTable";
            _CompanyService.TruncateTable(procname);

            //Replicate The Bank Master Values
            var BankMasters = TempData["BankMasters"] as IEnumerable<BankMaster>;
            foreach (var Bank in BankMasters)
            {
                Bank.modifiedon = DateTime.Now;
                _BankService.CreateBank(Bank);
            }

            //Replicate The Client Master Values
            var ClientMasters = TempData["ClientMasters"] as IEnumerable<ClientMaster>;
            foreach (var Client in ClientMasters)
            {
                Client.ModifiedOn = DateTime.Now;
                _ClientMasterService.CreateClient(Client);
            }

            //Replicate The Client Bank Values
            var ClientBanks = TempData["ClientBanks"] as IEnumerable<ClientBankDetail>;
            foreach (var Bank in ClientBanks)
            {
                Bank.ModifiedOn = DateTime.Now;
                _ClientBankDetailService.CreateBankDetails(Bank);
            }

            //Replicate The Designation Master Values
            var DesignationMasters = TempData["DesignationMasters"] as IEnumerable<DesignationMaster>;
            foreach (var Designation in DesignationMasters)
            {
                Designation.ModifiedOn = DateTime.Now;
                _DesignationMasterService.Create(Designation);
            }

            //Replicate The Design Master Values
            var DesignMasters = TempData["DesignMasters"] as IEnumerable<DesignMaster>;
            foreach (var Design in DesignMasters)
            {
                Design.ModifiedOn = DateTime.Now;
                _DesignService.CreateDesign(Design);
            }

            //Replicate The Brand Master Values
            var BrandMasters = TempData["BrandMasters"] as IEnumerable<BrandMaster>;
            foreach (var Brand in BrandMasters)
            {
                Brand.ModifiedOn = DateTime.Now;
                _BrandMasterService.Create(Brand);
            }

            //Replicate The Employee Master Values
            var EmployeeMasters = TempData["EmployeeMasters"] as IEnumerable<EmployeeMaster>;
            foreach (var Employee in EmployeeMasters)
            {
                Employee.ModifiedOn = DateTime.Now;
                _EmployeeMasterService.CreateEmployee(Employee);
            }

            //Replicate The Godown Master Values
            var GodownMasters = TempData["GodownMasters"] as IEnumerable<GodownMaster>;
            foreach (var Godown in GodownMasters)
            {
                Godown.ModifiedOn = DateTime.Now;
                _GodownService.CreateGodown(Godown);
            }

            //Replicate The Godown Address Values
            var GodownAddresses = TempData["GodownAddresses"] as IEnumerable<GodownAddress>;
            foreach (var Address in GodownAddresses)
            {
                _GodownAddressService.CreateGoDown(Address);
            }

            //Replicate The Item Category Values
            var ItemCategory = TempData["ItemCategory"] as IEnumerable<ItemCategory>;
            foreach (var Category in ItemCategory)
            {
                Category.ModifiedOn = DateTime.Now;
                _ItemCategoryService.CreateItemCatgory(Category);
            }

            //Replicate The Item SubCategory Values
            var ItemSubCategory = TempData["ItemSubCategory"] as IEnumerable<ItemSubCategory>;
            foreach (var SubCategory in ItemSubCategory)
            {
                SubCategory.ModifiedOn = DateTime.Now;
                _ItemSubCategoryService.CreateItemCatgory(SubCategory);
            }

            //Replicate The Item Master Values
            var ItemMasters = TempData["ItemMasters"] as IEnumerable<Item>;
            foreach (var Item in ItemMasters)
            {
                Item.modifedOn = DateTime.Now;
                _ItemService.createItem(Item);
            }

            //Replicate The Non Inventory Item Values
            var NonInventoryItems = TempData["NonInventoryItems"] as IEnumerable<NonInventoryItem>;
            foreach (var NonInventoryItem in NonInventoryItems)
            {
                NonInventoryItem.modifedOn = DateTime.Now;
                _NonInventoryItemService.createNonInventoryItem(NonInventoryItem);
            }

            //Replicate The Barcode Numbers Values
            var Barcodes = TempData["BarcodeNumbers"] as IEnumerable<BarcodeNumber>;
            foreach (var Barcode in Barcodes)
            {
                _BarcodeNumberService.CreateBarcode(Barcode);
            }

            //Replicate The Job Worker Values
            var JobWorkers = TempData["JobWorkers"] as IEnumerable<JobWorker>;
            foreach (var JobWorker in JobWorkers)
            {
                JobWorker.ModifiedOn = DateTime.Now;
                _JobWorkerService.Create(JobWorker);
            }

            //Replicate The Job Worker Type Values
            var JobWorkerTypes = TempData["JobWorkerTypes"] as IEnumerable<JobWorkType>;
            foreach (var JobWorkerType in JobWorkerTypes)
            {
                JobWorkerType.ModifiedOn = DateTime.Now;
                _JobWorkTypeService.Create(JobWorkerType);
            }

            ////Replicate The Sales Incentives Values
            //var SalesIncentives = TempData["SalesIncentives"] as IEnumerable<SalesIncentiveMaster>;
            //foreach (var SalesIncentive in SalesIncentives)
            //{
            //    SalesIncentive.ModifiedOn = DateTime.Now;
            //    _SalesIncentiveService.CreateSI(SalesIncentive);
            //}

            //Replicate The Shop Master Values
            var ShopMasters = TempData["ShopMasters"] as IEnumerable<ShopMaster>;
            foreach (var Shop in ShopMasters)
            {
                Shop.ModifiedOn = DateTime.Now;
                _ShopService.Create(Shop);
            }

            //Replicate The Type Of Material Values
            var TypeOfMaterials = TempData["TypeOfMaterials"] as IEnumerable<TypeOfMaterial>;
            foreach (var Material in TypeOfMaterials)
            {
                Material.ModifiedOn = DateTime.Now;
                _TypeOfMaterialService.AddTypeOfMaterial(Material);
            }

            //Replicate The Unit Master Values
            var UnitMasters = TempData["UnitMasters"] as IEnumerable<UnitMaster>;
            foreach (var Unit in UnitMasters)
            {
                Unit.modifiedOn = DateTime.Now;
                _UnitService.createunit(Unit);
            }

            //Replicate The Supplier Master Values
            var SupplierMasters = TempData["SupplierMasters"] as IEnumerable<SupplierMaster>;
            foreach (var Supplier in SupplierMasters)
            {
                Supplier.ModifiedOn = DateTime.Now;
                _SuppliersMasterService.CreateSupplier(Supplier);
            }

            //Replicate The Supplier Bank Values
            var SupplierBanks = TempData["SupplierBanks"] as IEnumerable<SupplierBankDetail>;
            foreach (var Bank in SupplierBanks)
            {
                Bank.ModifiedOn = DateTime.Now;
                _SupplierBankDetailService.CreateBankDetails(Bank);
            }

            //Replicate The Transport Master Values
            var TransportMasters = TempData["TransportMasters"] as IEnumerable<TransportMaster>;
            foreach (var Transport in TransportMasters)
            {
                Transport.ModifiedOn = DateTime.Now;
                _TransportService.CreateTransport(Transport);
            }

            //Replicate The Tranport Bank Values
            var TransportBanks = TempData["TransportBanks"] as IEnumerable<TransportBankDetail>;
            foreach (var Bank in TransportBanks)
            {
                Bank.ModifiedOn = DateTime.Now;
                _TransportBankDetailService.CreateBankDetails(Bank);
            }

            //Replicate The User Values
            var Users = TempData["Users"] as IEnumerable<User>;
            foreach (var User in Users)
            {
                User.ModifiedBy = DateTime.Now;
                _UserService.CreateUser(User);
            }

            //Replicate The Commission Master Values
            var CommissionMasters = TempData["CommissionMasters"] as IEnumerable<CommissionMaster>;
            foreach (var Commission in CommissionMasters)
            {
                Commission.ModifiedOn = DateTime.Now;
                _CommissionService.Create(Commission);
            }

            //Replicate The Module Values
            var Modules = TempData["Modules"] as IEnumerable<Module>;
            foreach (var module in Modules)
            {
                _ModuleService.CreateModule(module);
            }

            //Replicate The UserCredential Values
            var UserCredentials = TempData["UserCredentials"] as IEnumerable<UserCredential>;
            foreach (var usercredential in UserCredentials)
            {
                _UserCredentialService.CreateUserCredential(usercredential);
            }

            //Replicate The Employee Company Values
            var EmployeeCompanies = TempData["EmployeeCompanies"] as IEnumerable<EmployeesCompany>;
            foreach (var EmpComp in EmployeeCompanies)
            {
                _EmployeesCompanyService.Create(EmpComp);
            }

            //CompanyCode = "RetailManagement";
            //CompanyName = "RetailManagement";
            //DatabaseName = "RetailManagement";
            CompanyCode = "A2ZRetail";
            CompanyName = "A2ZRetail";
            DatabaseName = "A2ZRetail";
            return RedirectToAction("ReplicationDone");
        }

        [HttpGet]
        public ActionResult ReplicationDone()
        {
            var CompanyData = _CompanyService.GetCompanyDataByCompCode(TempData["LogedInCompanyCode"].ToString());
            CompanyData.ModifiedOn = DateTime.Now;
            CompanyData.Replicated = "Yes";
            _CompanyService.UpdateCompany(CompanyData);
            return View();
        }
    }
}

using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
  public  class RequisitionForShopService:IRequisitionForShopService
    {
       private readonly IRequisitionForShopRepository _RequisitionForShop;
       private readonly IUnitOfWork _unitOfWork;
       private readonly IGetItemsFromItemMasterRepository _GetItemsFromItemMaster;

       public RequisitionForShopService(IRequisitionForShopRepository RequisitionForShop, IUnitOfWork unitOfWork,IGetItemsFromItemMasterRepository 
            GetItemsFromItemMasterRepository)
        {
            this._RequisitionForShop = RequisitionForShop;
            this._unitOfWork = unitOfWork;
            this._GetItemsFromItemMaster = GetItemsFromItemMasterRepository;
        }

       public IEnumerable<GetItemsFromItemMaster> StoredProcedureToFetchItemsFromItemMaster(string procname, object[] id)
         {
             var list = _GetItemsFromItemMaster.ExecuteCustomStoredProcByParam(procname, id);
             return list;
         }

        public void CreateRequisitionForShop(RequisitionForShop requisitionforshop)
        {
            _RequisitionForShop.Add(requisitionforshop);
            _unitOfWork .Commit();
        }

        public void Update(RequisitionForShop requisitionforshop)
        {
            _RequisitionForShop.Update(requisitionforshop);
            _unitOfWork.Commit();
        }

        public IEnumerable<RequisitionForShop> GetRequisitionsForPo(string value)
        {
            var list = _RequisitionForShop.GetMany(vl => vl.RequestToPo == value);
            return list;
        }

        public IEnumerable<RequisitionForShop> getRequisitionForShopItemDetailsByGodownName(string name)
        {
            var item = _RequisitionForShop.GetMany(co => co.ToGodownName == name);
            return item;
        }

        public IEnumerable<RequisitionForShop> getDetailsOfToShopIfNotNull()
        {
            var item = _RequisitionForShop.GetMany(c => c.ToShopName != "null");
            return item;

        }

        public IEnumerable<RequisitionForShop> getDetailsOfToGodownIfNotNull()
        {
            var item = _RequisitionForShop.GetMany(c => c.ToGodownName != "null");
            return item;

        }

        public IEnumerable<RequisitionForShop> getRequisitionForShopdetailsBiItemCodeAndDate(string code, DateTime? date, string shopname,string toshopname)
        {
            var item = _RequisitionForShop.GetMany(co => co.ItemCode == code && co.ModifiedOn == date && co.LoggedInShopName == shopname && co.ToShopName==toshopname);
            return item;

        }

        public RequisitionForShop GetDetailsByIcAndfsAndDTAndRS(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _RequisitionForShop.Get(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToShopName == requisitionfromshopName);
            return data;
        }

        public RequisitionForShop GetDetailsByIcAndfsAndDTAndRSForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromgodownname)
        {
            var data = _RequisitionForShop.Get(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromgodownname);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetDetailsByICAndFShNameIenumerableForRequisitionOfShop(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _RequisitionForShop.GetMany(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToShopName == requisitionfromshopName);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetDetailsByICAndFShNameIenumerableForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _RequisitionForShop.GetMany(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromshopName);
            return data;
        }
        public IEnumerable<RequisitionForShop> GetDetailsByICodeAndSNIenumerableSkipFirstForRequisitionOfShop(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _RequisitionForShop.GetMany(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToShopName == requisitionfromshopName).Skip(1);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetDetailsByICodeAndSNIenumerableSkipFirstForGodown(string itemcode, string fromshopname, DateTime? date, string requisitionfromshopName)
        {
            var data = _RequisitionForShop.GetMany(d => d.ItemCode == itemcode && d.LoggedInShopName == fromshopname && d.ModifiedOn == date && d.ToGodownName == requisitionfromshopName).Skip(1);
            return data;
        }
        public IEnumerable<RequisitionForShop> getRequisitionForShopdetailsBiItemCodeAndDateForGodown(string code, DateTime? date, string shopname, string togodownname)
        {
            var item = _RequisitionForShop.GetMany(co => co.ItemCode == code && co.ModifiedOn == date && co.LoggedInShopName == shopname && co.ToGodownName == togodownname);
            return item;

        }
        public IEnumerable<RequisitionForShop> GetRequisitionDetailsByItemnameandShopName(string itemname, string shopname)
        {
            var data = _RequisitionForShop.GetMany(d => d.ItemName == itemname && d.LoggedInShopName == shopname);
            return data;
        }

        public IEnumerable<RequisitionForShop> getItemDetailsByDateForRequisitionForShop(DateTime? fromdate, DateTime? todate)
        {
            var data = _RequisitionForShop.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate);
            return data;
        }

        public IEnumerable<RequisitionForShop> getItemDetailsByDateAndItemNameForRequisitionForShop(DateTime fromdate, DateTime todate, string itemname)
        {
            var data = _RequisitionForShop.GetMany(c => c.ModifiedOn >= fromdate && c.ModifiedOn <= todate && c.ItemName == itemname);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetNamesByItemForRequisitionForShop(string name, string shopname)
        {
            var namelist = _RequisitionForShop.GetMany(itemname => itemname.LoggedInShopName == shopname && itemname.ItemName.ToLower().Contains(name.ToString().ToLower())).OrderBy(itemname => itemname.ItemName);
            return namelist;
        }

        public RequisitionForShop GetLastRequisition()
        {
            var Details = _RequisitionForShop.GetAll().LastOrDefault();
            return Details;
        }

        public IEnumerable<RequisitionForShop> GetAll()
        {
            var Details = _RequisitionForShop.GetAll();
            return Details;
        }

        public RequisitionForShop GetLastRow()
        {
            var data = _RequisitionForShop.GetAll().LastOrDefault();
            return data;
        }

        public RequisitionForShop GetDetailsById(int id)
        {
            var details = _RequisitionForShop.Get(m => m.Id == id);
            return details;
        }

        public IEnumerable<RequisitionForShop> GetDailyRequisitionsForShops(string loggedinshopname)
        {
            var list = _RequisitionForShop.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == DateTime.Now.Date && s.ToShopName != "null" && s.ToShopName != null && s.LoggedInShopName != loggedinshopname);
            return list;
        }

        public IEnumerable<RequisitionForShop> GetDailyRequisitionsForGodowns(string loggedinshopname)
        {
            var list = _RequisitionForShop.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == DateTime.Now.Date && s.ToGodownName != "null" && s.ToGodownName != null && s.LoggedInShopName != loggedinshopname);
            return list;
        }

        public IEnumerable<RequisitionForShop> GetDailyRequisitionsForSuperAdmin(string loggedinshopname)
        {
            var list = _RequisitionForShop.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<RequisitionForShop> getAllActiveRequisitions()
        {
            var item = _RequisitionForShop.GetMany(c => c.Balance != 0 && c.Balance != null);
            return item;
        }


        public IEnumerable<RequisitionForShop> GetRowsByFromAndToDate(DateTime? fromdate, DateTime? todate)
        {
            var data = _RequisitionForShop.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetRowsByFromAndToDateAndSubCat(DateTime? fromdate, DateTime? todate, string subcat)
        {
            var data = _RequisitionForShop.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date && c.SubCategory == subcat);
            return data;
        }

        public IEnumerable<RequisitionForShop> GetRowsByFromAndToDateAndItemCode(DateTime? fromdate, DateTime? todate, string itemcode)
        {
            var data = _RequisitionForShop.GetMany(c => Convert.ToDateTime(c.ModifiedOn).Date >= Convert.ToDateTime(fromdate).Date && Convert.ToDateTime(c.ModifiedOn).Date <= Convert.ToDateTime(todate).Date && c.ItemCode == itemcode);
            return data;
        }
    }
}

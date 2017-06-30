using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class SalesBillItemService : ISalesBillItemService
    {
        private readonly ISalesBillItemRepository _isalesbillitemrepository;
        private readonly IUnitOfWork _iunitofwork;
        public SalesBillItemService(ISalesBillItemRepository salesbillitem, IUnitOfWork unitofwork)
        {
            this._isalesbillitemrepository = salesbillitem;
            this._iunitofwork = unitofwork;
        }

        public void Create(SalesBillItem bill)
        {
            _isalesbillitemrepository.Add(bill);
            _iunitofwork.Commit();
        }

        public void Update(SalesBillItem bill)
        {
            _isalesbillitemrepository.Update(bill);
            _iunitofwork.Commit();
        }

        public void Delete(SalesBillItem bill)
        {
            _isalesbillitemrepository.Delete(bill);
            _iunitofwork.Commit();
        }

        public SalesBillItem GetItemById(int id)
        {
            var data = _isalesbillitemrepository.GetById(id);
            return data;
        }

        public IEnumerable<SalesBillItem> GetItemsBySalesBillNo(string bill)
        {
            var details = _isalesbillitemrepository.GetMany(b => b.SalesBillNo == bill);
            return details;
        }

        public SalesBillItem GetItemDetailsByinvoiceNoAndItemCode(string code, string itemcode)
        {
            var data = _isalesbillitemrepository.Get(s => s.SalesBillNo == code && s.ItemCode == itemcode);
            return data;
        }

        public IEnumerable<SalesBillItem> GetItemsByCodeAndType(string no)
        {
            var data = _isalesbillitemrepository.GetMany(n => n.SalesBillNo == no && n.ItemType == "Inventory");
            return data;
        }

        public IEnumerable<SalesBillItem> GetTaxFreeItemsByDate(DateTime Date)
        {
            var data = _isalesbillitemrepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<SalesBillItem> GetOnePerTaxItemsByDate(DateTime Date)
        {
            var data = _isalesbillitemrepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<SalesBillItem> GetFivePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _isalesbillitemrepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<SalesBillItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _isalesbillitemrepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "12.5");
            return data;
        }

        public IEnumerable<SalesBillItem> GetActiveSalesBillNo(string no)
        {
            var data = _isalesbillitemrepository.GetMany(d => d.SalesBillNo == no && d.Status == "Active");
            return data;
        }


        public IEnumerable<SalesBillItem> GetTaxFreeItemsByBillNo(string billno)
        {
            var data = _isalesbillitemrepository.GetMany(r => r.SalesBillNo == billno && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<SalesBillItem> GetOnePerTaxItemsByBillNo(string billno)
        {
            var data = _isalesbillitemrepository.GetMany(r => r.SalesBillNo == billno && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<SalesBillItem> GetFivePointFivePerTaxItemsByBillNo(string billno)
        {
            var data = _isalesbillitemrepository.GetMany(r => r.SalesBillNo == billno && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<SalesBillItem> GetTwelvePointFivePerTaxItemsByBillNo(string billno)
        {
            var data = _isalesbillitemrepository.GetMany(r => r.SalesBillNo == billno && r.ItemTax == "12.5");
            return data;
        }
    }
}

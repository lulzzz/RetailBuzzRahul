using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class RetailBillItemService : IRetailBillItemService
    {
        private readonly IRetailBillItemRepository _RetailInvoiceItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public RetailBillItemService(IRetailBillItemRepository RetailInvoiceItemRepository, IUnitOfWork UnitOfWork)
        {
            this._RetailInvoiceItemRepository = RetailInvoiceItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(RetailBillItem retailinvoiceitem)
        {
            _RetailInvoiceItemRepository.Add(retailinvoiceitem);
            _UnitOfWork.Commit();
        }

        public void Update(RetailBillItem retailinvoiceitem)
        {
            _RetailInvoiceItemRepository.Update(retailinvoiceitem);
            _UnitOfWork.Commit();
        }

        public void Delete(RetailBillItem retailinvoiceitem)
        {
            _RetailInvoiceItemRepository.Delete(retailinvoiceitem);
            _UnitOfWork.Commit();
        }

        public RetailBillItem GetById(int id)
        {
            var data = _RetailInvoiceItemRepository.GetById(id);
            return data;
        }

        public RetailBillItem GetLastItem()
        {
            var data = _RetailInvoiceItemRepository.GetAll().LastOrDefault();
            return data;
        }
        public IEnumerable<RetailBillItem> GetAll()
        {
            var data = _RetailInvoiceItemRepository.GetAll();
            return data;
        }

        public IEnumerable<RetailBillItem> GetDetailsByInvoiceNo(string no)
        {
            var data = _RetailInvoiceItemRepository.GetMany(n => n.RetailBillNo == no);
            return data;
        }

        public IEnumerable<RetailBillItem> GetDetailsByRetailBillNo(string code)
        {
            var data = _RetailInvoiceItemRepository.GetMany(cl => cl.RetailBillNo == code);
            return data;
        }

        public RetailBillItem GetItemDetailsByBillNoAndItemCode(string code, string itemcode)
        {
            var data = _RetailInvoiceItemRepository.Get(r => r.RetailBillNo == code && r.ItemCode == itemcode);
            return data;
        }

        public IEnumerable<RetailBillItem> GetDetailsByInvoiceNoAndStatus(string no)
        {
            var data = _RetailInvoiceItemRepository.GetMany(n => n.RetailBillNo == no && n.Status == "Active");
            return data;
        }

        public IEnumerable<RetailBillItem> GetItemsByCodeAndType(string no)
        {
            var data = _RetailInvoiceItemRepository.GetMany(n => n.RetailBillNo == no && n.ItemType == "Inventory");
            return data;
        }

        public IEnumerable<RetailBillItem> GetTaxFreeItemsByDate(DateTime Date)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<RetailBillItem> GetOnePerTaxItemsByDate(DateTime Date)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<RetailBillItem> GetFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5");
            return data;
        }

        public IEnumerable<RetailBillItem> GetFivePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<RetailBillItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "12.5");
            return data;
        }


        public IEnumerable<RetailBillItem> GetTaxFreeItemsByBillNo(string billno)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => r.RetailBillNo == billno && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<RetailBillItem> GetOnePerTaxItemsByBillNo(string billno)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => r.RetailBillNo == billno && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<RetailBillItem> GetFivePerTaxItemsByBillNo(string billno)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => r.RetailBillNo == billno && r.ItemTax == "5");
            return data;
        }

        public IEnumerable<RetailBillItem> GetFivePointFivePerTaxItemsByBillNo(string billno)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => r.RetailBillNo == billno && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<RetailBillItem> GetTwelvePointFivePerTaxItemsByBillNo(string billno)
        {
            var data = _RetailInvoiceItemRepository.GetMany(r => r.RetailBillNo == billno && r.ItemTax == "12.5");
            return data;
        }
    }
}

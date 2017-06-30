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
    public class RetailBillCreditNoteItemService : IRetailBillCreditNoteItemService
    {
        private readonly IRetailBillCreditNoteItemRepository _CreditNoteItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public RetailBillCreditNoteItemService(IRetailBillCreditNoteItemRepository CreditNoteItemRepository, IUnitOfWork UnitOfWork)
        {
            this._CreditNoteItemRepository = CreditNoteItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(RetailBillCreditNoteItem CreditNoteItem)
        {
            _CreditNoteItemRepository.Add(CreditNoteItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<RetailBillCreditNoteItem> GetAllItemsByCreditNote(string creditnoteno)
        {
            var list = _CreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno && c.Balance != 0);
            return list;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetItemListByCreditNote(string creditnoteno)
        {
            var list = _CreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno);
            return list;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetItemListByCreditNoteandItemType(string creditnoteno)
        {
            var list = _CreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno && c.ItemType == "Inventory");
            return list;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetTaxFreeItemsByDate(DateTime Date)
        {
            var data = _CreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetOnePerTaxItemsByDate(DateTime Date)
        {
            var data = _CreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _CreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5");
            return data;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetFivePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _CreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<RetailBillCreditNoteItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _CreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "12.5");
            return data;
        }
    }
}

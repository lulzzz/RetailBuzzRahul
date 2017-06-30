using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class SalesBillCreditNoteItemService : ISalesBillCreditNoteItemService
    {
        private readonly ISalesBillCreditNoteItemRepository _SalesBillCreditNoteItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public SalesBillCreditNoteItemService(ISalesBillCreditNoteItemRepository SalesBillCreditNoteItemRepository, IUnitOfWork UnitOfWork)
        {
            this._SalesBillCreditNoteItemRepository = SalesBillCreditNoteItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(SalesBillCreditNoteItem SalesBillCreditNoteItem)
        {
            _SalesBillCreditNoteItemRepository.Add(SalesBillCreditNoteItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<SalesBillCreditNoteItem> GetAllItemsByCreditNote(string creditnoteno)
        {
            var list = _SalesBillCreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno && c.Balance != 0);
            return list;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetItemListByCreditNote(string creditnoteno)
        {
            var list = _SalesBillCreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno);
            return list;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetItemListByCreditNoteandItemType(string creditnoteno)
        {
            var list = _SalesBillCreditNoteItemRepository.GetMany(c => c.CreditNoteNo == creditnoteno && c.ItemType == "Inventory");
            return list;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetTaxFreeItemsByDate(DateTime Date)
        {
            var data = _SalesBillCreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "0");
            return data;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetOnePerTaxItemsByDate(DateTime Date)
        {
            var data = _SalesBillCreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "1");
            return data;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetFivePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _SalesBillCreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "5.5");
            return data;
        }

        public IEnumerable<SalesBillCreditNoteItem> GetTwelvePointFivePerTaxItemsByDate(DateTime Date)
        {
            var data = _SalesBillCreditNoteItemRepository.GetMany(r => Convert.ToDateTime(r.ModifiedOn).Date == Date && r.ItemTax == "12.5");
            return data;
        }
    }
}

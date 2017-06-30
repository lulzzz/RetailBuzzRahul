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
    public class TemporaryCashMemoItemService : ITemporaryCashMemoItemService
    {
        private readonly ITemporaryCashMemoItemRepository _TemporaryCashMemoItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public TemporaryCashMemoItemService(ITemporaryCashMemoItemRepository TemporaryCashMemoItemRepository, IUnitOfWork UnitOfWork)
        {
            this._TemporaryCashMemoItemRepository = TemporaryCashMemoItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(TemporaryCashMemoItem cashmemoitem)
        {
            _TemporaryCashMemoItemRepository.Add(cashmemoitem);
            _UnitOfWork.Commit();
        }

        public void Update(TemporaryCashMemoItem cashmemoitem)
        {
            _TemporaryCashMemoItemRepository.Update(cashmemoitem);
            _UnitOfWork.Commit();
        }

        public TemporaryCashMemoItem GetById(int id)
        {
            var data = _TemporaryCashMemoItemRepository.GetById(id);
            return data;
        }

        public TemporaryCashMemoItem GetLastItem()
        {
            var data = _TemporaryCashMemoItemRepository.GetAll().LastOrDefault();
            return data;
        }
        public IEnumerable<TemporaryCashMemoItem> GetAll()
        {
            var data = _TemporaryCashMemoItemRepository.GetAll();
            return data;
        }

        public IEnumerable<TemporaryCashMemoItem> GetDetailsByInvoiceNo(string no)
        {
            var data = _TemporaryCashMemoItemRepository.GetMany(n => n.TempCashMemolNo == no);
            return data;
        }

        public IEnumerable<TemporaryCashMemoItem> GetDetailsByRetailBillNo(string code)
        {
            var data = _TemporaryCashMemoItemRepository.GetMany(cl => cl.TempCashMemolNo == code);
            return data;
        }

        public TemporaryCashMemoItem GetItemDetailsByBillNoAndItemCode(string code, string itemcode)
        {
            var data = _TemporaryCashMemoItemRepository.Get(r => r.TempCashMemolNo == code && r.ItemCode == itemcode);
            return data;
        }

        public IEnumerable<TemporaryCashMemoItem> GetDetailsByInvoiceNoAndStatus(string no)
        {
            var data = _TemporaryCashMemoItemRepository.GetMany(n => n.TempCashMemolNo == no && n.Status == "Active");
            return data;
        }

        public IEnumerable<TemporaryCashMemoItem> GetItemsByCodeAndType(string no)
        {
            var data = _TemporaryCashMemoItemRepository.GetMany(n => n.TempCashMemolNo == no && n.ItemType == "Inventory");
            return data;
        }

    }
}

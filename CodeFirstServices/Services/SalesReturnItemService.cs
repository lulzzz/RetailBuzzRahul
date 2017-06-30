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
    public class SalesReturnItemService : ISalesReturnItemService
    {
        private readonly ISalesReturnItemRepository _SalesReturnItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public SalesReturnItemService(ISalesReturnItemRepository SalesReturnItemRepository, IUnitOfWork UnitOfWork)
        {
            this._SalesReturnItemRepository = SalesReturnItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(SalesReturnItem SalesReturnItem)
        {
            _SalesReturnItemRepository.Add(SalesReturnItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<SalesReturnItem> GetAllNewItemsByBill(string billno)
        {
            var list = _SalesReturnItemRepository.GetMany(s => s.BillNo == billno && s.Balance != 0);
            return list;
        }

        public IEnumerable<SalesReturnItem> GetAllItemsBySales(string salesreturnno)
        {
            var list = _SalesReturnItemRepository.GetMany(s => s.SalesReturnNo == salesreturnno && s.Balance != 0);
            return list;
        }

        public IEnumerable<SalesReturnItem> GetItemListBySalesReturnNo(string salesreturnno)
        {
            var list = _SalesReturnItemRepository.GetMany(s => s.SalesReturnNo == salesreturnno);
            return list;
        }

        public IEnumerable<SalesReturnItem> GetItemListByBillNo(string no)
        {
            var list = _SalesReturnItemRepository.GetMany(s => s.SalesReturnNo == no && s.ItemType == "Inventory");
            return list;
        }
    }
}

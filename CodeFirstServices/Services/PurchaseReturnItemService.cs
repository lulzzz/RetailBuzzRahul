using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;

namespace CodeFirstServices.Services
{
    public class PurchaseReturnItemService : IPurchaseReturnItemService
    {
        private readonly IPurchaseReturnItemRepository _PurchaseReturnItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public PurchaseReturnItemService(IPurchaseReturnItemRepository PurchaseReturnItemRepository, IUnitOfWork UnitOfWork)
        {
            this._PurchaseReturnItemRepository = PurchaseReturnItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(PurchaseReturnItem PurchaseReturnItem)
        {
            _PurchaseReturnItemRepository.Add(PurchaseReturnItem);
            _UnitOfWork.Commit();
        }

        public void Update(PurchaseReturnItem PurchaseReturnItem)
        {
            _PurchaseReturnItemRepository.Update(PurchaseReturnItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<PurchaseReturnItem> GetItemsByPurchaseReturnNo(string code)
        {
            var list = _PurchaseReturnItemRepository.GetMany(p => p.PurchaseReturnNo == code);
            return list;
        }

        public IEnumerable<PurchaseReturnItem> GetReturnItemsByReturnNo(string returnno)
        {
            var list = _PurchaseReturnItemRepository.GetMany(p => p.PurchaseReturnNo == returnno && p.Balance != 0);
            return list;
        }
    }
}

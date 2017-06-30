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
    public class OutwardItemStockDistributionService : IOutwardItemStockDistributionService
    {
        private readonly IOutwardItemStockDistributionRepository _outwarditemtogodownrepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardItemStockDistributionService(IOutwardItemStockDistributionRepository outwarditemtogodownrepository, IUnitOfWork unitOfWork)
        {
            this._outwarditemtogodownrepository = outwarditemtogodownrepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardItemStockDistribution data)
        {
            _outwarditemtogodownrepository.Add(data);
            _unitOfWork.Commit();
        }

        public OutwardItemStockDistribution GetLastRowFromItemAndGodownCode(string itemcode, string godowncode)
        {
            var data = _outwarditemtogodownrepository.GetMany(r => r.ItemCode == itemcode && r.Code == godowncode).LastOrDefault();
            return data;
        }

        public IEnumerable<OutwardItemStockDistribution> GetItemsByOutwardCode(string code)
        {
            var list = _outwarditemtogodownrepository.GetMany(m => m.OutwardCode == code);
            return list;
        }

        public void Update(OutwardItemStockDistribution data)
        {
            _outwarditemtogodownrepository.Update(data);
            _unitOfWork.Commit();
        }

        public OutwardItemStockDistribution GetItemDetailsByItemCode(string itemcode)
        {
            var details = _outwarditemtogodownrepository.Get(m => m.ItemCode == itemcode);
            return details;
        }

        public IEnumerable<OutwardItemStockDistribution> GetItemsById(int id)
        {
            var list = _outwarditemtogodownrepository.GetMany(m => m.ItemId == id);
            return list;
        }

        public OutwardItemStockDistribution GetItemByItemAndOutwardCode(string outwardcode, string itemcode)
        {
            var details = _outwarditemtogodownrepository.Get(m => m.OutwardCode == outwardcode && m.ItemCode == itemcode);
            return details;
        }
    }
}

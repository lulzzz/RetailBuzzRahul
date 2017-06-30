using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class InwardItemFromStockDistributionService : IInwardItemFromStockDistributionService
    {
        private readonly IInwardItemFromStockDistributionRepository _InwardItemFromStockDistributionRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public InwardItemFromStockDistributionService(IInwardItemFromStockDistributionRepository InwardItemFromStockDistributionRepository,
            IUnitOfWork UnitOfWork)
        {
            this._InwardItemFromStockDistributionRepository = InwardItemFromStockDistributionRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(InwardItemFromStockDistribution item)
        {
            _InwardItemFromStockDistributionRepository.Add(item);
            _UnitOfWork.Commit();
        }


        public IEnumerable<InwardItemFromStockDistribution> GetItemListByCode(string code)
        {
           var det = _InwardItemFromStockDistributionRepository.GetMany(m => m.InwardNo == code);
           return det;
        }
    }
}

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
    public class ResetSalesBillItemService : IResetSalesBillItemService
    {
        private readonly IResetSalesBillItemRepository _ResetSalesBillItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public ResetSalesBillItemService(IResetSalesBillItemRepository ResetSalesBillItemRepository, IUnitOfWork UnitOfWork)
        {
            this._ResetSalesBillItemRepository = ResetSalesBillItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(ResetSalesBillItem SalesBillItem)
        {
            _ResetSalesBillItemRepository.Add(SalesBillItem);
            _UnitOfWork.Commit();
        }
    }
}

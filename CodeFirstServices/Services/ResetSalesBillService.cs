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
    public class ResetSalesBillService : IResetSalesBillService
    {
        private readonly IResetSalesBillRepository _ResetSalesBillRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public ResetSalesBillService(IResetSalesBillRepository ResetSalesBillRepository, IUnitOfWork UnitOfWork)
        {
            this._ResetSalesBillRepository = ResetSalesBillRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(ResetSalesBill SalesBill)
        {
            _ResetSalesBillRepository.Add(SalesBill);
            _UnitOfWork.Commit();
        }
    }
}

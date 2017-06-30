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
    public class ResetSalesBillAdjAmtDetailService : IResetSalesBillAdjAmtDetailService
    {
        private readonly IResetSalesBillAdjAmtDetailRepository _ResetSalesBillAdjAmtDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ResetSalesBillAdjAmtDetailService(IResetSalesBillAdjAmtDetailRepository ResetSalesBillAdjAmtDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._ResetSalesBillAdjAmtDetailRepository = ResetSalesBillAdjAmtDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(ResetSalesBillAdjAmtDetail SalesBillAdjAmt)
        {
            _ResetSalesBillAdjAmtDetailRepository.Add(SalesBillAdjAmt);
            _UnitOfWork.Commit();
        }
    }
}

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
    public class CashierSalesBillItemService:ICashierSalesBillItemService
    {
        private readonly ICashierSalesBillItemRepository _CashierDeliveryChallanItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierSalesBillItemService(ICashierSalesBillItemRepository CashierDeliveryChallanItemRepository, IUnitOfWork unitOfWork)
        {
            this._CashierDeliveryChallanItemRepository = CashierDeliveryChallanItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierSalesBillItem cash)
        {
            _CashierDeliveryChallanItemRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierSalesBillItem> GetDetailsByCashierCode(string code)
        {
            var data = _CashierDeliveryChallanItemRepository.GetMany(d => d.CashierCode == code);
            return data;
        }
    }
}

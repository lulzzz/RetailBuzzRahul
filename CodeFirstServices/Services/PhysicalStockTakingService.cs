using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Services
{
    public class PhysicalStockTakingService : IPhysicalStockTakingService
    {
        private readonly IPhysicalStockTakingRepository _PhysicalStockTakingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PhysicalStockTakingService(IPhysicalStockTakingRepository PhysicalStockTakingRepository, IUnitOfWork unitOfWork)
        {
            this._PhysicalStockTakingRepository = PhysicalStockTakingRepository;
            this._unitOfWork = unitOfWork;
        }

        public PhysicalStockTaking GetLastRow()
        {
            var details = _PhysicalStockTakingRepository.GetAll().LastOrDefault();
            return details;
        }


        public void Create(PhysicalStockTaking stock)
        {
            _PhysicalStockTakingRepository.Add(stock);
            _unitOfWork.Commit();
        }


        public IEnumerable<PhysicalStockTaking> GetRowsByCode(string Code)
        {
            var details = _PhysicalStockTakingRepository.GetMany(m => m.PhysicalStockTakingCode == Code);
            return details;
        }
    }
}

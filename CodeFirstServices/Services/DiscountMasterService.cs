using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class DiscountMasterService : IDiscountMasterService
    {
        private readonly IDiscountMasterRepository _DiscountMasterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DiscountMasterService(IDiscountMasterRepository DiscountMasterRepository, IUnitOfWork unitOfWork)
        {
            this._DiscountMasterRepository = DiscountMasterRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(DiscountMaster discount)
        {
            _DiscountMasterRepository.Add(discount);
            _unitOfWork.Commit();
        }

        public DiscountMaster getLastRow()
        {
            var data = _DiscountMasterRepository.GetAll().LastOrDefault();
            return data;
        }

        public DiscountMaster GetLastRowrByFinYr(string year)
        {
            var data = _DiscountMasterRepository.GetMany(p => p.DiscountCode.Contains(year)).OrderBy(p => p.DiscountCode).LastOrDefault();
            return data;
           
        }

        public DiscountMaster GetDetailsById(int id)
        {
            var data = _DiscountMasterRepository.GetById(id);
            return data;
        }

        public IEnumerable<DiscountMaster> getItemsByDiscount(DateTime? year)
        {
            var data = _DiscountMasterRepository.GetMany(p => p.ToDate >= Convert.ToDateTime(year));
            return data;
        }


    }
}

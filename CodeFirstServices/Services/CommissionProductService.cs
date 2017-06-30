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
    public class CommissionProductService : ICommissionProductService
    {
        private readonly ICommissionProductRepository _CommissionProductRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CommissionProductService(ICommissionProductRepository CommissionProductRepository, IUnitOfWork unitOfWork)
        {
            this._CommissionProductRepository = CommissionProductRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CommissionProduct CommProduct)
        {
            _CommissionProductRepository.Add(CommProduct);
            _unitOfWork.Commit();
        }


        public IEnumerable<CommissionProduct> GetDetailsByCommCode(string Code)
        {
            var details = _CommissionProductRepository.GetMany(c => c.CommissionCode == Code);
            return details;
        }


        public IEnumerable<CommissionProduct> GetAllEmployees()
        {
            var details = _CommissionProductRepository.GetMany(c => c.Status == "Active");
            return details;
        }


        public IEnumerable<CommissionProduct> GetDetailsByEmployeeName(string Name)
        {
            var details = _CommissionProductRepository.GetMany(c => c.EmployeeName == Name);
            return details;
        }


        public void Update(CommissionProduct comm)
        {
            _CommissionProductRepository.Update(comm);
            _unitOfWork.Commit();
        }


        public void Delete(CommissionProduct Comm)
        {
            _CommissionProductRepository.Delete(Comm);
            _unitOfWork.Commit();
        }


        public CommissionProduct GetSingleRowByCommCode(string Code)
        {
            var details = _CommissionProductRepository.Get(m => m.CommissionCode == Code);
            return details;
        }
    }
}

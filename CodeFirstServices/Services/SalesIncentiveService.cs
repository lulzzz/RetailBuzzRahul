using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;


namespace CodeFirstServices.Services
{
    public class SalesIncentiveService:ISalesIncentiveService
    {
        private readonly ISalesIncentiveRepository _salesincentivemasterRepository;
        private readonly IUnitOfWork _unitofwork;

        public SalesIncentiveService(ISalesIncentiveRepository salesincentivemasterRepository, IUnitOfWork unitofwork)
        {
            this._salesincentivemasterRepository = salesincentivemasterRepository;
            this._unitofwork = unitofwork;
        }

        public void CreateSI(SalesIncentiveMaster salesincentive)
        {
            _salesincentivemasterRepository.Add(salesincentive);
            _unitofwork.Commit();
        }

        public void UpdateSI(SalesIncentiveMaster salesincentive)
        {
            _salesincentivemasterRepository.Update(salesincentive);
            _unitofwork.Commit();
        }

        public void DeleteSI(SalesIncentiveMaster salesincentive)
        {
            _salesincentivemasterRepository.Delete(salesincentive);
            _unitofwork.Commit();
        }

        public SalesIncentiveMaster GetSIById(int Id)
        {
            var data = _salesincentivemasterRepository.Get(d => d.SIId == Id);
            return data;
        }

        public IEnumerable<SalesIncentiveMaster> GetSalesIncentiveDetails()
        {
            var list = _salesincentivemasterRepository.GetMany(l => l.Status == "Active");
            return list;
        }

        public IEnumerable<SalesIncentiveMaster> GetAllSI()
        {
            var details = _salesincentivemasterRepository.GetAll();
            return details;
        }
        //public SalesIncentiveMaster GetSIById(int Id)
        //{
        //    var data = _salesincentivemasterRepository.Get(d => d.SIId == Id);
        //    return data;
        //}

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class JobWorkStockService : IJobWorkStockService
    {
        public readonly IJobWorkStockRepository _jobworkstockrepository;
        private readonly IUnitOfWork _unitofwork;
        public JobWorkStockService(IJobWorkStockRepository jobworkstockrepository, IUnitOfWork unitofwork)
        {
            this._jobworkstockrepository = jobworkstockrepository;
            this._unitofwork = unitofwork;
        }

        public void Create(JobWorkStock jobworkstockitem)
        {
            _jobworkstockrepository.Add(jobworkstockitem);
            _unitofwork.Commit();
        }

        public void Update(JobWorkStock jobworkstockitem)
        {
            _jobworkstockrepository.Update(jobworkstockitem);
            _unitofwork.Commit();
        }

        public IEnumerable<JobWorkStock> GetRowsByOutwardNo(string outwardno)
        {
            var data = _jobworkstockrepository.GetMany(d => d.OutwardCode == outwardno);
            return data;
        }

        public IEnumerable<JobWorkStock> GetReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _jobworkstockrepository.GetMany(cl => cl.StockDate.Date >= fromdate.Date && cl.StockDate.Date <= todate.Date && cl.Status == "Active");
            return value;
        }

        public IEnumerable<JobWorkStock> GetActiveRowsByOutwardNo(string outwardno)
        {
            var data = _jobworkstockrepository.GetMany(d => d.OutwardCode == outwardno && d.Status == "Active");
            return data;
        }

        public IEnumerable<JobWorkStock> GetActiveStock()
        {
            var data = _jobworkstockrepository.GetMany(d => d.Status == "Active");
            return data;
        }

    }
}
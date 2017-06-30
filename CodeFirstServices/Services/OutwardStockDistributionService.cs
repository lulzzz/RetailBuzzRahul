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
    public class OutwardStockDistributionService : IOutwardStockDistributionService
    {
        private readonly IOutwardStockDistributionRepository _outwardtogodownrepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardStockDistributionService(IOutwardStockDistributionRepository outwardtogodownrepository, IUnitOfWork unitOfWork)
        {
            this._outwardtogodownrepository = outwardtogodownrepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardStockDistribution data)
        {
            _outwardtogodownrepository.Add(data);
            _unitOfWork.Commit();
        }


        public IEnumerable<OutwardStockDistribution> GetOutwardNo(string term,string code)
        {
            var details = _outwardtogodownrepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.Code == code && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }


        public OutwardStockDistribution GetOutwardByCode(string code)
        {
            var data = _outwardtogodownrepository.Get(m => m.OutwardCode == code);
            return data;
        }

        public void Update(OutwardStockDistribution data)
        {
            _outwardtogodownrepository.Update(data);
            _unitOfWork.Commit();
        }

        public OutwardStockDistribution GetDetailsById(int id)
        {
            var details = _outwardtogodownrepository.GetById(id);
            return details;
        }

        public IEnumerable<OutwardStockDistribution> GetOutwardStkDisNo(string term)
        {
            var details = _outwardtogodownrepository.GetMany(m => m.OutwardCode.ToLower().StartsWith(term.ToLower()) && m.Status == "Active").OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardStockDistribution> GetAllOutwardNos(string no)
        {
            var data = _outwardtogodownrepository.GetMany(b => b.OutwardCode.ToLower().StartsWith(no.ToString().ToLower())).OrderBy(b => b.OutwardCode);
            return data;
        }
    }
}

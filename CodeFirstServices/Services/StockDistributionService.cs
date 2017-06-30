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
    public class StockDistributionService:IStockDistributionService
    {
        private readonly IStockDistributionRepository _stockdistributionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public StockDistributionService(IStockDistributionRepository stockdistributionRepository, IUnitOfWork unitOfWork)
        {
            this._stockdistributionRepository = stockdistributionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(StockDistribution stock)
        {
            _stockdistributionRepository.Add(stock);
            _unitOfWork.Commit();
        }

        public StockDistribution GetLastRow()
        {
            var data = _stockdistributionRepository.GetAll().LastOrDefault();
            return data;
        }

        public StockDistribution GetDetailsByCode(string Code)
        {
            var details = _stockdistributionRepository.Get(d => d.StockDistributionCode == Code);
            return details;
        }

        public IEnumerable<StockDistribution> GetDetailsByGodownCode(string godowncode)
        {
            var data = _stockdistributionRepository.GetMany(d => d.Code == godowncode);
            return data;
        }

        public StockDistribution GetDetailsById(int id)
        {
            var details = _stockdistributionRepository.GetById(id);
            return details;
        }

        public StockDistribution GetLastStockDisByFinYr(string year)
        {
            var data = _stockdistributionRepository.GetMany(p => p.StockDistributionCode.Contains(year)).OrderBy(p => p.StockDistributionCode).LastOrDefault();
            return data;
        }

        public IEnumerable<StockDistribution> GetItemsByStatus()
        {
            var data = _stockdistributionRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<StockDistribution> GetStkDisNos(string no)
        {
            var data = _stockdistributionRepository.GetMany(b => b.StockDistributionCode.ToLower().StartsWith(no.ToString().ToLower()) && b.Status == "Active").OrderBy(b => b.StockDistributionCode);
            return data;
        }

        public StockDistribution GetReferenceNo(string RefNo)
        {
            var details = _stockdistributionRepository.Get(st => st.GatePass == RefNo);
            return details;
        }
    }
}

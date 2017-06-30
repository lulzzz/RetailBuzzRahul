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
    public class StockItemDistributionService : IStockItemDistributionService
    {
        private readonly IStockItemDistributionRepository _stockitemdistributionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListOfItemCodeRepository _listofitemcoderepository;
        private readonly IGetAllItemsFromStkDistrbtnRepository _GetAllItemsFromStkDistrbtnRepository;
        public StockItemDistributionService(IStockItemDistributionRepository stockitemdistributionRepository, IUnitOfWork unitOfWork, IListOfItemCodeRepository listofitemcoderepository, IGetAllItemsFromStkDistrbtnRepository GetAllItemsFromStkDistrbtnRepository)
        {
            this._stockitemdistributionRepository = stockitemdistributionRepository;
            this._unitOfWork = unitOfWork;
            this._GetAllItemsFromStkDistrbtnRepository = GetAllItemsFromStkDistrbtnRepository;
            this._listofitemcoderepository = listofitemcoderepository;
        }

        public void Create(StockItemDistribution stock)
        {
            _stockitemdistributionRepository.Add(stock);
            _unitOfWork.Commit();
        }

        public void Update(StockItemDistribution stock)
        {
            _stockitemdistributionRepository.Update(stock);
            _unitOfWork.Commit();
        }

        public IEnumerable<StockItemDistribution> GetRowsByItemCode(string code)
        {
            var data = _stockitemdistributionRepository.GetMany(c => c.ItemCode == code);
            return data;
        }

        public StockItemDistribution GetDetailsByItemCode(string code)
        {
            var data = _stockitemdistributionRepository.Get(c => c.ItemCode == code);
            return data;
        }

        public StockItemDistribution GetLastRowFromItemAndGodownCode(string itemcode, string godowncode)
        {
            var data = _stockitemdistributionRepository.GetMany(r => r.ItemCode == itemcode && r.Code == godowncode).LastOrDefault();
            return data;
        }

        public StockItemDistribution GetDetailsByCode(string Code)
        {
            var details = _stockitemdistributionRepository.Get(d => d.StockDistributionCode == Code);
            return details;
        }

        public IEnumerable<StockItemDistribution> GetDetailsByGodownCode(string code)
        {
            var data = _stockitemdistributionRepository.GetMany(c => c.Code == code);
            return data;
        }

        public IEnumerable<StockItemDistribution> GetRowsByStockDistributionCode(string code)
        {
            var data = _stockitemdistributionRepository.GetMany(c => c.StockDistributionCode == code);
            return data;
        }

        public IEnumerable<ListOfItemCode> GetItemsUsingStoreProc(string procname, object[] GodownCode)
        {
            var data = _listofitemcoderepository.ExecuteCustomStoredProcByParam(procname, GodownCode);
            return data;
        }

        public StockItemDistribution GetDetailsByItemCodeAndGodownCode(string itemcode, string godowncode)
        {
            var data = _stockitemdistributionRepository.Get(d => d.ItemCode == itemcode && d.Code == godowncode);
            return data;
        }

        public IEnumerable<StockItemDistribution> GetAll()
        {
            var details = _stockitemdistributionRepository.GetAll();
            return details;
        }

        public StockItemDistribution GetDetailsByItemCodeAndShopCode(string itemcode, string shopcode)
        {
            var data = _stockitemdistributionRepository.Get(d => d.ItemCode == itemcode && d.Code == shopcode);
            return data;
        }

        public IEnumerable<GetAllItemsFromStkDistrbtn> GetItemsBySubcategoryUsingProc(string procname, object[] Subcat)
        {
            var list = _GetAllItemsFromStkDistrbtnRepository.ExecuteCustomStoredProcByParam(procname, Subcat);
            return list;
        }

        public IEnumerable<StockItemDistribution> GetDetailsByStkCode(string Code)
        {
            var details = _stockitemdistributionRepository.GetMany(d => d.StockDistributionCode == Code);
            return details;
        }
    }
}
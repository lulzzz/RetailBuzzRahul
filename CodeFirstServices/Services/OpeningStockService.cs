using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class OpeningStockService : IOpeningStockService
    {
        private readonly IOpeningStockRepository _stockRepository;
        private readonly IUnitOfWork _unitofwork;

        public OpeningStockService(IOpeningStockRepository stockRepository, IUnitOfWork unitofwork)
        {
            this._stockRepository = stockRepository;
            this._unitofwork = unitofwork;
        }

        public void CreateStock(OpeningStockMaster stock)
        {
            _stockRepository.Add(stock);
            _unitofwork.Commit();
        }

        public void UpdateStock(OpeningStockMaster stock)
        {
            _stockRepository.Update(stock);
            _unitofwork.Commit();
        }

        public OpeningStockMaster GetLastRow()
        {
            var row = _stockRepository.GetAll().LastOrDefault();
            return row;
        }

        public OpeningStockMaster GetById(int Id)
        {
            var data = _stockRepository.GetById(Id);
            return data;
        }

        public OpeningStockMaster GetTotalQuantityByItem(string code)
        {
            var data = _stockRepository.Get(m => m.itemCode == code&& m.Status=="Active");
            return data;
        }

        public IEnumerable<OpeningStockMaster> GetListByStockCode(string stockcode)
        {
            var data = _stockRepository.GetMany(m => m.OpeningStockCode == stockcode);
            return data;
        }

        public OpeningStockMaster GetDataByItemCode(string code)
        {
            var data = _stockRepository.Get(c => c.itemCode == code && c.Status == "Active");
            return data;
        }

        public OpeningStockMaster GetAllItemsByItemCode(string code)
        {
            var data = _stockRepository.Get(c => c.itemCode == code);
            return data;
        }

        public OpeningStockMaster GetDetailsByItemCode(string code)
        {
            var data = _stockRepository.Get(m => m.itemCode == code);
            return data;
        }

        public IEnumerable<OpeningStockMaster> GetDetailsByDateAndCategory(DateTime fromdate,DateTime todate,string category)
        {
            var data = _stockRepository.GetMany(d => Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date && d.Category == category);
            return data;
        }

        public IEnumerable<OpeningStockMaster> GetDetailsBySubCategory(string subcat)
        {
            var data = _stockRepository.GetMany(d => d.SubCategory == subcat);
            return data;
        }

        public IEnumerable<OpeningStockMaster> GetAllItems()
        {
            var data = _stockRepository.GetAll();
            return data;
        }

        public IEnumerable<OpeningStockMaster> GetItemsByStatus()
        {
            var data = _stockRepository.GetMany(d=>d.Status=="Active");
            return data;
        }

        public OpeningStockMaster GetDetailsByStockCode(string code)
        {
            var data = _stockRepository.Get(m => m.OpeningStockCode == code);
            return data;
        }

        public OpeningStockMaster GetLastStockByFinYr(string year)
        {
            var data = _stockRepository.GetMany(p => p.OpeningStockCode.Contains(year)).OrderBy(p => p.OpeningStockCode).LastOrDefault();
            return data;
        }
    }
}

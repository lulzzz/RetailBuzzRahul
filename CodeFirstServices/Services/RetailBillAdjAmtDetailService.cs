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
    public class RetailBillAdjAmtDetailService : IRetailBillAdjAmtDetailService
    {
        private readonly IRetailBillAdjAmtDetailRepository _RetailBillAdjAmtDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RetailBillAdjAmtDetailService(IRetailBillAdjAmtDetailRepository RetailBillAdjAmtDetailRepository, IUnitOfWork unitOfWork)
        {
            this._RetailBillAdjAmtDetailRepository = RetailBillAdjAmtDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(RetailBillAdjAmtDetail data)
        {
            _RetailBillAdjAmtDetailRepository.Add(data);
            _unitOfWork.Commit();
        }

        public void Delete(RetailBillAdjAmtDetail data)
        {
            _RetailBillAdjAmtDetailRepository.Delete(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<RetailBillAdjAmtDetail> GetBillsByDate(DateTime date)
        {
            var data = _RetailBillAdjAmtDetailRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == date);
            return data;
        }

        public IEnumerable<RetailBillAdjAmtDetail> GetBillsByRetailNo(string no)
        {
            var data = _RetailBillAdjAmtDetailRepository.GetMany(s => s.RetailBillNo == no);
            return data;
        }

        public RetailBillAdjAmtDetail GetDetailsBySalesOrderNo(string no)
        {
            var data = _RetailBillAdjAmtDetailRepository.Get(s => s.AdjustedBill == no);
            return data;
        }
    }
}

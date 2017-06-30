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
    public class TemporaryCashMemoAdjAmtDetailService : ITemporaryCashMemoAdjAmtDetailService
    {
        private readonly ITemporaryCashMemoAdjAmtDetailRepository _TemporaryCashMemoAdjAmtDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TemporaryCashMemoAdjAmtDetailService(ITemporaryCashMemoAdjAmtDetailRepository TemporaryCashMemoAdjAmtDetailRepository, IUnitOfWork unitOfWork)
        {
            this._TemporaryCashMemoAdjAmtDetailRepository = TemporaryCashMemoAdjAmtDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(TemporaryCashMemoAdjAmtDetail data)
        {
            _TemporaryCashMemoAdjAmtDetailRepository.Add(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<TemporaryCashMemoAdjAmtDetail> GetBillsByDate(DateTime date)
        {
            var data = _TemporaryCashMemoAdjAmtDetailRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == date);
            return data;
        }


        public IEnumerable<TemporaryCashMemoAdjAmtDetail> GetBillsByTemporaryCashMemoNo(string no)
        {
            var data = _TemporaryCashMemoAdjAmtDetailRepository.GetMany(s => s.TempCashMemoNo == no);
            return data;
        }
    }
}

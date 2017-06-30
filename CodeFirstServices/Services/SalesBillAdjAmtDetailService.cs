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
    public class SalesBillAdjAmtDetailService : ISalesBillAdjAmtDetailService
    {
        private readonly ISalesBillAdjAmtDetailRepository _SalesBillAdjAmtDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SalesBillAdjAmtDetailService(ISalesBillAdjAmtDetailRepository SalesBillAdjAmtDetailRepository, IUnitOfWork unitOfWork)
        {
            this._SalesBillAdjAmtDetailRepository = SalesBillAdjAmtDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(SalesBillAdjAmtDetail data)
        {
            _SalesBillAdjAmtDetailRepository.Add(data);
            _unitOfWork.Commit();
        }

        public void Delete(SalesBillAdjAmtDetail data)
        {
            _SalesBillAdjAmtDetailRepository.Delete(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<SalesBillAdjAmtDetail> GetBillsByDate(DateTime date)
        {
            var data = _SalesBillAdjAmtDetailRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == date);
            return data;
        }

        public IEnumerable<SalesBillAdjAmtDetail> GetBillsBySalesNo(string no)
        {
            var data = _SalesBillAdjAmtDetailRepository.GetMany(s => s.SalesBillNo == no);
            return data;
        }
    }
}

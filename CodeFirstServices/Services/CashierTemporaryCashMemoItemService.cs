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
    public class CashierTemporaryCashMemoItemService : ICashierTemporaryCashMemoItemService
    {
        private readonly ICashierTemporaryCashMemoItemRepository _CashierTemporaryCashMemoItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierTemporaryCashMemoItemService(ICashierTemporaryCashMemoItemRepository CashierTemporaryCashMemoItemRepository, IUnitOfWork unitOfWork)
        {
            this._CashierTemporaryCashMemoItemRepository = CashierTemporaryCashMemoItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierTemporaryCashMemoItem cash)
        {
            _CashierTemporaryCashMemoItemRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierTemporaryCashMemoItem> GetDetailsByCashierCode(string code)
        {
            var data = _CashierTemporaryCashMemoItemRepository.GetMany(d => d.CashierCode == code);
            return data;
        }

    }
}

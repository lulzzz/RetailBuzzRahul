using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Services
{
    public class InwardInterGodownItemService : IInwardInterGodownItemService
    {
        private readonly IInwardInterGodownItemRepository _InwardInterGodownItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InwardInterGodownItemService(IInwardInterGodownItemRepository InwardInterGodownItemRepository, IUnitOfWork unitOfWork)
        {
            this._InwardInterGodownItemRepository = InwardInterGodownItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(InwardInterGodownItem inward)
        {
            _InwardInterGodownItemRepository.Add(inward);
            _unitOfWork.Commit();
        }

        public IEnumerable<InwardInterGodownItem> GetItemListByInwardCode(string InwardCode)
        {
            var details = _InwardInterGodownItemRepository.GetMany(i => i.InwardCode == InwardCode);
            return details;
        }
    }
}

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
    public class InwardInterShopItemService : IInwardInterShopItemService
    {
        private readonly IInwardInterShopItemRepository _InwardInterShopItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public InwardInterShopItemService(IInwardInterShopItemRepository InwardInterShopItemRepository, IUnitOfWork unitOfWork)
        {
            this._InwardInterShopItemRepository = InwardInterShopItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(InwardInterShopItem Item)
        {
            _InwardInterShopItemRepository.Add(Item);
            _unitOfWork.Commit();
        }

        public IEnumerable<InwardInterShopItem> GetItemListByInwardCode(string Code)
        {
            var details = _InwardInterShopItemRepository.GetMany(i => i.InwardCode == Code);
            return details;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class ItemTempDetailService : IItemTempDetailService
    {
        private readonly IItemTempDetailRepository _ItemTempDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemTempDetailService(IItemTempDetailRepository ItemTempDetailRepository, IUnitOfWork unitOfWork)
        {
            this._ItemTempDetailRepository = ItemTempDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<itemTempDetail> getAllItems()
        {
            var item = _ItemTempDetailRepository.GetMany(item1 => item1.status == "Active");
            return item;
        }

        public void createItemTempData(itemTempDetail item)
        {
            _ItemTempDetailRepository.Add(item);
            _unitOfWork.Commit();
        }

        public void updateItemTempData(itemTempDetail item)
        {
            _ItemTempDetailRepository.Update(item);
            _unitOfWork.Commit();
        }

        public void deleteItemTempData(itemTempDetail item)
        {
            _ItemTempDetailRepository.Delete(item);
            _unitOfWork.Commit();
        }

        public itemTempDetail getById(int id)
        {
            var itemid = _ItemTempDetailRepository.GetById(id);
            return itemid;
        }

        public IEnumerable<itemTempDetail> getTempListByDate(string category, string subCategory)
        {
            var itemlist = _ItemTempDetailRepository.GetMany(ti => ti.modifedOn.Value.Date == DateTime.Now.Date && ti.itemCategory == category && ti.itemSubCategory == subCategory);
            return itemlist;
        }


        public itemTempDetail CheckForData(BarcodeTempDetail Data)
        {
            var Details = _ItemTempDetailRepository.Get(x => x.itemName == Data.itemName && x.itemCategory == Data.itemCategory && x.itemSubCategory == Data.itemSubCategory
                && x.colorCode == Data.colorCode && x.designCode == Data.designCode && x.typeOfMaterial == Data.typeOfMaterial);
            return Details;
        }
    }
}

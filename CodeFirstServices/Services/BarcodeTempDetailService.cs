using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
    public class BarcodeTempDetailService : IBarcodeTempDetailService
    {
        private readonly IBarcodeTempDetailRepository _BarcodeTempDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public BarcodeTempDetailService(IBarcodeTempDetailRepository BarcodeTempDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._BarcodeTempDetailRepository = BarcodeTempDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(BarcodeTempDetail BarcodeTempDetail)
        {
            _BarcodeTempDetailRepository.Add(BarcodeTempDetail);
            _UnitOfWork.Commit();
        }

        public BarcodeTempDetail CheckForData(BarcodeTempDetail Data)
        {
            var Details = _BarcodeTempDetailRepository.Get(x => x.itemName == Data.itemName && x.itemCategory == Data.itemCategory && x.itemSubCategory == Data.itemSubCategory
                && x.colorCode == Data.colorCode && x.designCode == Data.designCode && x.typeOfMaterial == Data.typeOfMaterial);
            return Details;
        }
    }
}

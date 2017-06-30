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
    public class UniversalBarcodeNumberService : IUniversalBarcodeNumberService
    {
        private readonly IUniversalBarcodeNumberRepository _UniversalBarcodeNumberRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UniversalBarcodeNumberService(IUniversalBarcodeNumberRepository UniversalBarcodeNumberRepository, IUnitOfWork unitOfWork)
        {
            this._UniversalBarcodeNumberRepository = UniversalBarcodeNumberRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(UniversalBarcodeNumber universalbarcodenumber)
        {
            _UniversalBarcodeNumberRepository.Add(universalbarcodenumber);
            _unitOfWork.Commit();
        }

        public UniversalBarcodeNumber GetLastBarcode()
        {
            var details = _UniversalBarcodeNumberRepository.GetAll().LastOrDefault();
            return details;
        }
    }
}

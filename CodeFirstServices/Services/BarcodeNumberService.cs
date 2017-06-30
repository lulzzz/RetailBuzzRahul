using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class BarcodeNumberService : IBarcodeNumberService
    {
        private readonly IBarcodeNumberRepository _BarcodeNumberRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public BarcodeNumberService(IBarcodeNumberRepository BarcodeNumberRepository, IUnitOfWork UnitOfWork)
        {
            this._BarcodeNumberRepository = BarcodeNumberRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public BarcodeNumber GetLastBarcodeNumber()
        {
            var barcode = _BarcodeNumberRepository.GetAll().LastOrDefault();
            return barcode;
        }

        public void CreateBarcode(BarcodeNumber BarcodeNumber)
        {
            _BarcodeNumberRepository.Add(BarcodeNumber);
            _UnitOfWork.Commit();
        }

        public IEnumerable<BarcodeNumber> GetAllBarcodeNumbers()
        {
            var list = _BarcodeNumberRepository.GetAll();
            return list;
        }

        public BarcodeNumber GetBarcodeImageByBarcode(string number)
        {
            var bytes = _BarcodeNumberRepository.Get(m => m.Number == number);
            return bytes;
        }
    }
}
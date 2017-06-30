using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IBarcodeNumberService
    {
        BarcodeNumber GetLastBarcodeNumber();
        void CreateBarcode(BarcodeNumber BarcodeNumber);
        IEnumerable<BarcodeNumber> GetAllBarcodeNumbers();
        BarcodeNumber GetBarcodeImageByBarcode(string number);
    }
}

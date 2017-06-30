using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CodeFirstServices.Interfaces
{
  public interface IInwardItemFromGodownService
    {
      void Create(InwardItemFromGodown InwardItemFromGodown);
      IEnumerable<InwardItemFromGodown> GetDetailsByCode(string code);
    }
}

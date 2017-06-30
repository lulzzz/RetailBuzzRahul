using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICostCodeCreationService
    {
        void Create(CostCodeCreation code);
        void Update(CostCodeCreation code);
        IEnumerable<CostCodeCreation> GetActiveData();
        string GetDetailsByNumber(string no);
    }
}

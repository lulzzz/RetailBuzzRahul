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
    public class InwardItemFromGodownService : IInwardItemFromGodownService
    {
        private readonly IInwardItemFromGodownRepository _iinwarditemfromgodownrepository;
        private readonly IUnitOfWork _iunitofwork;

        public InwardItemFromGodownService(IInwardItemFromGodownRepository inwarditemfromgodownrepository, IUnitOfWork iunitofwork)
        {
            this._iinwarditemfromgodownrepository = inwarditemfromgodownrepository;
            this._iunitofwork = iunitofwork;
        }

        public void Create(InwardItemFromGodown InwardItemFromGodown)
        {
            _iinwarditemfromgodownrepository.Add(InwardItemFromGodown);
            _iunitofwork.Commit();
        }

        public IEnumerable<InwardItemFromGodown> GetDetailsByCode(string code)
        {
            var data = _iinwarditemfromgodownrepository.GetMany(d => d.InwardCode == code);
            return data;
        }
    }
}

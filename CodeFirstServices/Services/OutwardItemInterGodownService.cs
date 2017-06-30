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
    public class OutwardItemInterGodownService : IOutwardItemInterGodownService
    {
        private readonly IOutwardItemInterGodownRepository _OutwardItemInterGodownRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public OutwardItemInterGodownService(IOutwardItemInterGodownRepository OutwardItemInterGodownRepository,
            IUnitOfWork UnitOfWork) 
        {
                this._OutwardItemInterGodownRepository = OutwardItemInterGodownRepository;
                this._UnitOfWork = UnitOfWork;
        }

            
        public void Create(OutwardItemInterGodown outward)
        {
            _OutwardItemInterGodownRepository.Add(outward);
            _UnitOfWork.Commit();
        }


        public IEnumerable<OutwardItemInterGodown> GetDetailsByOutwardCode(string code)
        {
            var details = _OutwardItemInterGodownRepository.GetMany(m => m.OutwardCode == code);
            return details;
        }

        public void Delete(OutwardItemInterGodown outward)
        {
            _OutwardItemInterGodownRepository.Delete(outward);
            _UnitOfWork.Commit();
        }

        public OutwardItemInterGodown GetRowByOutwardCode(string OutwardCode)
        {
            var details = _OutwardItemInterGodownRepository.Get(o => o.OutwardCode == OutwardCode);
            return details;
        }
    }
}

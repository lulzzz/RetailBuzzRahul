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
    public class OutwardItemToClientService:IOutwardItemToClientService
    {
        private readonly IOutwardItemToClientRepository _outwarditemtoclientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardItemToClientService(IOutwardItemToClientRepository outwarditemtoclientRepository, IUnitOfWork unitOfWork)
        {
            this._outwarditemtoclientRepository = outwarditemtoclientRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardItemToClient data)
        {
            _outwarditemtoclientRepository.Add(data);
            _unitOfWork.Commit();
        }

        public IEnumerable<OutwardItemToClient> GetDetailsByOutwardCode(string code)
        {
            var data = _outwarditemtoclientRepository.GetMany(d => d.OutwardCode == code);
            return data;
        }
    }
}

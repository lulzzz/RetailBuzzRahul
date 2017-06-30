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
    public class UnitConversionService : IUnitConversionService
    {
        private readonly IUnitConversionRepository _UnitConversionRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public UnitConversionService(IUnitConversionRepository UnitConversionRepository, IUnitOfWork UnitOfWork)
        {
            this._UnitConversionRepository = UnitConversionRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(UnitConversion UnitConversion)
        {
            _UnitConversionRepository.Add(UnitConversion);
            _UnitOfWork.Commit();
        }

        public UnitConversion GetUnitValue(string fromunit, string tounit)
        {
            var data = _UnitConversionRepository.Get(u => u.FromUnit == fromunit && u.ToUnit == tounit);
            return data;
        }

        public IEnumerable<UnitConversion> GetAll()
        {
            var list = _UnitConversionRepository.GetAll();
            return list;
        }
    }
}

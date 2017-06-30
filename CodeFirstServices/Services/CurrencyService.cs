using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class CurrencyService : ICurrencyService
    {

        private readonly ICurrencyRepository _currencyrepository;
        private readonly IUnitOfWork _unitOfWork;




        public CurrencyService(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
        {
            this._currencyrepository = currencyRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Currency> getAllCurrencies()
        {
            var currency = _currencyrepository.GetAll();
            return currency;

        }

    }
}

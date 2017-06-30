using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class ResetRetailBillItemService : IResetRetailBillItemService
    {
        private readonly IResetRetailBillItemRepository _RetailInvoiceItemRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ResetRetailBillItemService(IResetRetailBillItemRepository RetailInvoiceItemRepository, IUnitOfWork UnitOfWork)
        {
            this._RetailInvoiceItemRepository = RetailInvoiceItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(ResetRetailBillItem retailinvoiceitem)
        {
            _RetailInvoiceItemRepository.Add(retailinvoiceitem);
            _UnitOfWork.Commit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class ResetRetailBillService : IResetRetailBillService
    {
        private readonly IResetRetailBillRepository _RetailInvoiceMasterRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ResetRetailBillService(IResetRetailBillRepository RetailInvoiceMasterRepository, IUnitOfWork UnitOfWork)
        {
            this._RetailInvoiceMasterRepository = RetailInvoiceMasterRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(ResetRetailBill retailinvoicemaster)
        {
            _RetailInvoiceMasterRepository.Add(retailinvoicemaster);
            _UnitOfWork.Commit();
        }
    }

}

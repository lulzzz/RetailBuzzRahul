using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class CustomerDetailService:ICustomerDetailService
    {
        private readonly ICustomerDetailRepository _CustomerDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerDetailService(ICustomerDetailRepository CustomerDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._CustomerDetailRepository = CustomerDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void CreateInvoice(CustomerDetail customerdetail)
        {
            _CustomerDetailRepository.Add(customerdetail);
            _UnitOfWork.Commit();
        }
        public void UpdateInvoice(CustomerDetail customerdetail)
        {
            _CustomerDetailRepository.Update(customerdetail);
            _UnitOfWork.Commit();
        }
        public CustomerDetail GetById(int id)
        {
            var data = _CustomerDetailRepository.GetById(id);
            return data;
        }
        public CustomerDetail GetLastInvoice()
        {
            var data = _CustomerDetailRepository.GetAll().LastOrDefault();
            return data;
        }
        public IEnumerable<CustomerDetail> GetAll()
        {
            var data = _CustomerDetailRepository.GetAll();
            return data;
        }
        public IEnumerable<CustomerDetail> GetCustomerName(string name)
        {
            var data = _CustomerDetailRepository.GetMany(emp => emp.Name.ToLower().StartsWith(name.ToString().ToLower()) && emp.Status == "Active");
            return data;
        }
        public CustomerDetail GetDetailsByName(string name)
        {
            var data = _CustomerDetailRepository.Get(n => n.Name == name);
            return data;
        }
       
    }
}

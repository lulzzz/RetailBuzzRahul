using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class SupplierCustomBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            int? pincode = 0;
            long? contactno = 0;
            int supplierId = Convert.ToInt32(request.Form.Get("SupplierDetails.SupplierId"));
            string TypeOfSupplier = request.Form.Get("SupplierDetails.SupplierType");
            //string OtherTypeOfSupplier = request.Form.Get("OtherTypeOfSuppliers");
            string suppliername = request.Form.Get("SupplierDetails.SupplierName");
            string add = request.Form.Get("SupplierDetails.Address");
            if (string.IsNullOrEmpty(request.Form.Get("SupplierDetails.Pincode")))
            {
                pincode = null;
            }
            else
            {
                pincode = Convert.ToInt32(request.Form.Get("SupplierDetails.Pincode"));
            }
            if (string.IsNullOrEmpty(request.Form.Get("SupplierDetails.contactNo")))
            {
                contactno = null;
            }
            else
            {
                contactno = (long)Convert.ToInt32(request.Form.Get("SupplierDetails.contactNo"));
            }

            string email = request.Form.Get("SupplierDetails.Email");
            string website = request.Form.Get("SupplierDetails.Website");
            //string BankName = request.Form.Get("SupplierDetails.bankName");
           // string bankIfscNo = request.Form.Get("SupplierDetails.BankIFSCNo");
            string stno = request.Form.Get("SupplierDetails.STNo");
            string vtNo = request.Form.Get("SupplierDetails.VTNo");
            string tdsNo = request.Form.Get("SupplierDetails.TDSNo");
            string Pancard = request.Form.Get("SupplierDetails.pancard");
            string status = "Active";
            string States = request.Form.Get("SupplierDetails.states");
            string Country = request.Form.Get("SupplierDetails.country");
            string City = request.Form.Get("SupplierDetails.city");
            string District = request.Form.Get("SupplierDetails.district");
           string suppliercode = request.Form.Get("SupplierDetails.SupplierCode");
           //string account_no = request.Form.Get("SupplierDetails.Account_No");
          // string bankaddress = request.Form.Get("SupplierDetails.BankAddress");
           //string accountType = request.Form.Get("SupplierDetails.AccountType");
            
            DateTime modifiedon = DateTime.Now;

            return new SupplierMaster
            {
                SupplierId = supplierId,
                SupplierType = TypeOfSupplier,              
                SupplierName = suppliername,
                Address = add,
                Pincode = pincode,
                ContactNo1 = contactno,
                Email = email,
                Website = website,
               // BankName = BankName,
               // BankIFSCNo = bankIfscNo,
                STNo = stno,
                VTNo = vtNo,
                TDSNo = tdsNo,
                Status = status,
                ModifiedOn = modifiedon,
                pancard=Pancard,
                states=States,
                country=Country,
                city=City,
                district=District,
                SupplierCode=suppliercode,
               // Account_No=account_no,
               // BankAddress=bankaddress,
               // AccountType=accountType,
            };
        }

    }
}
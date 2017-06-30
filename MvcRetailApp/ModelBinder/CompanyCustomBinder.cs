using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class CompanyCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int companyId = Convert.ToInt32(request.Form.Get("companydetails.companyId"));
            string companyName = request.Form.Get("companydetails.companyName");
            string address = request.Form.Get("companydetails.address");
            int pincode = Convert.ToInt32(request.Form.Get("companydetails.pincode"));
            long ContactNo = (long)Convert.ToDouble(request.Form.Get("companydetails.ContactNo"));
            string eMail = request.Form.Get("companydetails.eMail");
            string website = request.Form.Get("companydetails.website");
            string salesTaxNo = request.Form.Get("companydetails.salesTaxNo");
            string vatNo = request.Form.Get("companydetails.vatNo");
            string regNo = request.Form.Get("companydetails.regNo");
            string panCard = request.Form.Get("companydetails.panCard");
            DateTime dateOfRegistration = Convert.ToDateTime(request.Form.Get("companydetails.CompanyRegisterationDate"));
            string state = request.Form.Get("companydetails.State");
            string city = request.Form.Get("companydetails.City");
            string mailingaddress = request.Form.Get("companydetails.MailingAddress");
            DateTime financialyearfrom = Convert.ToDateTime(request.Form.Get("companydetails.FinancialYearFrom"));
            DateTime financialyearto = Convert.ToDateTime(request.Form.Get("companydetails.FinancialYearTo"));
            string companycode = request.Form.Get("companydetails.CompanyCode");
        
            string status = "Active";
            DateTime modifiedOn = DateTime.Now;
            
            
            return new Company
            {
                companyId = companyId,
                companyName = companyName,
                address = address,
                pincode = pincode,
               // ContactNo = ContactNo,
                eMail = eMail,
                website = website,
                salesTaxNo = salesTaxNo,
                vatNo = vatNo,
                regNo = regNo,

                CompanyRegisterationDate = dateOfRegistration,
                State = state,
                City = city,
                FinancialYearFrom = financialyearfrom,
                FinancialYearTo = financialyearto,
                panCard = panCard,
                MailingAddress = mailingaddress,
                isEnabled = status,
                ModifiedOn = modifiedOn,
                CompanyCode = companycode,
               
                
               

           
            };
        }

      
    }
}


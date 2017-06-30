using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class BankCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            int? pinCode = 0;
            string contactno2 = null;
            string contactno3 = null;
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int bankID = Convert.ToInt32(request.Form.Get("BankDetails.bankId"));
            string bankName = request.Form.Get("BankDetails.bankName");
            string address = request.Form.Get("BankDetails.address");
            if (!string.IsNullOrEmpty(request.Form.Get("BankDetails.pinCode")))
            {
                pinCode = Convert.ToInt32(request.Form.Get("BankDetails.pinCode"));
            }
            else
            {
                pinCode = null;
            }
            string contactno1 = (request.Form.Get("BankDetails.ContactNo1"));
            if (!string.IsNullOrEmpty(request.Form.Get("BankDetails.ContactNo2")))
            {
                contactno2 = (request.Form.Get("BankDetails.ContactNo2"));
            }
            else
            {
                contactno2 = null;
            }
            if (!string.IsNullOrEmpty(request.Form.Get("BankDetails.ContactNo3")))
            {
                contactno3 = (request.Form.Get("BankDetails.ContactNo3"));
            }
            else
            {
                contactno3 = null;
            }
            string email = request.Form.Get("BankDetails.email");
            string website = request.Form.Get("BankDetails.website");
            string ifsc = request.Form.Get("BankDetails.IFSC");
            string branch = request.Form.Get("BankDetails.Branch");
            string micr = request.Form.Get("BankDetails.MICRCode");

                string status = "Active";
                DateTime modifiedon = DateTime.Now;
                return new BankMaster
                {
                    bankId = bankID,
                    bankName = bankName,
                    address = address,
                    pinCode = pinCode,
                    ContactNo1=contactno1,
                    ContactNo2=contactno2,
                    ContactNo3=contactno3,
                    email = email,
                    website = website,
                    IFSC = ifsc,
                    status = status,
                    modifiedon = modifiedon,
                    Branch=branch,
                    MICRCode = micr,
                };
            }
        }
    }


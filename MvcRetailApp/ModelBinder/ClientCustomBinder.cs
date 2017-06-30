using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class ClientCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int? pinCode = 0;
            int clientId = Convert.ToInt32(request.Form.Get("ClientDetails.ClientId"));
            //int bankId =Convert.ToInt32(request.Form.Get("ClientDetails.BankId"));
            string typeofmembershipcard = request.Form.Get("ClientDetails.TypeOfMembershipCard");
            string clientName = request.Form.Get("ClientDetails.ClientName");
            string clientAddress = request.Form.Get("ClientDetails.Address");
            if (string.IsNullOrEmpty(request.Form.Get("ClientDetails.Pincode")))
            {
                pinCode=null;
            }
            else
            {
                pinCode = Convert.ToInt32(request.Form.Get("ClientDetails.Pincode"));
            }
            long? contactNo = (long)Convert.ToDouble(request.Form.Get("ClientDetails.ContactNo"));
            string email = request.Form.Get("ClientDetails.Email");
            string website = request.Form.Get("ClientDetails.Website");
            string STno = request.Form.Get("ClientDetails.STNo");
            string VTno = request.Form.Get("ClientDetails.VTNo");
            string TDSno = request.Form.Get("ClientDetails.TDSNo");
            string clientStatus = "Active";
            DateTime modifiedOn = DateTime.Now;
            string pancard = request.Form.Get("ClientDetails.PanCard");
            string country = request.Form.Get("ClientDetails.Country");
            string state = request.Form.Get("ClientDetails.State");
            string district = request.Form.Get("ClientDetails.District");
            string city = request.Form.Get("ClientDetails.City");
            //string bankName = request.Form.Get("ClientDetails.BankName");
            //string bankifscno = request.Form.Get("ClientDetails.BankIFSCNo");
            //string bankaddress=request.Form.Get("ClientDetails.BankAddress");
            //string accountype = request.Form.Get("ClientDetails.AccountType");
            //string accountno = request.Form.Get("ClientDetails.Account_No");
            string formtype = request.Form.Get("ClientDetails.FormType");
            string clientcode = request.Form.Get("ClientDetails.ClientCode");
            string nameonnard = request.Form.Get("ClientDetails.NameOnCard");
            long? cardnumber = Convert.ToInt64(request.Form.Get("ClientDetails.CardNumber"));
            DateTime issuedate =Convert.ToDateTime(request.Form.Get("ClientDetails.IssueDate"));
            return new ClientMaster
            {
                ClientId=clientId,
                //BankId=bankId,
                TypeOfMembershipCard=typeofmembershipcard,
                ClientName=clientName,
                Address=clientAddress,
                Pincode=pinCode,
                ContactNo=contactNo,
                Email=email,
                Website=website,
                STNo=STno,
                VTNo=VTno,
                TDSNo=TDSno,
                Status=clientStatus,
                ModifiedOn=modifiedOn,
                PanCard=pancard,
                Country=country,
                State=state,
                District=district,
                City=city,
               // BankName=bankName,
               // BankIFSCNo=bankifscno,
               // BankAddress=bankaddress,
               // AccountType=accountype,
               // Account_No=accountno,
                FormType=formtype,
                ClientCode=clientcode,
                NameOnCard=nameonnard,
                CardNumber=cardnumber,
                IssueDate=issuedate,
            };

        }
    }
}
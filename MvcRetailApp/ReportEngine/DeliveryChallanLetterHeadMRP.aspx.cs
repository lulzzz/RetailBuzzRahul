using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.Helpers;
using Microsoft.Reporting.WebForms;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;

namespace MvcRetailApp.ReportEngine
{
    public partial class DeliveryChallanLetterHeadMRP : System.Web.UI.Page
    {
        private string UserEmail
        {
            get { return (string)HttpContext.Current.Session["UserEmail"]; }
            set { HttpContext.Current.Session["UserEmail"] = value; }
        }
        private string CompanyCode
        {
            get { return (string)HttpContext.Current.Session["CompanyCode"]; }
            set { HttpContext.Current.Session["CompanyCode"] = value; }
        }
        private string CompanyName
        {
            get { return (string)HttpContext.Current.Session["CompanyName"]; }
            set { HttpContext.Current.Session["CompanyName"] = value; }
        }
        private string FinancialYear
        {
            get { return (string)HttpContext.Current.Session["FinancialYear"]; }
            set { HttpContext.Current.Session["FinancialYear"] = value; }
        }
        private string DatabaseName
        {
            get { return (string)HttpContext.Current.Session["DatabaseName"]; }
            set { HttpContext.Current.Session["DatabaseName"] = value; }
        }


        public int Decode(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return Convert.ToInt32(decodedvalue);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Reset();
                string id = Request.QueryString["id"];
                int Id = Decode(id);
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
                //SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=" + DatabaseName + ";Integrated Security=True");
               // SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=" + DatabaseName + ";Integrated Security=True");
                SqlDataAdapter adp = new SqlDataAdapter("select * from DeliveryChallans where Id=" + Id, con);
                DeliveryChallansDataSet ds = new DeliveryChallansDataSet();
                adp.Fill(ds);
                double grandtotal = Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]);
                string Words = NumberToWords(grandtotal);
                con.Open();
                ReportDataSource rds1 = new ReportDataSource("DataSet2", GetDataSet2(Id));
                ReportDataSource rds = new ReportDataSource("DataSet1", GetDataSet1());
                ReportDataSource rds2 = new ReportDataSource("DataSet3", GetDs2());
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.DataSources.Add(rds2);
                ReportViewer1.LocalReport.ReportPath = "ReportEngine/DeliveryChallanLetterHeadMRP.rdlc";
                ReportParameter parameter = new ReportParameter("AmountInWords", (Words + " Only"));
                ReportViewer1.LocalReport.SetParameters(parameter);
                ReportViewer1.LocalReport.Refresh();


                Warning[] warnings;
                string[] streamIds;
                string mimetype = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string title = "Retail Bill";
                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimetype, out encoding, out extension, out streamIds, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(bytes);
                Response.End();
                string filename = "DeliveryChallanLetterHeadMRP.pdf";
                string path = Server.MapPath("C");
                FileStream file = new FileStream(path + "/" + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                file.Write(bytes, 0, bytes.Length);
                file.Dispose();

                Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "DeliveryChallanLetterHeadMRP.aspx?file=" + filename));
 



            }

        }

        private DataTable GetDataSet2(int Id)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
           // SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=" + DatabaseName + ";Integrated Security=True");
            SqlDataAdapter adp = new SqlDataAdapter("select * from DeliveryChallans where Id=" + Id, con);
            DeliveryChallansDataSet ds = new DeliveryChallansDataSet();
            con.Open();
            adp.Fill(ds);
            string ChallanNo = ds.Tables[1].Rows[0]["ChallanNo"].ToString();
            Session["ChallanNo"] = ChallanNo;
            GetDataSet1();
            ds.Tables[1].Rows[0]["TotalAmount"] = Convert.ToDouble(Session["TotalAmount"]).ToString("#,###.00##");
            ds.Tables[1].Rows[0]["TotalDiscount"] = Convert.ToDouble(Session["TotalDiscount"]).ToString("#,###.00##");
            ds.Tables[1].Rows[0]["PackAndForwd"] = (Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]) - Convert.ToDouble(Session["TotalAmount"])).ToString("#,###.00##");
            ds.Tables[1].Rows[0]["GrandTotal"] = Convert.ToDouble(ds.Tables[1].Rows[0]["GrandTotal"]).ToString("#,###.00##");
            return ds.Tables[1];
        }

        private DataTable GetDataSet1()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
           // SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=" + DatabaseName + ";Integrated Security=True");
            string ChallanNo = Session["ChallanNo"].ToString();
            SqlDataAdapter adp = new SqlDataAdapter("select * from DeliveryChallanItems where ChallanNo='" + ChallanNo + "'", con);
            DeliveryChallanItemsDataSet ds = new DeliveryChallanItemsDataSet();
            con.Open();
            adp.Fill(ds);
            double? TotalAmount = 0;
            double? TotalDiscount = 0;
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                double? discountonmrp = ((Convert.ToDouble(ds.Tables[1].Rows[i]["Quantity"])) * (Convert.ToDouble(ds.Tables[1].Rows[i]["MRP"]))) * Convert.ToDouble(ds.Tables[1].Rows[i]["DiscountPercent"]) / 100;
                TotalDiscount = TotalDiscount + discountonmrp;
                TotalAmount = TotalAmount + ((Convert.ToDouble(ds.Tables[1].Rows[i]["Quantity"])) * (Convert.ToDouble(ds.Tables[1].Rows[i]["MRP"]))) - discountonmrp;
            }
            Session["TotalAmount"] = TotalAmount;
            Session["TotalDiscount"] = TotalDiscount;
            return ds.Tables[1];
        }
        private DataTable GetDs2()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
            //SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
            string rbno = Session["ChallanNo"].ToString();
            SqlDataAdapter adp3 = new SqlDataAdapter("select * from InventoryTaxes where Code='" + rbno + "'", con);
            RetailManagementDataSet6 ds3 = new RetailManagementDataSet6();
            con.Open();
            adp3.Fill(ds3);

            // find retail bill no using primary key
            //string Code = ds3.Tables[1].Rows[0]["Code"].ToString();
            //Session["Code"] = Code;
            return ds3.Tables[1];
        }

        public string NumberToWords(double grandtotal)
        {
            int number = Convert.ToInt32(grandtotal);
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[(int)number];
                else
                {
                    words += tensMap[(int)number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[(int)number % 10];
                }
            }

            return words;
        }
    }
}
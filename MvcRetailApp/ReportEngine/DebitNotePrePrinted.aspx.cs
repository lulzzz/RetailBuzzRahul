﻿using System;
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
    public partial class DebitNotePrePrinted : System.Web.UI.Page
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
                int DebitNoteId = Decode(id);
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
                SqlDataAdapter adp2 = new SqlDataAdapter("select * from DebitNotes where Id=" + DebitNoteId, con);
                DebitNotesDS ds2 = new DebitNotesDS();
                con.Open();
                adp2.Fill(ds2);
                ReportDataSource rds1 = new ReportDataSource("DataSet2", GetDs1(DebitNoteId));
                ReportDataSource rds = new ReportDataSource("DataSet1", GetDs(DebitNoteId));
                ReportDataSource rds2 = new ReportDataSource("DataSet3", GetDs2());
                ReportDataSource rds3 = new ReportDataSource("DataSet4", GetDs3());
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.DataSources.Add(rds2);
                ReportViewer1.LocalReport.DataSources.Add(rds3);
                ReportViewer1.LocalReport.ReportPath = "ReportEngine/DebitNotePrePrinted.rdlc";
                double grandtotal = Convert.ToDouble(ds2.Tables[1].Rows[0]["GrandTotal"]);
                string Words = NumberToWords(grandtotal);
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
                string filename = "DebitNotePrePrinted.pdf";
                string path = Server.MapPath("C");
                FileStream file = new FileStream(path + "/" + filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                file.Write(bytes, 0, bytes.Length);
                file.Dispose();

                Response.Write(string.Format("<script>window.open('{0}','_blank');</script>", "DebitNotePrePrinted.aspx?file=" + filename));


            }


            }

            
            private DataTable GetDs(int DId)
            {
               // SqlConnection con = new SqlConnection("Data Source=MARY-PC;Initial Catalog=A To Z Life Style(India) Pvt Ltd Retail 01-04-2016 To 31-03-2017;Integrated Security=True");
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
                string DebitNoteNo = Session["DebitNoteNo"].ToString();
                SqlDataAdapter adp1 = new SqlDataAdapter("select * from DebitNoteItems where DebitNoteNo='" + DebitNoteNo + "'", con);
                DebitNoteItems ds1 = new DebitNoteItems();
                con.Open();
                adp1.Fill(ds1);
                return ds1.Tables[1];
            }

            private DataTable GetDs1(int DId)
            {
               // SqlConnection con = new SqlConnection("Data Source=MARY-PC;Initial Catalog=A To Z Life Style(India) Pvt Ltd Retail 01-04-2016 To 31-03-2017;Integrated Security=True");
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
                SqlDataAdapter adp2 = new SqlDataAdapter("select * from DebitNotes where Id=" + DId, con);
                DebitNotesDS ds2 = new DebitNotesDS();
                con.Open();
                adp2.Fill(ds2);

                //find Challan no using primary key
                string DebitNoteNo = ds2.Tables[1].Rows[0]["DebitNoteNo"].ToString();
                string purchasereturnno = ds2.Tables[1].Rows[0]["PurchaseReturnNo"].ToString();
                Session["DebitNoteNo"] = DebitNoteNo;
                Session["PurchaseReturnNo"] = purchasereturnno;
                return ds2.Tables[1];
            
            }


            private DataTable GetDs2()
            {
               // SqlConnection con = new SqlConnection("Data Source=MARY-PC;Initial Catalog=A To Z Life Style(India) Pvt Ltd Retail 01-04-2016 To 31-03-2017;Integrated Security=True");
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
                string rbno = Session["PurchaseReturnNo"].ToString();
                SqlDataAdapter adp3 = new SqlDataAdapter("select * from PurchaseInventoryTaxes where Code='" + rbno + "'", con);
                InventoryTaxesDataSet ds3 = new InventoryTaxesDataSet();
                con.Open();
                adp3.Fill(ds3);

                // find retail bill no using primary key
                //string Code = ds3.Tables[1].Rows[0]["Code"].ToString();
                //Session["Code"] = Code;
                return ds3.Tables[1];
            }
            private DataTable GetDs3()
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RetailManagementConnectionString"].ConnectionString);
               // SqlConnection con = new SqlConnection("Data Source=MARY-PC;Initial Catalog=A To Z Life Style(India) Pvt Ltd Retail 01-04-2016 To 31-03-2017;Integrated Security=True");
                string rbno = Session["DebitNoteNo"].ToString();
                SqlDataAdapter adp3 = new SqlDataAdapter("select * from SalesReturns where BillNo='" + rbno + "'", con);
                SalesReturnsDataset ds4 = new SalesReturnsDataset();
                con.Open();
                adp3.Fill(ds4);

                // find retail bill no using primary key
                //string Code = ds3.Tables[1].Rows[0]["Code"].ToString();
                //Session["Code"] = Code;
                return ds4.Tables[1];
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

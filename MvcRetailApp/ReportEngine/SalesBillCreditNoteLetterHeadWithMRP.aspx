<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesBillCreditNoteLetterHeadWithMRP.aspx.cs" Inherits="MvcRetailApp.ReportEngine.SalesBillCreditNoteLetterHeadWithMRP" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="margin-top: 0px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>

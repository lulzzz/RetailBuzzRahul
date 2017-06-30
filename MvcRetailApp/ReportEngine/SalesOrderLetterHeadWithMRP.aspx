<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrderLetterHeadWithMRP.aspx.cs" Inherits="MvcRetailApp.ReportEngine.SalesOrderLetterHeadWithMRP" %>

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
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="649px" Width="809px">
        </rsweb:ReportViewer>
        <br />
    
    </div>
    </form>
</body>
</html>

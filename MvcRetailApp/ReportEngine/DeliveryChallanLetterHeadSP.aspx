﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryChallanLetterHeadSP.aspx.cs" Inherits="MvcRetailApp.ReportEngine.DeliveryChallanLetterHeadSP" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 392px">
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="499px" Width="985px">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
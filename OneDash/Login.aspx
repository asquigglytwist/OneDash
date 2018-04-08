<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%-- [BIB]:  http://www.4guysfromrolla.com/webtech/110701-1.2.shtml --%>
    <form id="frmLogin" runat="server">
        UserName: <asp:TextBox ID="TXTLUserName" runat="server" /> <br />
        PassWord: <asp:TextBox ID="TXTLPassWord" runat="server" TextMode="Password" />
        <p><asp:button id="BTNSubmit" Text="Login" runat="server" OnClick="BTNSubmit_Click" />
    </form>
</body>
</html>

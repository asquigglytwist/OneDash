<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Add.aspx.cs" Inherits="Products_Add" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #frmProducts
        {
            width: 90vw;
            margin: auto;
        }
        .displayGrid
        {
            min-width: 60vw;
            width: 65%;
            display: grid;
            grid-gap: 10px;
            grid-template-columns: 12em auto;
        }
        #frmProducts input[type="text"]
        {
            border: 1px dotted mediumaquamarine;
            transition: border-color var(--transition-time) ease-out;
        }
        #frmProducts [type="text"]:hover,
        #frmProducts [type="text"]:active,
        #frmProducts [type="text"]:focus
        {
            border-color: black;
        }
        .displayGrid > label:nth-of-type(even),
        .displayGrid > input[type="text"]:nth-of-type(odd)
        {
            background-color: var(--mild-alternating-color);
        }
        .ghostButton
        {
            display: inline-block;
            border: 1px solid black;
            border-radius: 1px;
            text-align: center;
            min-width: 50px;
            min-height: 25px;
            background-color: white;
            color: #3a3a3a;
            transition: background-color var(--transition-time) ease-out,
                color var(--transition-time) ease-out,
                border-radius var(--transition-time) ease-out,
                border-color var(--transition-time) ease-out;
        }
        .ghostButton:hover,
        .ghostButton:active,
        .ghostButton:focus
        {
            background-color: #7a7a7a;
            color: white;
            border-radius: 5px;
            border-color: #7a7a7a;
        }
    </style>
</head>
<body>
    <section id="AddNewProduct" class="boxDebug">
        <form id="frmProducts" runat="server" class="boxDebug">
            <h2>Add a new Product</h2>
            <section class="displayGrid boxDebug">
                <label for="PCodeName" class="boxDebug">Code Name</label>
                <asp:TextBox runat="server" ID="PCodeName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product's code name here"
                    ToolTip="Enter product's code name here" />

                <label for="PDisplayName" class="boxDebug">Display Name</label>
                <asp:TextBox runat="server" ID="PDisplayName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product's display name here"
                    ToolTip="Enter product's display name here" />

                <label for="PDesc" class="boxDebug">Description</label>
                <asp:TextBox runat="server" ID="PDesc" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter a brief description of the product here"
                    ToolTip="Enter a brief description of the product here" />

                <label for="PProjMgr" class="boxDebug">Project Manager</label>
                <asp:TextBox runat="server" ID="PProjMgr" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter Project Manager here"
                    ToolTip="Enter Project Manager here" />

                <label for="PProdMgr" class="boxDebug">Product Manager</label>
                <asp:TextBox runat="server" ID="PProdMgr" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter Product Manager here" title="Enter Product Manager here"
                    ToolTip="Enter Product Manager here" />
            </section>
            <asp:Button ID="btnAdd" Text="Add" CssClass="ghostButton" runat="server" />
            <asp:Button ID="btnReset" Text="Reset" CssClass="ghostButton" runat="server" />
        </form>
    </section>
</body>
</html>

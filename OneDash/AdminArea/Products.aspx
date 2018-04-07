<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="AdminArea_Products" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products</title>
    <link rel="stylesheet" href="../styles/global.css" />
    <link rel="stylesheet" href="../styles/products.css" />
</head>
<body>
    <h1><a href="#">Products</a></h1>
    <form id="Products" runat="server">
        <fieldset id="DropDownsPanel">
            <legend>Filter the Data:</legend>
            <label for="DDLExistingProducts">Product</label>
            <asp:DropDownList ID="DDLExistingProducts" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="DDLExistingProducts_SelectedIndexChanged" />
            <label for="DDLExistingVersions">Version</label>
            <asp:DropDownList ID="DDLExistingVersions" runat="server" AutoPostBack="true" Enabled="false"
                OnSelectedIndexChanged="DDLExistingVersions_SelectedIndexChanged" />
            <label for="DDLExistingReleases">Release</label>
            <asp:DropDownList ID="DDLExistingReleases" runat="server" AutoPostBack="true" Enabled="false"
                OnSelectedIndexChanged="DDLExistingReleases_SelectedIndexChanged" />
        </fieldset>
        <fieldset id="ProductPanel">
            <legend>Add / Edit a Product</legend>
            <section class="twoColGrid">
                <label for="TXTPCodeName">Code Name</label>
                <asp:TextBox runat="server" ID="TXTPCodeName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product's code name here"
                    ToolTip="Enter product's code name here" />
                <label for="TXTPDisplayName">Display Name</label>
                <asp:TextBox runat="server" ID="TXTPDisplayName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product's display name here"
                    ToolTip="Enter product's display name here" />
                <label for="TXTPDesc">Description</label>
                <asp:TextBox runat="server" ID="TXTPDesc" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter a brief description of the product here"
                    ToolTip="Enter a brief description of the product here" />
                <label for="TXTPProjMgr">Project Manager</label>
                <asp:TextBox runat="server" ID="TXTPProjMgr" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter Project Manager here"
                    ToolTip="Enter Project Manager here" />
                <label for="TXTPProdMgr">Product Manager</label>
                <asp:TextBox runat="server" ID="TXTPProdMgr" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter Product Manager here" title="Enter Product Manager here"
                    ToolTip="Enter Product Manager here" />
            </section>
            <section class="actionButtons">
                <asp:Button ID="BTNPAdd" Text="Add" CssClass="ghostButton" runat="server" OnClick="BTNPAdd_Click"
                    ToolTip="Add (/ Create) a new Product with the above details." />
                <asp:Button ID="BTNPUpdate" Text="Update" CssClass="ghostButton" runat="server" OnClick="BTNPUpdate_Click" Enabled="false"
                    ToolTip="Update details for an existing Product." />
                <asp:Button ID="BTNPDelete" Text="Delete" CssClass="ghostButton" runat="server" Enabled="false"
                    ToolTip="Delete the currently selected Product." OnClick="BTNPDelete_Click" />
            </section>
        </fieldset>
        <fieldset id="VersionPanel" runat="server">
            <legend>Add Version for a Product</legend>
            <section class="twoColGrid">
                <label for="TXTVVersion">Code Name</label>
                <asp:TextBox runat="server" ID="TXTVVersion" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product version (code) here"
                    ToolTip="Enter product version (code) here" />
                <label for="TXTVDisplayName">Display Name</label>
                <asp:TextBox runat="server" ID="TXTVDisplayName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product version (display) here"
                    ToolTip="Enter product version (display) here" />
                <label for="TXTVDescription">Description</label>
                <asp:TextBox runat="server" ID="TXTVDescription" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter version description here"
                    ToolTip="Enter version description here" />
            </section>
            <section class="actionButtons">
                <asp:Button ID="BTNVAdd" Text="Add" CssClass="ghostButton" runat="server" OnClick="BTNVAdd_Click"
                    ToolTip="Add (/ Create) a new Version with the above details." />
                <asp:Button ID="BTNVUpdate" Text="Update" CssClass="ghostButton" runat="server" OnClick="BTNVUpdate_Click" Enabled="false"
                    ToolTip="Update details for an existing Version." />
                <asp:Button ID="BTNVDelete" Text="Delete" CssClass="ghostButton" runat="server" OnClick="BTNVDelete_Click" Enabled="false"
                    ToolTip="Delete the currently selected Version." />
            </section>
        </fieldset>
        <fieldset id="ReleasePanel" runat="server">
            <legend>Add Release for a Version</legend>
            <section class="twoColGrid">
                <label for="TXTRReleaseCode">Code Name</label>
                <asp:TextBox runat="server" ID="TXTRReleaseCode" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter Release (code) name here"
                    ToolTip="Enter Release (code) name here" />
                <label for="TXTRDisplayName">Display Name</label>
                <asp:TextBox runat="server" ID="TXTRDisplayName" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter product version (display) here"
                    ToolTip="Enter product version (display) here" />
                <label for="TXTRDescription">Description</label>
                <asp:TextBox runat="server" ID="TXTRDescription" AutoCompleteType="Enabled" TextMode="SingleLine" Text=""
                    placeholder="Enter version description here"
                    ToolTip="Enter version description here" />
                <label for="DDLRStage">Stage</label>
                <asp:DropDownList ID="DDLRStage" runat="server" />
                <label for="TXTRTargetDate">Target Date</label>
                <asp:TextBox TextMode="Date" ID="TXTRTargetDate" runat="server" />
                <label for="DDLRRisk">Risk Assessment</label>
                <asp:DropDownList ID="DDLRRisk" runat="server" />
                <label for="DDLRRisk">Bug List (CSV)</label>
                <asp:FileUpload ID="FURBugList" runat="server" AllowMultiple="false" />
            </section>
            <section class="actionButtons">
                <asp:Button ID="BTNRAdd" Text="Add" CssClass="ghostButton" runat="server"
                    ToolTip="Add (/ Create) a new Release with the above details." OnClick="BTNRAdd_Click" />
                <asp:Button ID="BTNRUpdate" Text="Update" CssClass="ghostButton" runat="server" Enabled="false"
                    ToolTip="Update details for an existing Release." OnClick="BTNRUpdate_Click" />
                <asp:Button ID="BTNRDelete" Text="Delete" CssClass="ghostButton" runat="server" Enabled="false"
                    ToolTip="Delete the currently selected Release." OnClick="BTNRDelete_Click" />
            </section>
        </fieldset>
        <asp:Button ID="BTNWholeReset" Text="Reset" CssClass="ghostButton" runat="server" OnClick="BTNWholeReset_Click" />
    </form>
</body>
</html>

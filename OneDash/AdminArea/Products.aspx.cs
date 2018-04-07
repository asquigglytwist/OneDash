﻿using System;
using System.Web.UI.WebControls;

public partial class AdminArea_Products : System.Web.UI.Page
{
    #region Fields
    const string NoneSelectedValue = "--None--";
    const string GenericNoneSelectedText = "- - -";
    const string NoProdSelectedText = "Add New Product";
    const string NoVersionSelectedText = "Add New Version";
    const string NoReleaseSelectedText = "Add New Release";
    #endregion

    #region Helpers
    private bool IsValidProductInfo()
    {
        TextBox[] textBoxes = { TXTPCodeName, TXTPDesc, TXTPDisplayName, TXTPProjMgr, TXTPProdMgr };
        string[] errorMsgs = { "CodeName",
            "Description",
            "DisplayName",
            "ProjectManager",
            "ProductManager"
        };
        for (var i = 0; i < textBoxes.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(textBoxes[i].Text))
            {
                throw new Exception(string.Format("{0} for the product is mandatory.", errorMsgs[i]));
            }
        }
        return true;
    }

    private void SaveProductToFile(string codeName)
    {
        if (IsValidProductInfo())
        {
            var prod = new Product(codeName, TXTPDesc.Text, TXTPDisplayName.Text, TXTPProjMgr.Text, TXTPProdMgr.Text);
            if (prod.SaveToFile())
            {
                DDLExistingProducts.Items.Add(new ListItem(codeName, codeName));
                ResetUIState();
                //LastKnownProduct = prod;
            }
        }
    }

    private void ToggleVersionPanelEnabled(bool enabled)
    {
        TXTVVersion.Enabled = TXTVDisplayName.Enabled = TXTVDescription.Enabled = BTNVAdd.Enabled = enabled;
        TXTVVersion.Text = TXTVDisplayName.Text = TXTVDescription.Text = string.Empty;
    }

    private void ToggleReleasePanelEnabled(bool enabled)
    {
        TXTRReleaseCode.Enabled = TXTRDisplayName.Enabled = TXTRDescription.Enabled = BTNRAdd.Enabled =
            DDLRStage.Enabled = DDLRRisk.Enabled = TXTRTargetDate.Enabled = enabled;
        TXTRReleaseCode.Text = TXTRDisplayName.Text = TXTRDescription.Text = string.Empty;
    }

    private void ResetUIState()
    {
        TXTPCodeName.Text = TXTPDisplayName.Text = TXTPDesc.Text = TXTPProjMgr.Text = TXTPProdMgr.Text = string.Empty;
        TXTPCodeName.Enabled = BTNPAdd.Enabled = true;
        BTNPUpdate.Enabled = false;
        if (!DDLExistingProducts.SelectedValue.Equals(NoneSelectedValue))
        {
            DDLExistingProducts.SelectedValue = NoneSelectedValue;
        }
        DDLExistingVersions.Items.Clear();
        DDLExistingVersions.Enabled = false;
        ToggleVersionPanelEnabled(false);
    }

    private bool IsValidVersionInfo()
    {
        TextBox[] textBoxes = { TXTVVersion, TXTVDisplayName, TXTVDescription };
        string[] errorMsgs = { "CodeName.",
            "DisplayName",
            "Description"
        };
        for (var i = 0; i < textBoxes.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(textBoxes[i].Text))
            {
                throw new Exception(string.Format("{0} for the version is mandatory.", errorMsgs[i]));
            }
        }
        return true;
    }

    private bool IsValidReleaseInfo()
    {
        TextBox[] textBoxes = { TXTRReleaseCode, TXTRDisplayName, TXTRDescription, TXTRTargetDate };
        string[] errorMsgs = { "CodeName.",
            "DisplayName",
            "Description",
            "TargetDate"
        };
        for (var i = 0; i < textBoxes.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(textBoxes[i].Text))
            {
                throw new Exception(string.Format("{0} for the release is mandatory.", errorMsgs[i]));
            }
        }
        if (DDLRStage.SelectedValue.Equals(NoneSelectedValue))
        {
            throw new Exception("Stage of the release is mandatory.");
        }
        if (DDLRRisk.SelectedValue.Equals(NoneSelectedValue))
        {
            throw new Exception("RiskLevel of the release is mandatory.");
        }
        return true;
    }

    private void SaveVersionToFile(string codeName)
    {
        if (IsValidVersionInfo())
        {
            var versn = new Version(codeName, TXTVDisplayName.Text, TXTVDescription.Text);
            if (versn.SaveToFile(DDLExistingProducts.SelectedValue))
            {
                ResetUIState();
            }
        }
    }

    private void SaveReleaseToFile(string codeName)
    {
        if (IsValidReleaseInfo())
        {
            var release = new Release(codeName, TXTRDisplayName.Text, TXTRDescription.Text, DDLRStage.SelectedValue, TXTRTargetDate.Text, DDLRRisk.SelectedValue);
            if (release.SaveToFile(DDLExistingProducts.SelectedValue, DDLExistingVersions.SelectedValue))
            {
                ResetUIState();
            }
        }
    }

    private void OnProductSelectionChange()
    {
        if (DDLExistingProducts.SelectedValue.Equals(NoneSelectedValue))
        {
            ResetUIState();
            return;
        }
        var prodCodeName = DDLExistingProducts.SelectedValue;
        var prod = Product.LoadFromFile(prodCodeName);
        TXTPCodeName.Text = prod.CodeName;
        TXTPDisplayName.Text = prod.DisplayName;
        TXTPDesc.Text = prod.Description;
        TXTPProjMgr.Text = prod.ProjectManager;
        TXTPProdMgr.Text = prod.ProductManager;
        TXTPCodeName.Enabled = BTNPAdd.Enabled = false;
        BTNPUpdate.Enabled = true;
        DDLExistingVersions.Items.Clear();
        DDLExistingVersions.Items.Add(new ListItem(NoVersionSelectedText, NoneSelectedValue));
        foreach (var verCode in prod.GetAllVersionCodes())
        {
            DDLExistingVersions.Items.Add(new ListItem(verCode, verCode));
        }
        DDLExistingVersions.Enabled = true;
        DDLExistingVersions.SelectedIndex = 0;
        ToggleVersionPanelEnabled(true);
    }

    private void OnVersionSelectionChange()
    {
        ToggleVersionPanelEnabled(true);
        if (DDLExistingVersions.SelectedValue.Equals(NoneSelectedValue))
        {
            BTNVUpdate.Enabled = false;
            return;
        }
        var verCodeName = DDLExistingVersions.SelectedValue;
        var versn = Version.LoadFromFile(DDLExistingProducts.SelectedValue, verCodeName);
        TXTVVersion.Text = versn.CodeName;
        TXTVDisplayName.Text = versn.DisplayName;
        TXTVDescription.Text = versn.Description;
        TXTVVersion.Enabled = BTNVAdd.Enabled = false;
        BTNVUpdate.Enabled = true;
        DDLExistingReleases.Items.Clear();
        DDLExistingReleases.Items.Add(new ListItem(NoReleaseSelectedText, NoneSelectedValue));
        foreach (var verCode in versn.GetAllReleaseCodes(DDLExistingProducts.SelectedValue))
        {
            DDLExistingReleases.Items.Add(new ListItem(verCode, verCode));
        }
        DDLExistingReleases.Enabled = true;
        DDLExistingReleases.SelectedIndex = 0;
        ToggleReleasePanelEnabled(true);
    }

    private void OnReleaseSelectionChange()
    {
        ToggleReleasePanelEnabled(true);
        if (DDLExistingReleases.SelectedValue.Equals(NoneSelectedValue))
        {
            BTNRUpdate.Enabled = false;
            return;
        }
        var release = Release.LoadFromFile(DDLExistingProducts.SelectedValue, DDLExistingVersions.SelectedValue, DDLExistingReleases.SelectedValue);
        TXTRReleaseCode.Text = release.CodeName;
        TXTRDisplayName.Text = release.DisplayName;
        TXTRDescription.Text = release.Description;
        DDLRStage.SelectedValue = release.Stage.ToString();
        TXTRTargetDate.Text = release.TargetDate;
        DDLRRisk.SelectedValue = release.Risk.ToString();
        TXTRReleaseCode.Enabled = BTNRAdd.Enabled = false;
        BTNRUpdate.Enabled = true;
    }

    private void HandlePageRouteData()
    {
        var routeDataValues = Page.RouteData.Values;
        if (routeDataValues.TryGetValue("ProdCode", out object prodCodeObj))
        {
            var prodCode = prodCodeObj.ToString();
            if (DDLExistingProducts.Items.FindByValue(prodCode) != null)
            {
                DDLExistingProducts.SelectedValue = prodCode;
                OnProductSelectionChange();
                if (routeDataValues.TryGetValue("VerCode", out object verCodeObj))
                {
                    var verCode = verCodeObj.ToString();
                    if (DDLExistingVersions.Items.FindByValue(verCode) != null)
                    {
                        DDLExistingVersions.SelectedValue = verCode;
                        OnVersionSelectionChange();
                        if (routeDataValues.TryGetValue("RelCode", out object relCodeObj))
                        {
                            var relCode = relCodeObj.ToString();
                            if (DDLExistingReleases.Items.FindByValue(relCode) != null)
                            {
                                DDLExistingReleases.SelectedValue = relCode;
                                OnReleaseSelectionChange();
                            }
                        }
                    }
                }
            }
        }
    }

    private void AddEnumValuesToDropDown(DropDownList ddlTarget, Type typeOfEnum)
    {
        var itemNames = Enum.GetNames(typeOfEnum);
        ddlTarget.Items.Clear();
        ddlTarget.Items.Add(new ListItem(GenericNoneSelectedText, NoneSelectedValue));
        for (int i = 0; i <= itemNames.Length - 1; i++)
        {
            ddlTarget.Items.Add(new ListItem(itemNames[i], itemNames[i]));
        }
    }
    #endregion

    #region Page
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (DDLExistingProducts.Items.FindByValue(NoneSelectedValue) == null)
            {
                DDLExistingProducts.Items.Add(new ListItem(NoProdSelectedText, NoneSelectedValue));
            }
            var allProdCodeNames = Product.GetAllProductCodeNames();
            foreach (var codeName in allProdCodeNames)
            {
                if (DDLExistingProducts.Items.FindByValue(codeName) == null)
                {
                    DDLExistingProducts.Items.Add(new ListItem(codeName, codeName));
                }
            }
            AddEnumValuesToDropDown(DDLRRisk, typeof(RiskLevel));
            AddEnumValuesToDropDown(DDLRStage, typeof(ReleaseStages));
            ToggleVersionPanelEnabled(false);
            ToggleReleasePanelEnabled(false);
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //if (Page.IsPostBack)
        //{
        //    //UpdateUrlToReflectSelection();
        //}
        //else
        {
            HandlePageRouteData();
        }
    }

    protected void BTNWholeReset_Click(object sender, EventArgs e)
    {
        ResetUIState();
    }
    #endregion

    #region Product
    protected void DDLExistingProducts_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnProductSelectionChange();
    }

    protected void BTNPAdd_Click(object sender, EventArgs e)
    {
        var codeName = TXTPCodeName.Text;
        if (DDLExistingProducts.Items.FindByValue(codeName) != null)
        {
            throw new Exception(string.Format("Product {0} already exists; CodeName for product must be unique.", codeName));
        }
        SaveProductToFile(codeName);
    }

    protected void BTNPUpdate_Click(object sender, EventArgs e)
    {
        SaveProductToFile(TXTPCodeName.Text);
        ResetUIState();
    }

    protected void BTNPDelete_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region Version
    protected void DDLExistingVersions_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnVersionSelectionChange();
    }

    protected void BTNVAdd_Click(object sender, EventArgs e)
    {
        var codeName = TXTVVersion.Text;
        if (DDLExistingVersions.Items.FindByValue(codeName) != null)
        {
            throw new Exception(string.Format("Version {0} already exists; CodeName for version must be unique.", codeName));
        }
        SaveVersionToFile(codeName);
    }

    protected void BTNVDelete_Click(object sender, EventArgs e)
    {
    }

    protected void BTNVUpdate_Click(object sender, EventArgs e)
    {
        SaveVersionToFile(TXTVVersion.Text);
        ToggleVersionPanelEnabled(false);
    }
    #endregion

    #region Release
    protected void DDLExistingReleases_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnReleaseSelectionChange();
    }

    protected void BTNRAdd_Click(object sender, EventArgs e)
    {
        var codeName = TXTRReleaseCode.Text;
        if (DDLExistingReleases.Items.FindByValue(codeName) != null)
        {
            throw new Exception(string.Format("Release {0} already exists; CodeName for a release must be unique.", codeName));
        }
        SaveReleaseToFile(codeName);
    }

    protected void BTNRUpdate_Click(object sender, EventArgs e)
    {
        SaveReleaseToFile(TXTRReleaseCode.Text);
        ToggleReleasePanelEnabled(false);
    }

    protected void BTNRDelete_Click(object sender, EventArgs e)
    {
    }
    #endregion
}
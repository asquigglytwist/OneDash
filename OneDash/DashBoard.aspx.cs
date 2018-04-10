using System;
using System.Collections.Generic;

public partial class DashBoard : System.Web.UI.Page
{
    List<Product> Products;

    private void LoadAllInfo()
    {
        var ProductCodeNames = Product.GetAllProductCodeNames();
        Products = new List<Product>(ProductCodeNames.Count);
        foreach (var prodCodeName in ProductCodeNames)
        {
            var Prod = Product.LoadFromFile(prodCodeName);
            var VersionCodeNames = Prod.GetAllVersionCodes();
            Prod.ProductVersions = new List<Version>(VersionCodeNames.Count);
            foreach (var verCodeName in VersionCodeNames)
            {
                var Versn = Version.LoadFromFile(prodCodeName, verCodeName);
                var ReleaseCodeNames = Versn.GetAllReleaseCodes(prodCodeName);
                Versn.ProductVersionReleases = new List<Release>(ReleaseCodeNames.Count);
                foreach (var releaseCodeName in ReleaseCodeNames)
                {
                    var Relse = Release.LoadFromFileWithBugList(prodCodeName, verCodeName, releaseCodeName);
                    Versn.ProductVersionReleases.Add(Relse);
                }
                Prod.ProductVersions.Add(Versn);
            }
            Products.Add(Prod);
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.ReWriteUrlsWithAdminArea();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadAllInfo();
        if(Products.Count > 0)
        {
            rpCurrentProducts.DataSource = Products;
            rpCurrentProducts.DataBind();
        }
    }
}

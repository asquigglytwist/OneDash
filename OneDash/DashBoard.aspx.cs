using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//  [BIB]:  https://forums.asp.net/t/1595729.aspx?Remove+Hidden+Fields+From+Web+Page
public class PageBase : Page
{
    string[] tags = { "__VIEWSTATE", "__EVENTTARGET", "__EVENTARGUMENT", "__EVENTVALIDATION" };

    protected override void Render(HtmlTextWriter writer)
    {
        StringBuilder sb = new StringBuilder();
        HtmlTextWriter htw = new HtmlTextWriter(new StringWriter(sb));
        base.Render(htw);
        string html = sb.ToString();
        foreach (string tag in tags)
        {
            html = this.RemoveTag(tag, html);
        }
        writer.Write(html);
    }

    public string RemoveTag(string tag, string html)
    {
        int lowerBound = html.IndexOf(tag);
        if (lowerBound < 0)
        {
            return html;
        }
        while (html[lowerBound--] != '<')
        {
        }
        int upperBound = html.IndexOf("/>", lowerBound) + 2;
        html = html.Remove(lowerBound, upperBound - lowerBound);
        if (html.Contains(tag))
        {
            html = this.RemoveTag(tag, html);
        }
        return html;
    }
}

internal class Product
{
    public string CodeName
    { get; internal set; }
    public string DisplayName
    { get; internal set; }
    public string Description
    { get; internal set; }
    public string ProjectManager
    { get; internal set; }
    public string ProductManager
    { get; internal set; }
    public List<Version> Versions
    { get; internal set; }

    public Product()
    {
        Versions = new List<Version>();
    }
}

internal class Version
{
    public string DisplayVersion
    { get; internal set; }
    public List<Release> Releases
    { get; internal set; }

    public Version()
    {
        Releases = new List<Release>();
    }
}

enum ReleaseStages
{
    Estimation,
    DesignReview,
    TestReview,
    BudgetReview,
    PlanCommit,
    Started,
    InProgress,
    FC,
    Beta,
    RC,
    RTS,
    RTW,
    GA
}

enum RiskLevel
{
    Green,
    Yellow,
    Red
}

internal class Release
{
    public string ReleaseName
    { get; internal set; }
    public ReleaseStages Stage
    { get; internal set; }
    public DateTime TargetDate
    { get; internal set; }
    public RiskLevel Risk
    { get; internal set; }
}

//public partial class Products : PageBase
public partial class DashBoard : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Product> lst = new List<Product>();
        lst.Add(new Product() { CodeName = "AAWin", DisplayName = "Awesome App (Win)", Description="An Awesome Application for Windows" });
        lst.Add(new Product() { CodeName = "AALin", DisplayName = "Awesome App (Linux)", Description="An Awesome Application for Linux" });
        lst.Add(new Product() { CodeName = "AADroid", DisplayName = "Awesome App (Android)", Description="An Awesome Application for Android" });

        lst[0].Versions.Add(new Version() { DisplayVersion = "1.0" });
        lst[0].Versions.Add(new Version() { DisplayVersion = "1.1" });

        lst[0].Versions[0].Releases.Add(new Release() { ReleaseName = "First", Stage = ReleaseStages.InProgress, TargetDate = DateTime.Parse("2017-01-01"), Risk = RiskLevel.Yellow });
        lst[0].Versions[0].Releases.Add(new Release() { ReleaseName = "Second", Stage = ReleaseStages.InProgress, TargetDate = DateTime.Parse("2017-04-01"), Risk = RiskLevel.Red });
        lst[0].Versions[0].Releases.Add(new Release() { ReleaseName = "Third", Stage = ReleaseStages.InProgress, TargetDate = DateTime.Parse("2017-07-01"), Risk = RiskLevel.Green });

        lst[0].Versions[1].Releases.Add(new Release() { ReleaseName = "First", Stage = ReleaseStages.RTS, TargetDate = DateTime.Parse("2017-10-01"), Risk = RiskLevel.Yellow });
        lst[0].Versions[1].Releases.Add(new Release() { ReleaseName = "Second", Stage = ReleaseStages.RTS, TargetDate = DateTime.Parse("2018-01-01"), Risk = RiskLevel.Green });

        rpCurrentProducts.DataSource = lst;
        rpCurrentProducts.DataBind();
        if (Page.IsPostBack)
        {
        }
    }
}
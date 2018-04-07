using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;

public class Release
{
    public const string RelXMLExtension = "rel";
    const string xtRelease = "Release",
        xtReleaseCode = "ReleaseCode",
        xtDisplayName = "DisplayName",
        xtDescription = "Description",
        xtStage = "Stage",
        xtTargetDate = "TargetDate",
        xtRiskLevel = "RiskLevel";

    public string CodeName
    { get; set; }
    public string DisplayName
    { get; set; }
    public string Description
    { get; set; }
    public ReleaseStages Stage
    { get; set; }
    public string TargetDate
    { get; set; }
    public RiskLevel Risk
    { get; set; }
    public string PermaLink
    { get; set; }

    public string BugListAsTable
    { get; protected set; }

    public Release(string codeName, string displayName, string description, string stage, string targetDate, string risk)
    {
        CodeName = codeName ?? throw new ArgumentNullException(nameof(codeName));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        if (Enum.TryParse(stage, out ReleaseStages relStage))
        {
            Stage = relStage;
        }
        TargetDate = targetDate;
        if (Enum.TryParse(risk, out RiskLevel riskLevel))
        {
            Risk = riskLevel;
        }
    }

    public bool SaveToFile(string productCodeName, string versionCodeName)
    {
        var dirPath = Path.Combine(AppGlobal.AppDataDirectory, productCodeName, versionCodeName);
        var XMLFilePath = Path.Combine(dirPath, string.Format("{0}.{1}", CodeName, RelXMLExtension));
        var xDoc = new XmlDocument();
        var declarationNode = xDoc.CreateXmlDeclaration("1.0", "", "");
        xDoc.AppendChild(declarationNode);
        var comment = xDoc.CreateComment(string.Format("This file contains information about {0} - {1}", CodeName, DisplayName));
        xDoc.AppendChild(comment);
        var docRoot = xDoc.CreateElement(xtRelease);
        XmlNode ndCodeName = xDoc.CreateElement(xtReleaseCode),
            ndDisplayName = xDoc.CreateElement(xtDisplayName),
            ndDescription = xDoc.CreateElement(xtDescription),
            ndStage = xDoc.CreateElement(xtStage),
            ndTargetDate = xDoc.CreateElement(xtTargetDate),
            ndRiskLevel = xDoc.CreateElement(xtRiskLevel);
        ndCodeName.InnerText = CodeName;
        ndDisplayName.InnerText = DisplayName;
        ndDescription.InnerText = Description;
        ndStage.InnerText = Stage.ToString();
        ndTargetDate.InnerText = TargetDate;
        ndRiskLevel.InnerText = Risk.ToString();
        docRoot.AppendChild(ndCodeName);
        docRoot.AppendChild(ndDisplayName);
        docRoot.AppendChild(ndDescription);
        docRoot.AppendChild(ndStage);
        docRoot.AppendChild(ndTargetDate);
        docRoot.AppendChild(ndRiskLevel);
        xDoc.AppendChild(docRoot);
        try
        {
            AppGlobal.CreateDirectory(dirPath);
            xDoc.Save(XMLFilePath);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("Unable to save Version {0} to file.{1}Message:{2}.", CodeName, Environment.NewLine, e.Message), e);
        }
    }

    public static Release LoadFromFile(string prodCodeName, string verCodeName, string relCodeName)
    {
        string filePath = Path.Combine(AppGlobal.AppDataDirectory, prodCodeName, verCodeName, string.Format("{0}.{1}", relCodeName, RelXMLExtension));
        try
        {
            var xDoc = XDocument.Load(filePath);
            string xCodeName = xDoc.Descendants(xtReleaseCode).First().Value,
                displayName = xDoc.Descendants(xtDisplayName).First().Value,
                description = xDoc.Descendants(xtDescription).First().Value,
                stage = xDoc.Descendants(xtStage).First().Value,
                targetDate = xDoc.Descendants(xtTargetDate).First().Value,
                risk = xDoc.Descendants(xtRiskLevel).First().Value;
            var rel = new Release(xCodeName, displayName, description, stage, targetDate, risk);
            rel.PermaLink = string.Format("{0}/{1}/{2}", prodCodeName, verCodeName, relCodeName);
            return rel;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("Unable to read Release info for {0}.{1}Message:{1}{2}.", relCodeName, Environment.NewLine, e.Message), e);
        }
    }

    public static Release LoadFromFileWithBugList(string prodCodeName, string verCodeName, string relCodeName)
    {
        var rel = LoadFromFile(prodCodeName, verCodeName, relCodeName);
        var dirPath = Path.Combine(AppGlobal.AppDataDirectory, prodCodeName, verCodeName, relCodeName);
        string htmlFilePath = Path.Combine(dirPath, "BugList.htmlSnippet"), htmlSnippet = string.Empty;
        if (File.Exists(htmlFilePath))
        {
            htmlSnippet = File.ReadAllText(htmlFilePath);
        }
        else
        {
            string csvFilePath = Path.Combine(dirPath, "BugList.csv");
            var csvParser = new CSV2HTMLParser(csvFilePath);
            htmlSnippet = csvParser.ToHTMLString(rel.DisplayName);
            if (!htmlSnippet.Equals("<div>--No Bug List available--</div>"))
            {
                File.WriteAllText(htmlFilePath, htmlSnippet, System.Text.Encoding.UTF8);
            }
        }
        rel.BugListAsTable = htmlSnippet;
        return rel;
    }
}
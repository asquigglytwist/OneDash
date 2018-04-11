using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Represents a Product's release.
/// </summary>
public class Release
{
    /// <summary>
    /// The File-Extension to be used for the XML document where "Release" gets Serialized.
    /// </summary>
    public const string RelXMLExtension = "rel";
    /// <summary>
    /// XML Tag Names (for private use only).
    /// </summary>
    const string xtRelease = "Release",
        xtReleaseCode = "ReleaseCode",
        xtDisplayName = "DisplayName",
        xtDescription = "Description",
        xtStage = "Stage",
        xtTargetDate = "TargetDate",
        xtRiskLevel = "RiskLevel",
        xtBuildNumber = "BuildNumber",
        xtReleaseType = "ReleaseType";

    /// <summary>
    /// The CodeName of the Release - something catchy :).
    /// </summary>
    public string CodeName
    { get; set; }
    /// <summary>
    /// The Name that should be used when Displayed.
    /// </summary>
    public string DisplayName
    { get; set; }
    /// <summary>
    /// A short description of what this Release is about or what features are expected.
    /// </summary>
    public string Description
    { get; set; }
    /// <summary>
    /// The Current stage of the Release.
    /// </summary>
    public ReleaseStages Stage
    { get; set; }
    /// <summary>
    /// Target Date for the Release.
    /// </summary>
    public string TargetDate
    { get; set; }
    /// <summary>
    /// A manually assessed Risk Level for the Release.
    /// </summary>
    public RiskLevel Risk
    { get; set; }
    /// <summary>
    /// Direct link (URL) to this Release within the Products page.
    /// </summary>
    public string PermaLink
    { get; set; }
    /// <summary>
    /// Latest known (stable) Build Number for the Release.
    /// </summary>
    public string BuildNumber
    { get; set; }
    public ReleaseTypes ReleaseType
    { get; set; }

    /// <summary>
    /// (An optional) HTML Table snippet of the Bug List for the Release, if a CSV is available.
    /// </summary>
    public string BugListAsTable
    { get; protected set; }

    public Release(string codeName, string displayName, string description, string stage, string targetDate, string risk, string buildNumber, string relType)
    {
        CodeName = codeName ?? throw new ArgumentNullException(nameof(codeName));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        BuildNumber = buildNumber ?? throw new ArgumentNullException(nameof(buildNumber));
        if (Enum.TryParse(stage, out ReleaseStages relStage))
        {
            Stage = relStage;
        }
        else
        {
            throw new Exception(string.Format("Value specified for ReleaseStage - {0}, is not recognized.", stage));
        }
        TargetDate = targetDate;
        if (Enum.TryParse(risk, out RiskLevel riskLevel))
        {
            Risk = riskLevel;
        }
        else
        {
            throw new Exception(string.Format("Value specified for RiskLevel - {0}, is not recognized.", riskLevel));
        }
        if (Enum.TryParse(relType, out ReleaseTypes rType))
        {
            ReleaseType = rType;
        }
        else
        {
            throw new Exception(string.Format("Value specified for RiskLevel - {0}, is not recognized.", riskLevel));
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
            ndRiskLevel = xDoc.CreateElement(xtRiskLevel),
            ndBuildNumber = xDoc.CreateElement(xtBuildNumber);
        ndCodeName.InnerText = CodeName;
        ndDisplayName.InnerText = DisplayName;
        ndDescription.InnerText = Description;
        ndStage.InnerText = Stage.ToString();
        ndTargetDate.InnerText = TargetDate;
        ndRiskLevel.InnerText = Risk.ToString();
        ndBuildNumber.InnerText = BuildNumber.ToString();
        docRoot.AppendChild(ndCodeName);
        docRoot.AppendChild(ndDisplayName);
        docRoot.AppendChild(ndDescription);
        docRoot.AppendChild(ndStage);
        docRoot.AppendChild(ndTargetDate);
        docRoot.AppendChild(ndRiskLevel);
        docRoot.AppendChild(ndBuildNumber);
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

    /// <summary>
    /// Load information on a Release from (previously) saved XML (.rel) file.
    /// </summary>
    /// <param name="prodCodeName">The associated Product's CodeName.</param>
    /// <param name="verCodeName">The associated Version's CodeName.</param>
    /// <param name="relCodeName">The associated Release's CodeName.</param>
    /// <returns>A Release object constructed by DeSerializing the XML file.</returns>
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
                risk = xDoc.Descendants(xtRiskLevel).First().Value,
                buildNumber = xDoc.Descendants(xtBuildNumber).First().Value,
                relType = xDoc.Descendants(xtReleaseType).First().Value;
            var rel = new Release(xCodeName, displayName, description, stage, targetDate, risk, buildNumber, relType)
            {
                PermaLink = string.Format("{0}/{1}/{2}", prodCodeName, verCodeName, relCodeName)
            };
            return rel;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("Unable to read Release info for {0}.{1}Message:{1}{2}.", relCodeName, Environment.NewLine, e.Message), e);
        }
    }

    /// <summary>
    /// Same as <see cref="LoadFromFile(string, string, string)"/> with the addition of populating the BugListAsTable field.
    /// </summary>
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
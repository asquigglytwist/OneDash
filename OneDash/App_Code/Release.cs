﻿using System;
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
            return new Release(xCodeName, displayName, description, stage, targetDate, risk);
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("Unable to read Release info for {0}.{1}Message:{1}{2}.", relCodeName, Environment.NewLine, e.Message), e);
        }
    }
}
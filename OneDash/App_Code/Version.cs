using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;

public class Version
{
    public const string VerXMLExtension = "ver";

    const string xtVersion = "Version",
        xtVersionCode = "VersionCode",
        xtDisplayName = "DisplayName",
        xtDescription = "Description";

    public string CodeName
    { get; set; }
    public string DisplayName
    { get; set; }
    public string Description
    { get; set; }
    public string PermaLink
    { get; set; }

    public List<Release> ProductVersionReleases
    { get; set; }

    public Version(string versionCode, string displayVersion, string description)
    {
        CodeName = versionCode ?? throw new ArgumentNullException(nameof(versionCode));
        DisplayName = displayVersion ?? throw new ArgumentNullException(nameof(displayVersion));
        Description = description?? throw new ArgumentNullException(nameof(description));
    }

    public bool SaveToFile(string productCodeName)
    {
        var dirPath = Path.Combine(AppGlobal.AppDataDirectory, productCodeName);
        var XMLFilePath = Path.Combine(dirPath, string.Format("{0}.{1}", CodeName, VerXMLExtension));
        var xDoc = new XmlDocument();
        var declarationNode = xDoc.CreateXmlDeclaration("1.0", "", "");
        xDoc.AppendChild(declarationNode);
        var comment = xDoc.CreateComment(string.Format("This file contains information about {0} - {1}", CodeName, DisplayName));
        xDoc.AppendChild(comment);
        var docRoot = xDoc.CreateElement(xtVersion);
        XmlNode ndCodeName = xDoc.CreateElement(xtVersionCode),
            ndDisplayName = xDoc.CreateElement(xtDisplayName),
            ndDescription = xDoc.CreateElement(xtDescription);
        ndCodeName.InnerText = CodeName;
        ndDisplayName.InnerText = DisplayName;
        ndDescription.InnerText = Description;
        docRoot.AppendChild(ndCodeName);
        docRoot.AppendChild(ndDisplayName);
        docRoot.AppendChild(ndDescription);
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

    public List<string> GetAllReleaseCodes(string productCode)
    {
        var dirPath = Path.Combine(AppGlobal.AppDataDirectory, productCode, CodeName);
        return AppGlobal.GetFileNamesWithoutExtension(dirPath, string.Format("*.{0}", Release.RelXMLExtension));
    }

    public static Version LoadFromFile(string prodCodeName, string verCodeName)
    {
        string filePath = Path.Combine(AppGlobal.AppDataDirectory, prodCodeName, string.Format("{0}.{1}", verCodeName, VerXMLExtension));
        try
        {
            var xDoc = XDocument.Load(filePath);
            string xCodeName = xDoc.Descendants(xtVersionCode).First().Value,
                displayName = xDoc.Descendants(xtDisplayName).First().Value,
                description = xDoc.Descendants(xtDescription).First().Value;
            var ver = new Version(xCodeName, displayName, description);
            ver.PermaLink = string.Format("{0}/{1}", prodCodeName, verCodeName);
            return ver;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("Unable to read Version info for {0}.{1}Message:{1}{2}.", verCodeName, Environment.NewLine, e.Message), e);
        }
    }
}
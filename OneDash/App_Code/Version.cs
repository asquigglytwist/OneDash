using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;

/// <summary>
/// Represents a Product's Version.
/// </summary>
public class Version
{
    /// <summary>
    /// The File-Extension to be used for the XML document where "Version" gets Serialized.
    /// </summary>
    public const string VerXMLExtension = "ver";
    /// <summary>
    /// XML Tag Names (for private use only).
    /// </summary>
    const string xtVersion = "Version",
        xtVersionCode = "VersionCode",
        xtDisplayName = "DisplayName",
        xtDescription = "Description";

    /// <summary>
    /// The CodeName of the Version - something catchy :).
    /// </summary>
    public string CodeName
    { get; set; }
    /// <summary>
    /// The Name that should be used when Displayed.
    /// </summary>
    public string DisplayName
    { get; set; }
    /// <summary>
    /// A short description of what this Version is about.
    /// </summary>
    public string Description
    { get; set; }
    /// <summary>
    /// Direct link (URL) to this Version within the Products page.
    /// </summary>
    public string PermaLink
    { get; set; }

    /// <summary>
    /// A list of Release(s) that this Version has.
    /// </summary>
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

    /// <summary>
    /// Retrieves a list of all ReleaseCodes that this Version has.
    /// </summary>
    /// <param name="productCode">CodeName of the Product for which ReleaseCodes are to be retrieved.</param>
    /// <returns>List of ReleaseCodes for the Version.</returns>
    public List<string> GetAllReleaseCodes(string productCode)
    {
        var dirPath = Path.Combine(AppGlobal.AppDataDirectory, productCode, CodeName);
        return AppGlobal.GetFileNamesWithoutExtension(dirPath, string.Format("*.{0}", Release.RelXMLExtension));
    }

    /// <summary>
    /// Load information on a Version from (previously) saved XML (.ver) file.
    /// </summary>
    /// <param name="prodCodeName">The associated Product's CodeName.</param>
    /// <param name="verCodeName">The associated Version's CodeName.</param>
    /// <returns>A Version object constructed by DeSerializing the XML file.</returns>
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
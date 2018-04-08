using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Represents a Product.
/// </summary>
public class Product
{
    /// <summary>
    /// The File-Extension to be used for the XML document where "Product" gets Serialized.
    /// </summary>
    public const string ProdXMLExtension = "prod";
    /// <summary>
    /// XML Tag Names (for private use only).
    /// </summary>
    const string xtProduct = "Product",
        xtCodeName = "CodeName",
        xtDisplayName = "DisplayName",
        xtDescription = "Description",
        xtProjMgr = "ProjectManager",
        xtProdMgr = "ProductManager";

    readonly string XMLFilePath, VersionsDirPath;

    public Product(string codeName, string displayName, string description, string projectManager, string productManager)
    {
        CodeName = codeName ?? throw new ArgumentNullException(nameof(codeName));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ProjectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
        ProductManager = productManager ?? throw new ArgumentNullException(nameof(productManager));
        XMLFilePath = Path.Combine(AppGlobal.AppDataDirectory, string.Format("{0}.{1}", CodeName, ProdXMLExtension));
        VersionsDirPath = Path.Combine(AppGlobal.AppDataDirectory, CodeName);
    }

    /// <summary>
    /// The CodeName of the Product - something catchy :).
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
    /// The Project Manager for this Product.
    /// </summary>
    public string ProjectManager
    { get; set; }
    /// <summary>
    /// The Product Manager for this Product.
    /// </summary>
    public string ProductManager
    { get; set; }
    /// <summary>
    /// Direct link (URL) to this Product within the Products page.
    /// </summary>
    public string PermaLink
    { get; set; }

    /// <summary>
    /// A list of Version(s) that this Product has.
    /// </summary>
    public List<Version> ProductVersions
    { get; set; }

    public bool SaveToFile()
    {
        var xDoc = new XmlDocument();
        var declarationNode = xDoc.CreateXmlDeclaration("1.0", "", "");
        xDoc.AppendChild(declarationNode);
        var comment = xDoc.CreateComment(string.Format("This file contains information about {0} - {1}", CodeName, DisplayName));
        xDoc.AppendChild(comment);
        var docRoot = xDoc.CreateElement(xtProduct);
        XmlNode ndCodeName = xDoc.CreateElement(xtCodeName),
            ndDisplayName = xDoc.CreateElement(xtDisplayName),
            ndDescription = xDoc.CreateElement(xtDescription),
            ndProjectMgr = xDoc.CreateElement(xtProjMgr),
            ndProductMgr = xDoc.CreateElement(xtProdMgr);
        ndCodeName.InnerText = CodeName;
        ndDisplayName.InnerText = DisplayName;
        ndDescription.InnerText = Description;
        ndProjectMgr.InnerText = ProjectManager;
        ndProductMgr.InnerText = ProductManager;
        docRoot.AppendChild(ndCodeName);
        docRoot.AppendChild(ndDisplayName);
        docRoot.AppendChild(ndDescription);
        docRoot.AppendChild(ndProjectMgr);
        docRoot.AppendChild(ndProductMgr);
        xDoc.AppendChild(docRoot);
        try
        {
            xDoc.Save(XMLFilePath);
            Directory.CreateDirectory(VersionsDirPath);
            return true;
        }
        catch(Exception e)
        {
            throw new Exception(string.Format("Unable to save Product {0} to file.{1}Message:{2}.", CodeName, Environment.NewLine, e.Message), e);
        }
    }

    /// <summary>
    /// Retrieves a list of all VersionCodes that this Product has.
    /// </summary>
    /// <returns>List of VersionCodes for the Version.</returns>
    public List<string> GetAllVersionCodes()
    {
        return AppGlobal.GetFileNamesWithoutExtension(VersionsDirPath, string.Format("*.{0}", Version.VerXMLExtension));
    }

    /// <summary>
    /// Retrieves a list of all ProductCodes that are currently known (i.e., available).
    /// </summary>
    public static List<string> GetAllProductCodeNames()
    {
        return AppGlobal.GetFileNamesWithoutExtension(AppGlobal.AppDataDirectory, string.Format("*.{0}", Product.ProdXMLExtension));
    }

    /// <summary>
    /// Load information on a Product from (previously) saved XML (.prod) file.
    /// </summary>
    /// <param name="codeName">The associated Product's CodeName.</param>
    /// <returns>A Product object constructed by DeSerializing the XML file.</returns>
    public static Product LoadFromFile(string codeName)
    {
        var xmlFileName = string.Format("{0}.{1}", codeName, ProdXMLExtension);
        var xmlFilePath = Path.Combine(AppGlobal.AppDataDirectory, xmlFileName);
        try
        {
            var xDoc = XDocument.Load(xmlFilePath);
            string xCodeName = xDoc.Descendants(xtCodeName).First().Value,
                displayName = xDoc.Descendants(xtDisplayName).First().Value,
                description = xDoc.Descendants(xtDescription).First().Value,
                projMgr = xDoc.Descendants(xtProjMgr).First().Value,
                prodMgr = xDoc.Descendants(xtProdMgr).First().Value;
            if (codeName.Equals(xCodeName))
            {
                var prod = new Product(codeName, displayName, description, projMgr, prodMgr);
                prod.PermaLink = codeName;
                return prod;
            }
            else
            {
                throw new Exception(string.Format("Product code-name mismatch; Expected {0}, got {1} from {2}.", codeName, xCodeName, xmlFileName));
            }
        }
        catch(Exception e)
        {
            throw new Exception(string.Format("Unable to read Product info for {0}.{1}Message:{1}{2}.", codeName, Environment.NewLine, e.Message), e);
        }
    }
}

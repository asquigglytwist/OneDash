using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

public class Product
{
    public const string ProdXMLExtension = "prod";

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

    public string CodeName
    { get; set; }
    public string DisplayName
    { get; set; }
    public string Description
    { get; set; }
    public string ProjectManager
    { get; set; }
    public string ProductManager
    { get; set; }
    public string PermaLink
    { get; set; }

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

    public List<string> GetAllVersionCodes()
    {
        return AppGlobal.GetFileNamesWithoutExtension(VersionsDirPath, string.Format("*.{0}", Version.VerXMLExtension));
    }

    public static List<string> GetAllProductCodeNames()
    {
        return AppGlobal.GetFileNamesWithoutExtension(AppGlobal.AppDataDirectory, string.Format("*.{0}", Product.ProdXMLExtension));
    }

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

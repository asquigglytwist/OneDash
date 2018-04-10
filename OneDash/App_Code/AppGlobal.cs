using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using System.Web.UI;

/// <summary>
/// Static class to hold all common / shared functionality across the WebApp.
/// </summary>
public static class AppGlobal
{
    /// <summary>
    /// Represents the "App_Data" (ASP.NET special) directory where the WebApp's local data is stored.
    /// </summary>
    public static readonly string AppDataDirectory;
    static AppGlobal()
    {
        AppDataDirectory = HostingEnvironment.MapPath("~/App_Data");
        if (!Directory.Exists(AppDataDirectory))
        {
            Directory.CreateDirectory(AppDataDirectory);
        }
    }

    /// <summary>
    /// Registers a list of custom routes for routing requests within the WebApp.
    /// </summary>
    /// <param name="routes"></param>
    public static void RegisterOneDashCustomRoutes(RouteCollection routes)
    {
        routes.MapPageRoute("Defaults", "", "~/DashBoard.aspx", true);
        routes.MapPageRoute("DashBoard", "DashBoard", "~/DashBoard.aspx", true);
        routes.MapPageRoute("Products", "Products", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("ProductInfo", "Products/{ProdCode}", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("VersionInfo", "Products/{ProdCode}/{VerCode}", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("ReleaseInfo", "Products/{ProdCode}/{VerCode}/{RelCode}", "~/AdminArea/Products.aspx", true);
    }

    /// <summary>
    /// Gets a list of FileNames (names only and not path, without their extensions).
    /// </summary>
    /// <param name="dir">The Directory to search in (e.g: *.txt).</param>
    /// <param name="pattern">The Pattern to search for, within the directory.</param>
    /// <returns>A list of file names without extensions.</returns>
    public static List<string> GetFileNamesWithoutExtension(string dir, string pattern)
    {
        if (Directory.Exists(dir))
        {
            return new DirectoryInfo(dir).GetFiles(pattern, SearchOption.TopDirectoryOnly)
                .Select(file => Path.GetFileNameWithoutExtension(file.Name)).ToList();
        }
        return new List<string>();
    }

    public static List<string> GetCompleteFilePaths(string dir, string pattern)
    {
        if (Directory.Exists(dir))
        {
            return new DirectoryInfo(dir).GetFiles(pattern, SearchOption.TopDirectoryOnly)
                .Select(file => (file.FullName)).ToList();
        }
        return new List<string>();
    }

    /// <summary>
    /// Creates a Directory, if it doesn't exist.
    /// </summary>
    /// <param name="dirPath">Path of the Directory to Create (or check).</param>
    public static void CreateDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public static string DecodeUrlString(this Page page)
    {
        // [BIB]:  https://stackoverflow.com/questions/1405048/how-do-i-decode-a-url-parameter-using-c
        var url = page.Request.Url.ToString();
        var newUrl = Uri.UnescapeDataString(url);
        while ((newUrl = Uri.UnescapeDataString(newUrl)) != newUrl)
        {
            //newUrl = newUrl;
        }
        if (!newUrl.Equals(url))
        {
            page.Response.Redirect(newUrl, true);
        }
        return newUrl;
    }

    public static void ReWriteUrlsWithAdminArea(this System.Web.UI.Page page/*ref HttpRequest request, ref HttpResponse response*/)
    {
        bool changed = false;
        var newUrl = page.Request.Url.ToString();
        if (newUrl.Contains("AdminArea"))
        {
            newUrl = newUrl.Replace("AdminArea", "");
            changed = true;
        }
        if (newUrl.Contains(".aspx"))
        {
            newUrl = newUrl.Replace(".aspx", "");
            changed = true;
        }
        int i = newUrl.IndexOf("//", 8);
        if (i > 0)
        {
            newUrl = newUrl.Substring(0, i) + newUrl.Substring(i + 1);
            changed = true;
        }
        if (changed)
        {
            page.Response.Redirect(newUrl.ToString(), true);
        }
    }
}

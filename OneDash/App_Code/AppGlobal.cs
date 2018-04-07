using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;

public static class AppGlobal
{
    public static readonly string AppDataDirectory;
    static AppGlobal()
    {
        AppDataDirectory = HostingEnvironment.MapPath("~/App_Data");
        if (!Directory.Exists(AppDataDirectory))
        {
            Directory.CreateDirectory(AppDataDirectory);
        }
    }

    public static void RegisterOneDashCustomRoutes(RouteCollection routes)
    {
        routes.MapPageRoute("Products", "Products", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("ProductInfo", "Products/{ProdCode}", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("VersionInfo", "Products/{ProdCode}/{VerCode}", "~/AdminArea/Products.aspx", true);
        routes.MapPageRoute("ReleaseInfo", "Products/{ProdCode}/{VerCode}/{RelCode}", "~/AdminArea/Products.aspx", true);
    }

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

    public static void CreateDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

public class CSV2HTMLParser
{
    public string CSVFilePath
    { get; protected set; }

    public CSV2HTMLParser(string filePath)
    {
        CSVFilePath = filePath;
    }

    public string ToHTMLString(string tableCaption)
    {
        if (!File.Exists(CSVFilePath))
        {
            return "<div>--No Bug List available--</div>";
        }
        var sbHTable = new StringBuilder();
        var curLine = string.Empty;
        sbHTable.AppendFormat("<table><caption>{0}</caption>", tableCaption);
        foreach (var line in File.ReadLines(CSVFilePath))
        {
            sbHTable.Append("<tr>");
            var tokens = line.Split(new char[] { ',' }, StringSplitOptions.None);
            for (long i = 0; i < tokens.LongLength; i++)
            {
                var token = tokens[i];
                if (token.StartsWith("\"") && !token.EndsWith("\""))
                {
                    var sbFullToken = new StringBuilder();
                    while(!tokens[i].EndsWith("\""))
                    {
                        sbFullToken.Append(tokens[i++]);
                    }
                    token = sbFullToken.ToString();
                }
                sbHTable.AppendFormat("<td>{0}</td>", string.IsNullOrWhiteSpace(token) ? "&mdash;" : token);
            }
            sbHTable.Append("</tr>");
        }
        sbHTable.Append("<tbody>");
        sbHTable.Append("</tbody></table>");
        return sbHTable.ToString();
    }
}
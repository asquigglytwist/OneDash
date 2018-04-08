using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Converts CSV Data to a HTML Table.
/// </summary>
public class CSV2HTMLParser
{
    /// <summary>
    /// Path to the CSV File which either is to be or is already parsed.
    /// </summary>
    public string CSVFilePath
    { get; protected set; }

    public CSV2HTMLParser(string filePath)
    {
        CSVFilePath = filePath;
    }

    // [BIB]:  https://stackoverflow.com/a/8809437
    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    /// <summary>
    /// Converts the CSV file to a HTML Table.
    /// </summary>
    /// <param name="tableCaption">Caption for the table.</param>
    /// <param name="stripQuotes">Indicates whether Double-Quote characters should be removed from the CSV fields.</param>
    /// <returns>HTML snippet for the Table constructed from CSV.</returns>
    public string ToHTMLString(string tableCaption, /*bool firstRowIsHeader =true,*/ bool stripQuotes = true)
    {
        if (!File.Exists(CSVFilePath))
        {
            return "<div>--No Bug List available--</div>";
        }
        var sbHTable = new StringBuilder();
        var curLine = string.Empty;
        sbHTable.AppendFormat("<table><caption>{0}</caption>", tableCaption);
        sbHTable.Append("<tbody>");
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
                if (stripQuotes)
                {
                    token = token.Replace("\"", "");
                }
                sbHTable.AppendFormat("<td title=\"{0}\" data-tooltip=\"{0}\">{0}</td>", string.IsNullOrWhiteSpace(token) ? "&mdash;" : token);
            }
            sbHTable.Append("</tr>");
        }
        sbHTable.Append("</tbody></table>");
        //return ReplaceFirst(ReplaceFirst(sbHTable.ToString(), "<td>", "<th>"), "</td>", "</th>");
        return sbHTable.ToString();
    }
}

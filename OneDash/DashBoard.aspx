<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products</title>
    <style>
        @media print
        {
            details:not([open])
            {
                visibility: visible !important;
                display: block;
            }
            .boxDebug
            {
                border: 0 !important;
            }
        }
        body, div, section, article, details, summary
        {
            margin: 3px;
            padding: 3px;
        }
        body
        {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            --transition-time: 1.2s;
            --mild-alternating-color: #f5f5f5;
        }
        .boxDebug
        {
            /*border: 1px solid black;*/
        }
        #CurrentProducts details
        {
            margin-left: 15px;
            padding-left: 15px;
        }
        #CurrentProducts details .releaseDetails
        {
            border: 1px dotted transparent;
            border-radius: 1px;
            transition: border-color var(--transition-time) ease-out,
                border-radius var(--transition-time) ease-out,
                background-color var(--transition-time) ease-out;
        }
        #CurrentProducts details .releaseDetails:hover,
        #CurrentProducts details .releaseDetails:focus,
        #CurrentProducts details .releaseDetails:active
        {
            background-color: var(--mild-alternating-color);
            border-radius: 5px;
            border-color: black;
        }
        #CurrentProducts li.releaseDetails > details > section
        {
            min-width: 60vw;
            width: 65%;
            display: grid;
            grid-gap: 5px;
            grid-template-columns: 10em auto;
        }
        #CurrentProducts details[data-risk-level="Green"]
        {
            background-color: #99ff99;
        }
        #CurrentProducts details[data-risk-level="Yellow"]
        {
            background-color: #ffffa5;
        }
        #CurrentProducts details[data-risk-level="Red"]
        {
            background-color: #ff5050;
        }
    </style>
</head>
<body>
    <a href="#"><h1 class="boxDebug">Products</h1></a>
    <article id="CurrentProducts" class="boxDebug">
        <header class="boxDebug">
            <h2 class="boxDebug">Current Products</h2>
        </header>
        <asp:Repeater ID="rpCurrentProducts" runat="server">
            <ItemTemplate>
                <details id="prod<%# Eval("CodeName") %>" class="boxDebug">
                    <summary class="boxDebug" title="<%# Eval("Description") %>">
                        <%# Eval("DisplayName") %>
                    </summary>
                    <h3 class="boxDebug">Versions:</h3>
                    <ul style="list-style: none;" class="boxDebug">
                        <asp:Repeater ID="rpProductVersions" runat="server" DataSource='<%# Eval("Versions") %>'>
                            <ItemTemplate>
                                <li class="versionDetails boxDebug">
                                    <details class="boxDebug">
                                        <summary class="boxDebug">
                                            <h4 style="display: inline-block;" class="boxDebug"><%# Eval("DisplayVersion") %></h4>
                                        </summary>
                                        <ul style="list-style: none;" class="boxDebug">
                                            <asp:Repeater ID="rpVersionReleases" runat="server" DataSource='<%# Eval("Releases") %>'>
                                                <ItemTemplate>
                                                    <li class="releaseDetails boxDebug">
                                                        <details class="boxDebug" data-risk-level="<%# Eval("Risk").ToString() %>">
                                                            <summary class="boxDebug">
                                                                <%# Eval("ReleaseName") %>&nbsp;&nbsp;[<%# Eval("Risk").ToString() %>]
                                                            </summary>
                                                            <section class="boxDebug">
                                                                <div class="boxDebug">Status:</div>
                                                                <div class="boxDebug">
                                                                    <%# Eval("Stage").ToString() %>
                                                                </div>
                                                                <div class="boxDebug">TargetDate:</div>
                                                                <div class="boxDebug">
                                                                    <%# Eval("TargetDate").ToString() %>
                                                                </div>
                                                            </section>
                                                        </details>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </details>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </details>
            </ItemTemplate>
        </asp:Repeater>
        <footer class="boxDebug"></footer>
    </article>
    <script>
        function onBeforePrint()
        {
            var ndAllDetails = document.getElementsByTagName("details"), len = ndAllDetails.length;
            for (var i = 0; i < len; i++)
            {
                ndAllDetails[i].setAttribute("open", "");
            }
        }
        function onAfterPrint()
        {
            var ndAllDetails = document.getElementsByTagName("details"), len = ndAllDetails.length;
            for (var i = 0; i < len; i++)
            {
                ndAllDetails[i].removeAttribute("open");
            }
            onInit();
        }
        function onInit()
        {
            var ndCurrentProducts = document.getElementById("CurrentProducts").getElementsByTagName("details");
            if (ndCurrentProducts.length > 0)
            {
                var firstProduct = ndCurrentProducts[0];
                firstProduct.setAttribute("open", "");
                var ndReleases = firstProduct.getElementsByTagName("details");
                if (ndReleases.length > 0)
                {
                    var firstRelease = ndReleases[0];
                    firstRelease.setAttribute("open", "");
                }
            }
        }
        window.onload = onInit;
        window.onbeforeprint = onBeforePrint;
        window.onafterprint = onAfterPrint;
    </script>
</body>
</html>

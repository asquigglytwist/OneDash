<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products</title>
    <link rel="stylesheet" href="styles/global.css" />
    <link rel="stylesheet" href="styles/dashboard.css" />
</head>
<body>
    <a href="#"><h1>Products</h1></a>
    <article id="CurrentProducts">
        <header>
            <h2>Current Products</h2>
        </header>
        <asp:Repeater ID="rpCurrentProducts" runat="server">
            <ItemTemplate>
                <details id="prod<%# Eval("CodeName") %>">
                    <summary title="<%# Eval("Description") %>">
                        <div class="inlineBlock">
                            <%# Eval("DisplayName") %> &nbsp;&mdash;&nbsp; <%# Eval("Description") %>
                        </div>
                    </summary>
                    <h3>Versions:</h3>
                    <ul class="noListStyle">
                        <asp:Repeater ID="rpProductVersions" runat="server" DataSource='<%# Eval("Versions") %>'>
                            <ItemTemplate>
                                <li class="versionDetails">
                                    <details>
                                        <summary>
                                            <h4 class="inlineBlock"><%# Eval("DisplayVersion") %></h4>
                                        </summary>
                                        <ul class="noListStyle">
                                            <asp:Repeater ID="rpVersionReleases" runat="server" DataSource='<%# Eval("Releases") %>'>
                                                <ItemTemplate>
                                                    <li class="releaseDetails">
                                                        <details>
                                                            <summary data-risk-level="<%# Eval("Risk").ToString() %>">
                                                                <section>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("ReleaseName") %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("Stage").ToString() %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("TargetDate").ToString() %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("Risk").ToString() %>
                                                                    </div>
                                                                </section>
                                                            </summary>
                                                            <section>
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
        <footer></footer>
    </article>
    <script src="scripts/detailshandler.js" async="async" defer="defer">
    </script>
</body>
</html>

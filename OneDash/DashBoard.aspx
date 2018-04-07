<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DashBoard</title>
    <link rel="stylesheet" href="styles/global.css" />
    <link rel="stylesheet" href="styles/dashboard.css" />
</head>
<body>
    <a href="#"><h1 title="Products">Products</h1></a>
    <button class="ghostButton floatRight" title="Collapse All" onclick="toggleExpansionOfDetails(false);">Collapse All</button>
    <button class="ghostButton floatRight" title="Expand All" onclick="toggleExpansionOfDetails(true);">Expand All</button>
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
                        <div class="editLink"><a href="/Products/<%# Eval("PermaLink") %>" title="Click to Edit the Product details">Edit</a></div>
                    </summary>
                    <h3>Versions:</h3>
                    <ul class="noListStyle">
                        <asp:Repeater ID="rpProductVersions" runat="server" DataSource='<%# Eval("ProductVersions") %>'>
                            <ItemTemplate>
                                <li class="versionDetails">
                                    <details>
                                        <summary>
                                            <h4 class="inlineBlock"><%# Eval("DisplayName") %></h4>
                                            <div class="editLink"><a href="/Products/<%# Eval("PermaLink") %>" title="Click to Edit the Version details">Edit</a></div>
                                        </summary>
                                        <ul class="noListStyle">
                                            <asp:Repeater ID="rpVersionReleases" runat="server" DataSource='<%# Eval("ProductVersionReleases") %>'>
                                                <ItemTemplate>
                                                    <li class="releaseDetails">
                                                        <details>
                                                            <summary data-risk-level="<%# Eval("Risk").ToString() %>">
                                                                <section>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("DisplayName") %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("Stage").ToString() %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("TargetDate") %>
                                                                    </div>
                                                                    <div class="inlineBlock">
                                                                        <%# Eval("Risk").ToString() %>
                                                                    </div>
                                                                </section>
                                                                <div class="editLink"><a href="/Products/<%# Eval("PermaLink") %>" title="Click to Edit the Release details">Edit</a></div>
                                                            </summary>
                                                            <section>
                                                                <%# Eval("BugListAsTable").ToString() %>
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

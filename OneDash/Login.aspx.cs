using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BTNSubmit_Click(object sender, EventArgs e)
    {
        var membershipProvider = new ProductAdminMembershipProvider();
        if (membershipProvider.ValidateUser(TXTLUserName.Text, TXTLPassWord.Text))
        {
            FormsAuthentication.SetAuthCookie(TXTLUserName.Text, false);
            var urlQuery = Request.Url.PathAndQuery;
            var ReturnUrl = "ReturnUrl=";
            var ix = urlQuery.IndexOf(ReturnUrl) + ReturnUrl.Length;
            var redirectUrl = "~/";
            if (ix >= ReturnUrl.Length)
            {
                var expectedUrl = urlQuery.Substring(ix);
                redirectUrl += expectedUrl;
            }
            Response.Redirect(redirectUrl);
        }
        else
        {
            FormsAuthentication.SignOut();
            if (loginAttempts() > 3)
            {
                Response.Redirect("~/");
            }
        }
    }

    // [BIB]:  https://stackoverflow.com/questions/5844478/lock-out-users-after-three-unsuccessful-attempts-without-using-a-database
    public int loginAttempts()
    {
        if (!(Session["loginAttempts"] == null))
        {
            Session["loginAttempts"] = int.Parse(Session["loginAttempts"].ToString()) + 1;
            return int.Parse(Session["loginAttempts"].ToString());
        }
        else
        {
            Session["loginAttempts"] = 1;
            return 1;
        }
    }
}
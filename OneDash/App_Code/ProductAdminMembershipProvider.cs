using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// A custom MembershipProvider for Products Administration.
/// </summary>
// [BIB]:  https://docs.microsoft.com/en-us/aspnet/web-forms/overview/moving-to-aspnet-20/membership#implementing-membership-in-your-application
public class ProductAdminMembershipProvider : MembershipProvider
{
    public ProductAdminMembershipProvider()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public override bool EnablePasswordRetrieval => throw new NotImplementedException();

    public override bool EnablePasswordReset => throw new NotImplementedException();

    public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

    public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override int MaxInvalidPasswordAttempts
    {
        get
        {
            return 3;
        }
    }

    public override int PasswordAttemptWindow => throw new NotImplementedException();

    public override bool RequiresUniqueEmail => throw new NotImplementedException();

    public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

    public override int MinRequiredPasswordLength => throw new NotImplementedException();

    public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

    public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        throw new NotImplementedException();
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override int GetNumberOfUsersOnline()
    {
        throw new NotImplementedException();
    }

    public override string GetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        throw new NotImplementedException();
    }

    public override string GetUserNameByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public override string ResetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override bool UnlockUser(string userName)
    {
        throw new NotImplementedException();
    }

    public override void UpdateUser(MembershipUser user)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateUser(string username, string password)
    {
        if(username.Equals("admin") && password.Equals("admin"))
        {
            return true;
        }
        return false;
    }
}
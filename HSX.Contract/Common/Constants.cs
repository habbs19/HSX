namespace HSX.Contract.Common;

public static class Constants
{
    public const string AccessToken = nameof(AccessToken);
    public const string Auth = nameof(Auth);
    public const string MSSQLConnectionStringName = "HSX_CONNECTION_STRING";
    public const string MySQLConnectionStringName = "HSX_CONNECTION_STRING";
    public const string Success = nameof(Success);
    public const string Failed = nameof(Failed);
    public const string NotFound = nameof(NotFound);
    public const string PaginationHeader = "X-Pagination";

    public class CorsPolicyNames
    {
        public const string HSXPolicy = nameof(HSXPolicy);
    }

    public class Roles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
    }

}

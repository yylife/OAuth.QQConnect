namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    public static class QQConsts
    {
        public const string AuthenticationScheme = "QQ";

        public const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        public const string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        public const string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        public const string UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";
    }
}
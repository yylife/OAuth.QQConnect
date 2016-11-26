using Microsoft.AspNetCore.Authentication.QQConnect;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public class QQConnectOptions : OAuthOptions
    {
        public QQConnectOptions()
        {
            AuthenticationScheme = QQConsts.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            CallbackPath = "/signin-qq"; // implicit
            AuthorizationEndpoint = QQConsts.AuthorizationEndpoint;
            TokenEndpoint = QQConsts.TokenEndpoint;
            UserInformationEndpoint = QQConsts.UserInformationEndpoint;
            OpenIdEndpoint = QQConsts.OpenIdEndpoint;
        }

        public string OpenIdEndpoint { get; }

        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        public string AppKey
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        public bool IsMobile { get; set; }
    }
}
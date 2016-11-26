using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    internal class QQConnectHandler : OAuthHandler<QQConnectOptions>
    {
        public QQConnectHandler(HttpClient backchannel) : base(backchannel)
        {
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var url = base.BuildChallengeUrl(properties, redirectUri);
            if (Options.IsMobile) url += "&display=mobile";
            return url;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var query = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret},
                { "code", code},
                { "grant_type","authorization_code"}
            });
            var message = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint)
            {
                Content = query
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await Backchannel.SendAsync(message, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            result = "{\"" + result.Replace("=", "\":\"").Replace("&", "\",\"") + "\"}";
            return OAuthTokenResponse.Success(JObject.Parse(result));
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var openIdEndpoint = Options.OpenIdEndpoint + "?access_token=" + tokens.AccessToken;
            var response = await Backchannel.GetAsync(openIdEndpoint, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var tmp = await response.Content.ReadAsStringAsync();
            var regex = new System.Text.RegularExpressions.Regex("callback\\((?<json>[ -~]+)\\);");
            var json = JObject.Parse(regex.Match(tmp).Groups["json"].Value);
            var identifier = QQHelper.GetId(json);
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:qq:id", identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"oauth_consumer_key", Options.ClientId},
                {"access_token", tokens.AccessToken},
                {"openid", identifier}
            });
            response = await Backchannel.PostAsync(Options.UserInformationEndpoint, content);
            response.EnsureSuccessStatusCode();
            var info = JObject.Parse(await response.Content.ReadAsStringAsync());
            info.Add("id", identifier);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);

            var notification = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, info);

            var name = QQHelper.GetName(info);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:qq:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
            var figure = QQHelper.GetFigure(info);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim("urn:qq:figure", figure, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            await Options.Events.CreatingTicket(notification);
            return notification.Ticket;
        }
    }
}
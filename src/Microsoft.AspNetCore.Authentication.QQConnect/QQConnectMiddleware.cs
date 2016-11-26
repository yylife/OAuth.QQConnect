using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    public class QQConnectMiddleware : OAuthMiddleware<QQConnectOptions>
    {
        public QQConnectMiddleware(RequestDelegate next, IDataProtectionProvider dataProtectionProvider,
             ILoggerFactory loggerFactory, UrlEncoder encoder, IOptions<SharedAuthenticationOptions> sharedOptions,
             IOptions<QQConnectOptions> options)
             : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(Options.AppId))
            {
                throw new ArgumentException($"参数 {nameof(Options.AppId)} 值非法");
            }
            if (string.IsNullOrWhiteSpace(Options.AppKey))
            {
                throw new ArgumentException($"参数 {nameof(Options.AppKey)} 值非法");
            }
            if (Options.Scope.Count == 0)
            {
                Options.Scope.Add("get_user_info");
            }
        }

        protected override AuthenticationHandler<QQConnectOptions> CreateHandler()
        {
            return new QQConnectHandler(Backchannel);
        }
    }
}
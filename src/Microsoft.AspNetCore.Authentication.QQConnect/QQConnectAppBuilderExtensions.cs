using System;
using Microsoft.AspNetCore.Authentication.QQConnect;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add QQConnect authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class QQConnectAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="T:Microsoft.AspNetCore.Authentication.QQConnect.QQConnectMiddleware" /> middleware to the specified <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />, which enables QQConnect authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseQQConnectAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<QQConnectMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="T:Microsoft.AspNetCore.Authentication.QQConnect.QQConnectMiddleware" /> middleware to the specified <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />, which enables QQConnect authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="options">A <see cref="T:Microsoft.AspNetCore.Builder.QQConnectOptions" /> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseQQConnectAuthentication(this IApplicationBuilder app,
            QQConnectOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            return
                app.UseMiddleware<QQConnectMiddleware>(
                    (object) Microsoft.Extensions.Options.Options.Create(options));
        }
    }
}
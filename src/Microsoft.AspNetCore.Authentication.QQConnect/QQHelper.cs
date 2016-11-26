using System;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    internal static class QQHelper
    {
        internal static string GetName(JObject info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            return info.Value<string>("nickname");
        }

        internal static string GetFigure(JObject info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            return info.Value<string>("figureurl_qq_1");
        }

        internal static string GetId(JObject json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return json.Value<string>("openid");
        }
    }
}
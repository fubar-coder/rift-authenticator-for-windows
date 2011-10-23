using System;
using Microsoft.Phone.Info;
using Microsoft.Phone.Controls;
using System.Windows.Controls;

namespace RiftAuthenticator.Library.Platform.WP7
{
    public class Platform : IPlatform
    {
        public Platform(string userAgent)
        {
            DeviceId = GetDeviceUniqueId();
            UserAgent = userAgent;
            SecretKeyEncryption = new Library.PlatformUtils.WindowsPhone.WindowsPhoneSecretKeyEncryption();
        }

        public static string GetDeviceUniqueId()
        {
            byte[] result = null;
            object uniqueId;
            if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            {
                result = (byte[])uniqueId;
            }
            if (result == null)
                return null;
            return Util.BytesToHex(result);
        }

        public static string GetUserAgent(Grid contentGrid)
        {
            var wb = new WebBrowser()
            {
                IsScriptEnabled = true,
                Visibility = System.Windows.Visibility.Collapsed,
            };
            contentGrid.Children.Add(wb);
            var userAgentSetEvent = new System.Threading.AutoResetEvent(false);
            var userAgent = new System.Text.StringBuilder();
            wb.ScriptNotify += (sender, e) =>
            {
                contentGrid.Children.Remove(wb);
                userAgent.Append(e.Value);
                userAgentSetEvent.Set();
            };
            var htmlCode =
@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">
<html>
    <head>
        <script language=""JavaScript"" type=""text/JavaScript"">
            function printUserAgent() {
                window.external.notify(navigator.userAgent);
            }
        </script>
    </head>
    <body onload=""printUserAgent();"">
    </body>
</html>";
            wb.NavigateToString(htmlCode);
            if (!userAgentSetEvent.WaitOne())
                return null;
            return userAgent.ToString();
        }

        public string DeviceId { get; private set; }

        public string UserAgent { get; private set; }

        public ISecretKeyEncryption SecretKeyEncryption { get; private set; }
    }
}

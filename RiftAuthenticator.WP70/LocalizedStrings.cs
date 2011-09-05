using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RiftAuthenticator.WP7
{
    public class LocalizedStrings
    {
        private static WP7.Resources.AppResource _appRes = new Resources.AppResource();

        public LocalizedStrings()
        {
        }

        public WP7.Resources.AppResource Strings
        {
            get
            {
                return _appRes;
            }
        }
    }
}

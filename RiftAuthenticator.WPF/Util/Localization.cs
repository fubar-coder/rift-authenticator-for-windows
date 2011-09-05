using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace RiftAuthenticator.Util
{
    public class Localization : IValueConverter
    {
        /// <summary>
        /// The Resource Manager loads the resources out of the executing assembly (and the XAP File where there are embedded)
        /// </summary>
        private static readonly ResourceManager resourceManager =
            new ResourceManager("RiftAuthenticator.Resources.Strings", Assembly.GetExecutingAssembly());

        /// <summary>
        /// Use this property to specify the culture
        /// </summary>
        private static CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
        public static CultureInfo UiCulture
        {
            get { return uiCulture; }
            set { uiCulture = value; }
        }

        /// <summary>
        /// This method returns the localized string of the given resource.
        /// </summary>
        public string Get(string resource)
        {
            return resourceManager.GetString(resource, UiCulture);
        }

        #region IValueConverter Members

        /// <summary>
        /// This method is used to bind the silverlight component to the resource.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reader = this; // (Localization)value;
            return reader.Get((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack is not used, because the Localization is only a One Way binding
            throw new NotImplementedException();
        }

        #endregion
    }
}

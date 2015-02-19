using Windows.Storage;

namespace KioskApp
{
    public static class AppSettings
    {
        private const string apiServiceUriConfigKey = "ApiURI";
        private const string apiUserNameConfigKey = "ApiUserName";
        private const string apiUserPassConfigKey = "ApiUserPass";
        private const string loginUrlConfigKey = "LoginURL";
        private const string deviceIdConfigKey = "DeviceId";
        private const string isRegisteredConfigKey = "false";

        public static string ApiUri
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[apiServiceUriConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[apiServiceUriConfigKey] = value;
            }
        }

        public static string ApiUsername
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[apiUserNameConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[apiUserNameConfigKey] = value;
            }
        }

        public static string ApiUserPass
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[apiUserPassConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[apiUserPassConfigKey] = value;
            }
        }

        public static string LoginUrl
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[loginUrlConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[loginUrlConfigKey] = value;
            }
        }

        public static string DeviceId
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[deviceIdConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[deviceIdConfigKey] = value;
            }
        }

        public static string IsRegistered
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[isRegisteredConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[isRegisteredConfigKey] = value;
            }
        }
    }
}


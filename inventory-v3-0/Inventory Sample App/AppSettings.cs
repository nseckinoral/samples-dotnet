﻿using Windows.Storage;

namespace Inventory_Sample_App
{
    public static class AppSettings
    {
        private const string apiServiceUriConfigKey = "ApiURI";
        private const string apiUserNameConfigKey = "ApiUserName";
        private const string apiUserPassConfigKey = "ApiUserPass";
        private const string itemIdConfigKey = "ItemId";

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

        public static string ItemId
        {
            get
            {
                string retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[itemIdConfigKey];
                if (value != null)
                {
                    retVal = value.ToString();
                }
                return retVal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[itemIdConfigKey] = value;
            }
        }

    }
}
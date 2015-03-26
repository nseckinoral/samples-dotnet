using Windows.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_Sample_App
{
    public static class AppSettings
    {
        private const string apiServiceUriConfigKey = "ApiURI";
        private const string apiUserNameConfigKey = "ApiUserName";
        private const string apiUserPassConfigKey = "ApiUserPass";
        private const string itemIdsConfigKey = "1,2";

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

        public static List<int> ItemIds
        {
            get
            {
                List<int> retVal = null;
                object value = ApplicationData.Current.LocalSettings.Values[itemIdsConfigKey];
                if (value != null)
                {
                    var stringItemIds = value.ToString().Split(',');
                    var itemIds = stringItemIds.Select(int.Parse).ToList();
                    retVal = itemIds;
                }
                return retVal;
            }
            set
            {
                var stringItemIds = string.Format("{0},{1}", value[0], value[1]);
                ApplicationData.Current.LocalSettings.Values[itemIdsConfigKey] = stringItemIds;
            }
        }

    }
}

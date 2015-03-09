using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;
using XOMNI.SDK.Public.Models;

namespace Inventory_Sample_App.Converters
{
    class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //Real Location
            //Geolocator geolocator = new Geolocator();
            //Geoposition position = geolocator.GetGeopositionAsync().AsTask().Result;
            //var currentLocation = position.Coordinate.Point.Position;

            //Fake Location
            Bing.Maps.Location currentLocation = new Bing.Maps.Location(40.801112, -75.952134);
            var storeLocation = (Location)value;

            var storeLong = storeLocation.Longitude;
            var storeLat = storeLocation.Latitude;
            var currentLong = currentLocation.Longitude;
            var currentLat = currentLocation.Latitude;

            const double degreesToRadians = (Math.PI / 180.0);
            const double earthRadius = 6371; // kilometers

            // convert latitude and longitude values to radians
            var currentRadLat = currentLat * degreesToRadians;
            var currentRadLong = currentLong * degreesToRadians;
            var storeRadLat = storeLat * degreesToRadians;
            var storeRadLong = storeLong * degreesToRadians;

            // calculate radian delta between each position.
            var radDeltaLat = storeRadLat - currentRadLat;
            var radDeltaLong = storeRadLong - currentRadLong;

            // calculate distance
            var expr1 = (Math.Sin(radDeltaLat / 2.0) *
                         Math.Sin(radDeltaLat / 2.0)) +

                        (Math.Cos(currentRadLat) *
                         Math.Cos(storeRadLat) *
                         Math.Sin(radDeltaLong / 2.0) *
                         Math.Sin(radDeltaLong / 2.0));

            var expr2 = 2.0 * Math.Atan2(Math.Sqrt(expr1),Math.Sqrt(1 - expr1));
            var distance = (earthRadius * expr2);
            if(distance < 1)
            {
                distance = distance * 1000;  // return results as meters
                distance = Math.Truncate(distance);
                return string.Format("{0}m", distance);
            }
            else
            {
                distance = Math.Truncate(distance);
                return string.Format("{0}km", distance);
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

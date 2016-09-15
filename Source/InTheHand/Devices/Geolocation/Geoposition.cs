// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geoposition.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geoposition))]
#else

#if __IOS__
using CoreLocation;
#endif

namespace Windows.Devices.Geolocation
{
    public sealed class Geoposition
    {
#if __IOS__
        // constructor from CoreLocation location
        internal Geoposition(CLLocation location)
        {
            Coordinate = new Geocoordinate();
            if (location != null)
            {
                Coordinate.Point = new Geopoint(new BasicGeoposition() { Latitude = location.Coordinate.Latitude, Longitude = location.Coordinate.Longitude, Altitude = location.Altitude });

                Coordinate.Accuracy = location.HorizontalAccuracy;

                if (!double.IsNaN(location.VerticalAccuracy))
                {
                    Coordinate.AltitudeAccuracy = location.VerticalAccuracy;
                }

                if (!double.IsNaN(location.Course) && location.Course != -1)
                {
                    Coordinate.Heading = location.Course;
                }

                if (!double.IsNaN(location.Speed) && location.Speed != -1)
                {
                    Coordinate.Speed = location.Speed;
                }

                Coordinate.Timestamp = InTheHand.DateTimeOffsetHelper.FromNSDate(location.Timestamp);
            }
        }
#endif

        public Geocoordinate Coordinate
        {
            get;
            internal set;
        }
    }
}
#endif
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geocoordinate.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Contains the information for identifying a geographic location.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed class Geocoordinate
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geocoordinate _coordinate;

        private Geocoordinate(Windows.Devices.Geolocation.Geocoordinate coordinate)
        {
            _coordinate = coordinate;
        }

        public static implicit operator Windows.Devices.Geolocation.Geocoordinate(Geocoordinate gc)
        {
            return gc._coordinate;
        }

        public static implicit operator Geocoordinate(Windows.Devices.Geolocation.Geocoordinate gc)
        {
            return new Geolocation.Geocoordinate(gc);
        }
#elif WIN32
        private global::System.Device.Location.GeoCoordinate _coordinate;

        /*internal Geocoordinate(DateTimeOffset timestamp, global::System.Device.Location.GeoCoordinate coordinate)
        {
            Timestamp = timestamp;
            _coordinate = coordinate;
        }*/

        private Geocoordinate(global::System.Device.Location.GeoCoordinate coordinate)
        {
            _coordinate = coordinate;
        }

        public static implicit operator global::System.Device.Location.GeoCoordinate(Geocoordinate gc)
        {
            return gc._coordinate;
        }

        public static implicit operator Geocoordinate(global::System.Device.Location.GeoCoordinate gc)
        {
            return new Geocoordinate(gc);
        }
#endif
        /// <summary>
        /// The accuracy of the location in meters.
        /// </summary>
        /// <value>The accuracy in meters.</value>
        /// <remarks>The Windows Location Provider and the Windows Phone Location Services accuracy depends on the location data available.
        /// For example,iIf Wifi is available, data is accurate to within 50 meters.
        /// If Wifi is not available, the data could be accurate to within 10 miles or larger. 
        /// A GNSS device can provide data accurate to within a few meters.
        /// However, its accuracy can vary if the GNSS sensor is obscured by buildings, trees, or cloud cover.
        /// GNSS data may not be available at all within a building.</remarks>
        public double Accuracy
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _coordinate.Accuracy;
            }
#elif WIN32
            get
            {
                return _coordinate.HorizontalAccuracy;
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// The accuracy of the altitude, in meters.
        /// </summary>
        /// <value>The accuracy of the altitude.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL. 
        /// The Windows Location Provider and the Windows Phone Location Services do not set this property.</remarks>
        public double? AltitudeAccuracy
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _coordinate.AltitudeAccuracy;
            }
#elif WIN32
            get
            {
                if (!double.IsNaN(_coordinate.VerticalAccuracy))
                {
                    return _coordinate.VerticalAccuracy;
                }

                return null;
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// The current heading in degrees relative to true north.
        /// </summary>
        /// <value>The current heading in degrees relative to true north.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL.
        /// The Windows Location Provider does not set this property.</remarks>
        public double? Heading
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _coordinate.Heading;
            }
#elif WIN32
            get
            {
                return _coordinate.Course;
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// The location of the Geocoordinate.
        /// </summary>
        /// <value>The location of the Geocoordinate.</value>
        public Geopoint Point
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            get
            {
                return _coordinate.Point;
            }
#elif WINDOWS_PHONE
            get
            {
                return new Geopoint(new BasicGeoposition() { Altitude = _coordinate.Altitude.HasValue ? _coordinate.Altitude.Value : double.NaN, Latitude = _coordinate.Latitude, Longitude = _coordinate.Longitude });
            }
#elif WIN32
            get
            {
                return new Geopoint(new BasicGeoposition() { Altitude = _coordinate.Altitude, Latitude = _coordinate.Latitude, Longitude = _coordinate.Longitude });
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// Gets the source used to obtain a Geocoordinate.
        /// </summary>
        /// <value>The source used to obtain a Geocoordinate.</value>
        public PositionSource PositionSource
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return (PositionSource)((int)_coordinate.PositionSource);
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// Gets the time at which the associated Geocoordinate position was calculated.
        /// </summary>
        /// <value>The time at which the associated Geocoordinate position was calculated.</value>
        /// <remarks>When this property is not available, the value will be null.
        /// <para>The timestamp returned by this property depends on how the location was obtained and may be completely unrelated to the system time on the device.
        /// For example, if the position is obtained from the Global Navigation Satellite System (GNSS) the timestamp would be obtained from the satellites.
        /// If the position was is obtained from Secure User Plane Location (SUPL), the timestamp would be obtained from SUPL servers.
        /// This means that the timestamps obtained from these services will be precise and, most importantly, consistent across all devices regardless of whether the system time on the devices is set correctly.</para></remarks>
        public DateTimeOffset? PositionSourceTimestamp
        {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            get
            {
                return _coordinate.PositionSourceTimestamp;
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// The speed in meters per second.
        /// </summary>
        /// <value>The speed in meters per second.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL.
        /// The Windows Location Provider does not set this property.</remarks>
        public double? Speed
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _coordinate.Speed;
            }
#elif WIN32
            get
            {
                if (!double.IsNaN(_coordinate.Speed))
                {
                    return _coordinate.Speed;
                }

                return null;
            }
#else
            get;
            internal set;
#endif
        }

        /// <summary>
        /// The system time at which the location was determined.
        /// </summary>
        /// <value>The system time at which the location was determined.</value>
        public DateTimeOffset Timestamp
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _coordinate.Timestamp;
            }
#else
            get;
            internal set;
#endif
        }
    }
}
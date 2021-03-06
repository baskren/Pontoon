﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geopoint.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Describes a geographic point.
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
    public sealed class Geopoint
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Geolocation.Geopoint _point;

        private Geopoint(Windows.Devices.Geolocation.Geopoint point)
        {
            _point = point;
        }

        public static implicit operator Windows.Devices.Geolocation.Geopoint(Geopoint gp)
        {
            return gp._point;
        }

        public static implicit operator Geopoint(Windows.Devices.Geolocation.Geopoint gp)
        {
            return new Geopoint(gp);
        }
#endif
        /// <summary>
        /// Create a geographic point object for the given position.
        /// </summary>
        /// <param name="position">Create a geographic point object for the given position.</param>
        public Geopoint(BasicGeoposition position)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _point = new Windows.Devices.Geolocation.Geopoint(position);
#else
            Position = position;
#endif
        }

        /// <summary>
        /// The position of a geographic point.
        /// </summary>
        public BasicGeoposition Position
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            get
            {
                return _point.Position;
            }
#else

            get;
            private set;
#endif
        }
    }
}
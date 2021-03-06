﻿//-----------------------------------------------------------------------
// <copyright file="ScheduledToastNotification.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if __MAC__
using Foundation;
#endif

namespace InTheHand.UI.Notifications
{
    /// <summary>
    /// Contains the definition of the toast notification that will display at the scheduled time.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    /// <seealso cref="ToastNotificationCreator"/>
    public sealed partial class ScheduledToastNotification
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        internal Windows.UI.Notifications.ScheduledToastNotification _notification;

        internal ScheduledToastNotification(Windows.UI.Notifications.ScheduledToastNotification notification)
        {
            _notification = notification;
        }

        public static implicit operator Windows.UI.Notifications.ScheduledToastNotification(ScheduledToastNotification n)
        {
            return n._notification;
        }
#elif __MAC__
        private NSUserNotification _notification;

        private ScheduledToastNotification(NSUserNotification notification)
        {
            _notification = notification;
        }

        public static implicit operator NSUserNotification(ScheduledToastNotification tn)
        {
            return tn._notification;
        }

        public static implicit operator ScheduledToastNotification(NSUserNotification un)
        {
            return new ScheduledToastNotification(un);
        }

#elif __UNIFIED__
#else


        internal ScheduledToastNotification(string content, string title, DateTimeOffset deliveryTime)
        {
            Title = title;
            Content = content;
            DeliveryTime = deliveryTime;
        }

        internal string Title
        {
            get;
            private set;
        }

        internal string Content
        {
            get;
            private set;
        }


#endif

        /// <summary>
        /// Gets the time that this toast notification is scheduled to be displayed.
        /// </summary>
        public DateTimeOffset DeliveryTime
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            get
            {
                return _notification.DeliveryTime;
            }
#elif __MAC__
            get
            {
                return InTheHand.DateTimeOffsetHelper.FromNSDate(_notification.DeliveryDate);
            }
#elif __UNIFIED__
            get
            {
                return GetDeliveryTime();
            }
#else
            get;
            private set;
#endif
        }

        /// <summary>
        /// Gets or sets the group identifier for the notification.
        /// </summary>
        public string Group
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Group;

#elif __MAC__
                return string.Empty;

#elif __UNIFIED__
                return GetGroup();

#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Group = value;

#elif __IOS__
                SetGroup(value);
#endif
            }
        }

        /// <summary>
        /// Gets or sets whether a toast's pop-up UI is displayed on the user's screen.
        /// </summary>
        public bool SuppressPopup
        {

            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.SuppressPopup;
#elif __MAC__
                return false;

#elif __UNIFIED__
                return GetSuppressPopup();

#else
                return false;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.SuppressPopup = value;

#elif __MAC__

#elif __UNIFIED__
                SetSuppressPopup(value);
#endif
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of this notification within the notification Group.
        /// </summary>
        public string Tag
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _notification.Tag;

#elif __MAC__
                return _notification.Identifier;

#elif __UNIFIED__
                return GetTag();

#else
                return string.Empty;
#endif
            }
            set
            {
#if WINDOWS_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _notification.Tag = value;

#elif __MAC__
                _notification.Identifier = value;

#elif __UNIFIED__
                SetTag(value);
#endif
            }
        }
    }
}
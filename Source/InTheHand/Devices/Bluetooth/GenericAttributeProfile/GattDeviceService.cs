﻿//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InTheHand.Devices.Enumeration;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class GattDeviceService : IDisposable
    {
        private static readonly Guid BluetoothBase = new Guid(0x00000000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Converts a Bluetooth SIG defined short Id to a full GATT UUID.
        /// </summary>
        /// <param name="shortId">A 16-bit Bluetooth GATT Service UUID.</param>
        /// <returns>The corresponding 128-bit GATT Service UUID, that uniquely identifies this service.</returns>
        public static Guid ConvertShortIdToUuid(ushort shortId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.ConvertShortIdToUuid(shortId);

#else
            byte[] guidBytes = BluetoothBase.ToByteArray();
            BitConverter.GetBytes(shortId).CopyTo(guidBytes, 0);
            return new Guid(guidBytes);
#endif
        }

        public static Task<GattDeviceService> FromIdAsync(string deviceId)
        {
            return FromIdAsyncImpl(deviceId);
        }

        public static string GetDeviceSelectorFromShortId(ushort serviceShortId)
        {
            return GetDeviceSelectorFromShortIdImpl(serviceShortId);
        }

        public static string GetDeviceSelectorFromUuid(Guid serviceUuid)
        {
            return GetDeviceSelectorFromUuidImpl(serviceUuid);
        }


        public IReadOnlyList<GattCharacteristic> GetAllCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            GetAllCharacteristics(characteristics);

            return characteristics.AsReadOnly();
        }

        public IReadOnlyList<GattCharacteristic> GetCharacteristics(Guid characteristicUuid)
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            GetCharacteristics(characteristicUuid, characteristics);

            return characteristics.AsReadOnly();
        }

        public void Dispose()
        {
            DoDispose();
        }

        /*public Task<DeviceAccessStatus> RequestAccessAsync()
        {
            return DoRequestAccessAsync();
        }*/

        /// <summary>
        /// Gets the <see cref="BluetoothLEDevice"/> object describing the device associated with the current <see cref="GattDeviceService"/> object.
        /// </summary>
        public BluetoothLEDevice Device
        {
            get
            {
                return GetDevice();
            }
        }

        /// <summary>
        /// The GATT Service UUID associated with this GattDeviceService.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
            }
        }
    }
}
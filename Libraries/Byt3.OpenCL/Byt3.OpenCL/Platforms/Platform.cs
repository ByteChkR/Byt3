#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenCL.Devices;
using Byt3.OpenCL.Interop;
using Byt3.OpenCL.Interop.Devices;
using Byt3.OpenCL.Interop.Platforms;

#endregion

namespace Byt3.OpenCL.Platforms
{
    /// <summary>
    /// Represents an OpenCL platform.
    /// </summary>
    public class Platform : HandleBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="Platform"/> instance.
        /// </summary>
        /// <param name="handle">The handle to the OpenCL platform.</param>
        internal Platform(IntPtr handle)
            : base(handle, "Platform", false)
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves the specified information about the OpenCL platform.
        /// </summary>
        /// <typeparam name="T">The type of the data that is to be returned.</param>
        /// <param name="platformInformation">The kind of information that is to be retrieved.</param>
        /// <exception cref="OpenClException">If the information could not be retrieved, then an <see cref="OpenClException"/> is thrown.</exception>
        /// <returns>Returns the specified information.</returns>
        private T GetPlatformInformation<T>(PlatformInformation platformInformation)
        {
            // Retrieves the size of the return value in bytes, this is used to later get the full information
            UIntPtr returnValueSize;
            Result result = PlatformsNativeApi.GetPlatformInformation(Handle, platformInformation, UIntPtr.Zero, null,
                out returnValueSize);
            if (result != Result.Success)
            {
                throw new OpenClException("The platform information could not be retrieved.", result);
            }

            // Allocates enough memory for the return value and retrieves it
            byte[] output = new byte[returnValueSize.ToUInt32()];
            result = PlatformsNativeApi.GetPlatformInformation(Handle, platformInformation,
                new UIntPtr((uint) output.Length), output, out returnValueSize);
            if (result != Result.Success)
            {
                throw new OpenClException("The platform information could not be retrieved.", result);
            }

            // Returns the output
            return InteropConverter.To<T>(output);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all devices of the platform that match the specified device type.
        /// </summary>
        /// <param name="deviceType">The type of devices that is to be retrieved.</param>
        /// <exception cref="OpenClException">If the devices could not be retrieved, then a <see cref="OpenClException"/> is thrown.</exception>
        /// <returns>Returns a list of all devices that matched the specified device type.</returns>
        public IEnumerable<Device> GetDevices(Devices.DeviceType deviceType)
        {
            // Gets the number of available devices of the specified type
            uint numberOfAvailableDevices;
            Result result = DevicesNativeApi.GetDeviceIds(Handle, (Interop.Devices.DeviceType) deviceType, 0, null,
                out numberOfAvailableDevices);
            if (result != Result.Success)
            {
                throw new OpenClException("The number of available devices could not be queried.", result);
            }

            // Gets the pointers to the devices of the specified type
            IntPtr[] devicePointers = new IntPtr[numberOfAvailableDevices];
            result = DevicesNativeApi.GetDeviceIds(Handle, (Interop.Devices.DeviceType) deviceType,
                numberOfAvailableDevices, devicePointers, out numberOfAvailableDevices);
            if (result != Result.Success)
            {
                throw new OpenClException("The devices could not be retrieved.", result);
            }

            // Converts the pointer to device objects
            foreach (IntPtr devicePointer in devicePointers)
            {
                yield return new Device(devicePointer);
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets all the available platforms.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the platforms could not be queried, then an <see cref="InvalidOperationException"/> is thrown.</exception>
        /// <returns>Returns a list with all the availabe platforms.</returns>
        public static IEnumerable<Platform> GetPlatforms()
        {
            // Gets the number of available platforms
            uint numberOfAvailablePlatforms;
            Result result = PlatformsNativeApi.GetPlatformIds(0, null, out numberOfAvailablePlatforms);
            if (result != Result.Success)
            {
                throw new OpenClException("The number of platforms could not be queried.", result);
            }

            // Gets pointers to all the platforms
            IntPtr[] platformPointers = new IntPtr[numberOfAvailablePlatforms];
            result = PlatformsNativeApi.GetPlatformIds(numberOfAvailablePlatforms, platformPointers,
                out numberOfAvailablePlatforms);
            if (result != Result.Success)
            {
                throw new OpenClException("The platforms could not be retrieved.", result);
            }

            // Converts the pointers to platform objects
            foreach (IntPtr platformPointer in platformPointers)
            {
                yield return new Platform(platformPointer);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains the name of the OpenCL platform.
        /// </summary>
        private string name;

        /// <summary>
        /// Gets the name of the OpenCL platform.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = GetPlatformInformation<string>(PlatformInformation.Name);
                }

                return name;
            }
        }

        /// <summary>
        /// Contains the name of the vendor of the OpenCL platform.
        /// </summary>
        private string vendor;

        /// <summary>
        /// Gets the name of the vendor of the OpenCL platform.
        /// </summary>
        public string Vendor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(vendor))
                {
                    vendor = GetPlatformInformation<string>(PlatformInformation.Vendor);
                }

                return vendor;
            }
        }

        /// <summary>
        /// Contains the version of the OpenCL platform.
        /// </summary>
        private Version version;

        /// <summary>
        /// Gets the version of the OpenCL platform.
        /// </summary>
        public Version Version
        {
            get
            {
                if (version == null)
                {
                    version = new Version(GetPlatformInformation<string>(PlatformInformation.Version));
                }

                return version;
            }
        }

        /// <summary>
        /// Contains the profile supported by the OpenCL platform.
        /// </summary>
        private Profile? profile;

        /// <summary>
        /// Gets the profile supported by the OpenCL platform.
        /// </summary>
        public Profile Profile
        {
            get
            {
                if (!profile.HasValue)
                {
                    string profileName = GetPlatformInformation<string>(PlatformInformation.Profile);
                    if (profileName == "FULL_PROFILE")
                    {
                        profile = Profile.Full;
                    }
                    else
                    {
                        profile = Profile.Embedded;
                    }
                }

                return profile.Value;
            }
        }

        /// <summary>
        /// Contains the extensions supported by the OpenCL platform.
        /// </summary>
        private IEnumerable<string> extensions;

        /// <summary>
        /// Gets the extensions support by the OpenCL platform.
        /// </summary>
        public IEnumerable<string> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = GetPlatformInformation<string>(PlatformInformation.Extensions).Split(' ').ToList();
                }

                return extensions;
            }
        }

        /// <summary>
        /// Contains the the resolution of the host timer in nanoseconds.
        /// </summary>
        private long? platformHostTimerResolution;

        /// <summary>
        /// Gets the the resolution of the host timer in nanoseconds.
        /// </summary>
        public long PlatformHostTimerResolution
        {
            get
            {
                if (!platformHostTimerResolution.HasValue)
                {
                    platformHostTimerResolution =
                        (long) GetPlatformInformation<ulong>(PlatformInformation.PlatformHostTimerResolution);
                }

                return platformHostTimerResolution.Value;
            }
        }

        /// <summary>
        /// Contains the function name suffix used to identify extension functions to be directed to this platform by the ICD Loader.
        /// </summary>
        private string platformIcdSuffix;

        /// <summary>
        /// Gets the function name suffix used to identify extension functions to be directed to this platform by the ICD Loader.
        /// </summary>
        public string PlatformIcdSuffix
        {
            get
            {
                if (platformIcdSuffix == null)
                {
                    platformIcdSuffix = GetPlatformInformation<string>(PlatformInformation.PlatformIcdSuffix);
                }

                return platformIcdSuffix;
            }
        }

        #endregion
    }
}
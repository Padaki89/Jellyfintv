using System;
using System.Globalization;
using System.IO;
using System.Text;
using MediaBrowser.Common.Configuration;
using Microsoft.Extensions.Logging;

namespace Emby.Server.Implementations.Devices
{
    /// <summary>
    /// A device ID.
    /// </summary>
    public class DeviceId
    {
        private readonly IApplicationPaths _appPaths;
        private readonly ILogger _logger;

        private readonly object _syncLock = new object();

        private string CachePath => Path.Combine(_appPaths.DataPath, "device.txt");

        private string GetCachedId()
        {
            try
            {
                lock (_syncLock)
                {
                    var value = File.ReadAllText(CachePath, Encoding.UTF8);

                    if (Guid.TryParse(value, out var guid))
                    {
                        return value;
                    }

                    _logger.LogError("Invalid value found in device id file");
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading file");
            }

            return null;
        }

        private void SaveId(string id)
        {
            try
            {
                var path = CachePath;

                Directory.CreateDirectory(Path.GetDirectoryName(path));

                lock (_syncLock)
                {
                    File.WriteAllText(path, id, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing to file");
            }
        }

        private static string GetNewId()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }

        private string GetDeviceId()
        {
            var id = GetCachedId();

            if (string.IsNullOrWhiteSpace(id))
            {
                id = GetNewId();
                SaveId(id);
            }

            return id;
        }

        private string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceId"/> class.
        /// </summary>
        /// <param name="appPaths">The application paths.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DeviceId(IApplicationPaths appPaths, ILoggerFactory loggerFactory)
        {
            _appPaths = appPaths;
            _logger = loggerFactory.CreateLogger("SystemId");
        }

        /// <summary>
        /// Gets the value of the device ID.
        /// </summary>
        public string Value => _id ??= GetDeviceId();
    }
}

#pragma warning disable CS1591
#nullable enable
using System;
using System.Threading.Tasks;
using Emby.Dlna.Server.Service;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Dlna;
using Microsoft.Extensions.Logging;

namespace Emby.Dlna.Server.ConnectionManager
{
    public class DlnaConnectionManager : BaseService, IConnectionManager
    {
        private readonly IDlnaProfileManager _dlna;
        private readonly IServerConfigurationManager _configurationManager;

        public DlnaConnectionManager(
            ILogger logger,
            IServerConfigurationManager configurationManager,
            IHttpClient httpClient,
            IDlnaProfileManager dlna)
            : base(logger, httpClient)
        {
            _dlna = dlna ?? throw new NullReferenceException(nameof(dlna));
            _configurationManager = configurationManager ?? throw new NullReferenceException(nameof(configurationManager));
        }

        /// <inheritdoc />
        public string GetServiceXml()
        {
            return ConnectionManagerXmlBuilder.GetXml();
        }

        /// <inheritdoc />
        public Task<ControlResponse> ProcessControlRequestAsync(ControlRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var profile = _dlna.GetProfile(request.Headers) ?? _dlna.GetDefaultProfile();

            return new ControlHandler(_configurationManager, Logger, profile).ProcessControlRequestAsync(request);
        }
    }
}

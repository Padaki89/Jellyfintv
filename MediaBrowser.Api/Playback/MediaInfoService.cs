using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Devices;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.MediaEncoding;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Dlna;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Services;
using MediaBrowser.Model.Session;
using Microsoft.Extensions.Logging;

namespace MediaBrowser.Api.Playback
{
    [Route("/Items/{Id}/PlaybackInfo", "GET", Summary = "Gets live playback media info for an item")]
    public class GetPlaybackInfo : IReturn<PlaybackInfoResponse>
    {
        [ApiMember(Name = "Id", Description = "Item Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public Guid Id { get; set; }

        [ApiMember(Name = "UserId", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "query", Verb = "GET")]
        public Guid UserId { get; set; }
    }

    [Route("/Items/{Id}/PlaybackInfo", "POST", Summary = "Gets live playback media info for an item")]
    public class GetPostedPlaybackInfo : PlaybackInfoRequest, IReturn<PlaybackInfoResponse>
    {
    }

    [Route("/LiveStreams/Open", "POST", Summary = "Opens a media source")]
    public class OpenMediaSource : LiveStreamRequest, IReturn<LiveStreamResponse>
    {
    }

    [Route("/LiveStreams/Close", "POST", Summary = "Closes a media source")]
    public class CloseMediaSource : IReturnVoid
    {
        [ApiMember(Name = "LiveStreamId", Description = "LiveStreamId", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "POST")]
        public string LiveStreamId { get; set; }
    }

    [Route("/Playback/BitrateTest", "GET")]
    public class GetBitrateTestBytes
    {
        [ApiMember(Name = "Size", Description = "Size", IsRequired = true, DataType = "int", ParameterType = "query", Verb = "GET")]
        public long Size { get; set; }

        public GetBitrateTestBytes()
        {
            // 100k
            Size = 102400;
        }
    }

    [Authenticated]
    public class MediaInfoService : BaseApiService
    {
        private readonly IMediaSourceManager _mediaSourceManager;
        private readonly IDeviceManager _deviceManager;
        private readonly ILibraryManager _libraryManager;
        private readonly IServerConfigurationManager _config;
        private readonly INetworkManager _networkManager;
        private readonly IMediaEncoder _mediaEncoder;
        private readonly IUserManager _userManager;
        private readonly IJsonSerializer _json;
        private readonly IAuthorizationContext _authContext;

        public MediaInfoService(IMediaSourceManager mediaSourceManager, IDeviceManager deviceManager, ILibraryManager libraryManager, IServerConfigurationManager config, INetworkManager networkManager, IMediaEncoder mediaEncoder, IUserManager userManager, IJsonSerializer json, IAuthorizationContext authContext)
        {
            _mediaSourceManager = mediaSourceManager;
            _deviceManager = deviceManager;
            _libraryManager = libraryManager;
            _config = config;
            _networkManager = networkManager;
            _mediaEncoder = mediaEncoder;
            _userManager = userManager;
            _json = json;
            _authContext = authContext;
        }

        public object Get(GetBitrateTestBytes request)
        {
            var bytes = new byte[request.Size];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }

            return ResultFactory.GetResult(null, bytes, "application/octet-stream");
        }

        public async Task<object> Get(GetPlaybackInfo request)
        {
            var result = await GetPlaybackInfo(request.Id, request.UserId, new[] { MediaType.Audio, MediaType.Video }).ConfigureAwait(false);
            return ToOptimizedResult(result);
        }

        public async Task<object> Post(OpenMediaSource request)
        {
            var result = await OpenMediaSource(request).ConfigureAwait(false);

            return ToOptimizedResult(result);
        }

        private async Task<LiveStreamResponse> OpenMediaSource(OpenMediaSource request)
        {
            var authInfo = _authContext.GetAuthorizationInfo(Request);

            var result = await _mediaSourceManager.OpenLiveStream(request, CancellationToken.None).ConfigureAwait(false);

            var profile = request.DeviceProfile;
            if (profile == null)
            {
                var caps = _deviceManager.GetCapabilities(authInfo.DeviceId);
                if (caps != null)
                {
                    profile = caps.DeviceProfile;
                }
            }

            if (profile != null)
            {
                var item = _libraryManager.GetItemById(request.ItemId);

                SetDeviceSpecificData(item, result.MediaSource, profile, authInfo, request.MaxStreamingBitrate,
                    request.StartTimeTicks ?? 0, result.MediaSource.Id, request.AudioStreamIndex,
                    request.SubtitleStreamIndex, request.MaxAudioChannels, request.PlaySessionId, request.UserId, request.EnableDirectPlay, true, request.EnableDirectStream, true, true, true);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(result.MediaSource.TranscodingUrl))
                {
                    result.MediaSource.TranscodingUrl += "&LiveStreamId=" + result.MediaSource.LiveStreamId;
                }
            }

            if (result.MediaSource != null)
            {
                NormalizeMediaSourceContainer(result.MediaSource, profile, DlnaProfileType.Video);
            }

            return result;
        }

        public void Post(CloseMediaSource request)
        {
            var task = _mediaSourceManager.CloseLiveStream(request.LiveStreamId);
            Task.WaitAll(task);
        }

        public async Task<PlaybackInfoResponse> GetPlaybackInfo(GetPostedPlaybackInfo request)
        {
            var authInfo = _authContext.GetAuthorizationInfo(Request);

            var profile = request.DeviceProfile;

            //Logger.Info("GetPostedPlaybackInfo profile: {0}", _json.SerializeToString(profile));

            if (profile == null)
            {
                var caps = _deviceManager.GetCapabilities(authInfo.DeviceId);
                if (caps != null)
                {
                    profile = caps.DeviceProfile;
                }
            }

            var info = await GetPlaybackInfo(request.Id, request.UserId, new[] { MediaType.Audio, MediaType.Video }, request.MediaSourceId, request.LiveStreamId).ConfigureAwait(false);

            if (profile != null)
            {
                var mediaSourceId = request.MediaSourceId;

                SetDeviceSpecificData(request.Id, info, profile, authInfo, request.MaxStreamingBitrate ?? profile.MaxStreamingBitrate, request.StartTimeTicks ?? 0, mediaSourceId, request.AudioStreamIndex, request.SubtitleStreamIndex, request.MaxAudioChannels, request.UserId, request.EnableDirectPlay, true, request.EnableDirectStream, request.EnableTranscoding, request.AllowVideoStreamCopy, request.AllowAudioStreamCopy);
            }

            if (request.AutoOpenLiveStream)
            {
                var mediaSource = string.IsNullOrWhiteSpace(request.MediaSourceId) ? info.MediaSources.FirstOrDefault() : info.MediaSources.FirstOrDefault(i => string.Equals(i.Id, request.MediaSourceId, StringComparison.Ordinal));

                if (mediaSource != null && mediaSource.RequiresOpening && string.IsNullOrWhiteSpace(mediaSource.LiveStreamId))
                {
                    var openStreamResult = await OpenMediaSource(new OpenMediaSource
                    {
                        AudioStreamIndex = request.AudioStreamIndex,
                        DeviceProfile = request.DeviceProfile,
                        EnableDirectPlay = request.EnableDirectPlay,
                        EnableDirectStream = request.EnableDirectStream,
                        ItemId = request.Id,
                        MaxAudioChannels = request.MaxAudioChannels,
                        MaxStreamingBitrate = request.MaxStreamingBitrate,
                        PlaySessionId = info.PlaySessionId,
                        StartTimeTicks = request.StartTimeTicks,
                        SubtitleStreamIndex = request.SubtitleStreamIndex,
                        UserId = request.UserId,
                        OpenToken = mediaSource.OpenToken,
                        //EnableMediaProbe = request.EnableMediaProbe

                    }).ConfigureAwait(false);

                    info.MediaSources = new MediaSourceInfo[] { openStreamResult.MediaSource };
                }
            }

            if (info.MediaSources != null)
            {
                foreach (var mediaSource in info.MediaSources)
                {
                    NormalizeMediaSourceContainer(mediaSource, profile, DlnaProfileType.Video);
                }
            }

            return info;
        }

        private void NormalizeMediaSourceContainer(MediaSourceInfo mediaSource, DeviceProfile profile, DlnaProfileType type)
        {
            mediaSource.Container = StreamBuilder.NormalizeMediaSourceFormatIntoSingleContainer(mediaSource.Container, mediaSource.Path, profile, type);
        }

        public async Task<object> Post(GetPostedPlaybackInfo request)
        {
            var result = await GetPlaybackInfo(request).ConfigureAwait(false);

            return ToOptimizedResult(result);
        }

        private T Clone<T>(T obj)
        {
            // Since we're going to be setting properties on MediaSourceInfos that come out of _mediaSourceManager, we should clone it
            // Should we move this directly into MediaSourceManager?

            var json = _json.SerializeToString(obj);
            return _json.DeserializeFromString<T>(json);
        }

        private async Task<PlaybackInfoResponse> GetPlaybackInfo(Guid id, Guid userId, string[] supportedLiveMediaTypes, string mediaSourceId = null, string liveStreamId = null)
        {
            var user = _userManager.GetUserById(userId);
            var item = _libraryManager.GetItemById(id);
            var result = new PlaybackInfoResponse();

            if (string.IsNullOrWhiteSpace(liveStreamId))
            {
                IEnumerable<MediaSourceInfo> mediaSources;
                try
                {
                    // TODO handle supportedLiveMediaTypes ?
                    mediaSources = await _mediaSourceManager.GetPlayackMediaSources(item, user, true, false, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    mediaSources = new List<MediaSourceInfo>();
                    // TODO Log exception
                    // TODO PlaybackException ??
                    //result.ErrorCode = ex.ErrorCode;
                }

                result.MediaSources = mediaSources.ToArray();

                if (!string.IsNullOrWhiteSpace(mediaSourceId))
                {
                    result.MediaSources = result.MediaSources
                        .Where(i => string.Equals(i.Id, mediaSourceId, StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                }
            }
            else
            {
                var mediaSource = await _mediaSourceManager.GetLiveStream(liveStreamId, CancellationToken.None).ConfigureAwait(false);

                result.MediaSources = new MediaSourceInfo[] { mediaSource };
            }

            if (result.MediaSources.Length == 0)
            {
                if (!result.ErrorCode.HasValue)
                {
                    result.ErrorCode = PlaybackErrorCode.NoCompatibleStream;
                }
            }
            else
            {
                result.MediaSources = Clone(result.MediaSources);

                result.PlaySessionId = Guid.NewGuid().ToString("N");
            }

            return result;
        }

        private void SetDeviceSpecificData(Guid itemId,
            PlaybackInfoResponse result,
            DeviceProfile profile,
            AuthorizationInfo auth,
            long? maxBitrate,
            long startTimeTicks,
            string mediaSourceId,
            int? audioStreamIndex,
            int? subtitleStreamIndex,
            int? maxAudioChannels,
            Guid userId,
            bool enableDirectPlay,
            bool forceDirectPlayRemoteMediaSource,
            bool enableDirectStream,
            bool enableTranscoding,
            bool allowVideoStreamCopy,
            bool allowAudioStreamCopy)
        {
            var item = _libraryManager.GetItemById(itemId);

            foreach (var mediaSource in result.MediaSources)
            {
                SetDeviceSpecificData(item, mediaSource, profile, auth, maxBitrate, startTimeTicks, mediaSourceId, audioStreamIndex, subtitleStreamIndex, maxAudioChannels, result.PlaySessionId, userId, enableDirectPlay, forceDirectPlayRemoteMediaSource, enableDirectStream, enableTranscoding, allowVideoStreamCopy, allowAudioStreamCopy);
            }

            SortMediaSources(result, maxBitrate);
        }

        private void SetDeviceSpecificData(BaseItem item,
            MediaSourceInfo mediaSource,
            DeviceProfile profile,
            AuthorizationInfo auth,
            long? maxBitrate,
            long startTimeTicks,
            string mediaSourceId,
            int? audioStreamIndex,
            int? subtitleStreamIndex,
            int? maxAudioChannels,
            string playSessionId,
            Guid userId,
            bool enableDirectPlay,
            bool forceDirectPlayRemoteMediaSource,
            bool enableDirectStream,
            bool enableTranscoding,
            bool allowVideoStreamCopy,
            bool allowAudioStreamCopy)
        {
            var streamBuilder = new StreamBuilder(_mediaEncoder, Logger);

            var options = new VideoOptions
            {
                MediaSources = new MediaSourceInfo[] { mediaSource },
                Context = EncodingContext.Streaming,
                DeviceId = auth.DeviceId,
                ItemId = item.Id,
                Profile = profile,
                MaxAudioChannels = maxAudioChannels
            };

            if (string.Equals(mediaSourceId, mediaSource.Id, StringComparison.OrdinalIgnoreCase))
            {
                options.MediaSourceId = mediaSourceId;
                options.AudioStreamIndex = audioStreamIndex;
                options.SubtitleStreamIndex = subtitleStreamIndex;
            }

            var user = _userManager.GetUserById(userId);

            if (!enableDirectPlay)
            {
                mediaSource.SupportsDirectPlay = false;
            }
            if (!enableDirectStream)
            {
                mediaSource.SupportsDirectStream = false;
            }
            if (!enableTranscoding)
            {
                mediaSource.SupportsTranscoding = false;
            }

            if (item is Audio)
            {
                Logger.LogInformation("User policy for {0}. EnableAudioPlaybackTranscoding: {1}", user.Name, user.Policy.EnableAudioPlaybackTranscoding);
            }
            else
            {
                Logger.LogInformation("User policy for {0}. EnablePlaybackRemuxing: {1} EnableVideoPlaybackTranscoding: {2} EnableAudioPlaybackTranscoding: {3}",
                    user.Name,
                    user.Policy.EnablePlaybackRemuxing,
                    user.Policy.EnableVideoPlaybackTranscoding,
                    user.Policy.EnableAudioPlaybackTranscoding);
            }

            if (mediaSource.SupportsDirectPlay)
            {
                if (mediaSource.IsRemote && forceDirectPlayRemoteMediaSource)
                {
                }
                else
                {
                    var supportsDirectStream = mediaSource.SupportsDirectStream;

                    // Dummy this up to fool StreamBuilder
                    mediaSource.SupportsDirectStream = true;
                    options.MaxBitrate = maxBitrate;

                    if (item is Audio)
                    {
                        if (!user.Policy.EnableAudioPlaybackTranscoding)
                        {
                            options.ForceDirectPlay = true;
                        }
                    }
                    else if (item is Video)
                    {
                        if (!user.Policy.EnableAudioPlaybackTranscoding && !user.Policy.EnableVideoPlaybackTranscoding && !user.Policy.EnablePlaybackRemuxing)
                        {
                            options.ForceDirectPlay = true;
                        }
                    }

                    // The MediaSource supports direct stream, now test to see if the client supports it
                    var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                        streamBuilder.BuildAudioItem(options) :
                        streamBuilder.BuildVideoItem(options);

                    if (streamInfo == null || !streamInfo.IsDirectStream)
                    {
                        mediaSource.SupportsDirectPlay = false;
                    }

                    // Set this back to what it was
                    mediaSource.SupportsDirectStream = supportsDirectStream;

                    if (streamInfo != null)
                    {
                        SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                    }
                }
            }

            if (mediaSource.SupportsDirectStream)
            {
                options.MaxBitrate = GetMaxBitrate(maxBitrate, user);

                if (item is Audio)
                {
                    if (!user.Policy.EnableAudioPlaybackTranscoding)
                    {
                        options.ForceDirectStream = true;
                    }
                }
                else if (item is Video)
                {
                    if (!user.Policy.EnableAudioPlaybackTranscoding && !user.Policy.EnableVideoPlaybackTranscoding && !user.Policy.EnablePlaybackRemuxing)
                    {
                        options.ForceDirectStream = true;
                    }
                }

                // The MediaSource supports direct stream, now test to see if the client supports it
                var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                    streamBuilder.BuildAudioItem(options) :
                    streamBuilder.BuildVideoItem(options);

                if (streamInfo == null || !streamInfo.IsDirectStream)
                {
                    mediaSource.SupportsDirectStream = false;
                }

                if (streamInfo != null)
                {
                    SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                }
            }

            if (mediaSource.SupportsTranscoding)
            {
                options.MaxBitrate = GetMaxBitrate(maxBitrate, user);

                // The MediaSource supports direct stream, now test to see if the client supports it
                var streamInfo = string.Equals(item.MediaType, MediaType.Audio, StringComparison.OrdinalIgnoreCase) ?
                    streamBuilder.BuildAudioItem(options) :
                    streamBuilder.BuildVideoItem(options);

                if (streamInfo != null)
                {
                    streamInfo.PlaySessionId = playSessionId;

                    if (streamInfo.PlayMethod == PlayMethod.Transcode)
                    {
                        streamInfo.StartPositionTicks = startTimeTicks;
                        mediaSource.TranscodingUrl = streamInfo.ToUrl("-", auth.Token).TrimStart('-');

                        if (!allowVideoStreamCopy)
                        {
                            mediaSource.TranscodingUrl += "&allowVideoStreamCopy=false";
                        }
                        if (!allowAudioStreamCopy)
                        {
                            mediaSource.TranscodingUrl += "&allowAudioStreamCopy=false";
                        }
                        mediaSource.TranscodingContainer = streamInfo.Container;
                        mediaSource.TranscodingSubProtocol = streamInfo.SubProtocol;
                    }

                    // Do this after the above so that StartPositionTicks is set
                    SetDeviceSpecificSubtitleInfo(streamInfo, mediaSource, auth.Token);
                }
            }
        }

        private long? GetMaxBitrate(long? clientMaxBitrate, User user)
        {
            var maxBitrate = clientMaxBitrate;
            var remoteClientMaxBitrate = user == null ? 0 : user.Policy.RemoteClientBitrateLimit;

            if (remoteClientMaxBitrate <= 0)
            {
                remoteClientMaxBitrate = _config.Configuration.RemoteClientBitrateLimit;
            }

            if (remoteClientMaxBitrate > 0)
            {
                var isInLocalNetwork = _networkManager.IsInLocalNetwork(Request.RemoteIp);

                Logger.LogInformation("RemoteClientBitrateLimit: {0}, RemoteIp: {1}, IsInLocalNetwork: {2}", remoteClientMaxBitrate, Request.RemoteIp, isInLocalNetwork);
                if (!isInLocalNetwork)
                {
                    maxBitrate = Math.Min(maxBitrate ?? remoteClientMaxBitrate, remoteClientMaxBitrate);
                }
            }

            return maxBitrate;
        }

        private void SetDeviceSpecificSubtitleInfo(StreamInfo info, MediaSourceInfo mediaSource, string accessToken)
        {
            var profiles = info.GetSubtitleProfiles(_mediaEncoder, false, "-", accessToken);
            mediaSource.DefaultSubtitleStreamIndex = info.SubtitleStreamIndex;

            mediaSource.TranscodeReasons = info.TranscodeReasons;

            foreach (var profile in profiles)
            {
                foreach (var stream in mediaSource.MediaStreams)
                {
                    if (stream.Type == MediaStreamType.Subtitle && stream.Index == profile.Index)
                    {
                        stream.DeliveryMethod = profile.DeliveryMethod;

                        if (profile.DeliveryMethod == SubtitleDeliveryMethod.External)
                        {
                            stream.DeliveryUrl = profile.Url.TrimStart('-');
                            stream.IsExternalUrl = profile.IsExternalUrl;
                        }
                    }
                }
            }
        }

        private void SortMediaSources(PlaybackInfoResponse result, long? maxBitrate)
        {
            var originalList = result.MediaSources.ToList();

            result.MediaSources = result.MediaSources.OrderBy(i =>
            {
                // Nothing beats direct playing a file
                if (i.SupportsDirectPlay && i.Protocol == MediaProtocol.File)
                {
                    return 0;
                }

                return 1;

            }).ThenBy(i =>
            {
                // Let's assume direct streaming a file is just as desirable as direct playing a remote url
                if (i.SupportsDirectPlay || i.SupportsDirectStream)
                {
                    return 0;
                }

                return 1;

            }).ThenBy(i =>
            {
                switch (i.Protocol)
                {
                    case MediaProtocol.File:
                        return 0;
                    default:
                        return 1;
                }

            }).ThenBy(i =>
            {
                if (maxBitrate.HasValue)
                {
                    if (i.Bitrate.HasValue)
                    {
                        if (i.Bitrate.Value <= maxBitrate.Value)
                        {
                            return 0;
                        }

                        return 2;
                    }
                }

                return 1;

            }).ThenBy(originalList.IndexOf)
            .ToArray();
        }
    }
}

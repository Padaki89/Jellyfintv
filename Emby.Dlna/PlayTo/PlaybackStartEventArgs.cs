#pragma warning disable CS1591

using System;

namespace Emby.Dlna.PlayTo
{
    public class PlaybackStartEventArgs : EventArgs
    {
        public PlaybackStartEventArgs(uBaseObject mediaInfo)
        {
            MediaInfo = mediaInfo ?? throw new ArgumentNullException(nameof(mediaInfo));
        }

        public uBaseObject MediaInfo { get; }
    }
}

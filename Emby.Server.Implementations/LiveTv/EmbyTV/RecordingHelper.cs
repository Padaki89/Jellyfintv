#pragma warning disable CS1591

using System;
using System.Globalization;
using MediaBrowser.Controller.LiveTv;

namespace Emby.Server.Implementations.LiveTv.EmbyTV
{
    internal static class RecordingHelper
    {
        public static DateTime GetStartTime(TimerInfo timer)
        {
            return timer.StartDate.AddSeconds(-timer.PrePaddingSeconds);
        }

        public static string GetRecordingName(TimerInfo info)
        {
            var name = info.Name;

            if (info.IsProgramSeries)
            {
                var addHyphen = true;

                if (info.SeasonNumber.HasValue && info.EpisodeNumber.HasValue)
                {
                    name += string.Format(
                        CultureInfo.InvariantCulture,
                        " S{0}E{1}",
                        info.SeasonNumber.Value.ToString("00", CultureInfo.InvariantCulture),
                        info.EpisodeNumber.Value.ToString("00", CultureInfo.InvariantCulture));
                    addHyphen = false;
                }
                else if (info.OriginalAirDate.HasValue)
                {
                    if (info.OriginalAirDate.Value.Date.Equals(info.StartDate.Date))
                    {
                        name += " " + GetDateTimeString(info.StartDate);
                    }
                    else
                    {
                        name += " " + GetDateString(info.OriginalAirDate.Value);
                    }
                }
                else
                {
                    name += " " + GetDateTimeString(info.StartDate);
                }

                if (!string.IsNullOrWhiteSpace(info.EpisodeTitle))
                {
                    if (addHyphen)
                    {
                        name += " -";
                    }

                    name += " " + info.EpisodeTitle;
                }
            }
            else if (info.IsMovie && info.ProductionYear != null)
            {
                name += " (" + info.ProductionYear + ")";
            }
            else
            {
                name += " " + GetDateTimeString(info.StartDate);
            }

            return name;
        }

        private static string GetDateTimeString(DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy_MM_dd_HH_mm_ss", CultureInfo.InvariantCulture);
        }

        private static string GetDateString(DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}

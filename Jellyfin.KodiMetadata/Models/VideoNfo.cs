﻿#pragma warning disable CA1819

using System;
using System.Xml.Serialization;

namespace Jellyfin.KodiMetadata.Models
{
    /// <summary>
    /// The video specific nfo tags.
    /// </summary>
    [XmlRoot("movie")]
    public class VideoNfo : BaseNfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoNfo"/> class.
        /// </summary>
        public VideoNfo()
        {
            Artists = Array.Empty<string>();
        }

        /// <summary>
        /// Gets or sets the IMDB Top 250 ranking.
        /// </summary>
        [XmlElement("top250")]
        public int? Top250 { get; set; }

        /// <summary>
        /// Gets or sets the movie set.
        /// </summary>
        [XmlElement("set")]
        public SetNfo? Set { get; set; }

        /// <summary>
        /// Gets or sets the connected TV show.
        /// </summary>
        [XmlElement("showlink")]
        public string? ShowLink { get; set; }

        /// <summary>
        /// Gets or sets the music video album.
        /// </summary>
        [XmlElement("album")]
        public string? Album { get; set; }

        /// <summary>
        /// Gets or sets the music video artists.
        /// </summary>
        [XmlElement("artist")]
        public string[] Artists { get; set; }

        /// <summary>
        /// Gets or sets the imdb id.
        /// </summary>
        [XmlElement("id")]
        public string? Id { get; set; }
    }
}

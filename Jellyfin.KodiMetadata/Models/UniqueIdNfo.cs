﻿namespace Jellyfin.KodiMetadata.Models
{
    /// <summary>
    /// The nfo uniqueid tag.
    /// </summary>
    public class UniqueIdNfo
    {
        /// <summary>
        /// Gets or sets the scraper site identifier.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the default scraper.
        /// </summary>
        public bool? Default { get; set; }

        /// <summary>
        /// Gets or sets the scraper site id.
        /// </summary>
        public string? Id { get; set; }
    }
}

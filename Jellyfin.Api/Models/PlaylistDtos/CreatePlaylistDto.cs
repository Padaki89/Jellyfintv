﻿using System;

namespace Jellyfin.Api.Models.PlaylistDtos
{
    /// <summary>
    /// Create new playlist dto.
    /// </summary>
    public class CreatePlaylistDto
    {
        /// <summary>
        /// Gets or sets the name of the new playlist.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets item ids to add to the playlist.
        /// </summary>
        public string? Ids { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        public string? MediaType { get; set; }
    }
}

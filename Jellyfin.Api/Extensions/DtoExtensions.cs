﻿#nullable enable

using System;
using System.Linq;
using MediaBrowser.Controller.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;
using Microsoft.AspNetCore.Http;

namespace Jellyfin.Api.Extensions
{
    /// <summary>
    /// Dto Extensions.
    /// </summary>
    public static class DtoExtensions
    {
        /// <summary>
        /// Add Dto Item fields.
        /// </summary>
        /// <param name="dtoOptions">DtoOptions object.</param>
        /// <param name="fields">Comma delimited string of fields.</param>
        /// <returns>Modified DtoOptions object.</returns>
        internal static DtoOptions AddItemFields(this DtoOptions dtoOptions, string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                dtoOptions.Fields = Array.Empty<ItemFields>();
            }
            else
            {
                dtoOptions.Fields = fields.Split(',')
                    .Select(v =>
                    {
                        if (Enum.TryParse(v, true, out ItemFields value))
                        {
                            return (ItemFields?)value;
                        }

                        return null;
                    })
                    .Where(i => i.HasValue)
                    .Select(i => i!.Value)
                    .ToArray();
            }

            return dtoOptions;
        }

        /// <summary>
        /// Check if DtoOptions contains field.
        /// </summary>
        /// <param name="dtoOptions">DtoOptions object.</param>
        /// <param name="field">Field to check.</param>
        /// <returns>Field existence.</returns>
        internal static bool ContainsField(this DtoOptions dtoOptions, ItemFields field)
            => dtoOptions.Fields != null && dtoOptions.Fields.Contains(field);

        /// <summary>
        /// Add additional DtoOptions.
        /// </summary>
        /// <param name="dtoOptions">DtoOptions object.</param>
        /// <param name="enableImages">Enable images.</param>
        /// <param name="enableUserData">Enable user data.</param>
        /// <param name="imageTypeLimit">Image type limit.</param>
        /// <param name="enableImageTypes">Enable image types.</param>
        /// <returns>Modified DtoOptions object.</returns>
        internal static DtoOptions AddAdditionalDtoOptions(
            in DtoOptions dtoOptions,
            bool? enableImages,
            bool? enableUserData,
            int? imageTypeLimit,
            string enableImageTypes)
        {
            dtoOptions.EnableImages = enableImages ?? true;

            if (imageTypeLimit.HasValue)
            {
                dtoOptions.ImageTypeLimit = imageTypeLimit.Value;
            }

            if (enableUserData.HasValue)
            {
                dtoOptions.EnableUserData = enableUserData.Value;
            }

            if (!string.IsNullOrWhiteSpace(enableImageTypes))
            {
                dtoOptions.ImageTypes = enableImageTypes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => (ImageType)Enum.Parse(typeof(ImageType), v, true))
                    .ToArray();
            }

            return dtoOptions;
        }

        internal static DtoOptions AddClientFields(
            this DtoOptions dtoOptions, HttpRequest request)
        {
            if (dtoOptions.Fields == null)
            {
                dtoOptions.Fields = Array.Empty<ItemFields>();
            }

            if (!dtoOptions.ContainsField(ItemFields.RecursiveItemCount)
                || !dtoOptions.ContainsField(ItemFields.ChildCount))
            {
                string? client = ClaimHelpers.GetClient(request);

                // No client in claim
                if (string.IsNullOrEmpty(client))
                {
                    return dtoOptions;
                }

                if (client.IndexOf("kodi", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("wmc", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("media center", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("classic", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    int oldLen = dtoOptions.Fields.Length;
                    var arr = new ItemFields[oldLen + 1];
                    dtoOptions.Fields.CopyTo(arr, 0);
                    arr[oldLen] = ItemFields.RecursiveItemCount;
                    dtoOptions.Fields = arr;
                }

                if (client.IndexOf("kodi", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("wmc", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("media center", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("classic", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("roku", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("samsung", StringComparison.OrdinalIgnoreCase) != -1 ||
                    client.IndexOf("androidtv", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    int oldLen = dtoOptions.Fields.Length;
                    var arr = new ItemFields[oldLen + 1];
                    dtoOptions.Fields.CopyTo(arr, 0);
                    arr[oldLen] = ItemFields.ChildCount;
                    dtoOptions.Fields = arr;
                }
            }

            return dtoOptions;
        }
    }
}

// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.IO;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Include-path helpers matching thinBasic Oxygen <c>InclPath</c>.
    /// </summary>
    public static class OxygenHostPaths
    {
        /// <summary>
        /// Expand <paramref name="marker"/> and resolve relative include names under
        /// <paramref name="includeRoot"/>.
        /// </summary>
        /// <param name="path">Path requested by Oxygen.</param>
        /// <param name="includeRoot">Host include directory.</param>
        /// <param name="marker">Prefix replaced by <paramref name="includeRoot"/>.</param>
        /// <returns>Resolved file-system path.</returns>
        public static string Resolve(
            string path,
            string includeRoot,
            string marker = OxygenHostOptions.DefaultAppIncludeMarker)
        {
            string request = path ?? string.Empty;
            string root = includeRoot ?? string.Empty;
            string token = string.IsNullOrEmpty(marker)
                ? OxygenHostOptions.DefaultAppIncludeMarker
                : marker;

            if (request.StartsWith(token, StringComparison.OrdinalIgnoreCase))
            {
                string rest = request.Substring(token.Length).TrimStart('\\', '/');

                if (string.IsNullOrEmpty(root))
                {
                    return rest;
                }

                return string.IsNullOrEmpty(rest)
                    ? Path.GetFullPath(root)
                    : Path.GetFullPath(Path.Combine(root, rest));
            }

            if (!string.IsNullOrEmpty(root) && !Path.IsPathRooted(request))
            {
                return Path.GetFullPath(Path.Combine(root, request));
            }

            return request;
        }
    }
}

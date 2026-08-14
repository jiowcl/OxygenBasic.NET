// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.IO;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Options for <see cref="Oxygenbasic.Run(string, OxygenHostOptions)"/>.
    /// </summary>
    public class OxygenHostOptions
    {
        /// <summary>
        /// Default marker expanded to <see cref="IncludeRoot"/> (thinBasic Oxygen <c>%app_includepath%</c>).
        /// </summary>
        public const string DefaultAppIncludeMarker = "%app_includepath%";

        /// <summary>
        /// Directory used to resolve <c>#include</c> / <c>includepath</c> when
        /// <see cref="PathResolver"/> is not set.
        /// </summary>
        public string IncludeRoot { get; set; }

        /// <summary>
        /// Marker replaced by <see cref="IncludeRoot"/> (default <c>%app_includepath%</c>).
        /// </summary>
        public string AppIncludeMarker { get; set; } = DefaultAppIncludeMarker;

        /// <summary>
        /// Optional include/path callback. When null and <see cref="IncludeRoot"/> is set,
        /// a default resolver is registered.
        /// </summary>
        public OxygenPathResolver PathResolver { get; set; }

        /// <summary>
        /// Optional host-variable callback (<c>o2_varcall</c>).
        /// </summary>
        public OxygenVarResolver VarResolver { get; set; }

        /// <summary>
        /// Call <see cref="Oxygenbasic.InitHost"/> before compile (default true).
        /// </summary>
        public bool InitHost { get; set; } = true;

        /// <summary>
        /// Throw <see cref="OxygenException"/> on compile/execute failure (default true).
        /// When false, inspect <see cref="OxygenRunResult"/>.
        /// </summary>
        public bool ThrowOnError { get; set; } = true;

        /// <summary>
        /// Clear previously rooted Pathcall/Varcall delegates at the start of Run (default true).
        /// </summary>
        public bool ClearHostCallbacks { get; set; } = true;
    }

    /// <summary>
    /// Result of <see cref="Oxygenbasic.Run(string, OxygenHostOptions)"/>.
    /// </summary>
    public sealed class OxygenRunResult
    {
        /// <summary>
        /// OxygenRunResult
        /// </summary>
        /// <param name="success">Whether compile and execute succeeded.</param>
        /// <param name="failedStage">Stage that failed, or <see cref="OxygenRunStage.None"/>.</param>
        /// <param name="errno">Oxygen errno after the last attempted step.</param>
        /// <param name="error">Oxygen error text.</param>
        /// <param name="code">Pointer returned by <c>o2_basic</c>.</param>
        /// <param name="execResult">Pointer returned by <c>o2_exec</c>.</param>
        public OxygenRunResult(
            bool success,
            OxygenRunStage failedStage,
            int errno,
            string error,
            IntPtr code,
            IntPtr execResult)
        {
            Success = success;
            FailedStage = failedStage;
            Errno = errno;
            Error = error ?? string.Empty;
            Code = code;
            ExecResult = execResult;
        }

        /// <summary>
        /// True when compile and execute both reported errno 0.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Failed stage, or <see cref="OxygenRunStage.None"/> on success.
        /// </summary>
        public OxygenRunStage FailedStage { get; }

        /// <summary>
        /// Oxygen errno.
        /// </summary>
        public int Errno { get; }

        /// <summary>
        /// Oxygen error text.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Compiled code pointer from <c>o2_basic</c>.
        /// </summary>
        public IntPtr Code { get; }

        /// <summary>
        /// Execution result pointer from <c>o2_exec</c>.
        /// </summary>
        public IntPtr ExecResult { get; }
    }

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

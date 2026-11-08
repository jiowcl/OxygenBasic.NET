// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

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
}

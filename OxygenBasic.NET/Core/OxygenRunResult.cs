// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;

namespace OxygenBasic.NET.Core
{
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
}

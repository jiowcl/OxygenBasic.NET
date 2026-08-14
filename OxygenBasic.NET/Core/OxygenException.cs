// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Stage at which a hosted <see cref="Oxygenbasic.Run(string)"/> failed.
    /// </summary>
    public enum OxygenRunStage
    {
        /// <summary>
        /// No failure.
        /// </summary>
        None = 0,

        /// <summary>
        /// <c>o2_basic</c> compile failed.
        /// </summary>
        Compile = 1,

        /// <summary>
        /// <c>o2_exec</c> run failed.
        /// </summary>
        Execute = 2
    }

    /// <summary>
    /// Thrown when hosted compile or execute fails and <see cref="OxygenHostOptions.ThrowOnError"/> is true.
    /// </summary>
    public class OxygenException : Exception
    {
        /// <summary>
        /// OxygenException
        /// </summary>
        /// <param name="stage">Failed stage.</param>
        /// <param name="errno">Oxygen errno.</param>
        /// <param name="message">Oxygen error text.</param>
        public OxygenException(
            OxygenRunStage stage,
            int errno,
            string message)
            : base(FormatMessage(stage, errno, message))
        {
            Stage = stage;
            Errno = errno;
            OxygenError = message ?? string.Empty;
        }

        /// <summary>
        /// Failed stage.
        /// </summary>
        public OxygenRunStage Stage { get; }

        /// <summary>
        /// Oxygen <c>o2_errno</c> value.
        /// </summary>
        public int Errno { get; }

        /// <summary>
        /// Oxygen <c>o2_error</c> text.
        /// </summary>
        public string OxygenError { get; }

        /// <summary>
        /// FormatMessage
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="errno"></param>
        /// <param name="message"></param>
        /// <returns>Returns string.</returns>
        private static string FormatMessage(
            OxygenRunStage stage,
            int errno,
            string message)
        {
            string text = string.IsNullOrWhiteSpace(message) ? "(no error text)" : message.Trim();
            
            return "Oxygen " + stage + " failed (errno=" + errno + "): " + text;
        }
    }
}

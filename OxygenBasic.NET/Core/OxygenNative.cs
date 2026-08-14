// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Resolves <c>oxygen.dll</c> DllImports to <c>oxygen.dll</c> (x86) or <c>oxygen64.dll</c> (x64).
    /// </summary>
    /// <remarks>
    /// A 64-bit process would load <c>oxygen64.dll</c>, whose <c>DllMain</c> currently AVs.
    /// Loading is refused with <see cref="PlatformNotSupportedException"/> unless
    /// <c>OXYGENBASIC_TRY_X64=1</c> is set. Details: <c>docs/oxygen64-x64-runtime.md</c>.
    /// </remarks>
    internal static class OxygenNative
    {
        /// <summary>
        /// Set to 1/true/yes to attempt loading oxygen64.dll in a 64-bit process.
        /// </summary>
        public const string TryX64EnvironmentVariable = "OXYGENBASIC_TRY_X64";

        /// <summary>
        /// The name of the import.
        /// </summary>
        private const string ImportName = "oxygen.dll";

        /// <summary>
        /// The sync object.
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// True when the resolver is registered.
        /// </summary>
        private static bool _resolverRegistered;

        /// <summary>
        /// Register the DllImport resolver once before any oxygen P/Invoke.
        /// </summary>
        public static void EnsureResolver()
        {
            if (_resolverRegistered)
            {
                return;
            }

            lock (Sync)
            {
                if (_resolverRegistered)
                {
                    return;
                }

                NativeLibrary.SetDllImportResolver(typeof(OxygenNative).Assembly, Resolve);
                _resolverRegistered = true;
            }
        }

        /// <summary>
        /// Native file name for the current process architecture.
        /// </summary>
        public static string NativeLibraryFileName =>
            Environment.Is64BitProcess ? "oxygen64.dll" : "oxygen.dll";

        /// <summary>
        /// True when this process can safely load the matching Oxygen native DLL
        /// without an explicit <see cref="TryX64EnvironmentVariable"/> override.
        /// </summary>
        public static bool SupportsCurrentProcess =>
            !Environment.Is64BitProcess || IsTryX64Enabled();

        /// <summary>
        /// Message thrown when a 64-bit process would otherwise LoadLibrary oxygen64.dll.
        /// </summary>
        public static string X64ProcessNotSupportedMessage =>
            "OxygenBasic.NET requires a 32-bit (x86) process. " +
            "This process is x64; loading oxygen64.dll calls DllMain and raises " +
            "ACCESS_VIOLATION (0xC0000005) on current upstream binaries. " +
            "Build and run with PlatformTarget=x86 (for example: dotnet build -p:Platform=x86). " +
            "AnyCPU on 64-bit Windows runs as x64 and will hit this error. " +
            "See docs/oxygen64-x64-runtime.md. " +
            "Set " + TryX64EnvironmentVariable + "=1 only if you have a fixed oxygen64.dll.";

        /// <summary>
        /// Resolve the DllImport.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="assembly"></param>
        /// <param name="searchPath"></param>
        /// <returns>Returns IntPtr.</returns>
        private static IntPtr Resolve(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            if (!string.Equals(libraryName, ImportName, StringComparison.OrdinalIgnoreCase))
            {
                return IntPtr.Zero;
            }

            if (Environment.Is64BitProcess && !IsTryX64Enabled())
            {
                throw new PlatformNotSupportedException(X64ProcessNotSupportedMessage);
            }

            string fileName = NativeLibraryFileName;
            string baseDir = AppContext.BaseDirectory;

            if (!string.IsNullOrEmpty(baseDir))
            {
                string candidate = Path.Combine(baseDir, fileName);

                if (File.Exists(candidate) && NativeLibrary.TryLoad(candidate, out IntPtr handle))
                {
                    return handle;
                }
            }

            string assemblyDir = Path.GetDirectoryName(assembly.Location);

            if (!string.IsNullOrEmpty(assemblyDir))
            {
                string candidate = Path.Combine(assemblyDir, fileName);

                if (File.Exists(candidate) && NativeLibrary.TryLoad(candidate, out IntPtr handle))
                {
                    return handle;
                }
            }

            if (NativeLibrary.TryLoad(fileName, assembly, searchPath, out IntPtr loaded))
            {
                return loaded;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// IsTryX64Enabled
        /// </summary>
        /// <returns>Returns bool.</returns>
        private static bool IsTryX64Enabled()
        {
            string value = Environment.GetEnvironmentVariable(TryX64EnvironmentVariable);

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();
            
            return value == "1"
                || value.Equals("true", StringComparison.OrdinalIgnoreCase)
                || value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}

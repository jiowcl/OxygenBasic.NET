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
    internal static class OxygenNative
    {
        // The name of the import library.
        private const string ImportName = "oxygen.dll";

        // The synchronization object.
        private static readonly object Sync = new object();

        // The flag to indicate if the resolver is registered.
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
        /// <returns>Returns the native library file name.</returns>
        public static string NativeLibraryFileName =>
            Environment.Is64BitProcess ? "oxygen64.dll" : "oxygen.dll";

        /// <summary>
        /// Resolve the native library file name.
        /// </summary>
        /// <param name="libraryName">The name of the library to resolve.</param>
        /// <param name="assembly">The assembly to resolve the library from.</param>
        /// <param name="searchPath">The search path to resolve the library from.</param>
        /// <returns>Returns the resolved library file name.</returns>
        private static IntPtr Resolve(
            string libraryName,
            Assembly assembly,
            DllImportSearchPath? searchPath)
        {
            if (!string.Equals(libraryName, ImportName, StringComparison.OrdinalIgnoreCase))
            {
                return IntPtr.Zero;
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
    }
}

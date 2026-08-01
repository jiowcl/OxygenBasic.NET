// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Resolves include/path strings for <see cref="Oxygenbasic.Pathcall(OxygenPathResolver)"/>.
    /// </summary>
    /// <param name="path">Path requested by Oxygen.</param>
    /// <returns>Resolved path returned to Oxygen as a UTF-8 BSTR.</returns>
    public delegate string OxygenPathResolver(
        string path);

    /// <summary>
    /// Resolves host variable names for <see cref="Oxygenbasic.Varcall(OxygenVarResolver)"/>.
    /// </summary>
    /// <param name="name">Variable name requested by Oxygen.</param>
    /// <returns>Pointer to host variable data, or <see cref="IntPtr.Zero"/>.</returns>
    public delegate IntPtr OxygenVarResolver(
        string name);

    /// <summary>
    /// Native path callback matching oxygen.dll <c>o2_pathcall</c>
    /// (<c>char*</c> in, UTF-8 BSTR out; ownership transferred to Oxygen).
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate IntPtr OxygenPathCallback(
        IntPtr path);

    /// <summary>
    /// Native variable callback matching oxygen.dll <c>o2_varcall</c>
    /// (<c>char*</c> name in, data pointer out).
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate IntPtr OxygenVarCallback(
        IntPtr name);

    /// <summary>
    /// Keeps managed delegates rooted so GC cannot collect callbacks registered with oxygen.dll.
    /// </summary>
    internal static class HostCallbackTable
    {
        /// <summary>
        /// Roots
        /// </summary>
        private static readonly ConcurrentDictionary<IntPtr, Delegate> Roots =
            new ConcurrentDictionary<IntPtr, Delegate>();

        /// <summary>
        /// Adds a delegate to the callback table.
        /// </summary>
        /// <param name="callback">The callback to root.</param>
        /// <returns>Native function pointer.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is null.</exception>
        public static IntPtr Add(
            Delegate callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IntPtr ptr = Marshal.GetFunctionPointerForDelegate(callback);
            Roots[ptr] = callback;

            return ptr;
        }

        /// <summary>
        /// Clears the callback table.
        /// </summary>
        public static void Clear()
        {
            Roots.Clear();
        }
    }
}

// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using OxygenBasic.NET.Core;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OxygenBasic.Example
{
    /// <summary>
    /// Hosted OxygenBasic demo: InitHost, Pathcall (include paths), shared host variables.
    /// </summary>
    public class Program
    {
        // The marker for the app include path.
        private const string AppIncludeMarker = "%app_includepath%";

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns void.</returns>
        static void Main(
            string[] args)
        {
            string sampleRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Sample"));
            string includeRoot = Path.Combine(sampleRoot, "inc");
            string scriptPath = Path.Combine(sampleRoot, "hosted_demo.txt");
            string scriptTemplate = File.ReadAllText(scriptPath, Encoding.UTF8);

            // Pinned host storage shared with Oxygen via dim ... at <address>.
            int[] hostCounter = { 100 };
            int[] fibResult = { 0 };
            int[] addResult = { 0 };
            GCHandle hostCounterHandle = GCHandle.Alloc(hostCounter, GCHandleType.Pinned);
            GCHandle fibResultHandle = GCHandle.Alloc(fibResult, GCHandleType.Pinned);
            GCHandle addResultHandle = GCHandle.Alloc(addResult, GCHandleType.Pinned);

            try
            {
                IntPtr hostCounterPtr = hostCounterHandle.AddrOfPinnedObject();
                IntPtr fibResultPtr = fibResultHandle.AddrOfPinnedObject();
                IntPtr addResultPtr = addResultHandle.AddrOfPinnedObject();

                // Oxygen requires a simple sys variable for dim ... at <address>.
                string scriptBuffer = scriptTemplate
                    .Replace("{{HOST_COUNTER_PTR}}", FormatNativeAddress(hostCounterPtr))
                    .Replace("{{HOST_FIB_PTR}}", FormatNativeAddress(fibResultPtr))
                    .Replace("{{HOST_ADD_PTR}}", FormatNativeAddress(addResultPtr));

                Oxygenbasic.ClearHostCallbacks();
                Oxygenbasic.InitHost();

                Console.WriteLine("OxygenBasic.NET hosted demo");
                Console.WriteLine("Process: " + (Environment.Is64BitProcess ? "x64 (oxygen64.dll)" : "x86 (oxygen.dll)"));
                Console.WriteLine("O2 Version: " + Oxygenbasic.Version());
                Console.WriteLine("Include root: " + includeRoot);
                Console.WriteLine("hostCounter @ 0x" + hostCounterPtr.ToInt64().ToString("X") + " = " + hostCounter[0]);
                Console.WriteLine();

                // thinBasic-style include path callback.
                Oxygenbasic.Pathcall(path => ResolveIncludePath(path, includeRoot));

                // thinBasic-style variable pointer callback (available if Oxygen asks by name).
                Oxygenbasic.Varcall(name => ResolveHostVariable(name, hostCounterHandle, fibResultHandle, addResultHandle));

                Oxygenbasic.O2Basic(scriptBuffer);

                if (Oxygenbasic.Errno() != 0)
                {
                    Console.WriteLine("Compile error: " + Oxygenbasic.Error());
                    return;
                }

                Oxygenbasic.Exec();

                if (Oxygenbasic.Errno() != 0)
                {
                    Console.WriteLine("Runtime error: " + Oxygenbasic.Error());
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Results written by Oxygen into .NET memory:");
                Console.WriteLine("  Fibonacci(10) = " + fibResult[0]);
                Console.WriteLine("  Add(40, 2)    = " + addResult[0]);
                Console.WriteLine("  hostCounter   = " + hostCounter[0] + " (was 100)");
            }
            finally
            {
                Oxygenbasic.ClearHostCallbacks();
                FreeHandle(hostCounterHandle);
                FreeHandle(fibResultHandle);
                FreeHandle(addResultHandle);
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit...");
            try
            {
                Console.ReadLine();
            }
            catch (InvalidOperationException)
            {
                // Input redirected (CI / piped run).
            }
        }

        /// <summary>
        /// Mirrors thinBasic Oxygen <c>InclPath</c>: expand <c>%app_includepath%</c>
        /// and resolve relative include names under the host include directory.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <param name="includeRoot">The root include directory.</param>
        /// <returns>Returns the resolved path.</returns>
        private static string ResolveIncludePath(
            string path, 
            string includeRoot)
        {
            string request = path ?? string.Empty;

            if (request.StartsWith(AppIncludeMarker, StringComparison.OrdinalIgnoreCase))
            {
                string rest = request.Substring(AppIncludeMarker.Length).TrimStart('\\', '/');
                string resolved = string.IsNullOrEmpty(rest)
                    ? includeRoot
                    : Path.GetFullPath(Path.Combine(includeRoot, rest));

                Console.WriteLine("[Pathcall] " + request + " -> " + resolved);
                return resolved;
            }

            if (!Path.IsPathRooted(request))
            {
                string resolved = Path.GetFullPath(Path.Combine(includeRoot, request));
                Console.WriteLine("[Pathcall] " + request + " -> " + resolved);
                return resolved;
            }

            Console.WriteLine("[Pathcall] " + request);

            return request;
        }

        /// <summary>
        /// Mirrors thinBasic Oxygen <c>GetVarPtr</c>: return a pointer to host storage by name.
        /// </summary>
        /// <param name="name">The name of the variable to resolve.</param>
        /// <param name="hostCounterHandle">The handle to the host counter variable.</param>
        /// <param name="fibResultHandle">The handle to the fibonacci result variable.</param>
        /// <param name="addResultHandle">The handle to the add result variable.</param>
        /// <returns>Returns the pointer to the host storage.</returns>
        private static IntPtr ResolveHostVariable(
            string name,
            GCHandle hostCounterHandle,
            GCHandle fibResultHandle,
            GCHandle addResultHandle)
        {
            if (string.Equals(name, "hostCounter", StringComparison.OrdinalIgnoreCase))
            {
                return LogVar(name, hostCounterHandle.AddrOfPinnedObject());
            }

            if (string.Equals(name, "fibResult", StringComparison.OrdinalIgnoreCase))
            {
                return LogVar(name, fibResultHandle.AddrOfPinnedObject());
            }

            if (string.Equals(name, "addResult", StringComparison.OrdinalIgnoreCase))
            {
                return LogVar(name, addResultHandle.AddrOfPinnedObject());
            }

            Console.WriteLine("[Varcall] " + name + " -> (null)");
            
            return IntPtr.Zero;
        }

        /// <summary>
        /// Logs the variable name and pointer.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="ptr">The pointer to the variable.</param>
        /// <returns>Returns the pointer to the variable.</returns>
        private static IntPtr LogVar(
            string name, 
            IntPtr ptr)
        {
            Console.WriteLine("[Varcall] " + name + " -> 0x" + ptr.ToInt64().ToString("X"));

            return ptr;
        }

        /// <summary>
        /// Format a pointer for Oxygen <c>sys</c> literals (decimal, full width).
        /// </summary>
        /// <param name="ptr">The pointer to format.</param>
        /// <returns>Returns the formatted pointer.</returns>
        private static string FormatNativeAddress(IntPtr ptr)
        {
            return ptr.ToInt64().ToString();
        }

        /// <summary>
        /// Frees the handle.
        /// </summary>
        /// <param name="handle">The handle to free.</param>
        /// <returns>Returns void.</returns>
        private static void FreeHandle(
            GCHandle handle)
        {
            if (handle.IsAllocated)
            {
                handle.Free();
            }
        }
    }
}

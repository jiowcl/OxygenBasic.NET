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

                Console.WriteLine("OxygenBasic.NET hosted demo");
                Console.WriteLine("Process: " + (Environment.Is64BitProcess ? "x64 (oxygen64.dll)" : "x86 (oxygen.dll)"));
                Console.WriteLine("O2 Version: " + Oxygenbasic.Version());
                Console.WriteLine("Include root: " + includeRoot);
                Console.WriteLine("hostCounter @ 0x" + hostCounterPtr.ToInt64().ToString("X") + " = " + hostCounter[0]);
                Console.WriteLine();

                Oxygenbasic.Run(scriptBuffer, new OxygenHostOptions
                {
                    IncludeRoot = includeRoot,
                    PathResolver = path =>
                    {
                        string resolved = OxygenHostPaths.Resolve(path, includeRoot);
                        Console.WriteLine("[Pathcall] " + path + " -> " + resolved);
                        return resolved;
                    },
                    VarResolver = name => ResolveHostVariable(
                        name,
                        hostCounterHandle,
                        fibResultHandle,
                        addResultHandle)
                });

                Console.WriteLine();
                Console.WriteLine("Results written by Oxygen into .NET memory:");
                Console.WriteLine("  Fibonacci(10) = " + fibResult[0]);
                Console.WriteLine("  Add(40, 2)    = " + addResult[0]);
                Console.WriteLine("  hostCounter   = " + hostCounter[0] + " (was 100)");
            }
            catch (OxygenException ex)
            {
                Console.WriteLine(ex.Message);
                return;
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

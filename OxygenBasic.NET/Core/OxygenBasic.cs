// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Oxygen host wrapper. Native state in <c>oxygen.dll</c> is process-wide:
    /// do not call from multiple threads concurrently, and do not call
    /// <see cref="Abst"/> if you still need to <see cref="Run"/> or <see cref="O2Basic"/>
    /// in the same process. See <c>docs/oxygen-process-state.md</c>.
    /// </summary>
    public class Oxygenbasic
    {
        // Engine lock for process-wide state.
        private static readonly object EngineLock = new object();

        // Abstract mode flag.
        private static bool _abstractMode;

        /// <summary>
        /// Constructor
        /// </summary>
        static Oxygenbasic()
        {
            OxygenNative.EnsureResolver();
        }

        /// <summary>
        /// AbstNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_abst", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr AbstNative(IntPtr s);

        /// <summary>
        /// O2BasicNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_basic", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr O2BasicNative(IntPtr s);

        /// <summary>
        /// ExecNative
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_exec", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ExecNative(IntPtr p);

        /// <summary>
        /// BufNative
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_buf", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr BufNative(int n);

        /// <summary>
        /// ErrnoNative
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_errno", CallingConvention = CallingConvention.StdCall)]
        private static extern int ErrnoNative();

        /// <summary>
        /// ErrorNative
        /// </summary>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_error", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ErrorNative();

        /// <summary>
        /// LenNative
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_len", CallingConvention = CallingConvention.StdCall)]
        private static extern int LenNative();

        /// <summary>
        /// LibNative
        /// </summary>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_lib", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr LibNative();

        /// <summary>
        /// LinkNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_link", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr LinkNative(IntPtr s);

        /// <summary>
        /// ModeNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_mode", CallingConvention = CallingConvention.StdCall)]
        private static extern void ModeNative(int m);

        /// <summary>
        /// PathcallNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_pathcall", CallingConvention = CallingConvention.StdCall)]
        private static extern void PathcallNative(IntPtr m);

        /// <summary>
        /// PrepNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_prep", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr PrepNative(IntPtr s);

        /// <summary>
        /// VarcallNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_varcall", CallingConvention = CallingConvention.StdCall)]
        private static extern void VarcallNative(IntPtr m);

        /// <summary>
        /// VersionNative
        /// </summary>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_version", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr VersionNative();

        /// <summary>
        /// ViewNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_view", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ViewNative(IntPtr s);

        /// <summary>
        /// Native Oxygen DLL file name for the current process (<c>oxygen.dll</c> or <c>oxygen64.dll</c>).
        /// </summary>
        public static string NativeLibraryFileName => OxygenNative.NativeLibraryFileName;

        /// <summary>
        /// True when the current process architecture can load Oxygen without an x64 override.
        /// 64-bit (AnyCPU on 64-bit Windows) is refused until a safe <c>oxygen64.dll</c> is available.
        /// </summary>
        public static bool SupportsCurrentProcess => OxygenNative.SupportsCurrentProcess;

        /// <summary>
        /// Throws if this is a 64-bit process and <c>oxygen64.dll</c> must not be loaded.
        /// Called automatically on the first native call.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Current process is x64 without override.</exception>
        public static void ThrowIfProcessNotSupported()
        {
            if (!SupportsCurrentProcess)
            {
                throw new PlatformNotSupportedException(OxygenNative.X64ProcessNotSupportedMessage);
            }
        }

        /// <summary>
        /// True after <see cref="Abst"/> has switched oxygen.dll into abstract/assembler view.
        /// There is no native reset; compile/Run in this process will throw.
        /// </summary>
        public static bool IsAbstractMode
        {
            get
            {
                lock (EngineLock)
                {
                    return _abstractMode;
                }
            }
        }

        /// <summary>
        /// Message used when compile APIs are called after <see cref="Abst"/>.
        /// </summary>
        public static string AbstractModeMessage =>
            "oxygen.dll keeps process-wide compiler state. Abst() switched the engine " +
            "into abstract/assembler view and that cannot be undone in this process " +
            "(there is no native reset). Start a new process to compile or Run again. " +
            "Oxygen APIs are serialized on one lock but state is still global — " +
            "do not use parallel threads. See docs/oxygen-process-state.md.";

        /// <summary>
        /// Initialize the Oxygen host for .NET use (bstring UTF-8 mode).
        /// Matches thinBasic Oxygen module default <c>o2_mode(9)</c>.
        /// </summary>
        public static void InitHost()
        {
            Mode(Enums.Mode.Bstring);
        }

        /// <summary>
        /// Allocate a UTF-8 BSTR for an <see cref="OxygenPathCallback"/> return value.
        /// Ownership is transferred to oxygen.dll; do not free the pointer.
        /// </summary>
        /// <param name="value">The string to return.</param>
        /// <returns>UTF-8 BSTR pointer.</returns>
        public static IntPtr AllocReturnString(string value)
        {
            return AnsiBStrMarshal.Alloc(value);
        }

        /// <summary>
        /// Clears rooted Pathcall/Varcall delegates.
        /// </summary>
        public static void ClearHostCallbacks()
        {
            HostCallbackTable.Clear();
        }

        /// <summary>
        /// Abst (abstract/assembler listing). Leaves the process-wide engine in abstract mode;
        /// later <see cref="O2Basic"/> / <see cref="Run(string)"/> throw
        /// <see cref="InvalidOperationException"/>. See <c>docs/oxygen-process-state.md</c>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string Abst(string s)
        {
            return WithEngine(() =>
            {
                IntPtr p = AnsiBStrMarshal.Alloc(s);

                try
                {
                    string result = AnsiBStrMarshal.PtrToString(AbstNative(p));
                    _abstractMode = true;
                    return result;
                }
                finally
                {
                    AnsiBStrMarshal.Free(p);
                }
            });
        }

        /// <summary>
        /// O2Basic
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Pointer to compiled code, or <see cref="IntPtr.Zero"/> on failure.</returns>
        public static IntPtr O2Basic(string s)
        {
            return WithEngine(() =>
            {
                IntPtr p = AnsiBStrMarshal.Alloc(s);

                try
                {
                    return O2BasicNative(p);
                }
                finally
                {
                    AnsiBStrMarshal.Free(p);
                }
            }, requireCompileReady: true);
        }

        /// <summary>
        /// Exec
        /// </summary>
        /// <returns>Execution result pointer.</returns>
        public static IntPtr Exec()
        {
            return WithEngine(() => ExecNative(IntPtr.Zero), requireCompileReady: true);
        }

        /// <summary>
        /// Exec
        /// </summary>
        /// <param name="p">Optional code pointer (use <see cref="IntPtr.Zero"/> for default).</param>
        /// <returns>Execution result pointer.</returns>
        public static IntPtr Exec(IntPtr p)
        {
            return WithEngine(() => ExecNative(p), requireCompileReady: true);
        }

        /// <summary>
        /// Exec (x86-compatible address).
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Execution result pointer.</returns>
        public static IntPtr Exec(uint p)
        {
            return Exec(new IntPtr(unchecked((int)p)));
        }

        /// <summary>
        /// Buf
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Buffer pointer.</returns>
        public static IntPtr Buf(int n)
        {
            return WithEngine(() => BufNative(n), requireCompileReady: true);
        }

        /// <summary>
        /// Errno
        /// </summary>
        /// <returns>Returns int.</returns>
        public static int Errno()
        {
            return WithEngine(ErrnoNative);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns>Returns string.</returns>
        public static string Error()
        {
            return WithEngine(() => AnsiBStrMarshal.PtrToString(ErrorNative()));
        }

        /// <summary>
        /// Len
        /// </summary>
        /// <returns>Returns int.</returns>
        public static int Len()
        {
            return WithEngine(LenNative, requireCompileReady: true);
        }

        /// <summary>
        /// Lib
        /// </summary>
        /// <returns>Oxygen library module handle.</returns>
        public static IntPtr Lib()
        {
            return WithEngine(LibNative);
        }

        /// <summary>
        /// Link
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Pointer to linked code.</returns>
        public static IntPtr Link(string s)
        {
            return WithEngine(() =>
            {
                IntPtr p = AnsiBStrMarshal.Alloc(s);

                try
                {
                    return LinkNative(p);
                }
                finally
                {
                    AnsiBStrMarshal.Free(p);
                }
            }, requireCompileReady: true);
        }

        /// <summary>
        /// Eval (alias of <see cref="Link"/>; matches thinBasic <c>O2_Eval</c>).
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Pointer to linked code.</returns>
        public static IntPtr Eval(string s)
        {
            return Link(s);
        }

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Mode(int m)
        {
            WithEngine(() => ModeNative(m));
        }

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Mode(Enums.Mode m)
        {
            ModeNative((int)m);
        }

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(IntPtr m)
        {
            WithEngine(() => PathcallNative(m));
        }

        /// <summary>
        /// Pathcall (x86-compatible address).
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(uint m)
        {
            Pathcall(new IntPtr(unchecked((int)m)));
        }

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="callback">Native path callback.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(OxygenPathCallback callback)
        {
            IntPtr ptr = HostCallbackTable.Add(callback);
            PathcallNative(ptr);
        }

        /// <summary>
        /// Pathcall with a managed path resolver.
        /// </summary>
        /// <param name="resolver">Managed path resolver.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(OxygenPathResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            OxygenPathCallback callback = pathPtr =>
            {
                string path = Marshal.PtrToStringUTF8(pathPtr) ?? string.Empty;
                return AllocReturnString(resolver(path));
            };

            Pathcall(callback);
        }

        /// <summary>
        /// Prep
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string Prep(string s)
        {
            return WithEngine(() =>
            {
                IntPtr p = AnsiBStrMarshal.Alloc(s);

                try
                {
                    return AnsiBStrMarshal.PtrToString(PrepNative(p));
                }
                finally
                {
                    AnsiBStrMarshal.Free(p);
                }
            }, requireCompileReady: true);
        }

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(IntPtr m)
        {
            WithEngine(() => VarcallNative(m));
        }

        /// <summary>
        /// Varcall (x86-compatible address).
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(uint m)
        {
            Varcall(new IntPtr(unchecked((int)m)));
        }

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="callback">Native variable callback.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(OxygenVarCallback callback)
        {
            IntPtr ptr = HostCallbackTable.Add(callback);
            VarcallNative(ptr);
        }

        /// <summary>
        /// Varcall with a managed variable resolver.
        /// </summary>
        /// <param name="resolver">Managed variable resolver.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(OxygenVarResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            OxygenVarCallback callback = namePtr =>
            {
                string name = Marshal.PtrToStringUTF8(namePtr) ?? string.Empty;
                return resolver(name);
            };

            Varcall(callback);
        }

        /// <summary>
        /// Version
        /// </summary>
        /// <returns>Returns string.</returns>
        public static string Version()
        {
            return WithEngine(() => AnsiBStrMarshal.PtrToString(VersionNative()));
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string View(string s)
        {
            return WithEngine(() =>
            {
                IntPtr p = AnsiBStrMarshal.Alloc(s);

                try
                {
                    return AnsiBStrMarshal.PtrToString(ViewNative(p));
                }
                finally
                {
                    AnsiBStrMarshal.Free(p);
                }
            }, requireCompileReady: true);
        }

        /// <summary>
        /// Compile and execute Oxygen source (InitHost, optional Pathcall/Varcall, O2Basic, Exec).
        /// Throws <see cref="OxygenException"/> on failure.
        /// </summary>
        /// <param name="source">Oxygen source text.</param>
        /// <returns>Run result.</returns>
        public static OxygenRunResult Run(string source)
        {
            return Run(source, null);
        }

        /// <summary>
        /// Compile and execute Oxygen source from a file.
        /// Throws <see cref="OxygenException"/> on failure unless
        /// <see cref="OxygenHostOptions.ThrowOnError"/> is false.
        /// </summary>
        /// <param name="path">Script file path.</param>
        /// <param name="options">Host options, or null for defaults.</param>
        /// <returns>Run result.</returns>
        public static OxygenRunResult RunFile(
            string path,
            OxygenHostOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Script path is required.", nameof(path));
            }

            string source = File.ReadAllText(path);
            OxygenHostOptions host = options ?? new OxygenHostOptions();

            if (string.IsNullOrEmpty(host.IncludeRoot))
            {
                string scriptDir = Path.GetDirectoryName(Path.GetFullPath(path));

                if (!string.IsNullOrEmpty(scriptDir))
                {
                    host.IncludeRoot = scriptDir;
                }
            }

            return Run(source, host);
        }

        /// <summary>
        /// Compile and execute Oxygen source (InitHost, optional Pathcall/Varcall, O2Basic, Exec).
        /// </summary>
        /// <param name="source">Oxygen source text.</param>
        /// <param name="options">Host options, or null for defaults.</param>
        /// <returns>Run result.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
        /// <exception cref="OxygenException">Thrown on compile/execute failure when throwing is enabled.</exception>
        public static OxygenRunResult Run(
            string source,
            OxygenHostOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            OxygenHostOptions host = options ?? new OxygenHostOptions();

            ThrowIfProcessNotSupported();

            if (host.ClearHostCallbacks)
            {
                ClearHostCallbacks();
            }

            if (host.InitHost)
            {
                InitHost();
            }

            OxygenPathResolver pathResolver = host.PathResolver;

            if (pathResolver == null && !string.IsNullOrEmpty(host.IncludeRoot))
            {
                string includeRoot = host.IncludeRoot;
                string marker = host.AppIncludeMarker;
                pathResolver = path => OxygenHostPaths.Resolve(path, includeRoot, marker);
            }

            if (pathResolver != null)
            {
                Pathcall(pathResolver);
            }

            if (host.VarResolver != null)
            {
                Varcall(host.VarResolver);
            }

            IntPtr code = O2Basic(source);
            int errno = Errno();

            if (errno != 0)
            {
                return Fail(host, OxygenRunStage.Compile, errno, code, IntPtr.Zero);
            }

            IntPtr execResult = Exec();
            errno = Errno();

            if (errno != 0)
            {
                return Fail(host, OxygenRunStage.Execute, errno, code, execResult);
            }

            return new OxygenRunResult(
                true,
                OxygenRunStage.None,
                0,
                string.Empty,
                code,
                execResult);
        }

        /// <summary>
        /// Fail
        /// </summary>
        /// <param name="host"></param>
        /// <param name="stage"></param>
        /// <param name="errno"></param>
        /// <param name="code"></param>
        /// <param name="execResult"></param>
        /// <returns>Returns OxygenRunResult.</returns>
        private static OxygenRunResult Fail(
            OxygenHostOptions host,
            OxygenRunStage stage,
            int errno,
            IntPtr code,
            IntPtr execResult)
        {
            string error = Error() ?? string.Empty;
            
            OxygenRunResult result = new OxygenRunResult(
                false,
                stage,
                errno,
                error,
                code,
                execResult);

            if (host.ThrowOnError)
            {
                throw new OxygenException(stage, errno, error);
            }

            return result;
        }

        /// <summary>
        /// ThrowIfAbstractMode
        /// </summary>
        /// <returns>Returns void.</returns>
        private static void ThrowIfAbstractMode()
        {
            if (_abstractMode)
            {
                throw new InvalidOperationException(AbstractModeMessage);
            }
        }

        /// <summary>
        /// WithEngine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="requireCompileReady"></param>
        /// <returns>Returns T.</returns>
        private static T WithEngine<T>(
            Func<T> action,
            bool requireCompileReady = false)
        {
            lock (EngineLock)
            {
                ThrowIfProcessNotSupported();

                if (requireCompileReady)
                {
                    ThrowIfAbstractMode();
                }

                return action();
            }
        }

        /// <summary>
        /// WithEngine
        /// </summary>
        /// <param name="action"></param>
        /// <param name="requireCompileReady"></param>
        /// <returns>Returns void.</returns>
        private static void WithEngine(
            Action action,
            bool requireCompileReady = false)
        {
            WithEngine(() =>
            {
                action();
                return 0;
            }, requireCompileReady);
        }
    }
}

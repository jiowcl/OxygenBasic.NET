// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Oxygenbasic
    /// </summary>
    public class Oxygenbasic
    {
        /// <summary>
        /// AbstNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_abst", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr AbstNative(
            IntPtr s);

        /// <summary>
        /// O2BasicNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_basic", CallingConvention = CallingConvention.StdCall)]
        private static extern uint O2BasicNative(
            IntPtr s);

        /// <summary>
        /// ExecNative
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_exec", CallingConvention = CallingConvention.StdCall)]
        private static extern uint ExecNative(
            uint p = 0);

        /// <summary>
        /// BufNative
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_buf", CallingConvention = CallingConvention.StdCall)]
        private static extern uint BufNative(
            int n);

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
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_lib", CallingConvention = CallingConvention.StdCall)]
        private static extern uint LibNative();

        /// <summary>
        /// LinkNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_link", CallingConvention = CallingConvention.StdCall)]
        private static extern uint LinkNative(
            IntPtr s);

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
        private static extern void PathcallNative(
            IntPtr m);

        /// <summary>
        /// PrepNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns IntPtr.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_prep", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr PrepNative(
            IntPtr s);

        /// <summary>
        /// VarcallNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_varcall", CallingConvention = CallingConvention.StdCall)]
        private static extern void VarcallNative(
            IntPtr m);

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
        private static extern IntPtr ViewNative(
            IntPtr s);

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
        public static IntPtr AllocReturnString(
            string value)
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
        /// Abst
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string Abst(
            string s)
        {
            IntPtr p = AnsiBStrMarshal.Alloc(s);

            try
            {
                return AnsiBStrMarshal.PtrToString(AbstNative(p));
            }
            finally
            {
                AnsiBStrMarshal.Free(p);
            }
        }

        /// <summary>
        /// O2Basic
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        public static uint O2Basic(
            string s)
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
        }

        /// <summary>
        /// Exec
        /// </summary>
        /// <returns>Returns uint.</returns>
        public static uint Exec()
        {
            return ExecNative(0);
        }

        /// <summary>
        /// Exec
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns uint.</returns>
        public static uint Exec(
            uint p)
        {
            return ExecNative(p);
        }

        /// <summary>
        /// Buf
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns uint.</returns>
        public static uint Buf(
            int n)
        {
            return BufNative(n);
        }

        /// <summary>
        /// Errno
        /// </summary>
        /// <returns>Returns int.</returns>
        public static int Errno()
        {
            return ErrnoNative();
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns>Returns string.</returns>
        public static string Error()
        {
            return AnsiBStrMarshal.PtrToString(ErrorNative());
        }

        /// <summary>
        /// Len
        /// </summary>
        /// <returns>Returns int.</returns>
        public static int Len()
        {
            return LenNative();
        }

        /// <summary>
        /// Lib
        /// </summary>
        /// <returns>Returns uint.</returns>
        public static uint Lib()
        {
            return LibNative();
        }

        /// <summary>
        /// Link
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        public static uint Link(
            string s)
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
        }

        /// <summary>
        /// Eval (alias of <see cref="Link"/>; matches thinBasic <c>O2_Eval</c>).
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        public static uint Eval(
            string s)
        {
            return Link(s);
        }

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Mode(
            int m)
        {
            ModeNative(m);
        }

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Mode(
            Enums.Mode m)
        {
            ModeNative((int)m);
        }

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(
            uint m)
        {
            PathcallNative(new IntPtr(unchecked((int)m)));
        }

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="callback">Native path callback.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(
            OxygenPathCallback callback)
        {
            IntPtr ptr = HostCallbackTable.Add(callback);
            PathcallNative(ptr);
        }

        /// <summary>
        /// Pathcall with a managed path resolver.
        /// </summary>
        /// <param name="resolver">Managed path resolver.</param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(
            OxygenPathResolver resolver)
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
        public static string Prep(
            string s)
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
        }

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="m">Native function pointer.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(
            uint m)
        {
            VarcallNative(new IntPtr(unchecked((int)m)));
        }

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="callback">Native variable callback.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(
            OxygenVarCallback callback)
        {
            IntPtr ptr = HostCallbackTable.Add(callback);
            VarcallNative(ptr);
        }

        /// <summary>
        /// Varcall with a managed variable resolver.
        /// </summary>
        /// <param name="resolver">Managed variable resolver.</param>
        /// <returns>Returns void.</returns>
        public static void Varcall(
            OxygenVarResolver resolver)
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
            return AnsiBStrMarshal.PtrToString(VersionNative());
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string View(
            string s)
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
        }
    }
}

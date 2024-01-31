// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2024 Jiowcl. All rights reserved.

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
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_abst", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private static extern string AbstNative([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// O2BasicNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_basic", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint O2BasicNative([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// ExecNative
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_exec", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint ExecNative(uint p = 0);

        /// <summary>
        /// BufNative
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_buf", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint BufNative(int n);

        /// <summary>
        /// ErrnoNative
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_errno", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern int ErrnoNative();

        /// <summary>
        /// ErrorNative
        /// </summary>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_error", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private static extern string ErrorNative();

        /// <summary>
        /// LenNative
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_len", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern int LenNative();

        /// <summary>
        /// LibNative
        /// </summary>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_lib", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint LibNative();

        /// <summary>
        /// LinkNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_link", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint LinkNative([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// ModeNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_mode", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void ModeNative(int m);

        /// <summary>
        /// PathcallNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_pathcall", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void PathcallNative(uint m);

        /// <summary>
        /// PrepNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_prep", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private static extern string PrepNative([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// VarcallNative
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_varcall", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void VarcallNative(uint m);

        /// <summary>
        /// VersionNative
        /// </summary>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_version", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private static extern string VersionNative();

        /// <summary>
        /// ViewNative
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_view", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private static extern string ViewNative([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// Abst
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string Abst(string s)
        {
            return AbstNative(s);
        }

        /// <summary>
        /// O2Basic
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        public static uint O2Basic(string s)
        {
            return O2BasicNative(s);
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
        public static uint Exec(uint p)
        {
            return ExecNative(p);
        }

        /// <summary>
        /// Buf
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns uint.</returns>
        public static uint Buf(int n)
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
            return ErrorNative();
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
        public static uint Link(string s)
        {
            return LinkNative(s);
        }

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Mode(int m)
        {
            ModeNative(m);
        }

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Pathcall(uint m)
        {
            PathcallNative(m);
        }

        /// <summary>
        /// Prep
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string Prep(string s)
        {
            return PrepNative(s);
        }

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        public static void Varcall(uint m)
        {
            VarcallNative(m);
        }

        /// <summary>
        /// Version
        /// </summary>
        /// <returns>Returns string.</returns>
        public static string Version()
        {
            return VersionNative();
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        public static string View(string s)
        {
            return ViewNative(s);
        }
    }
}

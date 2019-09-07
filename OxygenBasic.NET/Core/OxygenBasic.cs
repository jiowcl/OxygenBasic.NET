using System.Runtime.InteropServices;

namespace OxygenBasic.NET.Core
{
    /// <summary>
    /// Oxygenbasic
    /// </summary>
    public class Oxygenbasic
    {
        /// <summary>
        /// Abst
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_abst", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public static extern string Abst([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// O2Basic
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_basic", CallingConvention = CallingConvention.StdCall)]
        public static extern uint O2Basic([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// Exec
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_exec", CallingConvention = CallingConvention.StdCall)]
        public static extern uint Exec(uint p = 0);

        /// <summary>
        /// Buf
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_buf", CallingConvention = CallingConvention.StdCall)]
        public static extern uint Buf(int n);

        /// <summary>
        /// Errno
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_errno", CallingConvention = CallingConvention.StdCall)]
        public static extern int Errno();

        /// <summary>
        /// Error
        /// </summary>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_error", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public static extern string Error();

        /// <summary>
        /// Len
        /// </summary>
        /// <returns>Returns int.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_len", CallingConvention = CallingConvention.StdCall)]
        public static extern int Len();

        /// <summary>
        /// Lib
        /// </summary>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_lib", CallingConvention = CallingConvention.StdCall)]
        public static extern uint Lib();

        /// <summary>
        /// Link
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns uint.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_link", CallingConvention = CallingConvention.StdCall)]
        public static extern uint Link([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// Mode
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_mode", CallingConvention = CallingConvention.StdCall)]
        public static extern void Mode(int m);

        /// <summary>
        /// Pathcall
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_pathcall", CallingConvention = CallingConvention.StdCall)]
        public static extern void Pathcall(uint m);

        /// <summary>
        /// Prep
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_prep", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public static extern string Prep([MarshalAs(UnmanagedType.AnsiBStr)] string s);

        /// <summary>
        /// Varcall
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Returns void.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_varcall", CallingConvention = CallingConvention.StdCall)]
        public static extern void Varcall(uint m);

        /// <summary>
        /// Version
        /// </summary>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_version", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public static extern string Version();

        /// <summary>
        /// View
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Returns string.</returns>
        [DllImport("oxygen.dll", EntryPoint = "o2_view", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public static extern string View([MarshalAs(UnmanagedType.AnsiBStr)] string s);
    }
}

// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2026 Jiowcl. All rights reserved.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace OxygenBasic.NET.Core.Tests
{
    /// <summary>
    /// oxygen.dll keeps process-wide compile state; tests must not run in parallel.
    /// <c>o2_abst</c> switches the engine into abstract/assembler view and must run last.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class OxygenbasicTests
    {
        [TestMethod]
        public void VersionTest()
        {
            string result = Oxygenbasic.Version();

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public void SupportsCurrentProcessTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.IsFalse(Oxygenbasic.SupportsCurrentProcess);
                Assert.ThrowsExactly<PlatformNotSupportedException>(
                    () => Oxygenbasic.ThrowIfProcessNotSupported());
                Assert.ThrowsExactly<PlatformNotSupportedException>(
                    () => Oxygenbasic.Version());
            }
            else
            {
                Assert.IsTrue(Oxygenbasic.SupportsCurrentProcess);
                Oxygenbasic.ThrowIfProcessNotSupported();
                Assert.AreEqual("oxygen.dll", Oxygenbasic.NativeLibraryFileName);
            }
        }

        [TestMethod]
        public void LibTest()
        {
            IntPtr result = Oxygenbasic.Lib();

            Assert.AreNotEqual(IntPtr.Zero, result);
        }

        [TestMethod]
        public void InitHostTest()
        {
            Oxygenbasic.InitHost();
            Oxygenbasic.Mode(Enums.Mode.Bstring);

            string version = Oxygenbasic.Version();

            Assert.IsFalse(string.IsNullOrWhiteSpace(version));
        }

        [TestMethod]
        public void O2BasicTest()
        {
            IntPtr result = Oxygenbasic.O2Basic("int a = 1234");

            Assert.AreNotEqual(IntPtr.Zero, result);
            Assert.AreEqual(0, Oxygenbasic.Errno());
        }

        [TestMethod]
        public void ExecTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.InitHost();
            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode(Enums.Mode.Asciiz);

            IntPtr result = Oxygenbasic.Exec();

            Assert.AreNotEqual(IntPtr.Zero, result);
            Assert.AreEqual(0, Oxygenbasic.Errno());
        }

        [TestMethod]
        public void BufTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode(Enums.Mode.Asciiz);

            IntPtr result = Oxygenbasic.Buf(0);

            Assert.AreNotEqual(IntPtr.Zero, result);
        }

        [TestMethod]
        public void ErrnoTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode(Enums.Mode.Asciiz);

            Assert.AreEqual(0, Oxygenbasic.Errno());
        }

        [TestMethod]
        public void ErrorTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode(Enums.Mode.Asciiz);

            string result = null;

            if (Oxygenbasic.Errno() != 0)
            {
                result = Oxygenbasic.Error();
            }

            Assert.IsNull(result);
        }

        [TestMethod]
        public void LenTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode(Enums.Mode.Asciiz);

            int result = Oxygenbasic.Len();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void LinkTest()
        {
            IntPtr result = Oxygenbasic.Link("int a = 1234");

            Assert.AreNotEqual(IntPtr.Zero, result);
        }

        [TestMethod]
        public void EvalTest()
        {
            IntPtr result = Oxygenbasic.Eval("int a = 1234");

            Assert.AreNotEqual(IntPtr.Zero, result);
        }

        [TestMethod]
        public void PrepTest()
        {
            string result = Oxygenbasic.Prep("int a = 1234");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ViewTest()
        {
            string result = Oxygenbasic.View("int a = 1234");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PathcallResolverTest()
        {
            Oxygenbasic.ClearHostCallbacks();
            Oxygenbasic.InitHost();

            OxygenPathResolver resolver = path => path;
            Oxygenbasic.Pathcall(resolver);

            Oxygenbasic.ClearHostCallbacks();
            Assert.IsNotNull(resolver);
        }

        [TestMethod]
        public void VarcallResolverTest()
        {
            Oxygenbasic.ClearHostCallbacks();
            Oxygenbasic.InitHost();

            OxygenVarResolver resolver = name => IntPtr.Zero;
            Oxygenbasic.Varcall(resolver);

            Oxygenbasic.ClearHostCallbacks();
            Assert.IsNotNull(resolver);
        }

        [TestMethod]
        public void RunTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            OxygenRunResult result = Oxygenbasic.Run(scriptBuffer);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Errno);
            Assert.AreEqual(OxygenRunStage.None, result.FailedStage);
            Assert.AreNotEqual(IntPtr.Zero, result.Code);
        }

        [TestMethod]
        public void RunFileTest()
        {
            OxygenRunResult result = Oxygenbasic.RunFile(@"Sample\test.txt");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Errno);
        }

        [TestMethod]
        public void RunCompileErrorTest()
        {
            OxygenException ex = Assert.ThrowsExactly<OxygenException>(
                () => Oxygenbasic.Run("this is not valid oxygen source !!!"));

            Assert.AreEqual(OxygenRunStage.Compile, ex.Stage);
            Assert.AreNotEqual(0, ex.Errno);
        }

        [TestMethod]
        public void RunCompileErrorNoThrowTest()
        {
            OxygenRunResult result = Oxygenbasic.Run(
                "this is not valid oxygen source !!!",
                new OxygenHostOptions { ThrowOnError = false });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(OxygenRunStage.Compile, result.FailedStage);
            Assert.AreNotEqual(0, result.Errno);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Error));
        }

        /// <summary>
        /// Must run after compile/exec tests: <c>o2_abst</c> leaves oxygen.dll in abstract mode.
        /// </summary>
        [TestMethod]
        public void ZzzAbstTest()
        {
            Assert.IsFalse(Oxygenbasic.IsAbstractMode);

            string result = Oxygenbasic.Abst("int a = 1234");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.IsTrue(Oxygenbasic.IsAbstractMode);

            InvalidOperationException ex = Assert.ThrowsExactly<InvalidOperationException>(
                () => Oxygenbasic.O2Basic("int a = 1234"));

            Assert.IsTrue(ex.Message.Contains("abstract", StringComparison.OrdinalIgnoreCase));

            Assert.ThrowsExactly<InvalidOperationException>(
                () => Oxygenbasic.Run("int a = 1234"));
        }
    }
}

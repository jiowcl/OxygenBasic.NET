// OxygenBasic.NET - OxygenBasic Programming Language for .NET
// Copyright (c) 2019-2024 Jiowcl. All rights reserved.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OxygenBasic.NET.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OxygenBasic.NET.Core.Tests
{
    [TestClass()]
    public class OxygenbasicTests
    {
        [TestMethod()]
        public void AbstTest()
        {
            string result = Oxygenbasic.Abst("int a = 1234");

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void O2BasicTest()
        {
            uint result = Oxygenbasic.O2Basic("int a = 1234");

            Assert.IsTrue(result > 0);
        }

        [TestMethod()]
        public void ExecTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int)Enums.Mode.Asciiz);

            uint result = Oxygenbasic.Exec();

            Assert.IsTrue(result > 0);
        }

        [TestMethod()]
        public void BufTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int)Enums.Mode.Asciiz);

            uint result = Oxygenbasic.Buf(0);

            Assert.IsTrue(result > 0);
        }

        [TestMethod()]
        public void ErrnoTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int)Enums.Mode.Asciiz);

            int result = Oxygenbasic.Errno();

            Assert.IsTrue(result == 0);
        }

        [TestMethod()]
        public void ErrorTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int)Enums.Mode.Asciiz);

            string result = null;

            if (Oxygenbasic.Errno() != 0)
            {
                result = Oxygenbasic.Error();
            }

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void LenTest()
        {
            string scriptPath = @"Sample\test.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int)Enums.Mode.Asciiz);

            int result = Oxygenbasic.Len();

            Assert.IsTrue(result > 0);
        }

        [TestMethod()]
        public void LibTest()
        {
            uint result = Oxygenbasic.Lib();

            Assert.IsTrue(result > 0);
        }
    }
}
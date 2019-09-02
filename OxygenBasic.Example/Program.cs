using OxygenBasic.NET.Core;
using System;
using System.IO;
using System.Text;

namespace OxygenBasic.Example
{
    /// <summary>
    /// Program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns void.</returns>
        static void Main(string[] args)
        {
            string scriptPath = @"Sample\test_fib.txt";
            string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Exec();

            Console.ReadKey();
        }
    }
}

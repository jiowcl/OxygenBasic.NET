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

            Console.WriteLine("O2 Version:" + Oxygenbasic.Version());

            Oxygenbasic.O2Basic(scriptBuffer);
            Oxygenbasic.Mode((int) Enums.Mode.Asciiz);

            if (Oxygenbasic.Errno() == 0)
            {
                Oxygenbasic.Exec();
            }
            else
            {
                Console.WriteLine("Error: " + Oxygenbasic.Error());
            }

            Console.ReadKey();
        }
    }
}

using System;
using EXETester.Models;

namespace EXETester
{
    class Program
    {

        static void Main(string[] args)
        {
            string exec_file = args[0];
            string argFile = args[1];
            var tester = new Tester(exec_file, argFile);
            //var tester = new Tester(@"C:\Users\wla0001\source\repos\console1\ConsoleApplication1\Debug\ConsoleApplication1.exe",
            //                        @"C:\Users\wla0001\Downloads\testArgs.txt");
            tester.Execute();

        }
    }
}

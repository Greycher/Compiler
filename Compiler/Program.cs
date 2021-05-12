using System;
using System.Collections.Generic;

namespace Compiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Pls enter empty line after writing code to compile it.");
            List<string> sourceCode = new List<string>();
            string line;
            do
            {
                line = Console.ReadLine().Trim();
                sourceCode.Add(line);
            } while (line != String.Empty);
            
            Console.WriteLine("Compiling...");
        
            Compiler compiler = new Compiler();
            compiler.Compile(sourceCode);
        }
    }
}
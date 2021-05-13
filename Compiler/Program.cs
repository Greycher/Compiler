using System;
using System.Collections.Generic;

namespace Compiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Pls enter empty line after writing code to compile it.");
                List<string> sourceCode = new List<string>();
                string line;
                do
                {
                    line = Console.ReadLine().Trim();
                    sourceCode.Add(line);
                } while (line != String.Empty);
                
                //Removing empty line which starts the compiling process
                sourceCode.RemoveAt(sourceCode.Count - 1);
                
                if (sourceCode.Count == 0)
                {
                    Console.WriteLine("There is no source code to compile.");
                    continue;
                }
            
                Console.WriteLine("Compiling...");
        
                Compiler compiler = new Compiler();
                compiler.Compile(sourceCode);
            }
        }
    }
}
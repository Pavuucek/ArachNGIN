using ArachNGIN.Tracer;
using ArachNGIN.Tracer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracerTestDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Tracer.InitDefault();
            Tracer.Trace();
            Tracer.Trace("Simple message test");
            Tracer.Trace(TracerLevel.Warning, "Warning test");
            Tracer.Trace<Program>("Class test");
            Tracer.Trace<Program>(TracerLevel.Warning, "Warning class test");

            // explicitly set tracer level
            Tracer.Level = TracerLevel.Error;
            Tracer.Trace(TracerLevel.Warning, "This message will not show!");
            Tracer.Level = Tracer.DefaultLevel;

            Console.WriteLine("Test over! Press any key.");
            Console.ReadKey();
        }
    }
}
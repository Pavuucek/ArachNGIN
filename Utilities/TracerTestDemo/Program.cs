/*
 * Copyright (c) 2006-2016 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using ArachNGIN.Tracer;
using ArachNGIN.Tracer.Helpers;
using System;

namespace TracerTestDemo
{
    /// <summary>
    ///     Test program
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Main function.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.Exception">Exception Test</exception>
        private static void Main(string[] args)
        {
            Tracer.InitDefault();
            Tracer.Trace();
            Tracer.Trace("Simple message test");
            Tracer.Trace(TracerLevel.Warning, "Warning test");
            Tracer.Trace<Program>("Class test");
            Tracer.Trace<Program>(TracerLevel.Warning, "Warning class test");

            // explicitly set tracer level
            Tracer.CurrentLevel = TracerLevel.Error;
            Tracer.Trace(TracerLevel.Warning, "This message will not show!");
            Tracer.CurrentLevel = Tracer.DefaultLevel;

            try
            {
                throw new Exception("Exception Test");
            }
            catch (Exception exception)
            {
                Tracer.Trace(exception);
                Tracer.Trace<Program>(exception);
            }

            Console.WriteLine("Test over! Press any key.");
            Console.ReadKey();
        }
    }
}
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

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ArachNGIN.Tracer.Helpers
{
    internal static class HelperMethods
    {
        /// <summary>
        ///     Gets the calling method base.
        /// </summary>
        /// <param name="stackFrame">The stack frame.</param>
        /// <returns></returns>
        internal static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod()
                : stackFrame.GetMethod();
        }

        /// <summary>
        ///     Finds the stack frame.
        /// </summary>
        /// <returns></returns>
        internal static StackFrame FindStackFrame()
        {
            var stackTrace = new StackTrace();
            for (var i = 0; i < stackTrace.GetFrames().Count(); i++)
            {
                var methodBase = stackTrace.GetFrame(i).GetMethod();
                var name = MethodBase.GetCurrentMethod().Name;
                if (!methodBase.Name.Equals("Trace") && !methodBase.Name.Equals(name))
                    return new StackFrame(i, true);
            }
            return null;
        }

        /// <summary>
        ///     Gets the full name of the application.
        /// </summary>
        /// <returns></returns>
        internal static string GetAppFullName()
        {
            return Assembly.GetEntryAssembly().Location;
        }

        /// <summary>
        ///     Gets the name of the application executable.
        /// </summary>
        /// <returns></returns>
        internal static string GetAppExeName()
        {
            return Path.GetFileName(GetAppFullName());
        }

        /// <summary>
        ///     Gets the application executable name without extension.
        /// </summary>
        /// <returns></returns>
        internal static string GetAppExeNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(GetAppFullName());
        }
    }
}
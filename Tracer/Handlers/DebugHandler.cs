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
using ArachNGIN.Tracer.MessageFormat;

namespace ArachNGIN.Tracer.Handlers
{
    /// <summary>
    ///     Handler for debug output
    /// </summary>
    /// <seealso cref="ArachNGIN.Tracer.ITracerHandler" />
    public class DebugHandler : ITracerHandler
    {
        private readonly IMessageFormat _messageFormat;

        /// <summary>
        ///     Creates a DebugHandler with message format
        /// </summary>
        /// <param name="messageFormat"></param>
        public DebugHandler(IMessageFormat messageFormat)
        {
            _messageFormat = messageFormat;
        }

        /// <summary>
        ///     Creates a DebugHandler with default message format
        /// </summary>
        public DebugHandler() : this(
#if (DEBUG)
            new DebugMessageFormat()
#else
            new DefaultMessageFormat()
#endif
        )
        {
        }

        /// <summary>
        ///     Trace function with debug output.
        /// </summary>
        /// <param name="tracerMessage">The tracer message.</param>
        public void Trace(TracerMessage tracerMessage)
        {
            Debug.WriteLine(_messageFormat.Format(tracerMessage));
        }
    }
}
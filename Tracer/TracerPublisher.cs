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

using System.Collections.Generic;

namespace ArachNGIN.Tracer
{
    /// <summary>
    ///     Tracer publisher class
    /// </summary>
    internal class TracerPublisher
    {
        private readonly IList<ITracerHandler> _tracerHandlers;
        private readonly IList<TracerMessage> _tracerMessages;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TracerPublisher" /> class.
        /// </summary>
        public TracerPublisher()
        {
            _tracerMessages = new List<TracerMessage>();
            _tracerHandlers = new List<ITracerHandler>();
        }

        /// <summary>
        ///     Gets the messages.
        /// </summary>
        /// <value>
        ///     The messages.
        /// </value>
        public IEnumerable<TracerMessage> Messages => _tracerMessages;

        /// <summary>
        ///     Traces the specified tracer message.
        /// </summary>
        /// <param name="tracerMessage">The tracer message.</param>
        public void Trace(TracerMessage tracerMessage)
        {
            _tracerMessages.Add(tracerMessage);
            foreach (var tracerHandler in _tracerHandlers)
                tracerHandler.Trace(tracerMessage);
        }

        /// <summary>
        ///     Adds the handler.
        /// </summary>
        /// <param name="tracerHandler">The tracer handler.</param>
        /// <returns></returns>
        public bool AddHandler(ITracerHandler tracerHandler)
        {
            if (tracerHandler != null)
            {
                _tracerHandlers.Add(tracerHandler);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Removes the handler.
        /// </summary>
        /// <param name="tracerHandler">The tracer handler.</param>
        /// <returns></returns>
        public bool RemoveHandler(ITracerHandler tracerHandler)
        {
            return _tracerHandlers.Remove(tracerHandler);
        }
    }
}
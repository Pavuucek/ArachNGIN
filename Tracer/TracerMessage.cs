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

using System;
using ArachNGIN.Tracer.Helpers;

namespace ArachNGIN.Tracer
{
    /// <summary>
    ///     Tracer Message class
    /// </summary>
    public class TracerMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TracerMessage" /> class.
        /// </summary>
        public TracerMessage()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TracerMessage" /> class.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="tracerLevel">The tracer level.</param>
        /// <param name="originClass">The origin class.</param>
        /// <param name="originFunction">The origin function.</param>
        /// <param name="originLine">The origin line.</param>
        /// <param name="message">The message.</param>
        public TracerMessage(DateTime timeStamp, TracerLevel tracerLevel, string originClass, string originFunction,
            int originLine, string message)
        {
            TimeStamp = timeStamp;
            Level = tracerLevel;
            OriginClass = originClass;
            OriginFunction = originFunction;
            OriginLine = originLine;
            Message = message;
        }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>
        ///     The time stamp.
        /// </value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public TracerLevel Level { get; set; }

        /// <summary>
        ///     Gets or sets the origin class.
        /// </summary>
        /// <value>
        ///     The origin class.
        /// </value>
        public string OriginClass { get; set; }

        /// <summary>
        ///     Gets or sets the origin function.
        /// </summary>
        /// <value>
        ///     The origin function.
        /// </value>
        public string OriginFunction { get; set; }

        /// <summary>
        ///     Gets or sets the origin line.
        /// </summary>
        /// <value>
        ///     The origin line.
        /// </value>
        public int OriginLine { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message { get; set; }
    }
}
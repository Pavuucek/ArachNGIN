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
using ArachNGIN.Tracer.Handlers;
using ArachNGIN.Tracer.Helpers;

namespace ArachNGIN.Tracer
{
    /// <summary>
    ///     Tracer class
    /// </summary>
    public static class Tracer
    {
        /// <summary>
        ///     The default tracer level
        /// </summary>
        public const TracerLevel DefaultLevel = TracerLevel.Info;

        private static readonly TracerPublisher TracerPublisher;

        /// <summary>
        ///     The current tracer level
        /// </summary>
        public static TracerLevel CurrentLevel = TracerLevel.Info;

        private static readonly object SyncLock = new object();

        static Tracer()
        {
            lock (SyncLock)
            {
                TracerPublisher = new TracerPublisher();
            }
        }

        /// <summary>
        ///     Default initialization.
        /// </summary>
        public static void InitDefault()
        {
            TracerPublisher.AddHandler(new FileHandler());
            TracerPublisher.AddHandler(new ConsoleHandler());
            TracerPublisher.AddHandler(new DebugHandler());
        }

        /// <summary>
        ///     Posts an introduction message to trace.
        /// </summary>
        public static void TraceIntroMessage()
        {
            Trace($"Starting {HelperMethods.GetAppExeName()}...");
        }

        /// <summary>
        ///     Trace function.
        /// </summary>
        /// <param name="messageLevel">The message level.</param>
        /// <param name="originClass">The origin class.</param>
        /// <param name="originMethod">The origin method.</param>
        /// <param name="line">The line.</param>
        /// <param name="message">The message.</param>
        public static void Trace(TracerLevel messageLevel, string originClass, string originMethod, int line,
            string message)
        {
            if (messageLevel < CurrentLevel) return; // do nothing if we're at lesser level
            var tracerMessage = new TracerMessage(DateTime.Now, messageLevel, originClass, originMethod, line, message);
            TracerPublisher.Trace(tracerMessage);
        }

        /// <summary>
        ///     Trace function with class specified.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="messageLevel">The message level.</param>
        /// <param name="message">The message.</param>
        public static void Trace<TClass>(TracerLevel messageLevel, string message) where TClass : class
        {
            var stackFrame = HelperMethods.FindStackFrame();
            var methodBase = HelperMethods.GetCallingMethodBase(stackFrame);
            var originMethod = methodBase.Name;
            var originClass = typeof(TClass).Name;
            var line = stackFrame.GetFileLineNumber();
            Trace(messageLevel, originClass, originMethod, line, message);
        }

        /// <summary>
        ///     Trace function with class specified.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="message">The message.</param>
        public static void Trace<TClass>(string message) where TClass : class
        {
            Trace<TClass>(DefaultLevel, message);
        }

        /// <summary>
        ///     Exception trace function with class specified.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="exception">The exception.</param>
        public static void Trace<TClass>(Exception exception) where TClass : class
        {
            var msg = $"Exception !\nMessage:\n{exception.Message}\nStackTrace:\n{exception.StackTrace}";
            Trace<TClass>(TracerLevel.Error, msg);
        }

        /// <summary>
        ///     Exception trace function.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Trace(Exception exception)
        {
            Trace(TracerLevel.Error, exception.Message);
        }

        /// <summary>
        ///     Trace function.
        /// </summary>
        /// <param name="tracerLevel">The tracer level.</param>
        /// <param name="message">The message.</param>
        public static void Trace(TracerLevel tracerLevel, string message)
        {
            var stackFrame = HelperMethods.FindStackFrame();
            var methodBase = HelperMethods.GetCallingMethodBase(stackFrame);
            var originMethod = methodBase.Name;
            if (methodBase.ReflectedType == null) return;
            var originClass = methodBase.ReflectedType.Name;
            var line = stackFrame.GetFileLineNumber();
            Trace(tracerLevel, originClass, originMethod, line, message);
        }

        /// <summary>
        ///     Trace function.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Trace(string message)
        {
            Trace(DefaultLevel, message);
        }

        /// <summary>
        ///     Trace test function.
        /// </summary>
        public static void Trace()
        {
            Trace("Trace test!");
        }

        /// <summary>
        ///     Adds the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static bool AddHandler(ITracerHandler handler)
        {
            return TracerPublisher.AddHandler(handler);
        }

        /// <summary>
        ///     Removes the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static bool RemoveHandler(ITracerHandler handler)
        {
            return TracerPublisher.RemoveHandler(handler);
        }
    }
}
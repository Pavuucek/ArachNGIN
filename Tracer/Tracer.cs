using ArachNGIN.Tracer.Handlers;
using ArachNGIN.Tracer.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArachNGIN.Tracer
{
    public static class Tracer
    {
        private static TracerPublisher tracerPublisher;
        public static readonly TracerLevel DefaultLevel = TracerLevel.Info;
        public static TracerLevel Level = TracerLevel.Info;
        private static object SyncLock = new object();

        static Tracer()
        {
            lock (SyncLock)
            {
                tracerPublisher = new TracerPublisher();
            }
        }

        public static void InitDefault()
        {
            tracerPublisher.AddHandler(new ConsoleHandler());
            tracerPublisher.AddHandler(new DebugHandler());
        }

        public static void Trace(TracerLevel messageLevel, string originClass, string originMethod, int line,
            string message)
        {
            if (messageLevel < Level) return; // do nothing if we're at lesser level
            var tracerMessage = new TracerMessage(DateTime.Now, messageLevel, originClass, originMethod, line, message);
            tracerPublisher.Trace(tracerMessage);
        }

        public static void Trace<TClass>(TracerLevel messageLevel, string message) where TClass : class
        {
            var stackFrame = HelperMethods.FindStackFrame();
            var methodBase = HelperMethods.GetCallingMethodBase(stackFrame);
            var originMethod = methodBase.Name;
            var originClass = typeof(TClass).Name;
            var line = stackFrame.GetFileLineNumber();
            Trace(messageLevel, originClass, originMethod, line, message);
        }

        public static void Trace<TClass>(string message) where TClass : class
        {
            Trace<TClass>(DefaultLevel, message);
        }

        public static void Trace<TClass>(Exception exception) where TClass : class
        {
            var msg = string.Format("Exception !\nMessage: {0}\nStackTrace: {1}", exception.Message,
                exception.StackTrace);
            Trace<TClass>(TracerLevel.Error, msg);
        }

        public static void Trace(Exception exception)
        {
            Trace(TracerLevel.Error, exception.Message);
        }

        public static void Trace(TracerLevel tracerLevel, string message)
        {
            var stackFrame = HelperMethods.FindStackFrame();
            var methodBase = HelperMethods.GetCallingMethodBase(stackFrame);
            var originMethod = methodBase.Name;
            var originClass = methodBase.ReflectedType.Name;
            var line = stackFrame.GetFileLineNumber();
            Trace(tracerLevel, originClass, originMethod, line, message);
        }

        public static void Trace(string message)
        {
            Trace(DefaultLevel, message);
        }

        public static void Trace()
        {
            Trace("Trace test!");
        }
    }
}
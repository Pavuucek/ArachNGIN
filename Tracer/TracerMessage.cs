using ArachNGIN.Tracer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace ArachNGIN.Tracer
{
    public class TracerMessage
    {
        public DateTime TimeStamp { get; set; }
        public TracerLevel Level { get; set; }
        public string OriginClass { get; set; }
        public string OriginFunction { get; set; }
        public int OriginLine { get; set; }
        public string Message { get; set; }

        public TracerMessage()
        {
        }

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

        public override string ToString()
        {
            return string.Format("{0:dd.MM.yyyy HH:mm:ss}: {1} [{2}.{3} -> line:{4}()]: {5}", TimeStamp, Level,
                OriginClass, OriginFunction, OriginLine, Message);
        }
    }
}
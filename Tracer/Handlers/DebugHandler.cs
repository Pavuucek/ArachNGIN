using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tracer.Handlers
{
    public class DebugHandler : ITracerHandler
    {
        public void Trace(TracerMessage tracerMessage)
        {
            System.Diagnostics.Debug.WriteLine(tracerMessage.ToString());
        }
    }
}
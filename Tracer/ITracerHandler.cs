using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tracer
{
    public interface ITracerHandler
    {
        void Trace(TracerMessage tracerMessage);
    }
}
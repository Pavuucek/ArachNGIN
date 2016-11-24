using System;

namespace ArachNGIN.Tracer.Handlers
{
    public class ConsoleHandler : ITracerHandler
    {
        public void Trace(TracerMessage tracerMessage)
        {
            Console.WriteLine(tracerMessage.ToString());
        }
    }
}
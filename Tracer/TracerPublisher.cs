using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArachNGIN.Tracer
{
    internal class TracerPublisher
    {
        private IList<TracerMessage> _TracerMessages;
        private IList<ITracerHandler> _TracerHandlers;

        public TracerPublisher()
        {
            _TracerMessages = new List<TracerMessage>();
            _TracerHandlers = new List<ITracerHandler>();
        }

        public void Trace(TracerMessage tracerMessage)
        {
            _TracerMessages.Add(tracerMessage);
            foreach (var tracerHandler in _TracerHandlers)
            {
                tracerHandler.Trace(tracerMessage);
            }
        }

        public IEnumerable<TracerMessage> Messages
        {
            get { return _TracerMessages; }
        }
    }
}
using System;

namespace ArachNGIN.KumoScript
{
    public class KumoScriptException: Exception
    {
        public KumoScriptException(string message)
            : base(message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

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

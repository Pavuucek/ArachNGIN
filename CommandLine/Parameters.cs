using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ArachNGIN.CommandLine
{
    public class Parameters
    {
        private readonly StringDictionary _dict = new StringDictionary();

        public Parameters(IEnumerable<string> args)
        {
            var splitter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string param = null;
            foreach (var s in args)
            {
                var parts = splitter.Split(s, 3);
                switch (parts.Length)
                {
                    case 1:
                        if (param != null)
                        {
                            if (_dict.ContainsKey(param))
                            {
                                parts[0] = remover.Replace(parts[0], "$1");
                                _dict.Add(param, parts[0]);
                            }
                            param = null;
                        }
                        break;

                    case 2:
                        if (param != null)
                        {
                            if (!_dict.ContainsKey(param)) _dict.Add(param, "true");
                        }
                        param = parts[1];
                        break;

                    case 3:
                        if (param != null)
                        {
                            if (!_dict.ContainsKey(param)) _dict.Add(param, "true");
                        }
                        param = parts[1];
                        if (!_dict.ContainsKey(param))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            _dict.Add(param, parts[2]);
                        }
                        param = null;
                        break;
                }
                if (param != null)
                {
                    if (!_dict.ContainsKey(param)) _dict.Add(param, "true");
                }
            }
        }

        public string this[string param]
        {
            get
            {
                return _dict[param];
            }
        }
    }
}
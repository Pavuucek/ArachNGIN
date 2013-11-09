using System;

namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Tøída parametrù události OnCommandEntered
    /// </summary>
    public class CommandEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// Konstruktor tøídy události OnCommandEntered
        /// </summary>
        /// <param name="cmd">pøíkaz (1 slovo)</param>
        /// <param name="parArray">parametry (ostatní slova) jako pole</param>
        /// <param name="parString">parametry (ostatní slova) jako øetìzec</param>
        /// <returns></returns>
        public CommandEnteredEventArgs(string cmd, string[] parArray, string parString )
        {
            _parametry = parArray;
            _prikaz = cmd;
            _parametryStr = parString;
        }

        readonly string _prikaz;
        /// <summary>
        /// Pøíkaz konzole
        /// </summary>
        public string Command
        {
            get
            {
                return _prikaz;
            }
        }

        readonly string[] _parametry;
        /// <summary>
        /// Parametry pøíkazu jako pole
        /// </summary>
        public string[] ParamArray
        {
            get
            {
                return _parametry;
            }
        }

        readonly string _parametryStr;
        /// <summary>
        /// Parametry pøíkazu jako øetìzec
        /// </summary>
        public string ParamString
        {
            get
            {
                return _parametryStr;
            }
        }
    }
}
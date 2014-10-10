using System;

namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// Třída parametrů události OnCommandEntered
    /// </summary>
    public class CommandEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// Konstruktor třídy události OnCommandEntered
        /// </summary>
        /// <param name="cmd">příkaz (1 slovo)</param>
        /// <param name="parArray">parametry (ostatní slova) jako pole</param>
        /// <param name="parString">parametry (ostatní slova) jako řetězec</param>
        /// <returns></returns>
        public CommandEnteredEventArgs(string cmd, string[] parArray, string parString )
        {
            _parametry = parArray;
            _prikaz = cmd;
            _parametryStr = parString;
        }

        readonly string _prikaz;
        /// <summary>
        /// Příkaz konzole
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
        /// Parametry příkazu jako pole
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
        /// Parametry příkazu jako řetězec
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
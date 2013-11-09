using System;

namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    /// T��da parametr� ud�losti OnCommandEntered
    /// </summary>
    public class CommandEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// Konstruktor t��dy ud�losti OnCommandEntered
        /// </summary>
        /// <param name="cmd">p��kaz (1 slovo)</param>
        /// <param name="parArray">parametry (ostatn� slova) jako pole</param>
        /// <param name="parString">parametry (ostatn� slova) jako �et�zec</param>
        /// <returns></returns>
        public CommandEnteredEventArgs(string cmd, string[] parArray, string parString )
        {
            _parametry = parArray;
            _prikaz = cmd;
            _parametryStr = parString;
        }

        readonly string _prikaz;
        /// <summary>
        /// P��kaz konzole
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
        /// Parametry p��kazu jako pole
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
        /// Parametry p��kazu jako �et�zec
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
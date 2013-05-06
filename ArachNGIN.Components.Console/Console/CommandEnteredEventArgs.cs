using System;

namespace ArachNGIN.Components.Console.Console
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
            parametry = parArray;
            prikaz = cmd;
            parametry_str = parString;
        }
		
        string prikaz;
        /// <summary>
        /// Pøíkaz konzole
        /// </summary>
        public string Command
        {
            get
            {
                return prikaz;
            }
        }
		
        string[] parametry;
        /// <summary>
        /// Parametry pøíkazu jako pole
        /// </summary>
        public string[] ParamArray
        {
            get
            {
                return parametry;
            }
        }
		
        string parametry_str;
        /// <summary>
        /// Parametry pøíkazu jako øetìzec
        /// </summary>
        public string ParamString
        {
            get
            {
                return parametry_str;
            }
        }
    }
}
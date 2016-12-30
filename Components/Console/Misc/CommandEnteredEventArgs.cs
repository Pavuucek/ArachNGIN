using System;

namespace ArachNGIN.Components.Console.Misc
{
    /// <summary>
    ///     Class for handling CommandEntered event
    /// </summary>
    public class CommandEnteredEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandEnteredEventArgs" /> class.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="parArray">The parameter array.</param>
        /// <param name="parString">The parameter string.</param>
        public CommandEnteredEventArgs(string cmd, string[] parArray, string parString)
        {
            ParamArray = parArray;
            Command = cmd;
            ParamString = parString;
        }

        /// <summary>
        ///     Gets the command.
        /// </summary>
        /// <value>
        ///     The command.
        /// </value>
        public string Command { get; }

        /// <summary>
        ///     Gets the parameter array.
        /// </summary>
        /// <value>
        ///     The parameter array.
        /// </value>
        public string[] ParamArray { get; }

        /// <summary>
        ///     Gets the parameter string.
        /// </summary>
        /// <value>
        ///     The parameter string.
        /// </value>
        public string ParamString { get; }
    }
}
using System;
using System.Collections.Generic;

namespace ArachNGIN.KumoScript
{
    /// <summary>
    /// Main execution block, named block or anonymous statement block.
    /// </summary>
    public class ScriptBlock
    {
        #region Private Variables

        private readonly Script _mScript;
        private readonly String _mStrName;
        private readonly List<Statement> _mListStatements;

        #endregion

        #region Internal Properties

        internal List<Statement> Statements
        {
            get { return _mListStatements; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs a script block for the given script and with
        /// the given name.
        /// </summary>
        /// <param name="script">Script containing the block.</param>
        /// <param name="strName">Name of the block.</param>
        public ScriptBlock(Script script, String strName)
        {
            _mScript = script;
            _mStrName = strName;
            _mListStatements = new List<Statement>();
        }

        /// <summary>
        /// Returns a string representation of the script block.
        /// </summary>
        /// <returns>String representation of the script block</returns>
        public override string ToString()
        {
            return "Block: " + _mStrName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Script containing the block.
        /// </summary>
        public Script Script
        {
            get { return _mScript; }
        }

        /// <summary>
        /// Name of the script block.
        /// </summary>
        public String Name
        {
            get { return _mStrName; }
        }

        #endregion
    }
}

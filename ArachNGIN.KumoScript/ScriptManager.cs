using System;
using System.Collections.Generic;

namespace ArachNGIN.KumoScript
{
    /// <summary>
    /// Manager class for defining a script environment and global
    /// variable scope.
    /// </summary>
    public class ScriptManager
    {
        #region Private Variables

        private IScriptLoader _mScriptLoader;
        private readonly VariableDictionary _mVariableDictionary;
        private readonly Dictionary<String, CommandPrototype> _mDictCommandPrototypes;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs a new script manager.
        /// </summary>
        public ScriptManager()
        {
            _mScriptLoader = new ScriptLoaderDefault();
            _mVariableDictionary = new VariableDictionary(null);
            _mDictCommandPrototypes = new Dictionary<string, CommandPrototype>();
        }

        /// <summary>
        /// Registers a new custom command using the given prototype.
        /// </summary>
        /// <param name="commandPrototype">Prototype for the command.</param>
        public void RegisterCommand(CommandPrototype commandPrototype)
        {
            String strKey = commandPrototype.Name.ToUpper();
            if (_mDictCommandPrototypes.ContainsKey(strKey))
                throw new ScriptException(
                    "Command " + commandPrototype.Name + " already registered.");

            _mDictCommandPrototypes[strKey] = commandPrototype;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Script loader bound to the manager. The default loader
        /// retrieves scripts from disk.
        /// </summary>
        public IScriptLoader Loader
        {
            get { return _mScriptLoader; }
            set { _mScriptLoader = value; }
        }

        /// <summary>
        /// Command prototypes registered with the script manager.
        /// </summary>
        public Dictionary<String, CommandPrototype> CommandPrototypes
        {
            get { return _mDictCommandPrototypes; }
        }

        /// <summary>
        /// Global variables currently set in the script manager.
        /// </summary>
        public VariableDictionary GlobalVariables
        {
            get { return _mVariableDictionary; }
        }

        #endregion
    }
}

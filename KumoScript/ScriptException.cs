using System;

namespace ArachNGIN.KumoScript
{
    /// <summary>
    /// The API's exception class for script compilation, runtime
    /// and host usage errors.
    /// </summary>
    public class ScriptException
        : Exception
    {
        #region Private Variables

        private readonly String _mStrMessage;
        private readonly Exception _mExceptionInner;
        private readonly Statement _mStatement;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs an exception without any parameters.
        /// </summary>
        public ScriptException()
        {
            _mStrMessage = "";
            _mExceptionInner = null;
            _mStatement = null;
        }

        /// <summary>
        /// Constructs an eexception with the given message.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        public ScriptException(String strMessage)
        {
            _mStrMessage = strMessage;
            _mExceptionInner = null;
            _mStatement = null;
        }

        /// <summary>
        /// Constructs an exception with the given message and inner exception.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="exceptionInner">Inner exception wrapped by this exception.</param>
        public ScriptException(String strMessage, Exception exceptionInner)
        {
            _mStrMessage = strMessage;
            _mExceptionInner = exceptionInner;
            _mStatement = null;
        }

        /// <summary>
        /// Constructs an exception with the given message and script statement.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="statement">Script statement where the runtime or
        /// compilation exception occured.</param>
        public ScriptException(String strMessage, Statement statement)
        {
            _mStrMessage = strMessage;
            _mExceptionInner = null;
            _mStatement = statement;
        }

        /// <summary>
        /// Constructs an exception with the given message, script statement
        /// and inner exception.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="exceptionInner">Inner exception wrapped by this exception.</param>
        /// <param name="statement">Script statement where the runtime or
        /// compilation exception occured.</param>
        public ScriptException(String strMessage, Exception exceptionInner, Statement statement)
        {
            _mStrMessage = strMessage;
            _mExceptionInner = exceptionInner;
            _mStatement = Statement;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Exception message.
        /// </summary>
        public override String Message
        {
            get
            {
                if (_mExceptionInner == null)
                    return _mStrMessage;
                return _mStrMessage + " Reason: " + _mExceptionInner.Message;
            }
        }

        /// <summary>
        /// Inner exception wrapped by this exception.
        /// </summary>
        public new Exception InnerException
        {
            get { return _mExceptionInner; }
        }

        /// <summary>
        /// Script statement where the runtime or
        /// compilation exception occured.
        /// </summary>
        public Statement Statement
        {
            get { return _mStatement; }
        }

        #endregion
    }
}

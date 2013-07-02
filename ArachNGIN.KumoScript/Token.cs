using System;

namespace ArachNGIN.KumoScript
{
    #region Internal Enumerations

    internal enum TokenType
    {
        Identifier,
        Integer,
        Float,
        Boolean,
        String,
        Equals,
        NotEquals,
        Greater,
        Less,
        Include,
        Setglobal,
        Set,
        Add,
        Subtract,
        Multiply,
        Divide,
        To,
        From,
        By,
        If,
        Then,
        Else,
        Endif,
        While,
        Endwhile,
        Call,
        Block,
        Endblock,
        Yield,
        // added by pvk
        Break
    }

    #endregion

    internal class Token
    {
        #region Private Variables

        private readonly TokenType _mTokenType;
        private readonly object _mObjectValue;

        #endregion

        #region Public Methods

        public Token(TokenType tokenType, object objectValue)
        {
            _mTokenType = tokenType;
            _mObjectValue = objectValue;
        }

        public override String ToString()
        {
            if (_mTokenType == TokenType.String)
                return _mTokenType + " (\"" + _mObjectValue + "\")";
            return _mTokenType + " (" + _mObjectValue + ")";
        }

        #endregion

        #region Public Properties

        public TokenType Type
        {
            get { return _mTokenType; }
        }

        public object Value
        {
            get { return _mObjectValue; }
        }

        #endregion
    }
}

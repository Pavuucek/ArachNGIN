using System;
using System.Collections.Generic;

namespace ArachNGIN.KumoScript
{
    internal struct Frame
    {
        public ScriptBlock MScriptBlock;
        public int MiNextStatement;

        public Frame(ScriptBlock scriptBlock, int iNextStatement)
        {
            MScriptBlock = scriptBlock;
            MiNextStatement = iNextStatement;
        }
    }

    /// <summary>
    /// Execution context for a specific code block within a script. An
    /// instance of this class represents a running instance of the
    /// script. Multiple contexts can be created for any given script,
    /// each of which having its own state and local variable scope.
    /// </summary>
    public class ScriptContext
    {
        #region Private Variables

        private readonly ScriptBlock _mScriptBlock;
        private readonly VariableDictionary _mVariableDicitonary;
        private int _mICurrentBlockSize;
        private readonly Stack<Frame> _mStackFrame;
        private bool _mBInterrupted;
        private IScriptHandler _mScriptHandler;
        private bool _mBInterruptOnCustomCommand;

        #endregion

        #region Private Methods

        private bool CompareParameters(Token tokenParameter1,
            TokenType tokenTypeOperator, Token tokenParameter2)
        {
            var objectValue1 = ResolveValue(tokenParameter1);
            var objectValue2 = ResolveValue(tokenParameter2);

            if (tokenParameter2.Type == TokenType.Identifier)
                objectValue2 = GetVariable(tokenParameter2.Value.ToString());
            else
                objectValue2 = tokenParameter2.Value;

            if (objectValue1 is int
                && objectValue2 is int)
            {
                // pure int comparison
                int iValue1 = (int)objectValue1;
                int iValue2 = (int)objectValue2;
                switch (tokenTypeOperator)
                {
                    case TokenType.Equals:
                        return iValue1 == iValue2;
                    case TokenType.NotEquals:
                        return iValue1 != iValue2;
                    case TokenType.Greater:
                        return iValue1 > iValue2;
                    case TokenType.Less:
                        return iValue1 < iValue2;
                }
            }
            else if (objectValue1 is bool
                || objectValue2 is bool)
            {
                bool bValue1 = (bool)objectValue1;
                bool bValue2 = (bool)objectValue2;
                switch (tokenTypeOperator)
                {
                    case TokenType.Equals:
                        return bValue1 == bValue2;
                    case TokenType.NotEquals:
                        return bValue1 != bValue2;
                    default:
                        throw new ScriptException("Boolean parameters cannot be compared using > or <.");
                }
            }
            else if (objectValue1 is string
                && objectValue2 is string)
            {
                // string comparison
                String strValue1 = (String)objectValue1;
                String strValue2 = (String)objectValue2;
                switch (tokenTypeOperator)
                {
                    case TokenType.Equals:
                        return strValue1.CompareTo(strValue2) == 0;
                    case TokenType.NotEquals:
                        return strValue1.CompareTo(strValue2) != 0;
                    case TokenType.Greater:
                        return strValue1.CompareTo(strValue2) > 0;
                    case TokenType.Less:
                        return strValue1.CompareTo(strValue2) < 0;
                }
            }
            else if ((objectValue1 is float
                || objectValue1 is int)
                && (objectValue2 is float
                || objectValue2 is int))
            {
                // pure float, or mixed number comparison
                float fValue1 = (float)objectValue1;
                float fValue2 = (float)objectValue2;
                switch (tokenTypeOperator)
                {
                    case TokenType.Equals:
                        return fValue1 == fValue2;
                    case TokenType.Greater:
                        return fValue1 > fValue2;
                    case TokenType.Less:
                        return fValue1 < fValue2;
                }
            }
            else
                throw new ScriptException("Paramaters cannot be compared.");

            return false;
        }

        private object ResolveValue(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier:
                    return GetVariable(token.Value.ToString());
                case TokenType.Integer:
                case TokenType.Float:
                case TokenType.Boolean:
                case TokenType.String:
                    return token.Value;
                default:
                    throw new ScriptException("Cannot resolve token value for type " + token.Type + ".");
            }
        }

        private object GetVariable(String strIdentifier)
        {
            return _mVariableDicitonary[strIdentifier.ToUpper()];
        }

        private void SetVariable(String strIdentifier, object objectValue)
        {
            _mVariableDicitonary[strIdentifier] = objectValue;

            // if handler set, notify it
            if (_mScriptHandler != null)
                _mScriptHandler.OnScriptVariableUpdate(this, strIdentifier, objectValue);
        }

        private void ExecuteSetGlobal(Statement statement)
        {
            Token token = statement.Tokens[1];
            String strIdentifier = token.Value.ToString();

            if (!GlobalVariables.HasVariable(strIdentifier)
                && LocalVariables.HasVariable(strIdentifier))
                throw new ScriptException(
                    "Variable " + strIdentifier + " already declared locally.",
                    statement);

            object objectValue = ResolveValue(statement.Tokens[3]);

            GlobalVariables[strIdentifier] = objectValue;

            // if handler set, notify it
            if (_mScriptHandler != null)
                _mScriptHandler.OnScriptVariableUpdate(this, strIdentifier, objectValue);
        }

        private void ExecuteSet(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;
            String strIdentifier = listTokens[1].Value.ToString();
            object objectValue = ResolveValue(listTokens[3]);
            SetVariable(strIdentifier, objectValue);
        }

        private void ExecuteAdd(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            object objectToAdd = ResolveValue(listTokens[1]);

            Token tokenVariable = listTokens[3];
            String strIdentifier = tokenVariable.Value.ToString();
            object objectValue = GetVariable(strIdentifier);
            object objectResult = null;

            Type typeValue = objectValue.GetType();
            if (typeValue == typeof(bool))
                throw new ScriptException(
                    "Cannot add a value to a boolean variable.", statement);

            Type typeToAdd = objectToAdd.GetType();

            if (typeValue == typeof(int))
            {
                if (typeToAdd == typeof(int))
                    objectResult = (int)objectValue + (int)objectToAdd;
                else if (typeToAdd == typeof(float))
                    objectResult = (float)((int)objectValue + (float)objectToAdd);
                else // string
                    objectResult = (int)objectValue + (String)objectToAdd;
            }
            else if (typeValue == typeof(float))
            {
                if (typeToAdd == typeof(int))
                    objectResult = (float)((float)objectValue + (int)objectToAdd);
                else if (typeToAdd == typeof(float))
                    objectResult = (float)objectValue + (float)objectToAdd;
                else // string
                    objectResult = (float)objectValue + (String)objectToAdd;
            }
            else // string
            {
                if (typeToAdd == typeof(int))
                    objectResult = (String)objectValue + (int)objectToAdd;
                else if (typeToAdd == typeof(float))
                    objectResult = (String)objectValue + (float)objectToAdd;
                else // string
                    objectResult = (String)objectValue + (String)objectToAdd;
            }

            SetVariable(strIdentifier, objectResult);
        }

        private void ExecuteSubtract(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            object objectToSubtract = ResolveValue(listTokens[1]);

            Token tokenVariable = listTokens[3];
            String strIdentifier = tokenVariable.Value.ToString();
            object objectValue = GetVariable(strIdentifier);

            Type typeValue = objectValue.GetType();
            if (typeValue == typeof(bool)
                || typeValue == typeof(String))
                throw new ScriptException(
                    "Cannot subtract a value from a boolean or string variable.",
                    statement);

            if (objectValue is int
                && objectToSubtract is int)
                // pure int
                SetVariable(strIdentifier,
                    (int)objectValue - (int)objectToSubtract);
            else
            {
                // float or mixed
                float fValue = objectValue is float
                    ? (float)objectValue : (int)objectValue;

                float fToSubtract = objectToSubtract is float
                    ? (float)objectToSubtract : (int)objectToSubtract;

                SetVariable(strIdentifier, fValue - fToSubtract);
            }
        }

        private void ExecuteMultiply(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            Token tokenVariable = listTokens[1];
            String strIdentifier = tokenVariable.Value.ToString();
            object objectValue = GetVariable(strIdentifier);

            Type typeValue = objectValue.GetType();
            if (typeValue == typeof(bool)
                || typeValue == typeof(String))
                throw new ScriptException(
                    "Cannot multiply boolean or string variable.",
                    statement);

            object objectMultiplier = ResolveValue(listTokens[3]);

            if (objectValue.GetType() == typeof(int)
                && objectMultiplier.GetType() == typeof(int))
                // pure int
                SetVariable(strIdentifier,
                    (int)objectValue * (int)objectMultiplier);
            else
            {
                // float or mixed
                float fValue = objectValue is float
                    ? (float)objectValue : (int)objectValue;

                float fMultiplier = objectMultiplier is float
                    ? (float)objectMultiplier : (int)objectMultiplier;

                SetVariable(strIdentifier, fValue * fMultiplier);
            }
        }

        private void ExecuteDivide(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            Token tokenVariable = listTokens[1];
            String strIdentifier = tokenVariable.Value.ToString();
            object objectValue = GetVariable(strIdentifier);

            Type typeValue = objectValue.GetType();
            if (typeValue == typeof(bool)
                || typeValue == typeof(String))
                throw new ScriptException(
                    "Cannot divide boolean or string variable.",
                    statement);

            object objectDivider = ResolveValue(listTokens[3]);

            float fDivider = 0.0f;
            if (objectDivider is int)
                fDivider = (int)objectDivider;
            else
                fDivider = (float)objectDivider;

            // check for divide by zero
            if (fDivider == 0.0f)
                throw new ScriptException("Cannot divide by zero.", statement);

            if (objectValue is int
                && objectDivider is int)
                // pure int
                SetVariable(strIdentifier,
                    (int)objectValue / (int)objectDivider);
            else
            {
                // float or mixed
                float fValue = objectValue is float
                    ? (float)objectValue : (int)objectValue;

                SetVariable(strIdentifier,
                    fValue / fDivider);
            }
        }

        private void ExecuteIf(Statement statement)
        {
            Script script = _mScriptBlock.Script;

            IfBlock ifBLock = script.IfBlocks[statement];

            // update next index for current frame
            Frame frame = _mStackFrame.Pop();
            frame.MiNextStatement = ifBLock.MiNextStatement;
            _mStackFrame.Push(frame);

            // perform comparison
            ScriptBlock scriptBlock = null;
            var listTokens = statement.Tokens;

            if (listTokens.Count == 3)
            {
                // simple boolean check
                Token token = listTokens[1];

                // literal TRUE or FALSE
                if (token.Type == TokenType.Boolean)
                    scriptBlock = (bool)token.Value
                        ? ifBLock.MScriptBlockTrue
                        : ifBLock.MScriptBlockFalse;
                else // is identifier
                {
                    String strIdentifier = token.Value.ToString();
                    object objectValue = GetVariable(strIdentifier);

                    // if bool var, process according to value
                    if (objectValue is bool)
                    {
                        scriptBlock = (bool)objectValue
                            ? ifBLock.MScriptBlockTrue
                            : ifBLock.MScriptBlockFalse;
                    }
                    else
                        throw new ScriptException("Variable must contain a boolean value.", statement);
                }
            }
            else // comparison
            {
                scriptBlock
                    = CompareParameters(listTokens[1], listTokens[2].Type, listTokens[3])
                        ? ifBLock.MScriptBlockTrue
                        : ifBLock.MScriptBlockFalse;
            }

            // push block
            if (scriptBlock != null) // else part is optional
            {
                frame = new Frame(scriptBlock, 0);
                _mStackFrame.Push(frame);
            }
        }

        private void ExecuteWhile(Statement statement)
        {
            Script script = _mScriptBlock.Script;

            WhileBlock whileBlock = script.WhileBlocks[statement];

            // perform comparison
            List<Token> listTokens = statement.Tokens;
            bool bLoopContinue = false;
            if (listTokens.Count == 2)
            {
                // simple boolean check
                Token token = listTokens[1];

                // literal TRUE or FALSE
                if (token.Type == TokenType.Boolean)
                    bLoopContinue = (bool)token.Value;
                else // is identifier
                {
                    String strIdentifier = token.Value.ToString();
                    object objectValue = GetVariable(strIdentifier);

                    // if bool var, process according to value
                    if (objectValue is bool)
                        bLoopContinue = (bool)objectValue;
                    else
                        throw new ScriptException("Variable must contain a boolean value.", statement);
                }
            }
            else // comparison
                bLoopContinue = CompareParameters(
                    listTokens[1], listTokens[2].Type, listTokens[3]);

            if (bLoopContinue)
            {
                // keep statement index on while statement
                Frame frame = _mStackFrame.Pop();
                frame.MiNextStatement--;
                _mStackFrame.Push(frame);

                // push while block
                frame = new Frame(whileBlock.MScriptBlock, 0);
                _mStackFrame.Push(frame);
            }
            else
            {
                // update next index for current frame
                Frame frame = _mStackFrame.Pop();
                frame.MiNextStatement = whileBlock.MiNextStatement;
                _mStackFrame.Push(frame);
            }
        }

        private void ExecuteCall(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            String strBlockName = listTokens[1].Value.ToString().ToUpper();
            ScriptBlock scriptBlock = _mScriptBlock.Script.Blocks[strBlockName];
            var frameToCall = new Frame(scriptBlock, 0);

            _mStackFrame.Push(frameToCall);
        }

        private void ExecuteYield(Statement statement)
        {
            _mBInterrupted = true;
        }

        private void ExecuteCustom(Statement statement)
        {
            List<Token> listTokens = statement.Tokens;

            Token token = listTokens[0];

            String strCommand = token.Value.ToString();

            ScriptManager scriptManager = _mScriptBlock.Script.Manager;

            if (!scriptManager.CommandPrototypes.ContainsKey(
                strCommand.ToUpper()))
                throw new ScriptException("Command " + strCommand + " not registered.");

            CommandPrototype commandPrototype
                = scriptManager.CommandPrototypes[strCommand.ToUpper()];

            var listParameters = new List<object>();
            for (int iIndex = 1; iIndex < listTokens.Count; iIndex++)
                listParameters.Add(ResolveValue(listTokens[iIndex]));

            try
            {
                commandPrototype.VerifyParameters(listParameters);

                // set interrupt flag to trigger break in
                // continuous execution
                if (_mBInterruptOnCustomCommand)
                    _mBInterrupted = true;

                // pass to handler
                if (_mScriptHandler != null)
                    _mScriptHandler.OnScriptCommand(this,
                        commandPrototype.Name, listParameters);
            }
            catch (Exception exception)
            {
                throw new ScriptException("Invalid command.", exception, statement);
            }
        }

        private void ExecuteBreak(Statement statement)
        {
            _mBInterrupted = true;
        }

        private void ExecuteNextStatement()
        {
            // reset interrupt
            _mBInterrupted = false;

            Frame frame = _mStackFrame.Pop();
            Statement statement = frame.MScriptBlock.Statements[frame.MiNextStatement++];
            _mStackFrame.Push(frame);

            Token token = statement.Tokens[0];
            switch (token.Type)
            {
                // SETGLOBAL
                case TokenType.Setglobal:
                    ExecuteSetGlobal(statement);
                    break;
                // SET
                case TokenType.Set:
                    ExecuteSet(statement);
                    break;
                // ADD
                case TokenType.Add:
                    ExecuteAdd(statement);
                    break;
                // SUBTRACT
                case TokenType.Subtract:
                    ExecuteSubtract(statement);
                    break;
                // MULTIPLY
                case TokenType.Multiply:
                    ExecuteMultiply(statement);
                    break;
                // DIVIDE
                case TokenType.Divide:
                    ExecuteDivide(statement);
                    break;
                // IF
                case TokenType.If:
                    ExecuteIf(statement);
                    break;
                // WHILE
                case TokenType.While:
                    ExecuteWhile(statement);
                    break;
                // CALL
                case TokenType.Call:
                    ExecuteCall(statement);
                    break;
                // YIELD
                case TokenType.Yield:
                    ExecuteYield(statement);
                    break;
                // custom command
                case TokenType.Identifier:
                    ExecuteCustom(statement);
                    break;
                // added by pvk
                case TokenType.Break:
                    ExecuteBreak(statement);
                    break;
            }

            // if interrupt set and handler set, notify it
            if (_mBInterrupted && _mScriptHandler != null)
                _mScriptHandler.OnScriptInterrupt(this);

            // pop stack frame(s) if end of block reached
            while (_mStackFrame.Count > 0)
            {
                frame = _mStackFrame.Pop();
                if (frame.MiNextStatement < frame.MScriptBlock.Statements.Count)
                {
                    _mStackFrame.Push(frame);
                    break;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs a script context for the given script block.
        /// </summary>
        /// <param name="scriptBlock">Code block from a specific script.</param>
        public ScriptContext(ScriptBlock scriptBlock)
        {
            _mScriptBlock = scriptBlock;
            _mVariableDicitonary = new VariableDictionary(
                scriptBlock.Script.Manager.GlobalVariables);
            _mICurrentBlockSize = scriptBlock.Statements.Count;
            _mStackFrame = new Stack<Frame>();
            _mBInterrupted = false;
            _mScriptHandler = null;
            _mStackFrame.Push(new Frame(scriptBlock, 0));
            _mBInterruptOnCustomCommand = false;
        }

        /// <summary>
        /// Constructs a script context for the main code block of
        /// the given script.
        /// </summary>
        /// <param name="script">Script referenced by the context.</param>
        public ScriptContext(Script script)
            : this(script.MainBlock)
        {
        }

        /// <summary>
        /// Executes the associated script block indefinitely until the
        /// end of the block is reached or an interrupt is generated.
        /// This method may retain control indefinitely if the script
        /// block contains an infinite loop and no YIELD statements
        /// are used within the loop.
        /// </summary>
        /// <returns>Number of statements executed.</returns>
        public int Execute()
        {
            _mBInterrupted = false;
            int iCount = 0;
            while (!Terminated && !_mBInterrupted)
            {
                ExecuteNextStatement();
                ++iCount;
            }
            return iCount;
        }

        /// <summary>
        /// Executes the associated script block for up to the given
        /// maximum of statements. The method may return before this
        /// maximum is reached if the end of the block is reached
        /// or an interrupt is triggered.
        /// </summary>
        /// <param name="iStatements">Maximum number of statements to execute.</param>
        /// <returns>Number of statements executed.</returns>
        public int Execute(int iStatements)
        {
            _mBInterrupted = false;
            int iCount = 0;
            while (iStatements-- > 0 && !Terminated && !_mBInterrupted)
            {
                ExecuteNextStatement();
                ++iCount;
            }
            return iCount;
        }

        /// <summary>
        /// Executes the associated script block for the given time
        /// interval. The method may return earlier if the end of the
        /// code block is reached or an interrupt is generated.
        /// </summary>
        /// <param name="tsInterval">Time slot allowed for the script.</param>
        /// <returns></returns>
        public int Execute(TimeSpan tsInterval)
        {
            DateTime dtIntervalEnd = DateTime.Now + tsInterval;
            _mBInterrupted = false;
            int iCount = 0;
            while (DateTime.Now < dtIntervalEnd && !Terminated && !_mBInterrupted)
            {
                ExecuteNextStatement();
                ++iCount;
            }
            return iCount;
        }

        /// <summary>
        /// Resets the execution state of the script and clears the
        /// local variable scope from all variables.
        /// </summary>
        public void Restart()
        {
            _mVariableDicitonary.Clear();
            _mStackFrame.Clear();
            _mStackFrame.Push(new Frame(_mScriptBlock, 0));
            _mBInterrupted = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Script block referenced by this context.
        /// </summary>
        public ScriptBlock Block
        {
            get { return _mScriptBlock; }
        }

        /// <summary>
        /// Enable/disable flag for automatic interrupt generation
        /// when custom commands are executed.
        /// </summary>
        public bool InterruptOnCustomCommand
        {
            get { return _mBInterruptOnCustomCommand; }
            set { _mBInterruptOnCustomCommand = value; }
        }

        /// <summary>
        /// Indicates if the context terminated due to an interrupt
        /// or otherwise.
        /// </summary>
        public bool Interrupted
        {
            get { return _mBInterrupted; }
        }

        /// <summary>
        /// Indicates if the end of the reference code block was
        /// reached in the last run.
        /// </summary>
        public bool Terminated
        {
            get { return _mStackFrame.Count == 0; }
        }

        /// <summary>
        /// The next statement in the execution pipeline or null
        /// if there are no statements to execute.
        /// </summary>
        public Statement NextStatement
        {
            get
            {
                if (_mStackFrame.Count == 0)
                    return null;
                int iNextStatement = _mStackFrame.Peek().MiNextStatement;
                Script script = _mScriptBlock.Script;
                if (iNextStatement >= script.Statements.Count)
                    return null;
                return script.Statements[iNextStatement];
            }
        }

        /// <summary>
        /// Local variable dictionary.
        /// </summary>
        public VariableDictionary LocalVariables
        {
            get { return _mVariableDicitonary; }
        }

        /// <summary>
        /// Global variable dictionary. This is also
        /// avialble from the associated script manager.
        /// </summary>
        public VariableDictionary GlobalVariables
        {
            get { return _mScriptBlock.Script.Manager.GlobalVariables; }
        }

        /// <summary>
        /// Script handler for this context.
        /// </summary>
        public IScriptHandler Handler
        {
            get { return _mScriptHandler; }
            set { _mScriptHandler = value; }
        }

        #endregion
    }
}

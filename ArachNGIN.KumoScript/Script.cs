using System;
using System.Collections.Generic;
using System.Text;

namespace ArachNGIN.KumoScript
{
    #region Internal Structures

    internal struct IfBlock
    {
        public Statement MStatementIf;
        public ScriptBlock MScriptBlockTrue;
        public ScriptBlock MScriptBlockFalse;
        public int MiNextStatement;
    }

    internal struct WhileBlock
    {
        public Statement MStatementWhile;
        public ScriptBlock MScriptBlock;
        public int MiNextStatement;
    }

    #endregion

    /// <summary>
    /// Class representation of a script containing the compiled statements,
    /// script blocks and other structures used to support execution.
    /// </summary>
    public class Script
    {
        #region Private Variables

        readonly String _mStrName;
        readonly ScriptManager _mScriptManager;
        private readonly List<Statement> _mListStatements;
        private ScriptBlock _mScriptBlockMain;
        private readonly Dictionary<String, ScriptBlock> _mDictScriptBlocks;
        private readonly Dictionary<Statement, IfBlock> _mDictIfBlocks;
        private readonly Dictionary<Statement, WhileBlock> _mDictWhileBlocks;

        #endregion

        #region Private Methods

        private void Compile()
        {
            // organise statements into main and other blocks
            _mScriptBlockMain = new ScriptBlock(this, "_MAIN");

            Stack<ScriptBlock> stackScriptBlocks = new Stack<ScriptBlock>();
            stackScriptBlocks.Push(_mScriptBlockMain);

            Stack<IfBlock> stackIfBlocks = new Stack<IfBlock>();
            IfBlock ifBlock;

            Stack<WhileBlock> stackWhileBlocks = new Stack<WhileBlock>();
            WhileBlock whileBlock;

            foreach (Statement statement in _mListStatements)
            {
                // ignore blank lines and comments
                if (statement.Type == StatementType.BlankLine) continue;
                if (statement.Type == StatementType.Comment) continue;

                Token token = statement.Tokens[0];

                ScriptBlock scriptBlock = null;
                switch (token.Type)
                {
                    case TokenType.Block:
                        if (stackScriptBlocks.Count > 1)
                            throw new ScriptException("A BLOCK cannot be defined within another BLOCK, IF or WHILE ",
                                statement);

                        String strBlockName = statement.Tokens[1].Value.ToString();

                        if (_mDictScriptBlocks.ContainsKey(strBlockName.ToUpper()))
                            throw new ScriptException("Block " + strBlockName + " is already defined.",
                                statement);

                        scriptBlock = new ScriptBlock(this, strBlockName);
                        stackScriptBlocks.Push(scriptBlock);

                        break;
                    case TokenType.Endblock:
                        if (stackScriptBlocks.Count > 2)
                            throw new ScriptException("ENDBLOCK cannot be specified within IF or WHILE.",
                                statement);
                        if (stackScriptBlocks.Count == 1)
                            throw new ScriptException("ENDBLOCK specified without a matching BLOCK.",
                                statement);

                        scriptBlock = stackScriptBlocks.Pop();
                        _mDictScriptBlocks[scriptBlock.Name.ToUpper()] = scriptBlock;

                        break;
                    case TokenType.If:
                        // place IF statement in current block
                        scriptBlock = stackScriptBlocks.Pop();
                        scriptBlock.Statements.Add(statement);
                        stackScriptBlocks.Push(scriptBlock);

                        scriptBlock = new ScriptBlock(this, "__TEMP_IF_TRUE");
                        stackScriptBlocks.Push(scriptBlock);

                        ifBlock = new IfBlock();
                        ifBlock.MStatementIf = statement;
                        ifBlock.MScriptBlockTrue = scriptBlock;
                        stackIfBlocks.Push(ifBlock);
                        break;
                    case TokenType.Else:
                        if (stackIfBlocks.Count == 0)
                            throw new ScriptException("ELSE without matching IF");

                        // pop true block
                        stackScriptBlocks.Pop();

                        scriptBlock = new ScriptBlock(this, "__TEMP_IF_FALSE");
                        stackScriptBlocks.Push(scriptBlock);

                        ifBlock = stackIfBlocks.Pop();
                        ifBlock.MScriptBlockFalse = scriptBlock;
                        stackIfBlocks.Push(ifBlock);
                        break;
                    case TokenType.Endif:
                        if (stackIfBlocks.Count == 0)
                            throw new ScriptException("ENDIF without matching IF", statement);

                        // pop false block
                        stackScriptBlocks.Pop();

                        // set next statement to next in current block
                        ifBlock = stackIfBlocks.Pop();
                        ifBlock.MiNextStatement = stackScriptBlocks.Peek().Statements.Count;

                        _mDictIfBlocks[ifBlock.MStatementIf] = ifBlock;

                        break;
                    case TokenType.While:
                        // place WHILE statement in current block
                        scriptBlock = stackScriptBlocks.Pop();
                        scriptBlock.Statements.Add(statement);
                        stackScriptBlocks.Push(scriptBlock);

                        scriptBlock = new ScriptBlock(this, "__TEMP_WHILE");
                        stackScriptBlocks.Push(scriptBlock);

                        whileBlock = new WhileBlock();
                        whileBlock.MStatementWhile = statement;
                        whileBlock.MScriptBlock = scriptBlock;
                        stackWhileBlocks.Push(whileBlock);
                        break;
                    case TokenType.Endwhile:
                        if (stackWhileBlocks.Count == 0)
                            throw new ScriptException("ENDWHILE without matching WHILE", statement);

                        // pop while block
                        stackScriptBlocks.Pop();

                        // set next statement to next in current block
                        whileBlock = stackWhileBlocks.Pop();
                        whileBlock.MiNextStatement = stackScriptBlocks.Peek().Statements.Count;

                        _mDictWhileBlocks[whileBlock.MStatementWhile] = whileBlock;

                        break;
                    default:
                        // any other statement: place in top frame
                        scriptBlock = stackScriptBlocks.Pop();
                        scriptBlock.Statements.Add(statement);
                        stackScriptBlocks.Push(scriptBlock);
                        break;
                }
            }

            if (stackScriptBlocks.Count > 1)
                throw new ScriptException("Missing ENDBLOCK, ENDIF or ENDWHILE command.");

            // ensure CALLs refer to existing blocks
            foreach (Statement statement in _mListStatements)
            {
                if (statement.Type != StatementType.Control) continue;
                if (statement.Tokens[0].Type != TokenType.Call) continue;
                String strBlockName = statement.Tokens[1].Value.ToString();
                if (!_mDictScriptBlocks.ContainsKey(strBlockName.ToUpper()))
                    throw new ScriptException("Block " + strBlockName + " not defined.", statement);
            }
        }

        #endregion

        #region Internal Properties

        internal List<Statement> Statements
        {
            get { return _mListStatements; }
        }

        internal Dictionary<Statement, IfBlock> IfBlocks
        {
            get { return _mDictIfBlocks; }
        }

        internal Dictionary<Statement, WhileBlock> WhileBlocks
        {
            get { return _mDictWhileBlocks; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs a new script for the given script manager and
        /// resource name. The name is used to retrieve the script
        /// from disk or from another source through the usage of
        /// custom script loaders.
        /// </summary>
        /// <param name="scriptManager">
        /// Script manager associated with the script.</param>
        /// <param name="strName">Resource name for the script.</param>
        public Script(ScriptManager scriptManager, String strName)
        {
            _mStrName = strName;
            _mScriptManager = scriptManager;
            _mListStatements = new List<Statement>();
            _mDictScriptBlocks = new Dictionary<string, ScriptBlock>();
            _mDictIfBlocks = new Dictionary<Statement, IfBlock>();
            _mDictWhileBlocks = new Dictionary<Statement, WhileBlock>();

            // load main script file
            List<String> listStatements
                = _mScriptManager.Loader.LoadScript(strName);

            // parse to statements
            foreach (String strStatement in listStatements)
                _mListStatements.Add(new Statement(this, 0, strStatement));

            // includes dictionary
            Dictionary<String, bool> dictIncludes = new Dictionary<string, bool>();

            // process statements for INCLUDEs
            for (int iIndex = 0; iIndex < _mListStatements.Count; iIndex++)
            {
                // skip anything that is not a control statement
                Statement statement = _mListStatements[iIndex];
                if (statement.Type != StatementType.Control) continue;

                // skip statements that are not INCLUDE
                Token token = statement.Tokens[0];
                if (token.Type != TokenType.Include) continue;

                // remove INCLUDE statement
                _mListStatements.RemoveAt(iIndex);

                // determine included script name
                String strIncludeName = statement.Tokens[1].Value.ToString();

                // ignore if already included
                if (dictIncludes.ContainsKey(strIncludeName.ToUpper())) continue;

                List<Statement> listStatementsIncluded = new List<Statement>();
                foreach (String strStatement in _mScriptManager.Loader.LoadScript(strIncludeName))
                    listStatementsIncluded.Add(new Statement(this, 0, strStatement));

                _mListStatements.InsertRange(iIndex, listStatementsIncluded);

                // mark include file as included
                dictIncludes[strIncludeName.ToUpper()] = true;
            }

            // renumber lines
            for (int iIndex = 0; iIndex < _mListStatements.Count; iIndex++)
                _mListStatements[iIndex].Line = iIndex + 1;

            Compile();
        }

        /// <summary>
        /// Returns a string representation of the script.
        /// </summary>
        /// <returns>String representation of the script</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Script: " + _mStrName + "\r\n");
            foreach (Statement statement in _mListStatements)
                stringBuilder.Append(statement.ToString() + "\r\n");
            return stringBuilder.ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Script resource name.
        /// </summary>
        public String Name
        {
            get { return _mStrName; }
        }

        /// <summary>
        /// Script manager bound to this script.
        /// </summary>
        public ScriptManager Manager
        {
            get { return _mScriptManager; }
        }

        /// <summary>
        /// Main execution block.
        /// </summary>
        public ScriptBlock MainBlock
        {
            get { return _mScriptBlockMain; }
        }

        /// <summary>
        /// Named blocks within the scripts.
        /// </summary>
        public Dictionary<String, ScriptBlock> Blocks
        {
            get { return _mDictScriptBlocks; }
        }

        #endregion
    }
}

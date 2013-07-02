using System;
using System.Collections.Generic;
using System.IO;

namespace ArachNGIN.KumoScript
{
    internal class ScriptLoaderDefault
        : IScriptLoader
    {
        #region Public Methods

        public List<String> LoadScript(String strResourceName)
        {
            try
            {
                var listStatements = new List<string>();

                var streamReader = new StreamReader(strResourceName);
                while (!streamReader.EndOfStream)
                {
                    String strStatement = streamReader.ReadLine();
                    listStatements.Add(strStatement);
                }
                streamReader.Close();

                return listStatements;
            }
            catch (Exception exception)
            {
                throw new ScriptException("Error while reading script file", exception);
            }
        }

        #endregion
    }
}

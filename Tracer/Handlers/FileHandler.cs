/*
 * Copyright (c) 2006-2016 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO;
using ArachNGIN.Tracer.Helpers;
using ArachNGIN.Tracer.MessageFormat;

namespace ArachNGIN.Tracer.Handlers
{
    /// <summary>
    ///     Handler for file output
    /// </summary>
    /// <seealso cref="ArachNGIN.Tracer.ITracerHandler" />
    public class FileHandler : ITracerHandler
    {
        private readonly string _directory;
        private readonly string _fileName;
        private readonly IMessageFormat _messageFormat;

        /// <summary>
        ///     Creates file handler with all parameters
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="messageFormat">format of the message</param>
        public FileHandler(IMessageFormat messageFormat, string fileName, string directory)
        {
            _fileName = fileName;
            _directory = directory;
            _messageFormat = messageFormat;
        }

        /// <summary>
        ///     Creates file handler with filename parameter
        /// </summary>
        /// <param name="fileName">file name</param>
        public FileHandler(string fileName) : this(fileName, string.Empty)
        {
        }

        /// <summary>
        ///     Creates file handler with filename and directory parameters
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="directory">directory name</param>
        public FileHandler(string fileName, string directory) : this(
#if(DEBUG)
            new DebugMessageFormat()
#else
            new DefaultMessageFormat()
#endif
            , fileName, directory)
        {
        }

        /// <summary>
        ///     Creates file handler with message format parameter
        /// </summary>
        /// <param name="messageFormat">message format</param>
        public FileHandler(IMessageFormat messageFormat) : this(messageFormat, GetLogFileName())
        {
        }

        /// <summary>
        ///     Creates file handler with message format and filename parameters
        /// </summary>
        /// <param name="messageFormat">message format</param>
        /// <param name="fileName">file name</param>
        public FileHandler(IMessageFormat messageFormat, string fileName) : this(messageFormat, fileName, string.Empty)
        {
        }

        /// <summary>
        ///     Creates file handler with default parameters
        /// </summary>
        public FileHandler() : this(GetLogFileName())
        {
        }


        /// <summary>
        ///     Trace function with file output.
        /// </summary>
        /// <param name="tracerMessage">The tracer message.</param>
        public void Trace(TracerMessage tracerMessage)
        {
            if (!string.IsNullOrEmpty(_directory))
            {
                var directoryInfo = new DirectoryInfo(Path.Combine(_directory));
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
            }

            using (var writer = new StreamWriter(File.Open(Path.Combine(_directory, _fileName), FileMode.Append)))
            {
                writer.WriteLine(_messageFormat.Format(tracerMessage));
            }
        }

        /// <summary>
        ///     Gets the name of the log file.
        /// </summary>
        /// <returns></returns>
        private static string GetLogFileName()
        {
            var date = DateTime.Now;
            return
                $"Log_{HelperMethods.GetAppExeNameWithoutExtension()}_{date.Year:0000}{date.Month:00}{date.Day:00}-{date.Hour:00}{date.Minute:00}.log";
        }
    }
}
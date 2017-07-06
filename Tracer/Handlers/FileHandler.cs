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

using ArachNGIN.Tracer.Helpers;
using System;
using System.IO;

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileHandler" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="directory">The directory.</param>
        public FileHandler(string fileName, string directory)
        {
            _fileName = fileName;
            _directory = directory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileHandler" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileHandler(string fileName) : this(fileName, string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileHandler" /> class.
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
                writer.WriteLine(tracerMessage.ToString());
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
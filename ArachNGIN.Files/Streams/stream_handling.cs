/*
 * Copyright (c) 2006-2013 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
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

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    /// Tøída pro práci s proudy plná statických procedur
    /// </summary>
    public class StreamHandling
    {
        /// <summary>
        /// Zkopíruje jeden proud do druhého
        /// </summary>
        /// <remarks>
        /// (Aneb Pavùèci rádi Delphi a to co jim z nich chybí si prostì pøeložej ;-)
        /// </remarks>
        /// <param name="sSource">zdrojový proud</param>
        /// <param name="sDest">cílový proud</param>
        /// <param name="iCount">poèet bajtù ke zkopírování. když je 0 tak se zkopíruje celý proud</param>
        /// <returns>poèet zkopírovaných bajtù</returns>
        public static long StreamCopy(Stream sSource, Stream sDest, long iCount)
        {
            long result;
            const int maxBufSize = 0xF000;
            int bufSize;
            var rInput = new BinaryReader(sSource);
            var wOutput = new BinaryWriter(sDest);

            if (iCount == 0)
            {
                sSource.Position = 0;
                iCount = sSource.Length;
            }
            result = iCount;
            if (iCount > maxBufSize) bufSize = maxBufSize;
            else bufSize = (int) iCount;

            try
            {
                // serizneme vystup
                sDest.SetLength(0);
                while (iCount != 0)
                {
                    int n;
                    if (iCount > bufSize) n = bufSize;
                    else n = (int) iCount;
                    byte[] buffer = rInput.ReadBytes(n);
                    wOutput.Write(buffer);
                    iCount = iCount - n;
                }
            }
            finally
            {
                // si po sobe hezky splachneme :-)
                wOutput.Flush();
            }
            return result;
        }

        /// <summary>
        /// Zkopíruje jeden stream do druhého.
        /// </summary>
        /// <param name="sSource">zdrojový stream</param>
        /// <param name="sDest">cílový stream</param>
        /// <param name="iCount">poèet bajtù ke zkopírování</param>
        /// <param name="iStartposition">startovní pozice</param>
        /// <returns>poèet zkopírovaných bajtù</returns>
        public static long StreamCopy(Stream sSource, Stream sDest, long iCount, long iStartposition)
        {
            const int maxBufSize = 0xF000;
            int bufSize;
            var rInput = new BinaryReader(sSource);
            var wOutput = new BinaryWriter(sDest);

            if (iCount == 0)
            {
                sSource.Position = 0;
                iCount = sSource.Length;
            }
            long result = iCount;
            if (iCount > maxBufSize) bufSize = maxBufSize;
            else bufSize = (int) iCount;

            try
            {
                // naseekujeme zapisovaci pozici
                wOutput.Seek((int) iStartposition, SeekOrigin.Begin);
                while (iCount != 0)
                {
                    int n;
                    if (iCount > bufSize) n = bufSize;
                    else n = (int) iCount;
                    byte[] buffer = rInput.ReadBytes(n);
                    wOutput.Write(buffer);
                    iCount = iCount - n;
                }
            }
            finally
            {
                // si po sobe hezky splachneme :-)
                wOutput.Flush();
            }
            return result;
        }

        /// <summary>
        /// Copies a stream.
        /// </summary>
        /// <param name="source">The stream containing the source data.</param>
        /// <param name="target">The stream that will receive the source data.</param>
        /// <remarks>
        /// This function copies until no more can be read from the stream
        ///  and does not close the stream when done.<br/>
        /// Read and write are performed simultaneously to improve throughput.<br/>
        /// If no data can be read for 60 seconds, the copy will time-out.
        /// </remarks>
        public static void StreamCopyAsync(Stream source, Stream target)
        {
            // This stream copy supports a source-read happening at the same time
            // as target-write.  A simpler implementation would be to use just
            // Write() instead of BeginWrite(), at the cost of speed.

            var readbuffer = new byte[4096];
            var writebuffer = new byte[4096];
            IAsyncResult asyncResult = null;

            for (;;)
            {
                // Read data into the readbuffer.  The previous call to BeginWrite, if any,
                //  is executing in the background..
                int read = source.Read(readbuffer, 0, readbuffer.Length);

                // Ok, we have read some data and we're ready to write it, so wait here
                //  to make sure that the previous write is done before we write again.
                if (asyncResult != null)
                {
                    // This should work down to ~0.01kb/sec
                    asyncResult.AsyncWaitHandle.WaitOne(60000);
                    target.EndWrite(asyncResult); // Last step to the 'write'.
                    if (!asyncResult.IsCompleted) // Make sure the write really completed.
                        throw new IOException("Stream write failed.");
                }

                if (read <= 0)
                    return; // source stream says we're done - nothing else to read.

                // Swap the read and write buffers so we can write what we read, and we can
                //  use the then use the other buffer for our next read.
                byte[] tbuf = writebuffer;
                writebuffer = readbuffer;
                readbuffer = tbuf;

                // Asynchronously write the data, asyncResult.AsyncWaitHandle will
                // be set when done.
                asyncResult = target.BeginWrite(writebuffer, 0, read, null, null);
            }
        }

        /// <summary>
        /// Funkce pro pøevod pole znakù na string
        /// </summary>
        /// <param name="cInput">vsupní pole znakù</param>
        /// <returns>výsledný øetìzec</returns>
        public static string PCharToString(char[] cInput)
        {
            // TODO: ODDELIT DO ArachNGIN.Strings! (az bude)
            string result = "";
            foreach (char c in cInput)
            {
                if (c == 0x0) break; // pcharovej konec retezce;
                result = result + c;
            }
            return result;
        }
    }
}
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
using System.Text;
using System.Threading;

namespace ArachNGIN.Files.Streams
{
    /// <summary>
    ///     Class for working with streams. Full of static functions
    /// </summary>
    public static class StreamHandling
    {
        /// <summary>
        ///     Copies one stream to another.
        /// </summary>
        /// <param name="sSource">The source stream.</param>
        /// <param name="sDest">The destination stream.</param>
        /// <param name="iCount">Bytes to copy.</param>
        /// <returns></returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public static long StreamCopy(Stream sSource, Stream sDest, long iCount)
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
            else bufSize = (int)iCount;

            try
            {
                // serizneme vystup
                sDest.SetLength(0);
                while (iCount != 0)
                {
                    int n;
                    if (iCount > bufSize) n = bufSize;
                    else n = (int)iCount;
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
        ///     Copies one stream to another.
        /// </summary>
        /// <param name="sSource">The source stream.</param>
        /// <param name="sDest">The destination stream.</param>
        /// <param name="iCount">Bytes to copy.</param>
        /// <param name="iStartposition">Starting position.</param>
        /// <returns></returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
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
            else bufSize = (int)iCount;

            try
            {
                // naseekujeme zapisovaci pozici
                wOutput.Seek((int)iStartposition, SeekOrigin.Begin);
                while (iCount != 0)
                {
                    int n;
                    if (iCount > bufSize) n = bufSize;
                    else n = (int)iCount;
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
        ///     Copies a stream.
        /// </summary>
        /// <param name="source">The stream containing the source data.</param>
        /// <param name="target">The stream that will receive the source data.</param>
        /// <remarks>
        ///     This function copies until no more can be read from the stream
        ///     and does not close the stream when done.<br />
        ///     Read and write are performed simultaneously to improve throughput.<br />
        ///     If no data can be read for 60 seconds, the copy will time-out.
        /// </remarks>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
        /// <exception cref="IOException">Stream write failed.</exception>
        /// <exception cref="AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
        /// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
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
        ///     Converts a PChar (null terminated string) to normal string
        /// </summary>
        /// <param name="cInput">The char array input.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts stream to string
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Converts string to stream
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Stream StringToStream(string source)
        {
            var byteArray = Encoding.UTF8.GetBytes(source);
            return new MemoryStream(byteArray);
        }
    }
}
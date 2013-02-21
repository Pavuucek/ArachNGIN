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
		/// Konstruktor tøídy. Nic nedìlá
		/// </summary>
		/// <returns>nic</returns>
		public StreamHandling()
		{
		}

		/// <summary>
		/// Zkopíruje jeden proud do druhého
		/// </summary>
		/// <remarks>
		/// (Aneb Pavùèci rádi Delphi a to co jim z nich chybí si prostì pøeložej ;-)
		/// </remarks>
		/// <param name="s_source">zdrojový proud</param>
		/// <param name="s_dest">cílový proud</param>
		/// <param name="i_count">poèet bajtù ke zkopírování. když je 0 tak se zkopíruje celý proud</param>
		/// <returns>poèet zkopírovaných bajtù</returns>
		public static long StreamCopy(Stream s_source, Stream s_dest, long i_count)
		{
			long result = 0;
			int MaxBufSize = 0xF000;
			int BufSize;
			byte[] Buffer;
			int N;
			BinaryReader r_input = new BinaryReader(s_source);
			BinaryWriter w_output = new BinaryWriter(s_dest);

			if (i_count == 0)
			{
				s_source.Position = 0;
				i_count = s_source.Length;
			}
			result = i_count;
			if (i_count > MaxBufSize) BufSize = MaxBufSize;
			else BufSize = (int)i_count;

			try
			{
				// serizneme vystup
				s_dest.SetLength(0);
				while(i_count != 0)
				{
					if(i_count > BufSize) N = BufSize;
					else N = (int)i_count;
					Buffer = r_input.ReadBytes(N);
					w_output.Write(Buffer);
					i_count = i_count - N;
				}
			}
			finally
			{
                // si po sobe hezky splachneme :-)
                w_output.Flush();
			}
			return result;
		}
        public static long StreamCopy(Stream s_source, Stream s_dest, long i_count,long i_startposition)
        {
            long result = 0;
            int MaxBufSize = 0xF000;
            int BufSize;
            byte[] Buffer;
            int N;
            BinaryReader r_input = new BinaryReader(s_source);
            BinaryWriter w_output = new BinaryWriter(s_dest);
            
            if (i_count == 0)
            {
                s_source.Position = 0;
                i_count = s_source.Length;
            }
            result = i_count;
            if (i_count > MaxBufSize) BufSize = MaxBufSize;
            else BufSize = (int)i_count;

            try
            {
                // naseekujeme zapisovaci pozici
                w_output.Seek((int)i_startposition, SeekOrigin.Begin);
                while (i_count != 0)
                {
                    if (i_count > BufSize) N = BufSize;
                    else N = (int)i_count;
                    Buffer = r_input.ReadBytes(N);
                    w_output.Write(Buffer);
                    i_count = i_count - N;
                }
            }
            finally
            {
                // si po sobe hezky splachneme :-)
                w_output.Flush();
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

            byte[] readbuffer = new byte[4096];
            byte[] writebuffer = new byte[4096];
            IAsyncResult asyncResult = null;

            for (; ; )
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
		/// <param name="c_input">vsupní pole znakù</param>
		/// <returns>výsledný øetìzec</returns>
		public static string PCharToString(char[] c_input)
		{
			// TODO: ODDELIT DO ArachNGIN.Strings! (az bude)
			string result = "";
			foreach(char c in c_input)
			{
				if (c == 0x0) break; // pcharovej konec retezce;
				result = result+c;
			}
			return result;
		}
	}

}

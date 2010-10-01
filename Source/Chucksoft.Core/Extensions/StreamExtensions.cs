
using System;
using System.Collections.Generic;
using System.IO;

namespace Chucksoft.Core.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads a stream line by line
        /// </summary>            
        /// <returns>The read lines</returns>
        public static List<string> ReadLines(this Stream stream)
        {
            var lines = new List<string>();
            using (var sr = new StreamReader(stream))
            {
                while (sr.Peek() >= 0)
                {
                    lines.Add(sr.ReadLine());
                }
            }
            return lines;
        }

        /// <summary>
        /// Reads a complete stream
        /// </summary>            
        /// <returns>The contents of the stream</returns>
        public static string ReadAll(this Stream stream)
        {         
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }         
        }

        /// <summary>
        /// Reads to end.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadToEnd(this Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }
    }
}
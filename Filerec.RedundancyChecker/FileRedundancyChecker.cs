using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Filerec.RedundancyChecker
{
    /// <summary>
    /// Checks if files are redundant.
    /// </summary>
    public class FileRedundancyChecker
    {
        /// <summary>
        /// If filenames should be compared
        /// </summary>
        public bool CompareNames { get; set; }
        /// <summary>
        /// If filesizes sould be compared
        /// </summary>
        public bool CompareSize { get; set; }
        /// <summary>
        /// If the filestream sould be compared
        /// </summary>
        public bool CompareStream { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="Filerec.RedundancyChecker.FileRedundancyChecker"/>
        /// </summary>
        /// <param name="compareNames">If names should be equal</param>
        /// <param name="compareSize">If file sizes sould be equal</param>
        /// <param name="compareStream">If file streams sould be equal</param>
        public FileRedundancyChecker(bool compareNames, bool compareSize, bool compareStream)
        {
            CompareNames = compareNames;
            CompareSize = compareSize;
            CompareStream = compareStream;
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, UIntPtr count);

        //[DllImport("msvcrt.dll")]
        //private static extern unsafe int memcmp(byte* b1, byte* b2, int count);

        /// <summary>
        /// Checks if two files are equal (redundant).
        /// </summary>
        /// <remarks>
        /// The comparison is made with the given settings like:
        /// -> compare file names
        /// -> compare file sizes
        /// -> compare file streams
        /// </remarks>
        /// <param name="sourceFileName">Filename of the source file</param>
        /// <param name="compareToFileName">Filename of the destination file</param>
        /// <returns>If the files are equal</returns>
        public bool FileEquals(string sourceFileName, string compareToFileName)
        {
            bool isFileEqual = false;

            if (!(CompareNames || CompareSize || CompareStream))
            {
                // When no settings are set, there is nothing to ckeck ^^
                return false;
            }

            if (String.Equals(sourceFileName, compareToFileName))
            {
                // source file is equal to the destination file -> no redundancy
                return false;
            }

            if (CompareNames)
            {
                isFileEqual = String.Equals(Path.GetFileName(sourceFileName), Path.GetFileName(compareToFileName));
            }

            if (CompareSize)
            {
                isFileEqual = (new FileInfo(sourceFileName).Length == new FileInfo(compareToFileName).Length);
            }

            if (CompareStream)
            {
                using (FileStream sourceFile = new FileStream(sourceFileName, FileMode.Open))
                {
                    using (FileStream compareToFile = new FileStream(compareToFileName, FileMode.Open))
                    {
                        isFileEqual = StreamEquals(sourceFile, compareToFile);
                    }
                }
            }

            return isFileEqual;
        }

        /// <summary>
        /// Compares if two streams are equal
        /// </summary>
        /// <param name="sourceStream">source stream</param>
        /// <param name="compareToStream">compare to stream</param>
        /// <returns>if the streams are equal</returns>
        private static bool StreamEquals(Stream sourceStream, Stream compareToStream)
        {
            const int bufferSize = 2048;
            byte[] sourceBuffer = new byte[bufferSize];
            byte[] compareToBuffer = new byte[bufferSize];

            while (true)
            {
                int sourceByteCount = sourceStream.Read(sourceBuffer, 0, bufferSize);
                int compareToByteCount = compareToStream.Read(compareToBuffer, 0, bufferSize);

                if (sourceByteCount != compareToByteCount)
                {
                    return false;
                }

                if (sourceByteCount == 0)
                {
                    return true;
                }

                if (!CompareBuffers(sourceBuffer, compareToBuffer, bufferSize))
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// memcmp wrapper
        /// </summary>
        /// <param name="buffer1">source buffer</param>
        /// <param name="buffer2">compare to buffer</param>
        /// <param name="count">Number of bytes to compare</param>
        /// <returns>if the bytes are equal</returns>
        private static unsafe bool CompareBuffers(byte[] buffer1, byte[] buffer2, int count)
        {
            return memcmp(buffer1, buffer2, new UIntPtr((uint)count)) == 0;
        }

    }
}

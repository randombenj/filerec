using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filerec.RedundancyChecker
{
    /// <summary>
    /// Checks redundancy
    /// </summary>
    public class RedundancyChecker
    {
        /// <summary>
        /// Initialises a new instance of <see cref="Filerec.RedundancyChecker.RedundancyChecker"/>
        /// </summary>
        /// <param name="compareNames">If the names sould be compared</param>
        /// <param name="compareSize">If the filesizes sould be compared</param>
        /// <param name="compareStream">If the filestreams sould be compared</param>
        public RedundancyChecker(bool compareNames,bool compareSize, bool compareStream)
        {
            FileRedundancyChecker = new Filerec.RedundancyChecker.FileRedundancyChecker(compareNames, compareSize, compareStream);
        }

        private FileRedundancyChecker FileRedundancyChecker { get; set; }
        private IEnumerable<string> SourceFiles { get; set; }
        private IEnumerable<string> DestinationFiles { get; set; }

        /// <summary>
        /// If filenames should be compared
        /// </summary>
        public bool CompareNames 
        {
            get
            {
                return FileRedundancyChecker.CompareNames;
            }
            set
            {
                FileRedundancyChecker.CompareNames = value;
            }
        }
        /// <summary>
        /// If filesizes sould be compared
        /// </summary>
        public bool CompareSize 
        {
            get
            {
                return FileRedundancyChecker.CompareSize;
            }
            set
            {
                FileRedundancyChecker.CompareSize = value;
            }
        }
        /// <summary>
        /// If the filestream sould be compared
        /// </summary>
        public bool CompareStream 
        {
            get
            {
                return FileRedundancyChecker.CompareStream;
            }
            set
            {
                FileRedundancyChecker.CompareStream = value;
            }
        }

        /// <summary>
        /// Returns redundant files found in the compareToDirecotry
        /// </summary>
        /// <param name="sourceDirectory">Source directory</param>
        /// <param name="compareToDirectory">Compare to directory</param>
        /// <returns>redundant files found in the compare to directory</returns>
        public IEnumerable<IEnumerable<FileMetadata>> CheckRedundancy(string sourceDirectory, string compareToDirectory)
        {
            SourceFiles = InitialiseMetadata(sourceDirectory);
            DestinationFiles = InitialiseMetadata(compareToDirectory);

            foreach (string sourceFile in SourceFiles)
	        {
                FileInfo fileInfo = new FileInfo(sourceFile);
                List<FileMetadata> fileMetadataList = new List<FileMetadata> 
                { 
                    new FileMetadata
                    {
                        FileName = sourceFile,
                        FileSize = fileInfo.Length,
                        CreateDate = fileInfo.CreationTime
                    }
                };

                foreach (string distinationFile in DestinationFiles)
	            {
                    fileInfo = new FileInfo(distinationFile);
                    if (FileRedundancyChecker.FileEquals(sourceFile, distinationFile))
	                {
                        fileMetadataList.Add(new FileMetadata
                            {
                                FileName = distinationFile,
                                FileSize = fileInfo.Length,
                                CreateDate = fileInfo.CreationTime
                            });    
	                }
	            }
                if (fileMetadataList.Count > 1)
	            {
                    yield return fileMetadataList;
	            }
	        }
        }

        private IEnumerable<string> InitialiseMetadata(string directory)
        {
            List<string> fileList = new List<string>();

            foreach (string d in Directory.GetDirectories(directory))
            {
                if ((new DirectoryInfo(d).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    foreach (string f in Directory.GetFiles(d, "*.*", SearchOption.AllDirectories))
                    {
                        // filter for file names which are to long
                        if (new FileInfo(f).Name.Count() < 259)
                        {
                            fileList.Add(f);
                        }
                    }
                    InitialiseMetadata(d);
                }
            }

            return fileList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filerec.RedundancyChecker
{
    public class FileMetadata
    {
        /// <summary>
        /// Full path to the file
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Filesize (in bytes)
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// Creation date of the file
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

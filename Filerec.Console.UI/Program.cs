using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filerec.Console.UI
{
    class Program
    {
        private static RedundancyChecker.RedundancyChecker redundancyChecker = 
            new RedundancyChecker.RedundancyChecker(compareNames: true, compareSize: true, compareStream: false);

        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                int count = HandleArguments(args);

                if ((args.Length - count) == 2)
                {

                    System.Console.WriteLine("-- FILE REDUNDANCY CHECKER: ----------------------------------------------------");
                    DateTime begin = DateTime.Now;

                    string sourcePath = args[args.Length - 2];
                    string compareToPath = args[args.Length - 1];

                    int totalRedundancys = 0;

                    foreach (IEnumerable<RedundancyChecker.FileMetadata> filelist in redundancyChecker.CheckRedundancy(sourcePath, compareToPath))
                    {
                        System.Console.WriteLine("{0,-104} {1} : {2:dd.MM.yyyy}", 
                            filelist.First().FileName,
                            filelist.First().FileSize, 
                            filelist.First().CreateDate.Date);

                        foreach (RedundancyChecker.FileMetadata file in filelist.Except(new List<RedundancyChecker.FileMetadata> { filelist.First() }))
                        {
                            System.Console.WriteLine(" -> {0,-100} {1} : {2:dd.MM.yyyy}", 
                                file.FileName, 
                                file.FileSize, 
                                file.CreateDate.Date);

                            totalRedundancys++;
                        }
                    }

                    System.Console.WriteLine("\n\n-- [OK] status: ----------------------------------------------------------------");
                    System.Console.WriteLine("Total redundancys found: {0}\n", totalRedundancys);

                    DateTime end = DateTime.Now;
                    System.Console.WriteLine("-- [TIME] {0:00000}:{1:00}:{2:0000} --------------------------------------------------------", 
                        (end - begin).Minutes, 
                        (end - begin).Seconds, 
                        (end - begin).Milliseconds);

                    System.Console.ReadKey();
                }
                else
                {
                    PrintHelp();
                }
            }
            else
            {
                PrintHelp();
            }
        }

        private static int HandleArguments(string[] args)
        {
            int count = args.Length;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        PrintHelp();
                        break;
                    case "-n":
                        redundancyChecker.CompareNames = false;
                        break;
                    case "-l":
                        redundancyChecker.CompareSize = false;
                        break;
                    case "-s":
                        redundancyChecker.CompareStream = false;
                        break;
                    default:
                        if (args[i].Contains('-') && args[i].Length == 2)
                        {
                            System.Console.WriteLine("{0} is an invalid argument. Type filerec -h for help.", args[i]);
                        }
                        else
                        {
                            count--;
                        }
                        break;
                }
            }
            return count;
        }

        private static void PrintHelp()
        {
            string helpText =
@"FILE REDUNDANCY CHECKER:

    Compares two directories if they contain the same files. 
    This can be done on different levels like: 
     - compare filenames
     - compare filesizes 
     - compare filestreams.

    Standard options are: filename: true, filesize: true, filestream: false.
                 
    USAGE:
                   
    filerec [options] source compareto        

    source:     the source directory
    compareto:  compared with this directory       
                 
    OPTIONS:

     -h       Shows this help information,
     -n       DON'T compare filenames
     -l       DON'T copmpare filesizes
     -s       Compare filestreams (This may take some time)";

            System.Console.WriteLine(helpText);
        }
    }
}

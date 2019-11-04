using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Threading;


using Common.Utils;



namespace DiskCleaner
{
    class Program
    {

        //public static string _parLetterToClean = "e";

       

        static void Main(string[] args)
        {

            double parMinFreeSizeGB;
            if (args.Count() < 1)
            {
                Console.WriteLine("Min size is not set. Input size in GB");
                return;
            }

            try
            {
                parMinFreeSizeGB = Convert.ToDouble(args[0]);
            }
            catch  (Exception e)
            {
                Console.WriteLine("Invalid min size format");
                return;
            }


            if (parMinFreeSizeGB <=0)
            {
                Console.WriteLine("Size must be positive");
                return;
            }



            CDriveSpaceChecker driveSpaceChecker = new CDriveSpaceChecker();

            driveSpaceChecker.CheckDriveSpace(parMinFreeSizeGB);

        }




        


    }
}

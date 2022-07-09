using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AreteTester.Core
{
    public class Globals
    {
        public static string LocalDir
        {
            get
            {
                string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\AreteTester\";
                if (Directory.Exists(local) == false)
                {
                    Directory.CreateDirectory(local);
                }

                return local;
            }
        }

        public static string LocalBinDir
        {
            get { return LocalDir + @"bin\"; }
        }

        public static string WebUrl
        {
            get { return "https://www.aretetester.com"; }
        }

        public static string WebDomain
        {
            get { return "aretetester.com"; }
        }

        static Globals()
        {
        }
    }
}

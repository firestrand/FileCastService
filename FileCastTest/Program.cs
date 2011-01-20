using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileCastBusiness;
using FileCastService;
using System.IO;

namespace FileCastTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileCaster fc = new FileCaster())
            {
                fc.TestStart();
            }

            Console.ReadKey();
        }
    }
}

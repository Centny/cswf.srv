using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace io.vty.cswf.srv.test.exe
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = "";
            if (args.Length > 1)
            {
                msg = args[1];
            }
            var i = 0;
            while (true)
            {
                i += 1;
                Console.WriteLine("Testing {0} {0}...", msg, i);
                Thread.Sleep(1000);
            }
        }
    }
}

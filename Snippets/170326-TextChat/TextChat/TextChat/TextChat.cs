using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TextChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
            GetLine();
        }

        static void GetLine()
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string result = wc.DownloadString("http://www.capsule03.com/wiki.html");
            Console.WriteLine(result);
        }
    }

}

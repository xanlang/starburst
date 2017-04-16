using System.Net;
using System;
using System.Text.RegularExpressions;

namespace IPtoolkit

{

class getIP {

    static void Main()
    {
        Console.WriteLine("Connecting to Capsule.\n");
        string myIP = GetIPFromCapsule();
        Console.WriteLine("The IP of this computer is currently: " + myIP);
    }

    static string GetIPFromCapsule()
    {
        /**
         * This script relies on IPConnect having a specific comment line
         * formatted as such:  <!-- CurrentIP:##########--> 
         * where ######### is the address, such as 192.168.0.1
         *
         * In this way, I can get the IP address of the current machine.
         * */

        System.Net.WebClient wc = new System.Net.WebClient();
        string result = wc.DownloadString("http://www.capsule03.com/s/ipconnect.php");
        //Console.WriteLine(result);
        
        string filteredAddress = Regex.Match(result, @"<!-- CurrentIP:(.+)-->", RegexOptions.Singleline).Groups[1].Value;

        if (result != null)
        {
            Console.WriteLine("Successfully accessed IpConnect on Capsule.\n");
            return filteredAddress;
        }
        else
            Console.WriteLine("Connection Failed.\n");

        return null;
    }
}

}

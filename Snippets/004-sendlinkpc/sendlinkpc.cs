using System.Net;
using System;
using System.Text;
using System.Collections.Specialized;

class Connect {
    static void Main()
    {
        Console.WriteLine("Specify Link: ");
        string theLink = Console.ReadLine();
        Console.WriteLine("Connecting to Capsule.\n");
        SendLink(theLink);
    }

    static void SendLink(string pLink)
    {
        string url="http://www.capsule03.com/sendlink/slink-server.php";
        System.Net.WebClient wc = new System.Net.WebClient();

        NameValueCollection data = new NameValueCollection();
        data.Add("sendlink","sendlink!anlknvdkj");
        data.Add("url", pLink);
        data.Add("username", "xansrv");

        byte[] responseBytes = wc.UploadValues(url, "POST", data);
        string responseFromServer = Encoding.UTF8.GetString(responseBytes);
        Console.WriteLine(responseFromServer);

        if (responseFromServer != null)
            Console.WriteLine("Successfully sent link.\n");
        else
            Console.WriteLine("Connection Failed.\n");
    }
}

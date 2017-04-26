using System.Net;
using System;
using System.Text;
using System.Collections.Specialized;

class Connect {
    static void Main()
    {
        Console.WriteLine("Connecting to Capsule.\n");
        GetLine();
    }

    static void GetLine()
    {
        string url="http://www.capsule03.com/s/sconnect-a.php";
        System.Net.WebClient wc = new System.Net.WebClient();

        NameValueCollection data = new NameValueCollection();
        data.Add("login","");
        data.Add("username","artemis");
        data.Add("password","falls");

        byte[] responseBytes = wc.UploadValues(url, "POST", data);
        string responseFromServer = Encoding.UTF8.GetString(responseBytes);
        Console.WriteLine(responseFromServer);

        if (responseFromServer != null)
            Console.WriteLine("Successfully accessed sconnect.\n");
        else
            Console.WriteLine("Connection Failed.\n");
    }
}

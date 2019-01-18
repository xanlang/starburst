using System.Net;
using System;
using System.Text;
using System.Collections.Specialized;
using System.Threading;


//based on https://msdn.microsoft.com/en-us/library/system.threading.timer(v=vs.110).aspx


class Connect {
    static void Main()
    {
        Console.WriteLine("Connecting to Capsule.\n");

        var Messager = new Messager();

        var autoEvent = new AutoResetEvent(false);
        var messageTimer = new Timer(Messager.SendMessage, null, 1000, 20000);

        // wait until we get a signal to stop the program.. Currently this won't happen.
        autoEvent.WaitOne();
        
        // close the program.
        messageTimer.Dispose();
        Console.WriteLine("Done.");

    }

}

class Messager {

    private int currentCount;

    public Messager()
    {
        currentCount = 0;
    }

    public void SendMessage(Object stateInfo)
    {
        AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
        string url="http://www.capsule03.com/S/sconnect-a.php";
        System.Net.WebClient wc = new System.Net.WebClient();

        NameValueCollection data = new NameValueCollection();
        data.Add("login","");
        data.Add("username","artemis");
        data.Add("password","falls");
        data.Add("message",(++currentCount).ToString());
        

        byte[] responseBytes = wc.UploadValues(url, "POST", data);
        string responseFromServer = Encoding.UTF8.GetString(responseBytes);
        //Console.WriteLine(responseFromServer);

        if (responseFromServer != null)
            Console.WriteLine("Sent message: " + (currentCount).ToString() + ".\n");
        else
            Console.WriteLine("Connection Failed.\n");

        bool stopMessaging = false; // if we put a condition to turn this to true, we 
                                    // will send a message to the main thread to stop.
        if(stopMessaging)
        {
            autoEvent.Set();
        }
    }

}


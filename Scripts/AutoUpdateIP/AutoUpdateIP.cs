using System.Net;
using System;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Text.RegularExpressions;

/*******
 *
 *  Auto Update IP
 *
 *  Synopsis: 
 *      Auto Update IP is a program that periodically queries www.capsule03.com/s/sconnectB.php,
 *      get the IP address of this machine (xansrv) and perform actions that are required to
 *      keep XanSRV online and running.
 *
 *      It exists because I run XanSRV off of a home connection, which means my IP address changes
 *      periodically. If I want to stay connected to XanSRV, XanSRV has to update its IP regularly.
 *
 *
 *
 * **/

namespace AutoUpdateIP
{

    class AutoUpdate {

    static bool debugMode = false;

    static void Main()
    {
        if(debugMode) {Console.WriteLine("Connecting to Capsule.\n");}

        var myMessenger = new Messenger();

        var autoEvent = new AutoResetEvent(false);
        var loopTimer = new Timer(myMessenger.DoActions, null, 1000, 600000);

        // wait until we get a signal to stop the program.. Currently this won't happen.
        autoEvent.WaitOne();
        
        // close the program.
        loopTimer.Dispose();
        if(debugMode) {Console.WriteLine("Done.");}

    }

}

class Messenger {
    // this class handles sending and retrieving data from capsule03.

    static bool debugMode = false; 
    private int currentCount;
    private string myIP;

    public Messenger()
    {
        currentCount = 0;
        myIP = "";
    }

    public void DoActions(Object stateInfo)
    {
        AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

        AccessCapsule(); // accesses Capsule and sets myIP.
                        // this seems like really shitty programming. It's not obvious that AccessCapsule sets myIP
        if(myIP != String.Empty)
        {
            if (XanWiki.AddressFixer.CheckCurrentAddress() == myIP) // check if the IP has changed from the IP that's stored in mediawiki's LocalSettings.php
            {
                // the addresses are the same.
                if(debugMode){Console.WriteLine("Mediawiki address is the same as current address.");}
            }
            else
            {
                if(debugMode) {Console.WriteLine("Mediawiki address is different than current address. Updating...");}
                XanWiki.AddressFixer.UpdateLocalSettingsIP(myIP, false);
            }

        }


        bool stopMessaging = false; // if we put a condition to turn this to true, we 
                                    // will send a message to the main thread to stop.
        if(stopMessaging)
        {
            autoEvent.Set();
        }
    }

    public void AccessCapsule()
    {
        string url = "http://www.capsule03.com/s/sconnect-b.php";
        System.Net.WebClient wc = new System.Net.WebClient();
       
        string serverResponse = ConnectToCapsule(wc, url);
        if (serverResponse != String.Empty && serverResponse != null)
        {
            myIP = GetIP(serverResponse);
        }
        else
        {
            // should log that we failed to connect.
            if(debugMode) {Console.WriteLine("Could not Connect");}
        }

    }

    private string ConnectToCapsule(System.Net.WebClient wc, string url)
    {

        NameValueCollection data = new NameValueCollection();
        data.Add("login","");
        data.Add("username","artemis");
        data.Add("password","falls");
        data.Add("message","Checking in, " + (++currentCount).ToString());
       
        byte[] responseBytes = null; 
        
        try
        {
            responseBytes = wc.UploadValues(url, "POST", data);
        }
        catch (Exception e)
        {
            if(debugMode) {Console.WriteLine("oops");}
            if(debugMode) {Console.WriteLine(e);}
            return String.Empty;
        }

        string responseFromServer = Encoding.UTF8.GetString(responseBytes);

        if (responseFromServer != null)
        {
            if(debugMode) {Console.WriteLine("Sent message: " + (currentCount).ToString() + ".\n");}
            return responseFromServer;
        }
        else
        {
            if(debugMode) {Console.WriteLine("Connection Failed.\n");}
            return String.Empty;
        }

    }

    private string GetIP(string pResponse)
    {
        string filteredAddress = Regex.Match(pResponse, @"<!-- CurrentIP:(.+)-->", RegexOptions.Singleline).Groups[1].Value;
        // the method looks for "<!-- CurrentIP:XXXXXX-->", and removes 'XXXXXX'.
        // if the regex failed, Regex.Match returns String.Empty.

        if (filteredAddress != String.Empty)
        {
            return filteredAddress;
        }

        // if we didn't get an address, return empty.
        // should log that this failed.
        return String.Empty;
    } 

    public string MyIP
    {
        get 
        {
            return this.myIP;
        }
        set
        {
            this.myIP = value;
        }
    }

}

}

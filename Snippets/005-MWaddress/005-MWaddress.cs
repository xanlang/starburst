using System;
using System.IO;

namespace MWaddress
{

class AddressFixer
{
    
    static string LocalSettingsPath = "/var/www/html/mediawiki/LocalSettings.php";
    static string TempLocalSettingsFile = "/var/www/html/mediawiki/LocalSettings-temp.php";
    static string searchLine = "$wgServer";    

    static void Main(string[] args)
    {

        bool printErrors = false; 

        if(args.Length == 0)
        {
            PrintManual();
            return;
        }

        string inputAddress = ""; // the address that the user put in
        bool addressSpecified = false; // keep track if we have an address entered in.

        for( int i = 0; i < args.Length; i++)
        {
            if(args[i].StartsWith("-"))
            {
                if(args[i] == "-check")
                {
                    CheckCurrentAddress();
                    return;
                }
                else if(args[i] == "-v")
                {
                    printErrors = true;
                }
                else // an invalid flag was entered. Just print the manual.
                {
                    PrintManual();
                    return;
                }
            }
            else
            {
                if(addressSpecified == true) // more than one address was specified.. so we have an invalid input
                {
                    PrintManual();
                    return;
                }
                inputAddress = args[i];
                addressSpecified = true;
            }
        }

        if(!addressSpecified) // no address was specified
        {
            PrintManual();
            return;
        }
                

        if(File.Exists(LocalSettingsPath))
        {
            Console.WriteLine("LocalSettings.php found.");

            try 
            {
                if(File.Exists(TempLocalSettingsFile))
                {
                    Console.WriteLine("Clearing previous temporary file...");
                    File.Delete(TempLocalSettingsFile);
                }
            }  
            catch (Exception e)
            {
                Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");
                if (printErrors) { Console.WriteLine(e); }
                return;
            }

            string lineToWrite = "$wgServer = \"http://" + inputAddress + "\";";

            string[] lines = File.ReadAllLines(LocalSettingsPath);

            try
            {
                using (StreamWriter writer = File.AppendText(TempLocalSettingsFile))
                {

                    for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                    {

                        if(lines[currentLine-1].StartsWith(searchLine))
                        {
                            writer.WriteLine(lineToWrite);
                            Console.WriteLine("Updating IP address to: http://" + args[0]);
                        }
                        else 
                        {
                            writer.WriteLine(lines[currentLine-1]);
                        }
                    }

                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");
                if (printErrors) { Console.WriteLine(e); }
                return;
            }
            

          
            Console.WriteLine("Saving new LocalSettings.php file.");
            string tempPath = LocalSettingsPath + ".b";
            if(File.Exists(tempPath))
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");
                    if (printErrors) { Console.WriteLine(e); }
                    return;
                }

            }
            
            try
            {
                File.Move(LocalSettingsPath,tempPath);
                File.Move(TempLocalSettingsFile,LocalSettingsPath);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorized Access Exception when trying to save the updated LocalSettings file. Try running as root (sudo)");
                if (printErrors) { Console.WriteLine(e); }
                return;
            }
            

            Console.WriteLine("Update Complete!");
        }
        else
        {
            Console.WriteLine("LocalSettings.php not found at target location. Exiting...");
        }
    }

    static void PrintManual()
    {

            Console.WriteLine("This script updates the IP address or web address of the mediawiki server in the LocalSettings.php file.");
            Console.WriteLine("Syntax: MWaddress <New IP address of mediawiki server>");
            Console.WriteLine("        MWaddress -check   - Checks the current IP in LocalSettings.php");
            Console.WriteLine("        MWaddress -v <New IP address of mediawiki server>   - Verbose. Prints any exceptions in full."); 
    }

    static void CheckCurrentAddress()
    {
        string[] lines = File.ReadAllLines(LocalSettingsPath);
        bool foundLine = false;

        for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
        {
            if(lines[currentLine-1].StartsWith(searchLine))
            {
                Console.WriteLine("The current line for $wgServer in LocalSettings.php is:");
                Console.WriteLine(lines[currentLine-1]);
                foundLine = true;
            }
        }

        if(foundLine==false)
        {
            Console.WriteLine("Error: Could not find the definition for $wgServer. Something is terribly wrong!");
        }
    }
}

        
}

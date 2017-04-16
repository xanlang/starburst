using System;
using System.IO;
using System.Text.RegularExpressions;

namespace XanWiki 
{

class AddressFixer
{
    static bool debugMode = false;
    
    static string LocalSettingsPath = "/var/www/html/mediawiki/LocalSettings.php";
    static string TempLocalSettingsFile = "/var/www/html/mediawiki/LocalSettings-temp.php";
    static string searchLine = "$wgServer";    

    public AddressFixer()
    {
    }
                
    public static void UpdateLocalSettingsIP(string pAddress, bool printErrors)
    {

        if(File.Exists(LocalSettingsPath))
        {
            if(debugMode) {Console.WriteLine("LocalSettings.php found.");}

            try 
            {
                if(File.Exists(TempLocalSettingsFile))
                {
                    if(debugMode) {Console.WriteLine("Clearing previous temporary file...");}
                    File.Delete(TempLocalSettingsFile);
                }
            }  
            catch (Exception e)
            {
                if (debugMode) {Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");}
                if (printErrors) { if(debugMode){Console.WriteLine(e);} }
                return;
            }

            string lineToWrite = "$wgServer = \"http://" + pAddress + "\";";

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
                            if(debugMode) {Console.WriteLine("Updating IP address to: http://" + pAddress);}
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
                if(debugMode) {Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");}
                if (printErrors) { if(debugMode) {Console.WriteLine(e); }}
                return;
            }
            

          
            if(debugMode) {Console.WriteLine("Saving new LocalSettings.php file.");}
            string tempPath = LocalSettingsPath + ".b";
            if(File.Exists(tempPath))
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch (UnauthorizedAccessException e)
                {
                    if(debugMode){Console.WriteLine("Unauthorized Access Exception when trying to delete existing temporary local settings file. Try running as root (sudo)");}
                    if (printErrors) { if(debugMode) {Console.WriteLine(e); }}
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
                if(debugMode) {Console.WriteLine("Unauthorized Access Exception when trying to save the updated LocalSettings file. Try running as root (sudo)");}
                if (printErrors) { if(debugMode) {Console.WriteLine(e); }}
                return;
            }
            

            if(debugMode) {Console.WriteLine("LocalSettings.php Update Complete!");}
        }
        else
        {
            if(debugMode) {Console.WriteLine("LocalSettings.php not found at target location. Exiting...");}
        }
    }

    static void PrintManual()
    {

            if(debugMode) {Console.WriteLine("This script updates the IP address or web address of the mediawiki server in the LocalSettings.php file.");}
            if(debugMode) {Console.WriteLine("Syntax: MWaddress <New IP address of mediawiki server>");}
            if(debugMode) {Console.WriteLine("        MWaddress -check   - Checks the current IP in LocalSettings.php");}
            if(debugMode) {Console.WriteLine("        MWaddress -v <New IP address of mediawiki server>   - Verbose. Prints any exceptions in full."); }
    }

    public static string CheckCurrentAddress()
    {
        string[] lines = File.ReadAllLines(LocalSettingsPath);

        for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
        {
            if(lines[currentLine-1].StartsWith(searchLine))
            {
                if(debugMode) {Console.WriteLine("The current line for $wgServer in LocalSettings.php is:");}
                if(debugMode) {Console.WriteLine(lines[currentLine-1]);}
                string currentIP = Regex.Match(lines[currentLine-1],@"http://(.+)""",RegexOptions.Singleline).Groups[1].Value;
                return currentIP; // if the regex doesnt find a match, it returns String.Empty
            }
        }

        return String.Empty;
    }
}

        
}

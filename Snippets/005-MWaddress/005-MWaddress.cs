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

        

        if(args.Length != 1)
        {
            Console.WriteLine("Syntax: 005-MWaddress <New IP address of mediawiki server>");
            return;
        }

        if(File.Exists(LocalSettingsPath))
        {
            Console.WriteLine("LocalSettings.php found.");

            if(File.Exists(TempLocalSettingsFile))
            {
                File.Delete(TempLocalSettingsFile);
                Console.WriteLine("Clearing previous temporary file...");
            }

            string lineToWrite = "$wgServer = \"http://" + args[0] + "\";";

            string[] lines = File.ReadAllLines(LocalSettingsPath);

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

          
            Console.WriteLine("Saving new LocalSettings.php file.");
            string tempPath = LocalSettingsPath + ".b";
            if(File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            File.Move(LocalSettingsPath,tempPath);
            File.Move(TempLocalSettingsFile,LocalSettingsPath);

            Console.WriteLine("Update Complete!");
        }
        else
        {
            Console.WriteLine("LocalSettings.php not found at target location. Exiting...");
        }
    }
}

        
}

/**
 *
 *  Credential Manager
 *  namespace xantools
 *
 *  Credential Manager is a standardized way to keep credentials and other 
 *  sensitive data (passwords, urls, etc) outside of the source classes
 *  so that the source classes can be kept in a git repo without exposing
 *  secure information.
 *
 *  Usage:
 *
 *  // constructor
 *  CredentialManager myCred = new CredentialManager();
 *
 *  // loading the file
 *
 *  if(myCred.LoadCreds(string filePath))
 *  {
 *   //successfully loaded
 *  }
 *  
 *  // getting a value, based on a key
 *  try
 *  {
 *      myDef = myCred.GetDef(string pKey);
 *  }
 *  catch (Exception e)
 *  {
 *      if(e is KeyNotFoundException)
 *      {
 *      }
 *      if(e is CredFileNotFoundException)
 *      {
 *      }
 *  }
 *
 *  // Currently there is no way to update credentials, but I suppose that 
 *  // could be added later
 *
 *
 *
 *
 * */



using System;
using System.Collections.Generic;

namespace xantools
{

public class CredentialManager
{

    Dictionary<string, string> keyDictionary;
    static char[] delimiterChars = { ' ' };

    public CredentialManager()
    {
        keyDictionary = new Dictionary<string, string>();
    }

    public bool LoadCreds(string pFilePath)
    {
        // load credentials into a dictionary
        // based on the given filepath
        // catch exception if filepath not found.
        //
        
        keyDictionary.Clear(); // empty the dictionary if it was previously initialized

        try
        {
            System.IO.StreamReader file = new System.IO.StreamReader(pFilePath);
            
            string line = String.Empty;

            while((line = file.ReadLine()) != null)
            {
                string[] tempLines = line.Split(delimiterChars);
                if(tempLines.Length != 2)
                {
                    // error
                    Console.WriteLine("Error, the following line in the credentials file is not formatted correctly:");
                    Console.WriteLine(line);
                    Console.WriteLine("Line not added.");
                } 
                else
                {
                    // decided not to use Add, since it throws an exception if a key already exists
                    //keyDictionary.Add(tempLines[0],tempLines[1]);

                    //if a key exists, this will simply overwrite the previous value
                    keyDictionary[tempLines[0]] = tempLines[1];
                }
            }

        file.Close();
        
        return true; // successfully loaded.

        }
        catch (Exception e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        return false;
    }
    
    public string GetDef(string pKey)
    {
        // check if we've loaded in the credentials
        if(keyDictionary.Count == 0)
        {
            throw new CredFileNotLoadedException();
        }
        
        string def; // the value for the key
        if (keyDictionary.TryGetValue(pKey, out def))
        {
            return def;
        }
        else
        {
            throw new KeyNotFoundException("The key '" + pKey + "' was not found in the credentials.");
        }
        
    }



}

public class KeyNotFoundException : Exception
{
    public KeyNotFoundException()
    {
    }

    public KeyNotFoundException(string message) 
        : base(message)
    {
    }

    public KeyNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }

}

public class CredFileNotLoadedException : Exception
{
    public CredFileNotLoadedException()
    {
    }

    public CredFileNotLoadedException(string message)
        : base(message)
    {
    }

    public CredFileNotLoadedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}


}

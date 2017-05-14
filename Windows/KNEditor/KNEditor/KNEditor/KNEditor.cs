using System;
using System.Collections.Generic;

namespace KNEditor
{
    //keynote file editor for REVIT 2017

    public class KNEditor
    {

        static char[] delimiterChars = { '\t' };

        Dictionary<string, string[]> KNDictionary;
        List<string> KNCategories;

        string keyNoteFilePath;

        public KNEditor()
        {
            KNDictionary = new Dictionary<string, string[]>();
            KNCategories = new List<string>();
        }

        // This loads keynotes from a REVIT keynotes file.
        public void LoadKeyNotesFromFile(string pFilePath)
        {
            keyNoteFilePath = pFilePath;
            KNDictionary.Clear(); // empty the dictionary if it was previously initialized.
            
            try
            {

                System.IO.StreamReader file = new System.IO.StreamReader(keyNoteFilePath);

                string line = String.Empty;

                while ((line = file.ReadLine()) != null)
                {

                    if (line != "") // skip blank lines
                    {

                        string[] tempLines = line.Split(delimiterChars);

                        if (tempLines.Length == 1)
                        {
                            // this is a category definition
                            if (!CheckIfInList(tempLines[0], KNCategories))
                            {
                                // category doesn't exist. add it.
                                KNCategories.Add(tempLines[0]);
                            }
                            else
                            {
                                // It's kind of strange if this happens.
                                ELog.LogError("Category '" + tempLines[0] + "' already exists... This will be fixed upon save.");
                            }
                        }
                        else if (tempLines.Length == 3)
                        {
                            // this is a key definition
                            if (CheckIfInList(tempLines[0], GetKeys()))
                            {
                                // check if the key is already present in the file. If so, we are going to ignore 
                                // the duplicate line.
                                ELog.LogError("Key '" + tempLines[0] + "' already exists.");
                                ELog.LogError("The line: '" + line + " , will be discarded upon save.");
                            }
                            else
                            {
                                // key does not exist.

                                if (!CheckIfInList(tempLines[2], KNCategories)) // make sure that the category listed on the line exists
                                {
                                    // it does not exist...
                                    if (!KNCategories.Contains("Error")) // check if an Error category already exists. 
                                    {
                                        KNCategories.Add("Error"); // if not, add it.
                                    }

                                    tempLines[2] = "Error"; // change the category to Error.
                                }

                                KNDictionary.Add(tempLines[0], new string[] { tempLines[1], tempLines[2] }); // add the entry into the dictionary.
                                //Console.WriteLine("Added : " + tempLines[0] + " , " + tempLines[1] + " , " + tempLines[2]);
                            }

                        }
                        else
                        {
                            // error. The line won't be read by this script.
                            ELog.LogError("Line: " + line + " does not have the correct number of terms.");
                        }
                    }
                }

            } catch (Exception e)
            {
                // if there was an error in loading or reading from the file
                ELog.LogError("The file could not be read:");
                ELog.LogLine(e.Message);
            }

        }

        public List<string> GetCategories()
        {
            return KNCategories;
        }

        public List<string> GetKeys()
        {
            List<string> keyList = new List<string>(KNDictionary.Keys);
            return keyList;
        }

        public Dictionary<string,string[]> GetDictionary()
        {
            return KNDictionary;
        }

        public void AddCategory(string pCategory)
        {
            if(!CheckIfInList(pCategory, KNCategories))
            {
                // add the category if it's not already in the list
                KNCategories.Add(pCategory);
            }
        }

        public void EnterKeyNote(string pKey, string pDescription, string pCategory)
        {

            // check if category exists
            if(CheckIfInList(pCategory, KNCategories))
            {
                // it exists. Continue.
                if (CheckIfInList(pKey, GetKeys()))
                {
                    // if the key exists, update the entry.
                    KNDictionary[pKey] = new string[] { pDescription, pCategory };

                } else
                {
                    // the key doesn't exist. Add a whole new entry.
                    KNDictionary.Add(pKey, new string[] { pDescription, pCategory });
                }


            } else
            {
                // Category doesn't exist. fail.
                ELog.LogError("Invalid Category: " + pCategory);
            }

        }

        public void DeleteKeyNote(string pKey)
        {
            if (CheckIfInList(pKey, GetKeys()))
            {
                KNDictionary.Remove(pKey);
            } else
            {
                ELog.LogError("Could not delete entry for key '" + pKey + ". Key does not exist.");
            }
        }


        private bool CheckIfInList(string pTheString, List<string> pListToCheck)
        {
            // checks pTheString against all the values in pArrayToCheck.
            // if pTheString already exists in pArrayToCheck, returns true
            // else, returns false

            if (pListToCheck.Count == 0)
            {
                return false; // the array is empty so there are no duplicates by default
            }

            for (int i = 0; i < pListToCheck.Count; i++)
            {
                if (pTheString == pListToCheck[i])
                {
                    return true; // there is a duplicate
                }
            }

            return false;
        }
    }


}
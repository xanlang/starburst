using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KNEditor
{
    class Interface
    {
       
        public static void PopulateTreeview(List<string> pCategoryList, Dictionary<string,string[]> pDictionary, TreeView pFormTreeView)
        {
            int numberOfCategories = pCategoryList.Count;
            pCategoryList.Sort();
            List<string> sortedCategoryList = pCategoryList;

            Console.WriteLine("Printing Category List");
            PrintList(sortedCategoryList);

            // we are going to make an array of Lists, with each list being one category.
            List<string>[] categoryArray = new List<string>[numberOfCategories];
            
            for(int i =0;i < numberOfCategories; i++)
            {
                // initialize the lists in the categoryArray.
                categoryArray[i] = new List<string>();
            }

            foreach(KeyValuePair<string,string[]> entry in pDictionary)
            {
                // go through the whole dictionary and add the entries in the keynotes file to the list
                // sorted by category.
                
                // find index # of the category
                int categoryIndex = sortedCategoryList.IndexOf(entry.Value[1]);
                

                //Console.WriteLine("The Index for Category '" + entry.Value[1] + "' is " + categoryIndex.ToString() + ".");

                // add the key to the CategoryArray
                categoryArray[categoryIndex].Add(entry.Key);
                //Console.WriteLine("Added '" + entry.Key + "' to category " + entry.Value[1] + ", now with " + categoryArray[categoryIndex].Count + " entries.");

            }
            
            // now we have a full array, with each element in the array being a list of keys.
            // now we should sort all of the arrays by their key values, so the keys are in alphabetical order.

            for(int i = 0; i < numberOfCategories; i++)
            {
                categoryArray[i].Sort();
            }

            // now everything is sorted. Let's add them into the treeview.



            // let's create our nodes based on our categories.
            TreeNode[] categoryTreeNodes = new TreeNode[numberOfCategories];

            for (int i = 0; i < numberOfCategories; i++)
            {
                
                // create the child objects for the category
                // these are all our key-description pairs

                List<string> ListOfKeysInCategory = categoryArray[i];
                int numberOfKeys = ListOfKeysInCategory.Count;

                TreeNode[] keyArray = new TreeNode[0];

                if (numberOfKeys > 0)
                {
                    keyArray = new TreeNode[numberOfKeys];

                    for (int j = 0; j < numberOfKeys; j++)
                    {
                        string[] descriptionCategoryPair;
                        
                        pDictionary.TryGetValue(ListOfKeysInCategory[j], out descriptionCategoryPair);

                        string keyNoteDescription = descriptionCategoryPair[0];

                        keyArray[j] = new TreeNode(ListOfKeysInCategory[j] + " : " + keyNoteDescription);
                            // we are adding a child node with the format '<key> : <description>'
                            // later this should be changed to be more useful. Like columns or something.
                        
                    }
                }
                // so after this super long if statement, we have an array of treenodes called keyArray
                // as long as numberOfKeys in this category is more than 0.
                
                if(numberOfKeys == 0)
                {

                    categoryTreeNodes[i] = new TreeNode(sortedCategoryList[i]);

                    // if there are 0 keys in this category, then add an empty node.
                    pFormTreeView.Nodes.Add(categoryTreeNodes[i]);
                }
                else
                {
                    // otherwise add the node with children.
                    categoryTreeNodes[i] = new TreeNode(sortedCategoryList[i], keyArray);

                    pFormTreeView.Nodes.Add(categoryTreeNodes[i]);
                }
                
                
            }
            //after this for loop has iterated through all of the Categories, we should have a completed TreeView.
            

        }

        private static void PrintList(List<string> pList)
        {

            if (pList.Count == 0)
                return;

            for(int i = 0; i < pList.Count; i++)
            {
                Console.WriteLine("[" + i + "] " + pList[i]);
            }

        }

    }
}

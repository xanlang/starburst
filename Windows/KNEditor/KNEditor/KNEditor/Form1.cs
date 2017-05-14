using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KNEditor
{
    public partial class Form1 : Form
    {

        KNEditor theEditor;

        public Form1()
        {
            InitializeComponent();

            theEditor = new KNEditor();
            
        }

        private void ButtonLoadFile_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();

            if(result == DialogResult.OK)
            {
                theEditor.LoadKeyNotesFromFile(openFileDialog1.FileName);
            }

            // original load from file, manually specifying the location.
            //theEditor.LoadKeyNotesFromFile("G:\\MAY\\CODE\\GitRepos\\starburst\\Windows\\KNEditor\\KNEditor\\keyNote.txt");

            List<string> myCategoryList = theEditor.GetCategories();
            Dictionary<string, string[]> myDictionary = theEditor.GetDictionary();

            Interface.PopulateTreeview(myCategoryList, myDictionary, TreeViewKeyNote);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}

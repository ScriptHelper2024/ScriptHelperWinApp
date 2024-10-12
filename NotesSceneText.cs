using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHelper
{
    public class NotesSceneText
    {
        public string myText { get; set; }
        public string myNote { get; set; }

        public string myLabel { get; set; }

        public string myBeatSheet { get; set; }

        public string myScript { get; set; }
        public List<NotesSceneScript> myScripts { get; set; }
        public NotesSceneText(string t, string n, string l, string s)
        {
            myText = t;
            myNote = n;
            myLabel = l;
            myScript = s;

            myBeatSheet = "";
            
            myScripts = new List<NotesSceneScript>();

        }

        public void addToSceneList(string b, string s, string n, string l)
        {
            myScripts.Add(new NotesSceneScript(b, s, n, l));
        }
    }

    public class NotesSceneScript
    {

        public string myBeatSheet { get; set; }
        public string myScript { get; set; }
        public string myNote { get; set; }
        public string myLabel { get; set; }

        public NotesSceneScript(string b, string s, string n,string l)
        {

            myBeatSheet = b;
            myScript = s;
            myNote = n;
            myLabel = l;

        }


    }
}

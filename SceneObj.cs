using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHelper
{
    public class SceneObj
    {
        public Guid SceneID { get; set; }

        public Boolean splitSceneMakeFlag { get; set; }
        public string Title { get; set; }
        public string SettingKey { get; set; }
        public string Hint { get; set; }
        public string compressedHint { get; set; }
        public string NarrativeText { get; set; }

        public string BeatSheetText { get; set; }

        public string SceneScript { get; set; }
        public string DialogFlavor { get; set; }

        public string CustomFlavor { get; set; }

        public List<NotesSceneText> myNoteTextList { get; set; }
        public SceneObj()
        {
            SceneID = Guid.NewGuid();
            Title = "";
            SettingKey = "";
            Hint = "";
            compressedHint = "";
            NarrativeText = "";
            BeatSheetText = "";
            SceneScript = "";
            DialogFlavor = "FlavorNone";
            myNoteTextList = new List<NotesSceneText>();
            splitSceneMakeFlag = false;
        }

    }
}

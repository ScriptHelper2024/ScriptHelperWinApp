using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScriptHelper
{
    public partial class FormApp1
    {
        private string makePrototypeMovieHint()
        {
            string hint = @"a 19 year old man <Robert> meets a 39 year old married woman <Beth>, and they both fall in love.  A passionate affair ensues.  <Beth> is married to <Oscar> a successful but cold and harsh 55 year old lawyer.    <Sally> the 12 year old daughter of <Beth> and <Oscar> knows about <Beth>'s affair with <Robert> and pleads with <Beth> to end it.  <Sally> warns <Beth> that <Oscar> is dangerous.   The affair ends tragically when <Oscar> kills <Robert> and <Beth>.  As a prominent citizen and using his skills as a lawyer, <Oscar> is not suspected by the police and he gets away with the crime.  But <Sally> knows that he did it, and torments him for the rest of his life including on his death bed 20 years later. ";
            return hint;
        }

        public void updateGPTErrorMsg(string errorMsg, string exMessage)
        {
            if (errorMsg.Trim().Length > 0)
            {
                errorMsg = "GPT Error: " + errorMsg + " see Debug";
            }
                
            ErrorMessage.Text = errorMsg;

            if (errorMsg.Length > 0)
            {
                ErrorLogBox.Text += exMessage + "\r\n";
            }

            Application.DoEvents();

        }

        public async Task<string> doMakeMovieText(string input)
        {

            string reply = await MyGPT.makeMovieText(input, GetGptModel(), (int)TimeLength.Value, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this, false, characters);

            return reply.ToString();
        }

        public async Task<int> doMakeScenesFromMovieText()
        {
            List<List<string>> ListofLists = new List<List<string>>();
            string sceneListLabel = "";
            if (MovieText.Text.Length < 200)
            {
                MessageBox.Show("Not Enough Movie Text.  Need at least 200 characters ");
            }

            else
            {
                sceneListLabel = Utils.rightOfArrow(myMovie.myNoteTextList[NotesList.SelectedIndex].myLabel);
                DateTime currentTime = DateTime.Now;
                string formattedTime = currentTime.ToString("yyyy:MM:dd:HH:mm");
                MovieSceneListLabel.Text = "Scenes from " + sceneListLabel + " using: " + GetGptModel() + " @ " + formattedTime;

                Utils.dataDictionary["SceneMaker"] = MovieSceneListLabel.Text.Trim();
                

                SceneDescriptions.Clear();
                SceneDescriptions.Text = Utils.makePendingMessage(GetGptModel());


                bool looper = true;
                int errorKount = 0;
                string jsonString = "";
                string originalJSONString = "";
                jsonString = await MyGPT.makeScenesFromMovieText(MovieText.Text, GetGptModel(), (int)SceneCount.Value, "", this);

                jsonString = Utils.TrimOutsideBrackets(jsonString);
                isError = Utils.checkForGPTErrors(jsonString, this);
                if (isError)
                {
                    return 0;
                }



                originalJSONString = jsonString;
                while (looper == true)
                {
                    try
                    {

                        ListofLists = JsonConvert.DeserializeObject<List<List<string>>>(jsonString);
                        SceneDescriptions.Text = jsonString;
                        looper = false;
                    }

                    catch (Exception ex)

                    {

                        if (errorKount == 0)

                        {
                            errorKount += 1;
                            jsonString = Utils.JSONFixer(originalJSONString);
                            Utils.nop();
                            looper = true;
                        }
                        else if (errorKount > 2)

                        {
                            updateGPTStatus("GPT Status: Unrepairable JSON error.  Make small adjustment in number of scenes and try again.", Color.LightGreen);
                            return 0;
                        }
                        else
                        {
                            errorKount += 1;
                            SceneDescriptions.Text = "error - trying to repair JSON. kount = " + errorKount.ToString() + "\r\n" + originalJSONString;
                            jsonString = await MyGPT.fixJSON(originalJSONString, GetGptModel(), $"JSON Error - trying to repair JSON.  Error Count = {errorKount}",this);
                            isError = Utils.checkForGPTErrors(jsonString, this);
                            if (isError)
                            {
                                return 0;
                            }


                            looper = true;
                        }

                    }
                }



                scenes = new List<SceneObj>();
                updateRTBText(ScenesListMovieTabRTB, "");
                string oldTitle = "";
                string rtfString = string.Empty;
                foreach (List<string> myScene in ListofLists)
                {

                    SceneObj scene = new SceneObj();
                    string myTitle = myScene[0];
                    string Hint = myScene[1];
                    scene.Title = myTitle;
                    scene.Hint = Hint;

                    if (myTitle != oldTitle)
                    {
                        
                        scenes.Add(scene);
                    }
                    
                    oldTitle = myTitle;
                }

                ScenesList.DataSource = null;
                ScenesList.DataSource = scenes;
                ScenesList.DisplayMember = "Title";


                updateRTBText(SceneText, "");
                updateRTBText(BeatSheetRichTextbox, "");
                updateRTBText(SceneScriptRichTextbox, "");
                updateRTBText(SceneNoteRichTextBox, "");
                updateRTBText(ScriptNotesRTB, "");

                SceneNotesListbox.DataSource = null;
                ScriptNotesListbox.DataSource = null;
                if (Utils.newScenesFlag == true)
                {
                    ScriptNotesListbox.DataSource = null;
                }
                


                
                rtfString = Utils.makeScenesRTF(scenes);

                ScenesListMovieTabRTB.Rtf = rtfString;
                Utils.newScenesFlag = true;
            }

            return 0;


        }

        public async Task<string> doMakeSceneText()
        {

            if (scenes[SceneInScenesList.SelectedIndex].splitSceneMakeFlag)
            {
                if (characters == null)
                {
                    MessageBox.Show("No characters found.  Please click the 'Gather Characters' button");
                    return null;
                }

                if (characters.Count == 0)
                {
                    MessageBox.Show("No characters found.  Please click the 'Gather Characters' button");
                    return null;
                }
            }

            int sceneIndex = SceneInScenesList.SelectedIndex;
            int noteIndex = SceneNotesListbox.SelectedIndex;
            string reply = "";
            // SceneText.Text = GetGptModel() + " awaiting reply...\r\n \r\n " + SceneHint.Text;
            updateRTBText(SceneText,GetGptModel() + " awaiting reply...\r\n \r\n " + SceneHint.Text);

            cursorTopRTB(SceneText);
            
            if (scenes[SceneInScenesList.SelectedIndex].splitSceneMakeFlag)
            {
                reply = await MyGPT.makeSplitSceneTextWithCharacterGuidance(GetGptModel(), myMovie, scenes, characters, SceneInScenesList.SelectedIndex + 1, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
                //reply = await MyGPT.makeSplitSceneText(GetGptModel(), myMovie, scenes, SceneInScenesList.SelectedIndex + 1, this);
                
            }
            else
            {
                 reply = await MyGPT.makeSceneText(GetGptModel(), myMovie, scenes, SceneInScenesList.SelectedIndex + 1, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
            }
            

            //SceneText.Text = reply;
            updateRTBText(SceneText, reply);
            updateRTBText(BeatSheetRichTextbox, "");

            scenes[SceneInScenesList.SelectedIndex].NarrativeText = reply;
            
            if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count == 0)
            {
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList = new List<NotesSceneText>();
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Add(new NotesSceneText(SceneText.Text, "", "Base:", ""));
            }
            else if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count > 0)
            {
                string mylabel = makeSceneMenuLabel(scenes[sceneIndex].myNoteTextList, " New", noteIndex);
                scenes[sceneIndex].myNoteTextList.Add(new NotesSceneText(SceneText.Text, "", mylabel, ""));
                updateRTBText(SceneScriptRichTextbox, "");
                updateRTBText(BeatSheetRichTextbox, "");
                updateRTBText(ScriptNotesRTB, "");
            }
            
            
            SceneNotesListbox.BeginUpdate();
            SceneNotesListbox.DataSource = null;
            SceneNotesListbox.DisplayMember = null;
            SceneNotesListbox.DataSource = scenes[sceneIndex].myNoteTextList;
            SceneNotesListbox.DisplayMember = "myLabel";
            SceneNotesListbox.SelectedIndex = SceneNotesListbox.Items.Count - 1;
            SceneNotesListbox.EndUpdate();

            Application.DoEvents();
            return "";
        }

        public async Task<string> doMakeBeatSheet()
        {
            if (SceneText.Text.Length > 50)
            {
                

                string reply = await MyGPT.makeBeatSheet(myMovie, SceneText.Text, GetGptModel(), (StyleElements)StyleSceneList.SelectedItem,this);
                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    return reply;
                }

                scenes[SceneInScenesList.SelectedIndex].BeatSheetText = reply;
                return reply;
            }
            else
            {
                MessageBox.Show("Not Enough Scene Text.  Need at least 50 characters ");
                return "";
            }
        }

        public async Task<string> doWriteSceneScript()
        {

            string dialogFlavor = getDialogFlavor();
            string customFlavor = "";

            if (dialogFlavor == "FlavorCustom")
            { 
                customFlavor = FlavorCustomText.Text;
                
            }
            else
            {
                customFlavor = "";
            }


            string reply = await MyGPT.makeSceneScript(myMovie, characters, BeatSheetRichTextbox.Text, SceneText.Text, GetGptModel(), GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem,(StyleElements)StyleSceneList.SelectedItem,dialogFlavor,customFlavor, this);
                                              
            scenes[SceneInScenesList.SelectedIndex].SceneScript = reply;

            return reply;
        }

        private Boolean StartUp(string filename)
        {

            string jsonString = "";
            string tempJsonString = "";
            List<object> containerList = new List<object>();
            Boolean   abort = true;


            jsonString = File.ReadAllText(filename);

            containerList = JsonConvert.DeserializeObject<List<object>>(jsonString);

            tempJsonString = containerList[0].ToString();
            dataVersion = JsonConvert.DeserializeObject<int>(tempJsonString);

            if (dataVersion > Utils.dataVersion)
            
            {
                MessageBox.Show($"Incompatible Version error!!!\r\n\r\nFile name: {filename}\r\n\r\nData Version: {dataVersion}\r\n\r\nRequires a latter version to read.\r\n\r\n\r\nPlease update to the most recent version of ScriptHelper.");
                abort = true;
                return abort;
            }


            
            tempJsonString = containerList[1].ToString();
            myMovie = JsonConvert.DeserializeObject<MovieObj>(tempJsonString);

            tempJsonString = containerList[2].ToString();
            scenes = JsonConvert.DeserializeObject<List<SceneObj>>(tempJsonString);

            if (Utils.dataDictionary == null)
            {
                Utils.dataDictionary = new Dictionary<string, object>();
            }
            
            if (dataVersion == 1)   // current version is 3
            {
                scenes = Utils.upgradeScenes1to2(scenes);

            }
            if (dataVersion >= 2  && containerList.Count >= 4)  
            {
                tempJsonString = containerList[3].ToString();
                Utils.dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempJsonString);

            }

            if (dataVersion >= 3)
            {
                tempJsonString = containerList[4].ToString();
                characters = JsonConvert.DeserializeObject<List<CharacterObj>>(tempJsonString);

                tempJsonString = containerList[5].ToString();
                charactersInScenes = JsonConvert.DeserializeObject<List<SceneCharacter>>(tempJsonString);
            }


            addLengthToMovieList();
            clearStartUp();
            fillStartUp();
            Movie.SelectedTab = MovieTab;
            NewAuthor.ShortcutsEnabled = true;
            abort = false;
            return abort;

        }

        private void clearStartUp()
        {
            MovieHintText.Text = "";
            MovieText.Text = "";
            NotesForMovieText.Text = "";
            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            ScenesList.DataSource = null;
            ScenesList.DisplayMember = null;
            SceneInScenesList.DataSource = null;
            SceneInScenesList.DisplayMember = null;

            // SceneText.Text = "";
            updateRTBText(SceneText, "");
            Utils.SceneSeedUpdateFlag = false;
            SceneHint.Text = "";
            Utils.SceneSeedUpdateFlag = true;

            SceneNoteRichTextBox.Text = "";
            SceneNotesListbox.DataSource = null;
            SceneNotesListbox.DisplayMember = null;
            ScriptNotesListbox.DataSource = null;
            ScriptNotesListbox.DisplayMember = null;
            BeatSheetRichTextbox.Text = "";
            SceneScriptRichTextbox.Text = "";

            CharacterNotes.Text = "";
            CharacterBiography.Text = "";
            CharacterAge.Text = "";
            CharacterSpeakingStyle.Text = "";
            CharacterPersonality.Text = "";
            if (characters != null)
            {
                if (characters.Count > 0)
                {
                    CharactersMaster.DataSource = null;
                    CharactersMaster.DisplayMember = null;
                    CharactersMaster.DataSource = characters;
                    CharactersMaster.DisplayMember = "tagName";
                    CharactersMaster.SelectedIndex = 0;
                    displayCharacterProfile();
                }
                else
                {
                    CharactersMaster.DataSource = null;
                    CharactersMaster.DisplayMember = null;
                    CharactersMaster.DataSource = characters;
                    CharactersMaster.DisplayMember = "tagName";
                    CharactersMaster.SelectedIndex = -1;
                }

            }
            

        }
        private void fillStartUp()
        {
            MovieHintText.Text = myMovie.movieHintText;
            
            // MovieText.Text = myMovie.movieText;
            updateRTBText(MovieText, myMovie.movieText);
            
            
            if (myMovie.myNoteTextList != null && myMovie.myNoteTextList.Count > 0)
            {
                NotesForMovieText.Text = myMovie.myNoteTextList[0].myNote;

                //MovieText.Text = myMovie.myNoteTextList[0].myMovieText;
                updateRTBText(MovieText, myMovie.myNoteTextList[0].myMovieText);

                NotesList.DataSource = null;
                NotesList.DisplayMember = null;
                NotesList.DataSource = myMovie.myNoteTextList;
                NotesList.DisplayMember = "myLabel";
                NotesList.SelectedIndex = myMovie.myNoteTextList.Count - 1;
            }


            if (dataVersion >= 3)
            {
                if (characters == null || characters.Count == 0 || CharactersMaster.SelectedIndex < 0)
                {

                    Utils.nop();
                }
                else if (CharactersMaster.SelectedIndex >= 0)
                {
                    CharactersMaster.DataSource = null;
                    CharactersMaster.DataSource = characters;
                    CharactersMaster.DisplayMember = "tagName";
                    CharactersMaster.SelectedIndex = 0;
                }
                else { Utils.nop(); }

            }
            

            if (scenes.Count > 0)
            {
                ScenesList.DataSource = null;
                ScenesList.DisplayMember = null;

                ScenesList.DataSource = scenes;
                ScenesList.DisplayMember = "Title";


                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;

                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";

                if (scenes[0].myNoteTextList != null && scenes[0].myNoteTextList.Count > 0)
                {
                    int maxSceneIndex = scenes[0].myNoteTextList.Count - 1;
                    
                    // SceneText.Text = scenes[0].myNoteTextList[maxSceneIndex].myText;
                    updateRTBText(SceneText,scenes[0].myNoteTextList[maxSceneIndex].myText);

                    SceneNoteRichTextBox.Text = scenes[0].myNoteTextList[maxSceneIndex].myNote;
                    SceneNotesListbox.BeginUpdate();
                    SceneNotesListbox.DataSource = null;
                    SceneNotesListbox.DisplayMember = null;
                    SceneNotesListbox.DataSource = scenes[0].myNoteTextList;
                    SceneNotesListbox.DisplayMember = "myLabel";
                    SceneNotesListbox.SelectedIndex = maxSceneIndex;
                    SceneNotesListbox.EndUpdate();

            
                }

                if (Utils.dataVersion > 1 && scenes[0].myNoteTextList.Count > 0)
                {
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[0].myNoteTextList[scenes[0].myNoteTextList.Count -1].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                    ScriptNotesListbox.EndUpdate();
                }
            }

            //EnableTextChangedEvent(MovieText);




        }

        public async void saveCurrentMovie()
        {
            await Task.Delay(200);

            Utils.Saving = true;

            List<object> listContainer = new List<object>();

            listContainer.Add(Utils.dataVersion);
            listContainer.Add(myMovie);
            listContainer.Add(scenes);
            listContainer.Add(Utils.dataDictionary);
            listContainer.Add(characters);
            listContainer.Add(charactersInScenes);

            string jsonString = JsonConvert.SerializeObject(listContainer);

            string fileName = Utils.currentMovieFilename;

            File.WriteAllText(fileName, jsonString);

            titleChangeNotSave = false;
            Utils.Saving = false;

        }

        private void cursorTopRTB(RichTextBox rtb)
        {
            rtb.SelectionStart = 0;
            rtb.SelectionLength = 0;
            rtb.ScrollToCaret();
        }

        private void EnableTextChangedEvent(RichTextBox myBox)
        {
            myBox.TextChanged += boxChanged;
        }

        private void DisableTextChangedEvent(RichTextBox myBox)
        {
            myBox.TextChanged -= boxChanged;
        }

        private void boxChanged(object sender, EventArgs e)
        {

            RichTextBox richTextBoxThatChanged = sender as RichTextBox;
            if (richTextBoxThatChanged == MovieText)
            {
                ShowApplyButton(MovieText);
            }
            if (richTextBoxThatChanged == SceneText)
            {
                ShowApplyButton(SceneText);
            }
            Utils.nop();


        }

        private void HideApplyButton(RichTextBox myBox)
        {
            if (myBox == MovieText)
            {
                ApplyMovieTextEdits.Visible = false;
                ApplyMovieTextEdits.Enabled = false;
                DiscardMovieTextEdits.Visible = false;
                DiscardMovieTextEdits.Enabled = false;
                ShowEditButton(MovieText);
            }

            if (myBox == SceneText)
            {
                ApplySceneTextEdits.Visible = false;
                ApplySceneTextEdits.Enabled = false;
                DiscardSceneTextEdits.Visible = false;
                DiscardSceneTextEdits.Enabled = false;
                ShowEditButton(SceneText);
            }

            if (myBox == SceneScriptRichTextbox)
            {
                ApplyScriptTextEdits.Visible = false;
                ApplyScriptTextEdits.Enabled = false;
                DiscardScriptTextEdits.Visible = false;
                DiscardScriptTextEdits.Enabled = false;
                ShowEditButton(SceneScriptRichTextbox);
            }
        }
        private void ShowApplyButton(RichTextBox myBox)
        {
            if (myBox == MovieText)
            {
                ApplyMovieTextEdits.Visible = true;
                ApplyMovieTextEdits.Enabled = true;
                DiscardMovieTextEdits.Visible = true;
                DiscardMovieTextEdits.Enabled = true;

            }
            if (myBox == SceneText)
            {
                ApplySceneTextEdits.Visible = true;
                ApplySceneTextEdits.Enabled = true;
                DiscardSceneTextEdits.Visible = true;
                DiscardSceneTextEdits.Enabled = true;
            }
            if (myBox == SceneScriptRichTextbox)
            {
                ApplyScriptTextEdits.Visible = true;
                ApplyScriptTextEdits.Enabled = true;
                DiscardScriptTextEdits.Visible = true;
                DiscardScriptTextEdits.Enabled = true;
            }
        }

        private string makeMovieMenuLabel(List<NotesMovieText> myList, string myType, int selected, int textLength)
        {
            
            
            string result = "";

            int resultLength = 0;
            if (myType.Contains("Base"))
            {
                result = "Base:";
                result = result + textLength.ToString().PadLeft(30 - result.Length, ' ');
                return result;
                
            
            }

            string leftSelected = Utils.rightOfArrow(myList[selected].myLabel);

            if (myType.Contains("New")) {leftSelected = "New"; }

            leftSelected = leftSelected.PadLeft(6, ' ');

            
            int myListCount = myList.Count;
            if (myListCount > 0)
            {
                int notes = 0;
                int edits = 0;
                foreach (NotesMovieText note in myList)
                {
                    if (note.myLabel.ToUpper().Contains("NOTE")) { notes++; }
                    if (note.myLabel.ToUpper().Contains("EDIT")) { edits++; }
                }

                
                
                result = myType + ": " + leftSelected + " -> Ver " + (myListCount + 1).ToString();
                               

            }


            resultLength = result.Length;
            result = result + textLength.ToString().PadLeft(30 - resultLength, ' ');
            
            return result;

        }

        private string makeMovieMenuLabel(List<NotesMovieText> myList, string myType, int selected,Boolean jumpNewFlag)
        {
            string leftSelected = Utils.rightOfArrow(myList[selected].myLabel);
            if (selected == 0) { leftSelected = "Base"; }

            leftSelected = leftSelected.PadLeft(6, ' ');

            string result = "";
            int myListCount = myList.Count;
            if (myListCount > 0)
            {
                int notes = 0;
                int edits = 0;
                foreach (NotesMovieText note in myList)
                {
                    if (note.myLabel.ToUpper().Contains("NOTE")) { notes++; }
                    if (note.myLabel.ToUpper().Contains("EDIT")) { edits++; }
                }



                if (jumpNewFlag)
                {
                    result = myType + ": " + "   New" + " -> Ver " + (myListCount + 1).ToString();
                }
                else
                {
                    result = myType + ": " + leftSelected + " -> Ver " + (myListCount + 1).ToString();
                }
                    
                    


            }
            return result;

        }
        private string makeSceneMenuLabel(List<NotesSceneText> myList, string myType, int selected)
        {
            string leftSelected = Utils.rightOfArrow(myList[selected].myLabel);
            if (selected == 0) { leftSelected = "Base"; }
            if (myType.Contains("New"))
            {
                leftSelected = "New";
            }
            leftSelected = leftSelected.PadLeft(6, ' ');

            string result = "";
            int myListCount = myList.Count;
            if (myListCount > 0)
            {
                int notes = 0;
                int edits = 0;
                foreach (NotesSceneText note in myList)
                {
                    if (note.myLabel.ToUpper().Contains("NOTE")) { notes++; }
                    if (note.myLabel.ToUpper().Contains("EDIT")) { edits++; }
                }



                result = myType + ": " + leftSelected + " -> Ver " + (myListCount + 1).ToString();


            }
            return result;

        }

        private string makeSceneScriptMenuLabel(List<NotesSceneScript> myList, string myType, int selected)
        {
            string leftSelected = Utils.rightOfArrow(myList[selected].myLabel);
            if (selected == 0) { leftSelected = "Base"; }
            if (myType.Contains("New") ){ leftSelected = "New";}
            leftSelected = leftSelected.PadLeft(6, ' ');

            string result = "";
            int myListCount = myList.Count;
            if (myListCount > 0)
            {
                int notes = 0;
                int edits = 0;
                foreach (NotesSceneScript note in myList)
                {
                    if (note.myLabel.ToUpper().Contains("NOTE")) { notes++; }
                    if (note.myLabel.ToUpper().Contains("EDIT")) { edits++; }
                }



                result = myType + ": " + leftSelected + " -> Ver " + (myListCount + 1).ToString();


            }
            return result;

        }
        private Boolean beginMovie()
        {

            int sceneKount = 0;
            Boolean thisAbort = true;
            Boolean abort = true;
            Utils.updateScriptStyleToDictionaryFlag = false;
            Utils.updateMovieStyleToDictionaryFlag = false;

            Utils.startupFlag = true;

            abort = StartUp(Utils.currentMovieFilename);
            
            if (abort) 
            {
                thisAbort = true;
                return thisAbort; 
            }
            
            sceneKount = scenes.Count;
            

            if (sceneKount == 0)
            {
                SceneCount.Value = Utils.originalSceneCount;
                Utils.currentSceneCount = Utils.originalSceneCount;
            }
            else
            {
                SceneCount.Value = sceneKount;
                Utils.currentSceneCount = sceneKount;
            }
            
            MovieTitle.Text = "Title: " + myMovie.title;

            ScenesList.DataSource = scenes;
            SceneInScenesList.DataSource = scenes;

            if (sceneKount > 0)
            {
                ScenesListMovieTabRTB.Rtf = Utils.makeScenesRTF(scenes);
            }
            else
            {
                ScenesListMovieTabRTB.Rtf = Utils.makeRTFEmpty();
            }
                
            
            ScenesList.DisplayMember = "Title";

            SceneInScenesList.DisplayMember = "Title";

            
            thisAbort = false;

            TopMovieTitle.Text = "Title: " + myMovie.title;

            updateSceneStyleListboxes();

            updateMovieTextStyleListboxes();

            updateTextBoxes();

            updateCheckboxes();

            updateTimeLength();

            updateLabels();

            Utils.startupFlag = false;

            return thisAbort;

        }

        private void updateLabels()

        {
            string outString = "";
            object retrievedObject;

            if (Utils.dataDictionary.TryGetValue("SceneMaker", out retrievedObject))

            {
                outString = (string)retrievedObject;

            }
            else
            {
                outString = "Scenes List";
            }

            TopMovieTitle.Text = "Title: " + myMovie.title;
            MovieSceneListLabel.Text = outString;


        }
        private void updateCheckboxes()
        {
            object retrievedObject;
            Boolean formatFlag;

            

            GlobalDialogFormat.Checked = false;
            GlobalDialogFormat.Visible = false;
            GlobalDialogFormat.Enabled= false;


            if (Utils.dataDictionary.TryGetValue("ScriptFormat",out retrievedObject))
            {

                FormatScript.Checked = (Boolean)retrievedObject;
            }

            UseProfileMovieText.Checked = myMovie.useProfileMovieText;
            UseProfileSceneText.Checked = myMovie.useProfileSceneText;
            UseProfileSceneScript.Checked = myMovie.useProfileSceneScript;
            UseProfileMakeScenes.Checked = myMovie.useProfileMakeScenes;
        }

        private void updateTimeLength()
        {
            object retrievedObject;
            if (Utils.dataDictionary.TryGetValue("TimeLength", out retrievedObject))
            {
                string temp = "";
                temp = retrievedObject.ToString();
                
                TimeLength.Value  = decimal.Parse(temp);
                myMovie.timeLength = (int)TimeLength.Value;
            }
            else
            {
                TimeLength.Value = myMovie.timeLength;
            }
        }

        private void updateSceneStyleListboxes()
        {
            object retrievedObject;
            string authorLabel;
            string guideLabel;
            int authorIndex;
            int guideIndex;

            Utils.updateScriptStyleToDictionaryFlag = false;

            if (Utils.dataDictionary.TryGetValue("StyleAuthorsIndex", out retrievedObject))
            {
                authorLabel = retrievedObject.ToString();
                authorIndex = getStyleIndex(styles,authorLabel);
            }
            else
            {
                authorIndex = 0;
            }

            if (Utils.dataDictionary.TryGetValue("StyleGuidesIndex", out retrievedObject))
            {
                guideLabel = retrievedObject.ToString();
                guideIndex = getStyleIndex(scriptStyleGuides, guideLabel);
            }
            else
            {
                guideIndex = 0;
            }

            usaRatings = Utils.getUSARatings();
            int tempIndex = usaRatings.IndexOf(myMovie.ratingUSA);

            USARatings.DataSource = usaRatings;

            // If the string is found, index will be >= 0
            if (tempIndex < 0) 
            { 
                tempIndex = 0; 
            }
            
            USARatings.SelectedIndex = tempIndex;
            
            StyleSceneList.DataSource = styles;
            StyleSceneList.DisplayMember = "label";
            StyleSceneList.SelectedIndex = authorIndex;
            
            ScriptStyleGuideListbox.DataSource = scriptStyleGuides;
            ScriptStyleGuideListbox.DisplayMember = "label";
            ScriptStyleGuideListbox.SelectedIndex = guideIndex;
            Utils.updateScriptStyleToDictionaryFlag = true;
        }

        private void updateMovieTextStyleListboxes()
        {
            object retrievedObject;
            string guideLabel;
            int guideIndex;

            Utils.updateMovieStyleToDictionaryFlag = false;

            if (Utils.dataDictionary.TryGetValue("MovieStyleGuidesIndex", out retrievedObject))
            {
                guideLabel = retrievedObject.ToString();
                guideIndex = getStyleIndex(movieTextStyleGuides, guideLabel);
            }
            else
            {
                guideIndex = 0;
            }

            //usaRatings = Utils.getUSARatings();
            //int tempIndex = usaRatings.IndexOf(myMovie.ratingUSA);

            //USARatings.DataSource = usaRatings;

            //// If the string is found, index will be >= 0
            //if (tempIndex < 0)
            //{
            //    tempIndex = 0;
            //}

            //USARatings.SelectedIndex = tempIndex;

            MovieTextStylesGuideListbox.DataSource = movieTextStyleGuides;
            MovieTextStylesGuideListbox.DisplayMember = "label";
            MovieTextStylesGuideListbox.SelectedIndex = guideIndex;
            Utils.updateMovieStyleToDictionaryFlag = true;
        }

        private void updateTextBoxes()
        {
            Genre.Text = myMovie.genre;
            Audience.Text = myMovie.audience;
            Guidance.Text = myMovie.guidance;

        }

        private int getStyleIndex(List<StyleElements> myList, string myLabel)
        {
            int returnValue = 0;
            int index = myList.FindIndex(style => style.label == myLabel);
            if (index > -1) returnValue = index; else returnValue = 0;
            return returnValue;
        }

        private string getLatestCurrentSceneText(int myScene)

        {

            int notesKount = scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count;
            string sceneText = "";


            for (int j = notesKount - 1; j >= 0; j--)
            {
                sceneText = scenes[myScene - 1].myNoteTextList[j].myText.Trim();
                if (sceneText.Length > 0) break;
            }

            return sceneText;
        }

        private void updateRTBText(RichTextBox myBox,string input)
        {
            input = Utils.singleAngleBrackets(input);
            if (myBox == SceneText)
            {
                HideApplyButton(SceneText);
                ShowEditButton(SceneText);
                                                
            }

            if (myBox == MovieText)
            {
                HideApplyButton(MovieText);
                ShowEditButton(MovieText);
                                
            }

            if (myBox == SceneScriptRichTextbox)
            {
                HideApplyButton(SceneScriptRichTextbox);
                ShowEditButton(SceneScriptRichTextbox);
                SceneScriptRichTextbox.Rtf = Utils.formatScript(input, FormatScript.Checked);
                return;
            }
            
            myBox.Text = input;
            cursorTopRTB(myBox);

        }
        private void ShowEditButton(RichTextBox myBox)
        {
            
            if (myBox == SceneText)
            {
                EditSceneText.Visible = true;
                EditSceneText.Enabled = true;
                SceneText.ReadOnly = true;
            }
            
            if (myBox == MovieText)
            {
                EditMovieText.Visible = true;
                EditMovieText.Enabled = true;
                MovieText.ReadOnly = true;
            }

            if (myBox == SceneScriptRichTextbox)
            {
                EditScriptTextButton.Visible = true;
                EditScriptTextButton.Enabled = true;
                SceneScriptRichTextbox.ReadOnly = true;
            }
        }

        private void HideEditButton(RichTextBox myBox)
        {
            if (myBox == SceneText)
            {
                EditSceneText.Visible = false;
                EditSceneText.Enabled = false;
                SceneText.ReadOnly = false;
            }

            if (myBox == MovieText)
            {
                EditMovieText.Visible = false;
                EditMovieText.Enabled = false;
                MovieText.ReadOnly = false;
            }

            if (myBox == SceneScriptRichTextbox)
            {
                EditScriptTextButton.Visible = false;
                EditScriptTextButton.Enabled = false;
                SceneScriptRichTextbox.ReadOnly = false;
            }

        }
        private void copyTextRTB(RichTextBox myBox)
        {
            int kount = 0;
            Boolean breakFlag = false;

            while (kount < 3)
            {
                kount++;

                try
                {
                    if (!string.IsNullOrEmpty(myBox.Text))

                    {
                        Clipboard.SetText(myBox.Text);
                    }
                    else
                    {
                        Clipboard.SetText("");
                    }
                    breakFlag = true;
                    break;
                }
                catch
                {
                    Thread.Sleep(30);
                }
            }

            if (!breakFlag) 
            
            { 
             
                MessageBox.Show("Could not copy text to clipboard");
            
            }







            
            

        }

        private int emptySceneTexts()
        {

            
            int notesListLength = 0;
            int emptyTextKount = 0;
            
            for (int j = 0;j < scenes.Count;j++) 
            
            {
                notesListLength = scenes[j].myNoteTextList.Count;
                if (notesListLength == 0 || scenes[j].myNoteTextList[notesListLength - 1].myText.Trim().Length == 0)
                {
                    emptyTextKount = emptyTextKount + 1;


                }
                
            }
            return emptyTextKount;
        }
        private int emptyScriptTexts()

        {
            int notesListLength = 0;
            int emptyKount = 0;

            for (int j = 0; j < scenes.Count; j++)

            {
                notesListLength = scenes[j].myNoteTextList.Count;
                if (notesListLength == 0 || scenes[j].myNoteTextList[notesListLength - 1].myScript.Trim().Length == 0)
                {
                    emptyKount = emptyKount + 1;


                }

            }
            return emptyKount;


        }

        public void updateScriptStylesToDictionary(ListBox myListBox)
        {
            string myLabel;
            if (Utils.updateScriptStyleToDictionaryFlag == false)
            {
                return;
            }
            if (myListBox == StyleSceneList && Utils.dataDictionary != null && StyleSceneList.SelectedIndex > -1 && StyleSceneList.Items.Count > 0)
            {
                myLabel = styles[myListBox.SelectedIndex].label;
                Utils.dataDictionary["StyleAuthorsIndex"] = myLabel;
            }
            if (myListBox == ScriptStyleGuideListbox && Utils.dataDictionary != null && ScriptStyleGuideListbox.SelectedIndex > -1 && ScriptStyleGuideListbox.Items.Count > 0)
            {
                myLabel = scriptStyleGuides[myListBox.SelectedIndex].label;
                Utils.dataDictionary["StyleGuidesIndex"] = myLabel;
            }
        }

        public void updateMovieTextStylesToDictionary(ListBox myListBox)
        {
            string myLabel;
            if (Utils.updateMovieStyleToDictionaryFlag == false)
            {
                return;
            }
            if (myListBox == MovieTextStylesGuideListbox && Utils.dataDictionary != null && MovieTextStylesGuideListbox.SelectedIndex > -1 && MovieTextStylesGuideListbox.Items.Count > 0)
            {
                myLabel = movieTextStyleGuides[myListBox.SelectedIndex].label;
                Utils.dataDictionary["MovieStyleGuidesIndex"] = myLabel;
            }
        }

        public void clearMovieText()
        {
            MovieText.Text = "";
        }

        public Boolean getNoBeatSheetFlag()
        {
            Boolean noBeatSheetFlag = false;

            if (NoBeatSheet.Checked == true)
            {
                noBeatSheetFlag = true;
            }

            return noBeatSheetFlag;
        }

        public void addLengthToMovieList()
        {
            int lookCount = 0;
            foreach (NotesMovieText note in myMovie.myNoteTextList)
            { 
                if (note.myLabel.Length < 24)
                {
                    note.myLabel = note.myLabel.Trim();
                    note.myLabel = note.myLabel + note.myMovieText.Length.ToString().PadLeft(30 - note.myLabel.Length);


                }
            
            }

        }

        public void setDiaglogFlavorChecked()
        {
            foreach (Control control in DialogFlavorPanel.Controls)
            {
                if (control is RadioButton)
                {
                    (control as RadioButton).CheckedChanged += CommonDialogFlavorRadioButton_CheckedChanged;
                }
            }
        }

        public string getDialogFlavor()
        {
            string dialogFlavor = "FlavorNone";
            foreach (Control control in DialogFlavorPanel.Controls)
            {
                if (control is RadioButton)
                {
                    if ((control as RadioButton).Checked == true)
                    {
                        dialogFlavor = (control as RadioButton).Name;
                        break;
                    }
                }
            }
            return dialogFlavor;
        }

        private void CommonDialogFlavorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null && radioButton.Checked)
            {
                // Handle the event. 
                // Note: Only the radio button that gets checked will enter this block.
                // The one that gets unchecked will not.
                Console.WriteLine($"{radioButton.Name} is selected.");

                if (SceneInScenesList.SelectedIndex > -1) 
                {   
                    scenes[SceneInScenesList.SelectedIndex].DialogFlavor = radioButton.Name;
                    
                }
            }
        }

        public string getSceneScriptText()
        {
            string working;

            if (SceneInScenesList.SelectedIndex > -1 && SceneNotesListbox.SelectedIndex > -1 && ScriptNotesListbox.SelectedIndex > -1)
            {
                working = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myScript;
            }
            else
            {
                working = "";
            }


            return working;
        }

        public void postPrompts(string output, Boolean clearFlag)
        {
            if (clearFlag)
            {
                MagicalMysteryTour.Text = "";
            }
            else
            {
                MagicalMysteryTour.Text = output + MagicalMysteryTour.Text;
            }
                

        }

        public void SetHideSelectionForAllRichTextBoxes(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // If the control is a RichTextBox, set HideSelection to false
                if (control is RichTextBox richTextBox)
                {
                    richTextBox.HideSelection = false;
                }

                // If the control has children, recursively call the function to handle nested controls
                if (control.HasChildren)
                {
                    SetHideSelectionForAllRichTextBoxes(control);
                }
            }
        }

        public Boolean checkSelected(RichTextBox myBox)
        {
            if (myBox.SelectedText == null || myBox.SelectedText.Length == 0) { return false; }

            MessageBox.Show("Can't Run a Magic Note Generator when there is Selected Text in the text box.\r\n\r\nClick anywhere in the text box to deselect all text ");

            return true;
        }

        public Boolean isCharacterProfiles()

        {
            if (characterProfiles== null) { return false; }
            if (characters.Count == 0) { return false; }

            return true;
        }

        private RadioButton GetCheckedRadioButtonInPanel(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is RadioButton rb && rb.Checked)
                {
                    return rb;
                }
            }
            return null;  // return null if no radio button is checked
        }

        public Boolean boolSpeakScript()
        {
            if (NoSpeakScript.Checked) { return false; } else { return true; }
        }

        public Boolean boolpersonalityScript()
        {
            if (NoPersonalityScript.Checked) { return false; } else { return true; }
        }

        public Boolean checkedUseProfileMovieText()
        {
            if (UseProfileMovieText.Checked) { return true; } else { return false; }
        }
        public Boolean checkedUseProfileSceneText()
        {
            if (UseProfileSceneText.Checked) { return true; } else { return false; }
        }
        public Boolean checkedUseProfileSceneScript()
        {
            if (UseProfileSceneScript.Checked) { return true; } else { return false; }
        }

        public Boolean checkedUseProfileMakeScenes()
        {
            if (UseProfileMakeScenes.Checked) { return true; } else { return false; }
        }

        public MovieObj getMyMovie()
        {
            return myMovie;
        }

    }
}
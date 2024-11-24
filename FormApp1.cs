using Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;

using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptHelper
{
    public struct StyleElements
    {
        public string label { get; set; }
        public string type { get; set; }
        public string style { get; set; }

        public StyleElements(string element1, string element2, string element3)
        {
            label = element1;
            type = element2;
            style = element3;
        }

    }

    public struct SceneCharacter
    {
        public Guid character { get; set; }
        public Guid scene { get; set; }

        public SceneCharacter(Guid element1, Guid element2)
        {
            character = element1;
            scene = element2;
        }
    }

    public struct TitleText
    {
        public string title { get; set; }
        public string text { get; set; }
        public string scene_number { get; set; }
        public TitleText(string element1, string element2,string element3)
        {
            title = element1;
            text = element2;
            scene_number = element3;
        }
    }

    public partial class FormApp1 : Form
    {
        public int dataVersion = 0;
        Boolean isError = false;

        int NotesTextKount = 0;
        List<SceneObj> scenes;
        List<SceneObj> scenesinscenes;
        List<CharacterObj> characters = new List<CharacterObj>();
        List<SceneCharacter> charactersInScenes = new List<SceneCharacter>();

        List<CharacterProfiles> characterProfiles;

        public List<StyleElements> styles = new List<StyleElements>();
        public List<StyleElements> scriptStyleGuides = new List<StyleElements>();
        public List<StyleElements> movieTextStyleGuides = new List<StyleElements>();

        public List<string> usaRatings = new List<string>();

        SceneObj scene;

        MovieObj myMovie = new MovieObj();

        public string __gptModel = "automatic";

        string rootDir = Utils.rootDir();
        string moviesDir = Utils.moviesDir();
        string styleFile = "c:\\scripthelper-001\\stylesV2.json";
        string scriptStyleGuideFile = "C:\\scripthelper-001\\styleGuides.json";
        string movieTextStyleGuideFile = "C:\\scripthelper-001\\movieTextStyleGuides.json";
        string jsonMovieDump = "C:\\scripthelper-001\\movieDump.json";
        string jsonScenesDump = "C:\\scripthelper-001\\scenesDump.json";

        Boolean titleChangeNotSave = false;

        private bool isClosing = false;

        public FormApp1()
        {
            
            InitializeComponent();

            NotesForMovieText.EnableTripleClick(text => OpenBigBox(text, true, NotesForMovieText));
            MovieText.EnableTripleClick(text => OpenBigBox(text, true, MovieText));
            MovieHintText.EnableTripleClick(text => OpenBigBox(text, true, MovieHintText));
            ScenesListMovieTabRTB.EnableTripleClick(text => OpenBigBox(text, false, ScenesListMovieTabRTB));
            SceneText.EnableTripleClick(text => OpenBigBox(text, true, SceneText));
            SceneScriptRichTextbox.EnableTripleClick(text => OpenBigBox(text, false, SceneScriptRichTextbox));
            SceneHint.EnableTripleClick(text => OpenBigBox(text, true, SceneHint));
            ScriptNotesRTB.EnableTripleClick(text => OpenBigBox(text, true, ScriptNotesRTB));
            DisplayStyleGuideRTB.EnableTripleClick(text => OpenBigBox(text, false, DisplayStyleGuideRTB));
            MovieTextStylesGuideRichTextBox.EnableTripleClick(text => OpenBigBox(text, false, MovieTextStylesGuideRichTextBox));

            LoadKey();


            Utils.EnableFormRightClickForCutPaste(this);

            string jsonString = "";

            Utils.McKeeFlag = false;

            Version.Text = "ScriptHelper Version: " + Utils.programVersion;
            this.Hide();
            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }

            if (!Directory.Exists(moviesDir))
            {
                Directory.CreateDirectory(moviesDir);
            }
            
            if (!File.Exists(styleFile))
            {
                styles = Utils.getAuthorStyles();
                jsonString = JsonConvert.SerializeObject(styles);
                File.WriteAllText(styleFile, jsonString);
            }
            else
            {
                jsonString = File.ReadAllText(styleFile);
                styles = JsonConvert.DeserializeObject<List<StyleElements>>(jsonString);

            }

            if (!File.Exists(scriptStyleGuideFile))
            {
                scriptStyleGuides = Utils.getScriptStyleGuides();
                jsonString = JsonConvert.SerializeObject(scriptStyleGuides);
                File.WriteAllText(scriptStyleGuideFile, jsonString);
            }
            else
            {
                jsonString = File.ReadAllText(scriptStyleGuideFile);
                scriptStyleGuides = JsonConvert.DeserializeObject<List<StyleElements>>(jsonString);
            }

            if (!File.Exists(movieTextStyleGuideFile))
            {
                movieTextStyleGuides = Utils.getMovieTextStyleGuides();
                jsonString = JsonConvert.SerializeObject(movieTextStyleGuides);
                File.WriteAllText(movieTextStyleGuideFile, jsonString);
            }
            else
            {
                jsonString = File.ReadAllText(movieTextStyleGuideFile);
                movieTextStyleGuides = JsonConvert.DeserializeObject<List<StyleElements>>(jsonString);
                // check if movie text style guide has the defualt entries
                var defualtMovieTextStyleGuides = Utils.getMovieTextStyleGuides();
                foreach (var defualtMovieTextStyleGuide in defualtMovieTextStyleGuides)
                {
                    if (!movieTextStyleGuides.Any(x => x.label == defualtMovieTextStyleGuide.label))
                    {
                        movieTextStyleGuides.Add(defualtMovieTextStyleGuide);
                    }
                }
                // put label "-none-" first in the list, "McKee Style" second, and "Depurple" third in list and leave the rest in the same order
                var noneStyle = movieTextStyleGuides.FirstOrDefault(x => x.label == "-none-");
                var mckeeStyle = movieTextStyleGuides.FirstOrDefault(x => x.label == "McKee Style");
                var depurple = movieTextStyleGuides.FirstOrDefault(x => x.label == "Depurple");
                movieTextStyleGuides.Remove(noneStyle);
                movieTextStyleGuides.Remove(mckeeStyle);
                movieTextStyleGuides.Remove(depurple);
                movieTextStyleGuides.Insert(0, noneStyle);
                movieTextStyleGuides.Insert(1, mckeeStyle);
                movieTextStyleGuides.Insert(2, depurple);
            }

            SceneCount.Value = 24;
            Utils.currentSceneCount = 24;
            Utils.currentTokenCount = 40;

            if (!ShowStartScreen())
            {
                Environment.Exit(0);
            }
            this.Visible = true;

            __gptModel = "automatic";

            //  read in the current movie 

            Boolean abort = beginMovie();

            while (abort)
            {

                this.Hide();
                FormStart startForm2 = new FormStart();
                startForm2.StartPosition = FormStartPosition.CenterScreen;
                startForm2.ShowDialog();

                startForm2.Close();

                abort = beginMovie();
                if (!abort) { this.Show(); }
            }

            Movie.SelectedIndexChanged += Movie_SelectedIndexChanged;
        }

        private string GetGptModel(string overrideModel = null)
        {
            if (__gptModel == "automatic")
            {
                if (overrideModel != null)
                {
                    return overrideModel;
                }
                return "or/openai/gpt-4o";
            }
            return __gptModel;
        }

        private void OpenBigBox(string text, bool canEdit, RichTextBox rtb)
        {
            FormBigBox newForm = new FormBigBox(text, canEdit, rtb);
            newForm.StartPosition = FormStartPosition.CenterScreen;
            newForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // this.WindowState = FormWindowState.Maximized;

            // this.Size = new Size(1700, 1500);

            //this.MaximumSize = new Size(1900, 1500);

            SetHideSelectionForAllRichTextBoxes(this);

            //MovieSceneListLabel.AutoSize = false;
            //MovieSceneListLabel.Width = 350;
            //MovieSceneListLabel.Height = 300;
            //MovieSceneListLabel.MaximumSize = new Size(350, 300);

            this.StartPosition = FormStartPosition.CenterScreen;
            setDiaglogFlavorChecked(); // sets single event handler for all Flavor Dialog Buttons
            ErrorMessage.Text = "";

            this.AutoScaleMode= AutoScaleMode.Dpi;

            // async load of open router models
            _ = LoadOpenRouterModels();

        }

        // Event handler method
        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            // You can access the currently selected tab using the SelectedTab property
            TabPage current = (sender as TabControl).SelectedTab;

            // Display the name of the selected tab
            MessageBox.Show(current.Text);
        }

        private void Movie_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl != null)
            {
                int selectedIndex = tabControl.SelectedIndex;

                if (selectedIndex == 1 && scenes.Count > 0)
                {
                    string authorStyle = styles[StyleSceneList.SelectedIndex].label;
                    AuthorStyleLabel.Text = "Author Style: " + authorStyle;

                    string styleGuide = scriptStyleGuides[ScriptStyleGuideListbox.SelectedIndex].label;
                    StyleGuideLabel.Text = "Style Guide: " + styleGuide;

                    SceneInScenesList.DataSource = scenes;
                    SceneInScenesList.DisplayMember = "Title";
                    SceneInScenesList.Focus();
                    Application.DoEvents();

                    int picked = SceneInScenesList.SelectedIndex;
                    SceneHint.Text = scenes[picked].Hint;
                    Application.DoEvents();
                }
                // Perform your desired actions based on the selected index.
                // MessageBox.Show($"Selected tab index: {selectedIndex}");
            }
        }

        private async void MakeMovieTextButton_Click(object sender, EventArgs e)
        {
            string menuItem = "";

            // DisableTextChangedEvent(MovieText);
            HideApplyButton(MovieText);

            //MovieText.Text = 
            updateRTBText(MovieText, GetGptModel() + " awaiting reply...\r\n \r\n" + MovieHintText.Text);

            cursorTopRTB(MovieText);
            string reply = await doMakeMovieText(MovieHintText.Text);

            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                return;
            }

            updateRTBText(MovieText, reply);

            myMovie.movieText = reply;

            if (myMovie.myNoteTextList.Count == 0)
            {
                myMovie.myNoteTextList = new List<NotesMovieText>();
                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Base", 0, myMovie.movieText.Length);

                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();
                myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem)) ;
            }
            else if (myMovie.myNoteTextList.Count > 0)
            {
                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, " New", NotesList.SelectedIndex,myMovie.movieText.Length);
                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, "", menuItem));
                updateRTBText(NotesForMovieText, "");
            }
            NotesList.DataSource = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = myMovie.myNoteTextList.Count - 1;
            Application.DoEvents();
            ShowEditButton(MovieText);
        }

        
        private void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?" , "Save Movie", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                myMovie.movieHintText = MovieHintText.Text;
                saveCurrentMovie();
                LastSaved.Text = "Last saved: " + DateTime.Now.ToString("HH:mm:ss");
            }


            if (!ShowStartScreen())
            {
                Environment.Exit(0);
            }
        }

        private bool ShowStartScreen()
        {
            var abort = true;
            var exitedFromStartup = false;
            this.Hide();
            while (abort && !exitedFromStartup)
            {
                FormStart startForm2 = new FormStart();
                startForm2.StartPosition = FormStartPosition.CenterScreen;
                startForm2.ShowDialog();
                startForm2.Close();
                exitedFromStartup = startForm2.Exited;
                var loggedOutFromStartup = startForm2.LoggedOut;
                
                if (!exitedFromStartup && !loggedOutFromStartup)
                {
                    abort = beginMovie();
                }

            }

            if (!exitedFromStartup)
                this.Show();

            return !exitedFromStartup;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MovieObj movie = new MovieObj();
            MovieHintText.Text = movie.movieHintText;
        }

        
               



        private async void TradMakeScenesButton_Click(object sender, EventArgs e)
        {
            int nop;
            
            nop = await doMakeScenesFromMovieText();




        }

        private void SceneInScenesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideApplyButton(SceneText);
            ShowEditButton(SceneText);
            HideApplyButton(SceneScriptRichTextbox);
            ShowEditButton(SceneScriptRichTextbox);
            

            if (SceneInScenesList.SelectedIndex > -1)
            {
                int picked = SceneInScenesList.SelectedIndex;
                int sceneNumber = picked + 1;
                long currentTimeMillis;

                string search = scenes[SceneInScenesList.SelectedIndex].DialogFlavor;
                Control myControl = DialogFlavorPanel.Controls[$"{search}"];
                
                if (myControl != null)
                {
                    RadioButton myRadioButton = (RadioButton)myControl;

                    myRadioButton.Checked = true;
                }
                else
                {
                    FlavorNone.Checked = true;

                }
                
                FlavorCustomText.Text = scenes[SceneInScenesList.SelectedIndex].CustomFlavor;
                

                /*

                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;

                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";

                SceneInScenesList.SelectedIndex = picked;
                SceneInScenesList.EndUpdate();
                */


                // SceneText.Text = scenes[picked].NarrativeText;
                updateRTBText(SceneText, scenes[picked].NarrativeText);

                
                // stop flicker on text boxes 
                currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (currentTimeMillis - Utils.lastUpdateSceneInScenes > 200)
                {
                    SceneTitle.Text = $"Scene #{sceneNumber} of {scenes.Count}: " + scenes[picked].Title;
                    SceneHint.Text = scenes[picked].Hint;
                    // SceneScriptRichTextbox.Text = scenes[picked].SceneScript;
                    SceneNoteRichTextBox.Text = "";
                }
                Utils.lastUpdateSceneInScenes = currentTimeMillis;


                

                int listKount = scenes[picked].myNoteTextList.Count;
                SceneNotesListbox.DataSource = null;
                

                if (listKount > 0)
                {
                    SceneNotesListbox.BeginUpdate();
                    SceneNotesListbox.DataSource = null;
                    SceneNotesListbox.DataSource = scenes[picked].myNoteTextList;
                    SceneNotesListbox.DisplayMember = "myLabel";
                    SceneNotesListbox.SelectedIndex = listKount - 1;
                    SceneNotesListbox.EndUpdate();

                    SceneNoteRichTextBox.Text = scenes[picked].myNoteTextList[listKount - 1].myNote;

                    //SceneText.Text = scenes[picked].myNoteTextList[listKount - 1].myText;
                    // updateRTBText(SceneText, scenes[picked].myNoteTextList[listKount - 1].myText);
                }

                if (sceneNumber > 0)
                {
                    RefactorButton.Text = $"Refactor Scenes After Scene #{sceneNumber}";
                    HeavyRefactorButton.Text = $"Refactor Scenes After Scene #{sceneNumber}";
                    Utils.currentSceneNumber = sceneNumber;
                }
                Application.DoEvents();
            }
        }

        private void SceneHint_TextChanged(object sender, EventArgs e)
        {
            Boolean passFlag = true;
            if (scenes.Count == 0 && passFlag && Utils.SceneSeedUpdateFlag)
            {
                MessageBox.Show("can't write a Seed until Scenes are created on the Movie Tab");
                
                return;
            }
            if (SceneHint == this.ActiveControl)
            {
                scenes[SceneInScenesList.SelectedIndex].Hint = SceneHint.Text;
            }
        }

        private async void MakeSceneTextButton_Click(object sender, EventArgs e)
        {
            Utils.nop();
            if (scenes.Count == 0) return;
            string nothing = await doMakeSceneText();
        }

        private async void WriteSceneButton_Click(object sender, EventArgs e)
        {
            string work = "";
            string beatSheet, sceneScript;
            string reply;
            string oldText = getSceneScriptText();
            
            if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count == 0)
            {
                MessageBox.Show("Must Have a Scene Text Made to Create Script");
                return;
            }
            
            /* if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count > 1)
            {
                DialogResult result = MessageBox.Show("You already have at least 1 Note or Edit for this Scene Script. Proceeding will create a new Scene Script  and will eliminate your Notes and Edits.  Do you want to proceed?", "New Story Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }  */

            if (SceneText.Text.Length > 100)
            {
                int sceneIndex = SceneInScenesList.SelectedIndex;
                int SceneTextIndex = SceneNotesListbox.SelectedIndex;

                SceneScriptRichTextbox.Text = GetGptModel() + " creating Beat Sheet....\r\n \r\n" + oldText;
                BeatSheetRichTextbox.Text = "making beat sheet from scene description.... ";
                reply = await doMakeBeatSheet();
                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    return;
                }
                beatSheet = reply;
                BeatSheetRichTextbox.Text = reply;
                scenes[SceneInScenesList.SelectedIndex].BeatSheetText = reply;
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myBeatSheet = reply;

                SceneScriptRichTextbox.Text = GetGptModel() + " creating Scene Script....\r\n \r\n" + oldText;
                Application.DoEvents();
                reply = await doWriteSceneScript();

                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    return;
                }

                sceneScript = reply;

                
                scenes[SceneInScenesList.SelectedIndex].SceneScript = reply;
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScript = reply;

                // working on formating 

                work = Utils.formatScript(reply, FormatScript.Checked);
                SceneScriptRichTextbox.Rtf = work;
                // CodeReWrite.Rtf = work;


                if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count == 0)
                {
                    scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts = new List<NotesSceneScript>();
                    scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(beatSheet, sceneScript, "", "Base:"));
                }
                    
                else if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count > 0)
                {
                    // scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(beatSheet, sceneScript, "", " New:"));
                    string newLabel = makeSceneScriptMenuLabel(scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts, " New", ScriptNotesListbox.SelectedIndex);
                    scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(beatSheet, sceneScript, ScriptNotesRTB.Text, newLabel));
                    updateRTBText(ScriptNotesRTB, "");
                }

                if (Utils.dataVersion > 1)
                {
                    
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneTextIndex].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count -1;
                    ScriptNotesListbox.EndUpdate();
                }
                updateRTBText(ScriptNotesRTB, "");
            }
            else
            {

                MessageBox.Show("Must have at least 100 characters in SceneText to make Script");
                return;

            }


        }

        private void SceneText_TextChanged(object sender, EventArgs e)
        {

        }

        public void updateGPTStatus(string statusMsg, Color color)
        {
            GPTStatus.Text = statusMsg;
            GPTStatus.ForeColor = color;
            Application.DoEvents();
        }

        public void updateGPTStatusFromTask(string statusMessage, Color color)
        {
            BeginInvoke(new Action(() =>
            {
                GPTStatus.Text = statusMessage;
                GPTStatus.ForeColor = color;
            }));
        }

        private async void button5_Click_3(object sender, EventArgs e)
        {
            if (NotesForMovieText.Text.Length < 6)
            {
                MessageBox.Show("Must have at least 5 characters in Notes to make Movie Text");
                return;
            }
            
            
            HideApplyButton(MovieText);
            string originalMovieText = MovieText.Text;
            string originalNote = NotesForMovieText.Text;
            int sourceIndex = NotesList.SelectedIndex;

            string selectedText = MovieText.SelectedText.Trim();
            string spliceInText = "";
            
            int startSelection = MovieText.SelectionStart;
            
            if (selectedText.Length == 0)
            {
                //MovieText.Text = GetGptModel() + " applying Notes to Text....\r\n \r\n" + MovieText.Text;
                updateRTBText(MovieText, GetGptModel() + " applying Notes to Text....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);
                string response = await MyGPT.NotesForMovieText(GetGptModel(), MovieText.Text, MovieHintText.Text, NotesForMovieText.Text, this);

                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }
                //MovieText.Text = response;
                updateRTBText(MovieText, response);

                myMovie.movieText = response;
                NotesTextKount += 1;

                string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Note", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));

            }
            else
            {
                updateRTBText(MovieText, GetGptModel() + " applying Notes to Selected Region of Movie Text....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);

                string response = await MyGPT.notesForSELECTEDMovieText(GetGptModel(), originalMovieText, selectedText, startSelection, originalNote, this);
                
                
                
                spliceInText = response;
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }


                response = Utils.spliceWithLocs(originalMovieText, selectedText, startSelection, selectedText.Length, spliceInText);

                myMovie.movieText = response;

                updateRTBText(MovieText, response);
                cursorTopRTB(MovieText);


                string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Note", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));
            }

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;

            // EnableTextChangedEvent(MovieText);
            ShowEditButton(MovieText);

            Application.DoEvents();

            if (selectedText.Trim().Length > 0)
            {
                MovieText.Focus();
                MovieText.SelectionStart = startSelection;
                MovieText.SelectionLength = spliceInText.Length;
            }
        }

        private void NotesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NotesList.SelectedIndex >= 0 && NotesList.SelectedIndex <= myMovie.myNoteTextList.Count)
            {
                //MovieText.Text = myMovie.myNoteTextList[NotesList.SelectedIndex].myMovieText;
                updateRTBText(MovieText, myMovie.myNoteTextList[NotesList.SelectedIndex].myMovieText);

                NotesForMovieText.Text = myMovie.myNoteTextList[NotesList.SelectedIndex].myNote;
                HideApplyButton(MovieText);
            }
        }

        private void SentencesInSceneHint_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_4(object sender, EventArgs e)
        {
            string  reply = Utils.JSONFixer(BoldBox.Text);
            BoldBox.Text = reply;
        }

        private async void SceneApplyNoteButton_Click(object sender, EventArgs e)
        {
            if (SceneNoteRichTextBox.Text.Length < 5 || SceneText.Text.Length < 50)
            {
                MessageBox.Show("Must have at least 50 characters in SceneText and 5 in Notes to apply Notes");
                return;
            }

            string selectedText = SceneText.SelectedText.Trim();
            string spliceInText = "";
            int startSelection = SceneText.SelectionStart;
            int noteIndex = SceneNotesListbox.SelectedIndex;
            int notesKount = SceneNotesListbox.Items.Count;


            int sceneIndex = SceneInScenesList.SelectedIndex;

            string originalSceneText = SceneText.Text;

            if (selectedText.Length == 0)  // no selected text
            {
                //SceneText.Text = GetGptModel() + " applying Notes to Scene text....\r\n \r\n" + originlSceneText;
                updateRTBText(SceneText, GetGptModel() + " applying Notes to Scene text....\r\n \r\n" + originalSceneText);

                cursorTopRTB(SceneText);

                string response = await MyGPT.NotesForSceneText(GetGptModel(), originalSceneText, SceneNoteRichTextBox.Text, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }
                //SceneText.Text = response;
                updateRTBText(SceneText, response);
                cursorTopRTB(SceneText);

                scenes[sceneIndex].NarrativeText = response;
                string mylabel = makeSceneMenuLabel(scenes[sceneIndex].myNoteTextList, "Note", noteIndex);
                scenes[sceneIndex].myNoteTextList.Add(new NotesSceneText(SceneText.Text, SceneNoteRichTextBox.Text, mylabel, ""));

                notesKount += 1;
            }
            else   // selected text 
            {
                updateRTBText(SceneText, GetGptModel() + " applying Notes to Selected Region of Scene Text....\r\n \r\n" + originalSceneText);

                cursorTopRTB(SceneText);

                string response = await MyGPT.notesForSELECTEDSceneText(GetGptModel(), originalSceneText, selectedText, startSelection, SceneNoteRichTextBox.Text, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
                spliceInText = response;
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }


                response = Utils.spliceWithLocs(originalSceneText, selectedText, startSelection,selectedText.Length, response);

                
                updateRTBText(SceneText, response);
                cursorTopRTB(SceneText);

                /* 
                SceneText.Focus();
                SceneText.SelectionStart = startSelection;
                SceneText.SelectionLength = spliceInText.Length;

                SceneText.SelectionStart = 10;
                SceneText.SelectionLength = 20;
                */

                scenes[sceneIndex].NarrativeText = response;
                string mylabel = makeSceneMenuLabel(scenes[sceneIndex].myNoteTextList, "Note", noteIndex);
                scenes[sceneIndex].myNoteTextList.Add(new NotesSceneText(SceneText.Text, SceneNoteRichTextBox.Text, mylabel, ""));

                notesKount += 1;
            }

            updateRTBText(ScriptNotesRTB, "");
            updateRTBText(SceneScriptRichTextbox, "");

            SceneNotesListbox.BeginUpdate();
            SceneNotesListbox.DataSource = null;
            SceneNotesListbox.DisplayMember = null;
            SceneNotesListbox.DataSource = scenes[sceneIndex].myNoteTextList;
            SceneNotesListbox.DisplayMember = "myLabel";
            SceneNotesListbox.SelectedIndex = notesKount - 1;
            SceneNotesListbox.EndUpdate();

            updateRTBText(ScriptNotesRTB, "");
            updateRTBText(SceneScriptRichTextbox, "");
            Application.DoEvents();
            
            
            if (selectedText.Trim().Length > 0)

            {
                SceneText.Focus();
                SceneText.SelectionStart = startSelection;
                SceneText.SelectionLength = spliceInText.Length;

                
            }
        }

        private void SceneNotesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sceneIndex = SceneNotesListbox.SelectedIndex;
            int scriptsCount = 0;
            long currentTimeMillis = 0;

            string working = "";

            

            if (SceneNotesListbox.SelectedIndex > -1 )
            {
                if (Utils.dataVersion > 1)
                {
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                    ScriptNotesListbox.EndUpdate();
                }
                if (Utils.dataVersion > 1)
                {
                    scriptsCount = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts.Count;
                }
                else
                {
                    scriptsCount = 0;
                }

                //SceneText.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myText;
                updateRTBText(SceneText, scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myText);
                updateRTBText(BeatSheetRichTextbox, scenes[SceneInScenesList.SelectedIndex].BeatSheetText);
                
                SceneNoteRichTextBox.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myNote;

                currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (currentTimeMillis - Utils.lastUpdateSceneNotesList > 200)
                {
                    if (scriptsCount == 0)
                    {
                        
                        ScriptNotesRTB.Text = "";
                        SceneScriptRichTextbox.Rtf = Utils.makeRTFBlank();
                    }
                    else
                    {
                        
                        working = Utils.formatScript(scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myScript, FormatScript.Checked);
                        SceneScriptRichTextbox.Rtf = working;
                        BeatSheetRichTextbox.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myBeatSheet;
                        ScriptNotesRTB.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myNote;

                    }

                    if (scriptsCount == 0)
                    {
                        
                        ScriptNotesRTB.Text = "";
                    }
                    else
                    {
                        SceneScriptRichTextbox.Rtf = Utils.formatScript(scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myScript,FormatScript.Checked);
                        BeatSheetRichTextbox.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myBeatSheet;
                        ScriptNotesRTB.Text = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts[scriptsCount - 1].myNote;

                    }



                }
                Utils.lastUpdateSceneNotesList = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (Utils.dataVersion > 1)
                {
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count - 1;
                    ScriptNotesListbox.EndUpdate();
                }
                Utils.lastUpdateSceneNotesList = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            

        }

        

        private void RebaseMovie_Click(object sender, EventArgs e)
        {

            string tempText;
            int listIndex = NotesList.SelectedIndex;

            if (listIndex > 0)
            {
                tempText = myMovie.myNoteTextList[listIndex].myMovieText;

                //MovieText.Text = tempText;
                updateRTBText(MovieText, tempText);

                NotesForMovieText.Text = "";
                myMovie.myNoteTextList = new List<NotesMovieText>();

                myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", makeMovieMenuLabel(myMovie.myNoteTextList, "Base", 0, myMovie.movieText.Length)));

                
                NotesList.DataSource = myMovie.myNoteTextList;
                NotesList.DisplayMember = "myLabel";
            }
        }

        private void ClearSceneNoteButton_Click(object sender, EventArgs e)
        {
            SceneNoteRichTextBox.Text = "";
        }

        private void RebaseScene_Click(object sender, EventArgs e)
        {
            string tempText;
            string tempScript;
            if (scenes.Count == 0) return;

            Utils.nop();
            int startCount = scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count;
            int startLast = startCount - 1;
            if (startCount > 0)
            {
                tempText = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myText;
                tempScript = getSceneScriptText();
                scenes[SceneInScenesList.SelectedIndex].NarrativeText = tempText;
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList = new List<NotesSceneText>();
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Add(new NotesSceneText(tempText, "", "Base:", getSceneScriptText()));
                
                SceneNotesListbox.BeginUpdate();
                SceneNotesListbox.DataSource = null;
                SceneNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList;
                SceneNotesListbox.DisplayMember = "myLabel";
                SceneNotesListbox.EndUpdate();

                // SceneText.Text = tempText;
                updateRTBText(SceneText, tempText);

                SceneScriptRichTextbox.Rtf = Utils.formatScript(tempScript,FormatScript.Checked);

            }

        }

        private void ClearMovieNoteButton_Click(object sender, EventArgs e)
        {
            NotesForMovieText.Text = "";
        }

        private void ClearMovieSeedButton_Click(object sender, EventArgs e)
        {
            MovieHintText.Text = "";
        }

        



        private void SaveButton_Click(object sender, EventArgs e)
        {

            myMovie.movieHintText = MovieHintText.Text;
            saveCurrentMovie();
            LastSaved.Text = "Last saved: " + DateTime.Now.ToString("HH:mm:ss");
        }


        private void ApplyMovieTextEdits_Click(object sender, EventArgs e)
        {
            doApplyEdits();
        }

        public void doApplyEdits()
        {
            HideApplyButton(MovieText);


            int sourceIndex = NotesList.SelectedIndex;


            NotesTextKount += 1;
            myMovie.movieText = MovieText.Text;

            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Edit", NotesList.SelectedIndex, myMovie.movieText.Length);

            myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
            ShowEditButton(MovieText);
            Application.DoEvents();
        }

        

        private void ScenesList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ScenesTab_Click(object sender, EventArgs e)
        {
            SceneInScenesList.Focus();
            
        }

        private async void RefactorButton_Click(object sender, EventArgs e)
        {
            List<List<string>> ListofLists = new List<List<string>>();

            int currentSceneNumber = SceneInScenesList.SelectedIndex + 1;
            int firstSceneRefactor = currentSceneNumber + 1;
            int errorKount = 0;
            Boolean looper = true;
            string sceneText;
            string mb = "";

            sceneText = getLatestCurrentSceneText(currentSceneNumber);

            if (sceneText.Length == 0)
            {

                mb = $"At least one Scene Text is required for Scene {currentSceneNumber} to perform Scene Refactoring";
                MessageBox.Show(mb);
                return;
            }
            if (!(GetGptModel().Contains("gpt-4")))
            {
                MessageBox.Show("Must be in onw of the GPT-4 modea to use Scene Refactoring.  Please change to a GPT-4 mode.");
                return;
            }

            FormScenesRefactor refactorForm = new FormScenesRefactor();
            refactorForm.ShowDialog();


            Utils.nop();

            if (Utils.refactor)
            {
                

                string jsonString = await MyGPT.refactorSceneAfter(GetGptModel(), scenes, currentSceneNumber, Utils.currentSceneCount,  sceneText, myMovie.movieText, this);
                isError = Utils.checkForGPTErrors(jsonString, this);
                if (isError)
                {
                    return;
                }

                string originalJSONString = jsonString;

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
                        if (errorKount > 2)
                        {
                            updateGPTStatus("GPT Status: Unrepairable JSON error.  Make small adjustment in number of scenes and try again.", Color.LightGreen);
                            return;
                        }
                        if (errorKount == 0)

                        {
                            errorKount = 1;
                            jsonString = Utils.JSONFixer(originalJSONString);
                            Utils.nop();
                            looper = true;
                        }

                        if (errorKount > 1)

                        {
                            errorKount += 1;
                            SceneDescriptions.Text = "error - trying to repair JSON. kount = " + errorKount.ToString() + "\r\n" + originalJSONString;
                            jsonString = await MyGPT.fixJSON(originalJSONString, GetGptModel(), $"JSON Error - trying to repair JSON.  Error Count = {errorKount}", this);
                            isError = Utils.checkForGPTErrors(jsonString, this);
                            if (isError)
                            {
                                return;
                            }


                            looper = true;
                        }

                    }
                    Utils.nop();


                }

                // create new list of scenes
                List<SceneObj> tempScenes = new List<SceneObj>();

                // add in all those up to and including current Scene number
                for (int j = 0; j <= Utils.currentSceneNumber - 1; j++)
                {
                    tempScenes.Add(scenes[j]);

                }

                //add the new ones 
                foreach (List<string> myScene in ListofLists)
                {

                    SceneObj scene = new SceneObj();
                    string myTitle = myScene[0];
                    string Hint = myScene[1];
                    scene.Title = myTitle;
                    scene.Hint = Hint;
                    tempScenes.Add(scene);
                }

                scenes = tempScenes;

                ScenesList.BeginUpdate();
                ScenesList.DataSource = null;
                ScenesList.DataSource = scenes;
                ScenesList.DisplayMember = "Title";
                ScenesList.SelectedIndex = Utils.currentSceneNumber - 1;
                ScenesList.EndUpdate();

                SceneInScenesList.BeginUpdate();    
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";
                SceneInScenesList.SelectedIndex = Utils.currentSceneNumber - 1;
                SceneInScenesList.Focus();
                SceneInScenesList.EndUpdate();  

            }

        }

        private void SceneCount_ValueChanged(object sender, EventArgs e)
        {
            Utils.currentSceneCount = (int)SceneCount.Value;
        }

        private void MovieTitle_Click(object sender, EventArgs e)
        {

        }

        private void ForkMovieButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Save Current Movie before forking?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                saveCurrentMovie();
            }

            FormFork forkForm = new FormFork(myMovie);
            forkForm.StartPosition = FormStartPosition.CenterScreen;
            forkForm.ShowDialog();

            if (!Utils.fork) return;

            string newFileName = Utils.makeFilename(Utils.forkTitle);

            Utils.currentMovieFilename = moviesDir + "\\" + newFileName;

            myMovie.title = Utils.forkTitle;

            MovieTitle.Text = Utils.forkTitle;
            TopMovieTitle.Text = "Title: " + Utils.forkTitle;

            saveCurrentMovie();
        }

        private void ApplySceneTextEdits_Click(object sender, EventArgs e)
        {
            // DisableTextChangedEvent(SceneText);
            HideApplyButton(SceneText);

            int noteIndex = SceneNotesListbox.SelectedIndex;
            int notesKount = SceneNotesListbox.Items.Count;


            int sceneIndex = SceneInScenesList.SelectedIndex;

            string originalSceneText = SceneText.Text;


            scenes[sceneIndex].NarrativeText = SceneText.Text;
            string mylabel = makeSceneMenuLabel(scenes[sceneIndex].myNoteTextList, "Edit", noteIndex);
            scenes[sceneIndex].myNoteTextList.Add(new NotesSceneText(SceneText.Text, SceneNoteRichTextBox.Text, mylabel, "" ));


            notesKount += 1;

            SceneNotesListbox.BeginUpdate();
            SceneNotesListbox.DataSource = null;
            SceneNotesListbox.DisplayMember = null;
            SceneNotesListbox.DataSource = scenes[sceneIndex].myNoteTextList;
            SceneNotesListbox.DisplayMember = "myLabel";
            SceneNotesListbox.SelectedIndex = notesKount - 1;
            SceneNotesListbox.EndUpdate();
            
            updateRTBText(ScriptNotesRTB, "");
            updateRTBText(SceneScriptRichTextbox, "");

            
            HideApplyButton(SceneText);
            ShowEditButton(SceneText);

            Application.DoEvents();
        }

        private void EditSceneText_Click(object sender, EventArgs e)
        {
            // EnableTextChangedEvent(SceneText);

            Utils.originalSceneText = SceneText.Text;
            HideEditButton(SceneText);
            ShowApplyButton(SceneText);
            SceneText.Focus();
        }

        private void DiscardSceneTextEdits_Click(object sender, EventArgs e)
        {
            updateRTBText(SceneText, Utils.originalSceneText);


        }

        private void EditMovieText_Click(object sender, EventArgs e)
        {
            Utils.originalMovieText = MovieText.Text;
            HideEditButton(MovieText);
            ShowApplyButton(MovieText);
            MovieText.Focus();
        }

        private void Collate_Click(object sender, EventArgs e)
        {
            FormCollate collateForm = new FormCollate(scenes, myMovie);
            collateForm.StartPosition = FormStartPosition.CenterScreen;
            collateForm.ShowDialog();


        }

        private void CopyMovieTextButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(MovieText);
        }

        private void CopySceneTextButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(SceneText);
        }

        private void CopySceneScriptTextButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(SceneScriptRichTextbox);
        }

        private void SceneUpArrow_Click(object sender, EventArgs e)
        {
            int startIndex = SceneInScenesList.SelectedIndex;



            if (startIndex > 0)

            {
                List<SceneObj> tempSceneList = new List<SceneObj>();
                tempSceneList = scenes;
                string title = "";
                SceneObj tempScene = tempSceneList[startIndex];


                // Remove the item from its current position
                tempSceneList.RemoveAt(startIndex);


                // Insert the item at the new position
                tempSceneList.Insert(startIndex - 1, tempScene);




                scenes = tempSceneList;

                SceneInScenesList.SelectedIndex = startIndex - 1;

                SceneInScenesList.DataSource = null;
                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";
                    
            }
        }

        private void SceneDownArrow_Click(object sender, EventArgs e)
        {
            int startIndex = SceneInScenesList.SelectedIndex;
            int startCount = SceneInScenesList.Items.Count;


            if (startIndex < startCount - 1)

            {
                List<SceneObj> tempSceneList = new List<SceneObj>();
                tempSceneList = scenes;
                
                SceneObj tempScene = tempSceneList[startIndex];


                // Remove the item from its current position
                tempSceneList.RemoveAt(startIndex);


                // Insert the item at the new position
                tempSceneList.Insert(startIndex + 1, tempScene);




                scenes = tempSceneList;

                SceneInScenesList.SelectedIndex = startIndex + 1;

                SceneInScenesList.DataSource = null;
                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";
            }
        }

        private void DeleteScene_Click(object sender, EventArgs e)
        {
            string myTitle = scenes[SceneInScenesList.SelectedIndex].Title;

            int startIndex = SceneInScenesList.SelectedIndex;


            DialogResult result = MessageBox.Show($"Proceeding will permanenetly delete scene:\r\n{myTitle} \r\n\r\n Do you want to proceed?", "Delete Scene", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            List<SceneObj> tempSceneList = null;
            tempSceneList = scenes;
            tempSceneList.RemoveAt(startIndex);

            scenes = tempSceneList;


            if (startIndex > 0)
            {
                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;

                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";

                SceneInScenesList.SelectedIndex = startIndex - 1;
                SceneInScenesList.EndUpdate();


                // SceneInScenesList_SelectedIndexChanged(this, EventArgs.Empty);}

            }
            else
            {
                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;

                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";

                SceneInScenesList.SelectedIndex = 0;
                SceneInScenesList.EndUpdate();
                // SceneInScenesList_SelectedIndexChanged(this, EventArgs.Empty);}
            }
        }
        private void AddSceneButton_Click(object sender, EventArgs e)
        {
            int startIndex = SceneInScenesList.SelectedIndex;
            
            FormAddScene addSceneForm = new FormAddScene(startIndex,scenes);
            addSceneForm.ShowDialog();

            if (addSceneForm.added)
            {
                scenes.Insert(startIndex + 1,addSceneForm.newScene);
                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;
                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";
                SceneInScenesList.SelectedIndex = startIndex + 1;
                SceneInScenesList.EndUpdate();  
                
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void CopyBeatSheetButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(BeatSheetRichTextbox);
        }

        private async void MakeBeatSheet_Click(object sender, EventArgs e)
        {
            string reply;
            string oldText = BeatSheetRichTextbox.Text;
            if (SceneText.Text.Length > 100)
            {
                BeatSheetRichTextbox.Text = GetGptModel() + " creating Beat Sheet....\r\n \r\n" + oldText;
                reply = await doMakeBeatSheet();
                
                
                BeatSheetRichTextbox.Text = reply;

                
                scenes[SceneInScenesList.SelectedIndex].BeatSheetText = reply;
                // scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].Beat = reply;


            }
            else
            {

                MessageBox.Show("Must have at least 100 characters in SceneText to make Beat Sheet");

            }
        }

        private void EnlargeScriptText_Click(object sender, EventArgs e)
        {
            FormEnlargeText enlargeForm = new FormEnlargeText(SceneScriptRichTextbox.Rtf);
            enlargeForm.ShowDialog();
            if ( Utils.enlargeScript && enlargeForm.finalText.Length > 0 )
            { 
                scenes[SceneInScenesList.SelectedIndex].SceneScript = enlargeForm.finalText;
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScript = enlargeForm.finalText;


                string newLabel = makeSceneScriptMenuLabel(scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts, "Edit", ScriptNotesListbox.SelectedIndex);
                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(BeatSheetRichTextbox.Text, enlargeForm.finalText, "", newLabel));

                if (Utils.dataVersion > 1)
                {
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                    ScriptNotesListbox.EndUpdate();
                }

                SceneScriptRichTextbox.Rtf = Utils.formatScript(enlargeForm.finalText,FormatScript.Checked);


            
            }
        }

        private void EditTitle_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = new DialogResult();


            if (titleChangeNotSave)
            {
                dialogResult = MessageBox.Show("You have changed movie title, but haved not saved Movie\r\n\r\n To make another title change you must save Movie. \r\n\r\n Save Movie Now? ", "Save", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveCurrentMovie();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }



            FormEditTitle formEditTitle = new FormEditTitle(myMovie, GetGptModel(), this);
            formEditTitle.ShowDialog();
            if (formEditTitle.finalTitle.Length > 0)
            {
                myMovie.title = formEditTitle.finalTitle;
                MovieTitle.Text = formEditTitle.finalTitle;

                string newFileName = Utils.makeFilename(myMovie.title);
                string newFullPath = moviesDir + "\\" + newFileName;
                if (File.Exists(newFullPath))
                {
                    File.Delete(newFullPath);

                }
                File.Move(Utils.currentMovieFilename, newFullPath);

                Utils.currentMovieFilename = newFullPath;
                dialogResult = MessageBox.Show("Save Movie Now To Preserve New Title? ", "Save", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveCurrentMovie();
                }
                else if (dialogResult == DialogResult.No)
                {
                    titleChangeNotSave = true;
                }

            }
        }

        private async void MakeAllEmptySceneTextsButton_Click(object sender, EventArgs e)
        {
            int emptySceneKount = emptySceneTexts();

            if (scenes.Count == 0)
            {
                MessageBox.Show("Error:  No Scenes.  Please make Scenes, first");
                return;
            }

            if (emptySceneKount == 0) 
            {
                MessageBox.Show("No empty Scene Texts to make.");
                return; 
            }
            
            DialogResult result = MessageBox.Show($"Proceeding will create Scene Texts for all {emptySceneKount} Scenes that currently don't have Scene Texts.  This will take up to 90 seconds per Scene Text to be generated. Do you want to proceed?", "Make Story Texts", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            int sceneIndex = 0;
            int noteIndex = 0;
            for(int j = 0;j < scenes.Count;j++)
            {
                if (scenes[j].NarrativeText.Trim().Length == 0)
                {
                    SceneInScenesList.SelectedIndex = j;
                    
                    sceneIndex = SceneInScenesList.SelectedIndex;
                    
                    updateRTBText(SceneText, $"writing Scene Text with {GetGptModel()} - awaiting reply...\r\n \r\n " + SceneHint.Text);

                    cursorTopRTB(SceneText);
                    string reply = await MyGPT.makeSceneText(GetGptModel(), myMovie, scenes, SceneInScenesList.SelectedIndex + 1, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);

                    //SceneText.Text = reply;
                    updateRTBText(SceneText, reply);
                    updateRTBText(BeatSheetRichTextbox, "");

                    scenes[j].NarrativeText = reply;
                    scenes[j].myNoteTextList = new List<NotesSceneText>();
                    scenes[j].myNoteTextList.Add(new NotesSceneText(reply, "", "Base:",""));

                    SceneInScenesList.SelectedIndex = j;

                    SceneNotesListbox.BeginUpdate();    
                    SceneNotesListbox.DataSource = null;
                    SceneNotesListbox.DisplayMember = null;
                    SceneNotesListbox.DataSource = scenes[j].myNoteTextList;
                    SceneNotesListbox.DisplayMember = "myLabel";
                    SceneNotesListbox.SelectedIndex = SceneNotesListbox.Items.Count - 1;
                    SceneNotesListbox.EndUpdate();

                    Application.DoEvents();
                }
            }
        }

        private async void MakeAllEmptySceneScriptsNutton_Click(object sender, EventArgs e)
        {
            int sceneIndex = 0;
            int noteIndex = 0;
            int scriptLength = 0;
            int textLength = 0;
            int notesListLength = 0;
            string reply = "";
            string tempBeatSheet = "";
            string tempSceneScript = "";
            int emptyScriptKount = emptyScriptTexts();

            if (scenes.Count == 0)
            {

                MessageBox.Show("Error:  No Scenes.  Please make Scenes, first");
                return;
            }


            if (emptyScriptKount == 0)
            {

                MessageBox.Show("No empty Scene Scripts to make.");

                return;

            }

            DialogResult result = MessageBox.Show($"Proceeding will attempt to create Scene Scripts for all {emptyScriptKount} Scenes that currently don't have Scene Scripts.  Those Scenes without Scene Texts will be skipped.  This will take up to 90 seconds per Script Text to be generated. Do you want to proceed?", "Make Scene Scripts", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }



            for (int j = 0; j < scenes.Count; j++)
            {

                if (scenes[j].myNoteTextList == null || scenes[j].myNoteTextList.Count <= 0) 
                
                { 
                    break; 
                }


                notesListLength = scenes[j].myNoteTextList.Count;

                
                
                scriptLength = scenes[j].myNoteTextList[notesListLength - 1].myScript.Trim().Length;

                textLength = scenes[j].myNoteTextList[notesListLength - 1].myText.Trim().Length;

                if (scriptLength == 0 && textLength > 50)
                {
                    SceneInScenesList.SelectedIndex = j;

                    sceneIndex = SceneInScenesList.SelectedIndex;

                    
                    updateRTBText(BeatSheetRichTextbox, GetGptModel() + " - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);

                    cursorTopRTB(BeatSheetRichTextbox);

                    updateRTBText(SceneScriptRichTextbox, $"making Beat Sheet with {GetGptModel()} - awaiting reply...");
                    cursorTopRTB(SceneScriptRichTextbox);


                    reply = await MyGPT.makeBeatSheet(myMovie, scenes[j].NarrativeText, GetGptModel(), (StyleElements)StyleSceneList.SelectedItem, this);

                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                       // return;
                    }
                    else
                    {
                        tempBeatSheet = reply;

                        updateRTBText(BeatSheetRichTextbox, reply);
                    }


                    tempBeatSheet = reply;
             
                    updateRTBText(BeatSheetRichTextbox, reply);

                    updateRTBText(SceneScriptRichTextbox,$"making Scene Script with {GetGptModel()} - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);
                    string dialogFlavor, customFlavor;
                    dialogFlavor = getDialogFlavor();
                    if (dialogFlavor == "FlavorCustom")
                    {
                        customFlavor = FlavorCustomText.Text;
                    }
                    else
                    {
                        customFlavor = "";
                    }
                    reply = await MyGPT.makeSceneScript(myMovie, characters, tempBeatSheet, scenes[j].NarrativeText, GetGptModel(), GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem,(StyleElements)StyleSceneList.SelectedItem, getDialogFlavor() , customFlavor , this);
                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                       // return;
                    }

                    tempSceneScript = reply;
                    scenes[j].myNoteTextList[notesListLength - 1].myScripts = new List<NotesSceneScript>();

                    scenes[j].myNoteTextList[notesListLength - 1].myScripts.Add(new NotesSceneScript(tempBeatSheet, tempSceneScript, "", "Base:"));
                    if (Utils.dataVersion > 1)

                    {
                        ScriptNotesListbox.BeginUpdate();
                        ScriptNotesListbox.DataSource = null;
                        ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[notesListLength - 1].myScripts;
                        ScriptNotesListbox.DisplayMember = "myLabel";
                        ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                        ScriptNotesListbox.EndUpdate();

                    }

                    SceneInScenesList.SelectedIndex = j;
                    SceneScriptRichTextbox.Rtf = Utils.formatScript(reply,FormatScript.Checked);
                    scenes[j].SceneScript = reply;
                    scenes[j].myNoteTextList[notesListLength - 1].myScript = reply;
                                        
                    Application.DoEvents();
                }




            }
        }

        private void SceneCount_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private async void HeavyRefactorButton_Click(object sender, EventArgs e)
        {
            List<List<string>> ListofLists = new List<List<string>>();

            int currentSceneNumber = SceneInScenesList.SelectedIndex + 1;
            int firstSceneRefactor = currentSceneNumber + 1;
            int errorKount = 0;
            Boolean looper = true;
            string sceneText;
            string mb = "";
            Boolean isError = false;

            sceneText = getLatestCurrentSceneText(currentSceneNumber);

            if (sceneText.Length == 0)
            {

                mb = $"At least one Scene Text is required for Scene {currentSceneNumber} to perform Scene Refactoring";
                MessageBox.Show(mb);
                return; 
            }
            if (!(GetGptModel().Contains("gpt-4")))
            {
                MessageBox.Show("Must be in onw of the GPT-4 modea to use Scene Refactoring.  Please change to a GPT-4 mode.");
                return;
            }

            FormScenesRefactor refactorForm = new FormScenesRefactor();
            refactorForm.ShowDialog();


            Utils.nop();

            if (Utils.refactor)
            {
                //trial string jsonString = await MyGPT.refactorSceneAfter(GetGptModel(), scenes, currentSceneNumber, Utils.currentSceneCount, Utils.currentTokenCount, sceneText, myMovie.movieText, this);


                string jsonString = await MyGPT.refactorSceneAfterHeavy(GetGptModel(), scenes, currentSceneNumber, Utils.currentSceneCount,  sceneText, myMovie.movieText, this);

                isError = Utils.checkForGPTErrors(jsonString, this);
                if (isError)
                {
                    return;
                }
                
                
                string originalJSONString = jsonString;

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
                        if (errorKount > 2)
                        {
                           
                            updateGPTStatus("GPT Status: Unrepairable JSON error.  Make small adjustment in number of scenes and try again.", Color.LightGreen);
                            return;
                        }
                        if (errorKount == 0)

                        {
                            errorKount = 1;
                            jsonString = Utils.JSONFixer(originalJSONString);
                            Utils.nop();
                            looper = true;
                        }

                        if (errorKount > 1)

                        {
                            errorKount += 1;
                            
                            jsonString = await MyGPT.fixJSON(originalJSONString, GetGptModel(), $"JSON Error - trying to repair JSON.  Error Count = {errorKount}", this);
                            isError = Utils.checkForGPTErrors(jsonString, this);
                            if (isError)
                            {
                                return;
                            }

                            looper = true;
                        }

                    }
                    Utils.nop();


                }

                // create new list of scenes
                List<SceneObj> tempScenes = new List<SceneObj>();

                // add in all those up to and including current Scene number
                for (int j = 0; j <= Utils.currentSceneNumber - 1; j++)
                {
                    tempScenes.Add(scenes[j]);

                }

                //add the new ones 
                foreach (List<string> myScene in ListofLists)
                {

                    SceneObj scene = new SceneObj();
                    string myTitle = myScene[0];
                    string Hint = myScene[1];
                    scene.Title = myTitle;
                    scene.Hint = Hint;
                    tempScenes.Add(scene);
                }

                scenes = tempScenes;

                ScenesList.BeginUpdate();
                ScenesList.DataSource = null;
                ScenesList.DataSource = scenes;
                ScenesList.DisplayMember = "Title";
                ScenesList.SelectedIndex = Utils.currentSceneNumber - 1;
                ScenesList.EndUpdate();

                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";
                SceneInScenesList.SelectedIndex = Utils.currentSceneNumber - 1;
                SceneInScenesList.Focus();
                SceneInScenesList.EndUpdate();
            }
        }

        private void UITestCountTokensButton_Click(object sender, EventArgs e)
        {
            
            int tokens = Utils.tokenCount(TokenBox.Text);
            TokenCount.Text = $"tokens: {tokens}";

        }

        private void UITestRegexButton_Click(object sender, EventArgs e)
        {
            string output = Regex.Replace(RegExBox.Text, "(?<=[^\"\\]])]", "\"]");

            RegExBox.Text = output;

        }

        private async void UITestCodeReWriteButton_Click(object sender, EventArgs e)
        {

            string systemPrompt = "You are a code writing assistant.  You will be given a sample of the code in the user prompt plus instruction on how to modify it. ";
            systemPrompt += "You will reply with the new version of code as directed";
            string userPrompt = "This is the code to rewrite: \r\n\r\n" + CodeReWrite.Text;
            userPrompt += "These are the instructions for modifying the code: \r\n\r\n";
            userPrompt += CodeMod.Text + "\r\n\r\n";
            userPrompt += "please return the modified code";



            string reply = await UtilsGPT.doGPT("UITestCodeRewrite", GetGptModel(), 8000, .7, userPrompt, systemPrompt, "clue", this, "rewriting some code");

            ReturnedCode.Text = reply;
        }

        private void Automations_Click(object sender, EventArgs e)
        {

        }

        private async void button22_Click(object sender, EventArgs e)
        {
            string reply, beatSheet, sceneScript;

            string oldText = "";
            if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count >= 1)
            {
                oldText = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myScript;
            }
            else
            {
                oldText = "";
            }
                


            if (scenes.Count == 0)
            {
                MessageBox.Show("No Scenes created yet.  Need Scenes to use this function");
                return;
            }

            if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count > 1)

            {
                DialogResult result = MessageBox.Show("You already have at least 1 Note or Edit for this Scene Script. Proceeding will create a new Scene Script  and will eliminate your Notes and Edits.  Do you want to proceed?", "New Story Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }



            }


            if (scenes[SceneInScenesList.SelectedIndex].myNoteTextList.Count > 1)

            {
                DialogResult result = MessageBox.Show("You already have at least 1 Note or Edit for this Scene Text. Proceeding will create a new Scene Text and will eliminate your Notes and Edits.  Do you want to proceed?", "New Story Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }



            }

            
            string nothing =  await doMakeSceneText();

            SceneScriptRichTextbox.Text = GetGptModel() + " creating Beat Sheet....\r\n \r\n" + oldText;
            reply = await doMakeBeatSheet();

            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                // return;
            }
            else
            {
                BeatSheetRichTextbox.Text = reply;
            }
            BeatSheetRichTextbox.Text = reply;
            beatSheet = reply;
            SceneScriptRichTextbox.Text = GetGptModel() + " creating Scene Script....\r\n \r\n" + oldText;
            Application.DoEvents();
            reply = await doWriteSceneScript();
            
            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                return;
            }
            sceneScript = reply;
            int sceneIndex = SceneInScenesList.SelectedIndex;

            SceneScriptRichTextbox.Rtf = Utils.formatScript(reply,FormatScript.Checked);

            scenes[SceneInScenesList.SelectedIndex].SceneScript = reply;
            scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScript = reply;

            scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts = new List<NotesSceneScript>();
            scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(beatSheet, sceneScript, "", "Base:"));
            if (Utils.dataVersion > 1)
            {
                
                ScriptNotesListbox.BeginUpdate();
                ScriptNotesListbox.DataSource = null;
                ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[sceneIndex].myScripts;
                ScriptNotesListbox.DisplayMember = "myLabel";
                ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                ScriptNotesListbox.EndUpdate();
            }
        }

        private async void MakeSceneScriptUsingAllStylesButton_Click(object sender, EventArgs e)
        {
            string reply;
            int loopMax = StyleSceneList.Items.Count - 1;
            int oldStyleIndex = StyleSceneList.SelectedIndex;
            if (SceneText.Text.Length > 100)
            {

                DialogResult result = MessageBox.Show($"We are about to create {StyleSceneList.Items.Count} variations of the Scene Script for Scene #{SceneInScenesList.SelectedIndex + 1} using all of the Styles in the Styles menu.\r\n\r\n Would you like to proceed?", "Make Variations?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                SceneScriptRichTextbox.Text = $"Scene {SceneInScenesList.SelectedIndex + 1} {scenes[SceneInScenesList.SelectedIndex].Title}\r\n\r\n" ;
                SceneScriptRichTextbox.Text += $"Creating {StyleSceneList.Items.Count} Style variations. \r\n\r\n";
                BeatSheetRichTextbox.Text = $"Scene {SceneInScenesList.SelectedIndex + 1} {scenes[SceneInScenesList.SelectedIndex].Title}\r\n\r\n";
                BeatSheetRichTextbox.Text += $"Creating {StyleSceneList.Items.Count} Style variations. \r\n\r\n";

                string workScript = "";

                for (int j = 0; j <= loopMax; j++)
                {

                    StyleSceneList.SelectedIndex = j;

                    workScript += $" Style: {styles[j].label}\r\n\r\n";
                    BeatSheetRichTextbox.Text = $" Style: {styles[j].label}\r\n\r\n";

                    
                    reply = await doMakeBeatSheet();
                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                        // return;
                    }
                    else
                    {
                        BeatSheetRichTextbox.Text += reply + "\r\n\r\n";
                    }

                    
                    Application.DoEvents();
                    reply = await doWriteSceneScript();

                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                        // return;
                    }
                    else
                    { 
                        workScript += reply + "\r\n\r\n"; 
                    }
                }
                SceneScriptRichTextbox.Rtf = Utils.formatScript(workScript, FormatScript.Checked);

                StyleSceneList.SelectedIndex = oldStyleIndex;

            }
            else
            {
                MessageBox.Show("Must have at least 100 characters in SceneText to make Script");
            }
        }

        private async void RegenAllScriptsButton_Click(object sender, EventArgs e)
        {
            int sceneIndex = 0;
            int scriptLength = 0;
            int textLength = 0;
            int notesListLength = 0;
            string reply = "";
            string tempBeatSheet = "";
            string tempSceneScript = "";

            //  int emptyScriptKount = emptyScriptTexts();

            if (scenes.Count == 0)
            {

                MessageBox.Show("Error:  No Scenes.  Please make Scenes, first");
                return;
            }

            DialogResult result = MessageBox.Show($"Proceeding will create new Scene Scripts for {SceneInScenesList.Items.Count} Scenes.  This will overwrite the existing Scene Scripts for your latest Scene Text in each Scene.  This will also create Scene Scripts for those Scenes without them.  This will action will take up to 100 seconds per Script Text to be generated. Do you want to proceed?", "Make All New Scripts", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            for (int j = 0; j < scenes.Count; j++)
            {

                if (scenes[j].myNoteTextList == null || scenes[j].myNoteTextList.Count <= 0)

                {
                    break;
                }

                notesListLength = scenes[j].myNoteTextList.Count;

                scriptLength = scenes[j].myNoteTextList[notesListLength - 1].myScript.Trim().Length;

                textLength = scenes[j].myNoteTextList[notesListLength - 1].myText.Trim().Length;

                SceneInScenesList.SelectedIndex = j;

                sceneIndex = SceneInScenesList.SelectedIndex;

                updateRTBText(BeatSheetRichTextbox, GetGptModel() + " - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);

                cursorTopRTB(BeatSheetRichTextbox);

                updateRTBText(SceneScriptRichTextbox, $"making Beat Sheet with {GetGptModel()} - awaiting reply...");
                cursorTopRTB(SceneScriptRichTextbox);

                reply = await MyGPT.makeBeatSheet(myMovie, scenes[j].NarrativeText, GetGptModel(), (StyleElements)StyleSceneList.SelectedItem, this);

                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    // return;
                }
                else
                {
                    tempBeatSheet = reply;

                    updateRTBText(BeatSheetRichTextbox, reply);
                }


                tempBeatSheet = reply;

                updateRTBText(BeatSheetRichTextbox, reply);

                updateRTBText(SceneScriptRichTextbox, $"making Scene Script with {GetGptModel()} - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);
                string dialogFlavor, customFlavor;
                dialogFlavor = getDialogFlavor();
                if (dialogFlavor == "FlavorCustom")
                {
                    customFlavor = FlavorCustomText.Text;
                }
                else
                {
                    customFlavor = "";
                }
                reply = await MyGPT.makeSceneScript(myMovie, characters, tempBeatSheet, scenes[j].NarrativeText, GetGptModel(), GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem,(StyleElements)StyleSceneList.SelectedItem, dialogFlavor,customFlavor, this);
                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    // return;
                }

                tempSceneScript = reply;

                SceneInScenesList.SelectedIndex = j;
                SceneScriptRichTextbox.Rtf = Utils.formatScript(reply,FormatScript.Checked);
                scenes[j].SceneScript = reply;
                scenes[j].myNoteTextList[notesListLength - 1].myScript = reply;
                                
                scenes[j].myNoteTextList[notesListLength - 1].myScripts = new List<NotesSceneScript>();

                scenes[j].myNoteTextList[notesListLength - 1].myScripts.Add(new NotesSceneScript(tempBeatSheet, tempSceneScript, "", "Base:"));
                if (Utils.dataVersion > 1)
                
                {
                    ScriptNotesListbox.BeginUpdate();
                    ScriptNotesListbox.DataSource = null;
                    ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[notesListLength - 1].myScripts;
                    ScriptNotesListbox.DisplayMember = "myLabel";
                    ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                    ScriptNotesListbox.EndUpdate();

                }

                Application.DoEvents();
            }
        }

        private void MovieTab_Click(object sender, EventArgs e)
        {

        }

        private async void ScriptApplyNoteButton_Click(object sender, EventArgs e)
        {
            int noteIndex = ScriptNotesListbox.SelectedIndex;
            int notesKount = ScriptNotesListbox.Items.Count;


            int sceneIndex = SceneInScenesList.SelectedIndex;

            string originlScriptText = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myScript;
            string spliceInText = "";
            int originalSelectionStart = SceneScriptRichTextbox.SelectionStart;
            int originalSelectionLenth = SceneScriptRichTextbox.SelectionLength;

            string originalSelectedText = SceneScriptRichTextbox.SelectedText;

            int selectionFind = originlScriptText.IndexOf(originalSelectedText);

            if (SceneScriptRichTextbox.SelectionLength == 0)  // nothing selected 
            {
                if (SceneScriptRichTextbox.Rtf.Length > 50 && ScriptNotesRTB.Text.Length > 20)
                {
                    
                    

                    //SceneText.Text = GetGptModel() + " applying Notes to Scene text....\r\n \r\n" + originlSceneText;
                    updateRTBText(SceneScriptRichTextbox, GetGptModel() + " applying Notes to Scene Script....\r\n \r\n" + originlScriptText);

                    cursorTopRTB(SceneScriptRichTextbox);

                    string response = await MyGPT.NotesForSceneScript(GetGptModel(), originlScriptText, SceneText.Text, ScriptNotesRTB.Text, GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem, (StyleElements)StyleSceneList.SelectedItem, this);
                    isError = Utils.checkForGPTErrors(response, this);
                    if (isError)
                    {
                        return;
                    }
                    //SceneText.Text = response;
                    updateRTBText(SceneScriptRichTextbox, response);
                    cursorTopRTB(SceneScriptRichTextbox);


                    scenes[sceneIndex].SceneScript = response;

                    string newLabel = makeSceneScriptMenuLabel(scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts, "Note", ScriptNotesListbox.SelectedIndex);
                    scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(BeatSheetRichTextbox.Text, response, ScriptNotesRTB.Text, newLabel));


                    notesKount += 1;
                    /*
                    SceneNotesListbox.DataSource = null;
                    SceneNotesListbox.DisplayMember = null;
                    SceneNotesListbox.DataSource = scenes[sceneIndex].myNoteTextList;
                    SceneNotesListbox.DisplayMember = "myLabel";
                    SceneNotesListbox.SelectedIndex = notesKount - 1;
                    */

                    if (Utils.dataVersion > 1)
                    {
                        ScriptNotesListbox.BeginUpdate();
                        ScriptNotesListbox.DataSource = null;
                        ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                        ScriptNotesListbox.DisplayMember = "myLabel";
                        ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                        ScriptNotesListbox.EndUpdate();
                    }
                    Application.DoEvents();
                }
                else
                {

                    MessageBox.Show("Not Enough Script Text or Text Notes.  Notes need at least 20 characters.  Script Text needs at least 50 characters ");
                }
            }
            else //something selected  
            {
                if (SceneScriptRichTextbox.Rtf.Length > 50 && ScriptNotesRTB.Text.Length > 5)
                {
                    

                    
                    
                    updateRTBText(SceneScriptRichTextbox, GetGptModel() + " applying Notes to Selected Region in the Scene Script....\r\n \r\n" + getSceneScriptText());

                    cursorTopRTB(SceneScriptRichTextbox);

                    // string response = await MyGPT.NotesForSceneScript(GetGptModel(), originlScriptText, SceneText.Text, ScriptNotesRTB.Text, GlobalDialogFormat.Checked, (StyleElements)StyleGuideListbox.SelectedItem, (StyleElements)StyleSceneList.SelectedItem, this);
                    

                    string response = await MyGPT.NotesForSELECTEDSceneScript(GetGptModel(), getSceneScriptText(),  originalSelectedText, originalSelectionStart, SceneText.Text, ScriptNotesRTB.Text, GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem, (StyleElements)StyleSceneList.SelectedItem, this);
                    spliceInText = response;

                    isError = Utils.checkForGPTErrors(response, this);
                    if (isError)
                    {
                        return;
                    }

                    response = Utils.spliceWithLocs(SceneScriptRichTextbox.Text, originalSelectedText, originalSelectionStart, originalSelectedText.Length, response);

                    //SceneText.Text = response;
                    updateRTBText(SceneScriptRichTextbox, response);
                    cursorTopRTB(SceneScriptRichTextbox);


                    scenes[sceneIndex].SceneScript = response;

                    string newLabel = makeSceneScriptMenuLabel(scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts, "Note", ScriptNotesListbox.SelectedIndex);
                    scenes[sceneIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(new NotesSceneScript(BeatSheetRichTextbox.Text, response, ScriptNotesRTB.Text, newLabel));


                    notesKount += 1;
                    

                    if (Utils.dataVersion > 1)
                    {
                        ScriptNotesListbox.BeginUpdate();
                        ScriptNotesListbox.DataSource = null;
                        ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                        ScriptNotesListbox.DisplayMember = "myLabel";
                        ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                        ScriptNotesListbox.EndUpdate();
                    }

                    if (originalSelectedText.Trim().Length > 0)

                    {
                        SceneScriptRichTextbox.Focus();
                        SceneScriptRichTextbox.SelectionStart = originalSelectionStart;
                        SceneScriptRichTextbox.SelectionLength = spliceInText.Length;


                    }
                    Application.DoEvents();
                }
                else
                {

                    MessageBox.Show("Not Enough Script Text or Text Notes.  Notes need at least 20 characters.  Script Text needs at least 50 characters ");
                }




            }
            
            
            
        }

        private void ScriptNotesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newScript, newBeatSheet, newNote;

            /* if (Utils.newScenesFlag == true)
            {
                Utils.newScenesFlag = false;
                return;
            } */

            if (SceneInScenesList.SelectedIndex < 0 ||  SceneNotesListbox.SelectedIndex < 0 )
            {
                return;
            }
                if ((scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count > 0 && ScriptNotesListbox.SelectedIndex > -1))
                {

                    Utils.newScenesFlag = false;
                    if (SceneInScenesList.SelectedIndex > -1 && SceneInScenesList.SelectedIndex < SceneInScenesList.Items.Count && SceneNotesListbox.SelectedIndex > -1 && SceneNotesListbox.SelectedIndex < SceneNotesListbox.Items.Count
                        && ScriptNotesListbox.SelectedIndex > -1 && ScriptNotesListbox.SelectedIndex < scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Count)
                    {
                        newScript = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myScript;
                        newBeatSheet = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myBeatSheet;
                        newNote = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myNote;


                        updateRTBText(SceneScriptRichTextbox, newScript);
                        cursorTopRTB(SceneScriptRichTextbox);

                        updateRTBText(BeatSheetRichTextbox, newBeatSheet);
                        cursorTopRTB(BeatSheetRichTextbox);

                        updateRTBText(ScriptNotesRTB, newNote);
                        cursorTopRTB(ScriptNotesRTB);
                    }
                    else
                    {
                        /*
                        if (Utils.dataVersion > 1)
                        {
                            ScriptNotesListbox.BeginUpdate();
                            ScriptNotesListbox.DataSource = null;
                            ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                            ScriptNotesListbox.DisplayMember = "myLabel";
                            ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                            ScriptNotesListbox.EndUpdate();
                        }
                        */
                        // last 07/10/2023 noon 
                    }


                }
                else
                {
                    /* if (Utils.dataVersion > 1)
                    {
                        ScriptNotesListbox.BeginUpdate();
                        ScriptNotesListbox.DataSource = null;
                        ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                        ScriptNotesListbox.DisplayMember = "myLabel";
                        ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                        ScriptNotesListbox.EndUpdate();
                    }
                    */
                }

            
        }
            

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            ScriptNotesRTB.Text = "";
        }

        private async void button27_Click(object sender, EventArgs e)
        {
            if (checkSelected(SceneScriptRichTextbox)) { return; }

            Utils.magicSceneScriptNoteFlag = false;
            List<string> listFactors = new List<string>();
            if (CustomMagicScriptText.Checked == true)
            {
                FormSceneScriptMagic magicSceneScriptForm = new FormSceneScriptMagic();
                magicSceneScriptForm.ShowDialog();
                if (Utils.magicSceneScriptNoteFlag == false) { return; }
                Utils.nop();
                listFactors = new List<string>();

                listFactors = magicSceneScriptForm.criticFactors;
                listFactors.Reverse();
            }
            else  // traditional style 
            {
                listFactors = new List<string>();
            }

            if (Utils.magicSceneScriptNoteFlag == false) // don't process listFactors
            {
                listFactors = new List<string>();
            }

            ScriptNotesRTB.Text = $"making magic note...using {GetGptModel()}... ";


            string sceneScript = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex].myScript;
            string reply = await MyGPT.makeMagicSceneScriptNote(GetGptModel(), sceneScript, SceneText.Text, listFactors,this);

            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                return;
            }

            ScriptNotesRTB.Text = reply;

        }


        private async void WTF()
        {
            ScriptNotesRTB.Text = "making magic note...";

            
        }



       
        

        private void GlobalDialogFormat_CheckedChanged_1(object sender, EventArgs e)
        {
            Boolean dialogFormatingOn = false;

            if (GlobalDialogFormat.Checked == true)
            {
                dialogFormatingOn = true;
                Utils.dataDictionary["DialogFormat"] = dialogFormatingOn;
            }
        }

        private void McKee_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void StyleSceneList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            updateScriptStylesToDictionary(StyleSceneList);
        }

        private void ScriptStylesAddAuthor_Click(object sender, EventArgs e)
        {
            string jsonString;
            if (NewAuthor.Text.Length > 5)
            {
                styles.Add(new StyleElements($"{NewAuthor.Text}", "author", $"{NewAuthor.Text}"));
                StyleSceneList.DataSource = null;
                StyleSceneList.DataSource = styles;
                StyleSceneList.DisplayMember = "label";
                StyleSceneList.SelectedIndex = StyleSceneList.Items.Count - 1;
                jsonString = JsonConvert.SerializeObject(styles);
                File.WriteAllText(styleFile, jsonString);
                NewAuthor.Text = "";
                updateScriptStylesToDictionary(StyleSceneList);
            }
            else
            {
                MessageBox.Show("At least 5 characters required for author name");
            }
        }

        private void ScriptStylesDeleteAuthorButton_Click(object sender, EventArgs e)
        {
            string myStyle = styles[StyleSceneList.SelectedIndex].style;

            int startIndex = StyleSceneList.SelectedIndex;

            string jsonString;

            if (myStyle == "-none-")
            {
                MessageBox.Show("You can not delete the \"-none-\" style");
                return;

            }
            DialogResult result = MessageBox.Show($"Proceeding will permanenetly delete style:\r\n{myStyle} \r\n\r\n Do you want to proceed?", "Delete Author", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            List<StyleElements> tempStyleList = null;
            tempStyleList = styles;
            tempStyleList.RemoveAt(startIndex);

            styles = tempStyleList;


            if (startIndex > 0)
            {
                StyleSceneList.BeginUpdate();
                StyleSceneList.DataSource = null;
                StyleSceneList.DisplayMember = null;

                StyleSceneList.DataSource = styles;
                StyleSceneList.DisplayMember = "label";

                StyleSceneList.SelectedIndex = startIndex - 1;
                StyleSceneList.EndUpdate();
            }
            else
            {
                StyleSceneList.BeginUpdate();
                StyleSceneList.DataSource = null;
                StyleSceneList.DisplayMember = null;

                StyleSceneList.DataSource = styles;
                StyleSceneList.DisplayMember = "label";

                StyleSceneList.SelectedIndex = 0;
                StyleSceneList.EndUpdate();
            }

            jsonString = JsonConvert.SerializeObject(styles);
            File.WriteAllText(styleFile, jsonString);

            updateScriptStylesToDictionary(StyleSceneList);
        }

        private void StyleGuideListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScriptStyleGuideListbox.SelectedIndex > -1)
            {
                DisplayStyleGuideRTB.Text = scriptStyleGuides[ScriptStyleGuideListbox.SelectedIndex].style.Trim();
                if (scriptStyleGuides[ScriptStyleGuideListbox.SelectedIndex].label == "-none-")
                {
                    DisplayStyleGuideRTB.ReadOnly = true;
                }
                else
                {
                    DisplayStyleGuideRTB.ReadOnly = false;
                }
            }
            updateScriptStylesToDictionary(ScriptStyleGuideListbox);
        }

        private void ScriptStylesSaveButton_Click(object sender, EventArgs e)
        {
            int index = ScriptStyleGuideListbox.SelectedIndex;
            string newStyle = DisplayStyleGuideRTB.Text;
            string label = scriptStyleGuides[index].label;

            scriptStyleGuides[index] = new StyleElements { label = label, type = "guide", style = newStyle };
            string jsonString = JsonConvert.SerializeObject(scriptStyleGuides);
            File.WriteAllText(scriptStyleGuideFile, jsonString);
            ScriptStylesSaveLabel.Text = "Last saved: " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void ScriptStylesDeleteStyleButton_Click(object sender, EventArgs e)
        {
            string myStyle = scriptStyleGuides[ScriptStyleGuideListbox.SelectedIndex].label;

            int startIndex = ScriptStyleGuideListbox.SelectedIndex;

            string jsonString;

            if (myStyle == "-none-")
            {
                MessageBox.Show("You can not delete the \"-none-\" style");
                return;

            }
            DialogResult result = MessageBox.Show($"Proceeding will permanenetly delete style:\r\n{myStyle} \r\n\r\n Do you want to proceed?", "Delete Author", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            List<StyleElements> tempStyleGuides = null;
            tempStyleGuides = scriptStyleGuides;
            tempStyleGuides.RemoveAt(startIndex);

            scriptStyleGuides = tempStyleGuides;


            if (startIndex > 0)
            {
                ScriptStyleGuideListbox.BeginUpdate();
                ScriptStyleGuideListbox.DataSource = null;
                ScriptStyleGuideListbox.DisplayMember = null;

                ScriptStyleGuideListbox.DataSource = scriptStyleGuides;
                ScriptStyleGuideListbox.DisplayMember = "label";

                ScriptStyleGuideListbox.SelectedIndex = startIndex - 1;
                ScriptStyleGuideListbox.EndUpdate();




            }
            else
            {
                ScriptStyleGuideListbox.BeginUpdate();
                ScriptStyleGuideListbox.DataSource = null;
                ScriptStyleGuideListbox.DisplayMember = null;

                ScriptStyleGuideListbox.DataSource = styles;
                ScriptStyleGuideListbox.DisplayMember = "label";

                ScriptStyleGuideListbox.SelectedIndex = 0;
                ScriptStyleGuideListbox.EndUpdate();

            }

            jsonString = JsonConvert.SerializeObject(scriptStyleGuides);
            File.WriteAllText(scriptStyleGuideFile, jsonString);

            updateScriptStylesToDictionary(ScriptStyleGuideListbox);
        }

        private void AuthorStyleLabel_Click(object sender, EventArgs e)
        {

        }

        private void ScriptStylesCopyButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(DisplayStyleGuideRTB);
        }

        private void ScriptStylesAddStyleButton_Click(object sender, EventArgs e)
        {

            string jsonString;
            int listboxCount = ScriptStyleGuideListbox.Items.Count; 
            if (NewScriptStyleGuide.Text.Length >= 5)
            {
                scriptStyleGuides.Add(new StyleElements($"{NewScriptStyleGuide.Text}", "guide", "style guide here..."));
                ScriptStyleGuideListbox.DataSource = null;
                ScriptStyleGuideListbox.DataSource = scriptStyleGuides;
                ScriptStyleGuideListbox.DisplayMember = "label";
                ScriptStyleGuideListbox.SelectedIndex = ScriptStyleGuideListbox.Items.Count - 1;
                jsonString = JsonConvert.SerializeObject(scriptStyleGuides);
                File.WriteAllText(scriptStyleGuideFile, jsonString);
                NewScriptStyleGuide.Text = "";
                DisplayStyleGuideRTB.Text = scriptStyleGuides[(ScriptStyleGuideListbox.Items.Count) - 1].style;
            }
            else
            {
                MessageBox.Show("At least 5 characters required for Style Guide ");
            }

            updateScriptStylesToDictionary(ScriptStyleGuideListbox);

        }

        private async void MakeSceneMagicNoteButton_Click(object sender, EventArgs e)
        {

            if (checkSelected(SceneText)) { return; }

            Utils.magicSceneTextNoteFlag = false;
            List<string> listFactors = new List<string>();
            if (CustomMagicSceneText.Checked == true)
            {
                FormSceneTextMagic magicSceneTextForm = new FormSceneTextMagic();
                magicSceneTextForm.ShowDialog();
                if (Utils.magicSceneTextNoteFlag == false) { return;}
                
                Utils.nop();
                listFactors = new List<string>();

                listFactors = magicSceneTextForm.criticFactors;
                listFactors.Reverse();
            }
            else  // traditional style 
            {
                listFactors = new List<string>();
            }



            SceneNoteRichTextBox.Text = $"making magic note...using {GetGptModel()}... ";

            string reply = await MyGPT.makeMagicSceneTextNote(GetGptModel(), SceneHint.Text,  SceneText.Text,myMovie,scenes,listFactors,this);

            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                return;
            }

            SceneNoteRichTextBox.Text = reply;
            
        }

        private async void MakeMagicNotesButton_Click(object sender, EventArgs e)
        {
            if (checkSelected(MovieText)) { return; };

            string originalNote = NotesForMovieText.Text;
            
            NotesForMovieText.Text = $"making magic note...using {GetGptModel()}";
                List<string> listFactors = new List<string>();
                if (CustomMagicNotes.Checked == true)
                {
                Utils.magicMovieNoteFlag = false;
                    FormMovieTextMagic magicMoveTextForm = new FormMovieTextMagic();
                    magicMoveTextForm.ShowDialog();
                    if (Utils.magicMovieNoteFlag == false)

                        {
                            NotesForMovieText.Text = originalNote;
                            return;
                        }
                    Utils.nop();
                    listFactors = new List<string>();

                    listFactors = magicMoveTextForm.criticFactors;
                    listFactors.Reverse();
                }
                else  // traditional style 
                {
                    listFactors = new List<string>();
                }
               

               
                string reply = await MyGPT.makeMagicMovieTextNote(GetGptModel(), myMovie.movieText, myMovie.movieHintText , listFactors, this);

                isError = Utils.checkForGPTErrors(reply, this);
                if (isError)
                {
                    return;
                }

                NotesForMovieText.Text = reply;
              


            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EditScriptTextButton_Click(object sender, EventArgs e)
        {
            Utils.originalScriptText = getSceneScriptText();
            HideEditButton(SceneScriptRichTextbox);
            ShowApplyButton(SceneScriptRichTextbox);
            SceneScriptRichTextbox.Focus();
        }

        private void ApplyScriptTextEdits_Click(object sender, EventArgs e)
        {
            // DisableTextChangedEvent(SceneText);
            HideApplyButton(SceneScriptRichTextbox);

            int noteSceneIndex = SceneNotesListbox.SelectedIndex;
            int noteScriptIndex = ScriptNotesListbox.SelectedIndex;
            


            int sceneIndex = SceneInScenesList.SelectedIndex;

            string originalScriptText = Utils.originalScriptText;


            scenes[sceneIndex].SceneScript = SceneScriptRichTextbox.Text;

            scenes[sceneIndex].myNoteTextList[noteSceneIndex].myScript = SceneScriptRichTextbox.Text;
            
            string mylabel = makeSceneScriptMenuLabel(scenes[sceneIndex].myNoteTextList[noteSceneIndex].myScripts, "Edit", noteScriptIndex);


            scenes[sceneIndex].myNoteTextList[noteSceneIndex].myScripts.Add(new NotesSceneScript(BeatSheetRichTextbox.Text, SceneScriptRichTextbox.Text,  "", mylabel));


            

            ScriptNotesListbox.BeginUpdate();
            ScriptNotesListbox.DataSource = null;
            ScriptNotesListbox.DisplayMember = null;
            ScriptNotesListbox.DataSource = scenes[sceneIndex].myNoteTextList[noteSceneIndex].myScripts;
            ScriptNotesListbox.DisplayMember = "myLabel";
            ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
            ScriptNotesListbox.EndUpdate();

            updateRTBText(ScriptNotesRTB, "");
            


            HideApplyButton(SceneScriptRichTextbox);
            ShowEditButton(SceneScriptRichTextbox);
        }

        private void DiscardScriptTextEdits_Click(object sender, EventArgs e)
        {
            updateRTBText(SceneScriptRichTextbox, Utils.originalScriptText);
            
        }

        private void button37_Click(object sender, EventArgs e)
        {
            
            if (scenes.Count == 0 || SceneNotesListbox.SelectedIndex < 0 || ScriptNotesListbox.SelectedIndex < 0) return;

            { 
            
                NotesSceneScript selectedScript = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts[ScriptNotesListbox.SelectedIndex];
                selectedScript.myLabel = "base:";

                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts = new List<NotesSceneScript>();

                scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts.Add(selectedScript);

                SceneScriptRichTextbox.Rtf = Utils.formatScript(selectedScript.myScript,FormatScript.Checked);
                BeatSheetRichTextbox.Text = selectedScript.myBeatSheet;

            

                ScriptNotesListbox.BeginUpdate();
                ScriptNotesListbox.DataSource = null;
                ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[SceneNotesListbox.SelectedIndex].myScripts;
                ScriptNotesListbox.DisplayMember = "myLabel";
                ScriptNotesListbox.EndUpdate();

                updateRTBText(ScriptNotesRTB, "");

            }
        }

        private async void MovieTextJumpSTart_Click(object sender, EventArgs e)
        {
            string originalMovieText = MovieText.Text;
            FormJumpStart jumpStartForm = new FormJumpStart(MovieHintText.Text, MovieText.Text, this);
            Boolean newJumpFlag = false;

            jumpStartForm.ShowDialog();
            
            string myMovieText = "";
            string myMagicMovieNote;
            
            if (Utils.jumpStartFlag)

            {
                DataDumps.Text = "";

                List<string> emptyList = new List<string>();
                
                if (jumpStartForm.lengthMovieText == 0)
                {
                    MovieText.Text = $"Jump Start making Movie Text using {GetGptModel()}...";
                    myMovieText = await doMakeMovieText(MovieHintText.Text);
                    DataDumps.Text = "::::: Initial Movie Text Generated From Movie Seed :::::\r\n\r\n";
                    DataDumps.Text += myMovieText;
                }
                else
                {
                    myMovieText = MovieText.Text;
                    DataDumps.Text = "::::: Initial Movie Text Generated From Movie Seed :::::\r\n\r\n";
                    DataDumps.Text += myMovieText;
                }



                string compiledNotes = "";

                for (int j = 1; j <= jumpStartForm.loopCount; j++)
                {
                    MovieText.Text = $"Jump Start running {j} of {jumpStartForm.loopCount} loops making and applying Magic Notes to Movie Text using {GetGptModel()}...\r\n\r\n{myMovieText}";

                    
                    myMagicMovieNote = await MyGPT.makeMagicMovieTextNote(GetGptModel(), myMovieText, MovieHintText.Text, emptyList,this);

                    DataDumps.Text += $"::::: Round {j} Magic Notes ::::::\r\n\r\n{myMagicMovieNote}";
                    compiledNotes += $"::::: Round {j} Magic Notes ::::::\r\n\r\n{myMagicMovieNote}";

                    myMovieText = await MyGPT.NotesForMovieText(GetGptModel(), myMovieText, MovieHintText.Text, myMagicMovieNote, this);

                    DataDumps.Text += $"\r\n\r\n::::: Round {j} Movie Text From Applied Magic Notes :::::\r\n\r\n{myMovieText}";
                }


                myMovie.movieText = myMovieText;
                if (jumpStartForm.lengthMovieText == 0) { newJumpFlag = true; }  

                if (jumpStartForm.lengthMovieText == 0 && myMovie.myNoteTextList.Count < 1)
                {
                    myMovie.myNoteTextList = new List<NotesMovieText>();

                    myMovie.myNoteTextList.Add(new NotesMovieText(myMovieText, compiledNotes, "Base:"));
                }

                else
                {
                    // note 4 parameter version   see newJumpFlag boolean 
                    string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Jump", NotesList.SelectedIndex, newJumpFlag);

                    myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, compiledNotes, menuItem));
                }
                
                NotesList.DataSource = null; 
                NotesList.DataSource = myMovie.myNoteTextList;
                NotesList.DisplayMember = "myLabel";
                NotesList.SelectedIndex = NotesList.Items.Count -1;
                MovieText.Text = myMovieText;
                updateRTBText(MovieText, myMovieText);

            }

            else
            {
                MovieText.Text = originalMovieText;
            }
        }

        private void ScriptStyles_Click(object sender, EventArgs e)
        {

        }

        private void MovieText_TextChanged(object sender, EventArgs e)
        {

        }

        private async void MovieTextJumpStart_Click_1(object sender, EventArgs e)
        {
            string originalMovieText = MovieText.Text;
            FormJumpStart jumpStartForm = new FormJumpStart(MovieHintText.Text, MovieText.Text, this);
            Boolean newJumpFlag = false;
            string menuItem = "";
            jumpStartForm.ShowDialog();

            string myMovieText = "";
            string myMagicMovieNote;

            if (Utils.jumpStartFlag)

            {
                DataDumps.Text = "";

                List<string> emptyList = new List<string>();

                if (jumpStartForm.lengthMovieText == 0)
                {
                    MovieText.Text = $"Jump Start making Movie Text using {GetGptModel()}...";
                    myMovieText = await doMakeMovieText(MovieHintText.Text);
                    DataDumps.Text = "::::: Initial Movie Text Generated From Movie Seed :::::\r\n\r\n";
                    DataDumps.Text += myMovieText;
                }
                else
                {
                    myMovieText = MovieText.Text;
                    DataDumps.Text = "::::: Initial Movie Text Generated From Movie Seed :::::\r\n\r\n";
                    DataDumps.Text += myMovieText;
                }



                string compiledNotes = "";

                for (int j = 1; j <= jumpStartForm.loopCount; j++)
                {
                    MovieText.Text = $"Jump Start running {j} of {jumpStartForm.loopCount} loops making and applying Magic Notes to Movie Text using {GetGptModel()}...\r\n\r\n{myMovieText}";


                    myMagicMovieNote = await MyGPT.makeMagicMovieTextNote(GetGptModel(), myMovieText, MovieHintText.Text, emptyList, this);

                    DataDumps.Text += $"::::: Round {j} Magic Notes ::::::\r\n\r\n{myMagicMovieNote}";
                    compiledNotes += $"::::: Round {j} Magic Notes ::::::\r\n\r\n{myMagicMovieNote}";

                    myMovieText = await MyGPT.NotesForMovieText(GetGptModel(), myMovieText, MovieHintText.Text, myMagicMovieNote, this);

                    DataDumps.Text += $"\r\n\r\n::::: Round {j} Movie Text From Applied Magic Notes :::::\r\n\r\n{myMovieText}";
                }


                myMovie.movieText = myMovieText;
                if (jumpStartForm.lengthMovieText == 0) { newJumpFlag = true; }

                if (jumpStartForm.lengthMovieText == 0 && myMovie.myNoteTextList.Count < 1)
                {
                    myMovie.myNoteTextList = new List<NotesMovieText>();
                    menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Base", 0, myMovie.movieText.Length);

                    menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();
                    myMovie.myNoteTextList.Add(new NotesMovieText(myMovieText, compiledNotes, menuItem));
                }

                else
                {
                    // note 4 parameter version   see newJumpFlag boolean 
                    menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Jump", NotesList.SelectedIndex, myMovie.movieText.Length);
                    menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();
                    myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, compiledNotes, menuItem));
                }

                
                NotesList.DataSource = null;
                NotesList.DataSource = myMovie.myNoteTextList;
                NotesList.DisplayMember = "myLabel";
                NotesList.SelectedIndex = NotesList.Items.Count - 1;
                MovieText.Text = myMovieText;
                updateRTBText(MovieText, myMovieText);

            }

            else
            {
                MovieText.Text = originalMovieText;
            }
        }

        private async void FullScriptAutomationsButton_Click(object sender, EventArgs e)
        {
            FormFullAuto fullAutoForm = new FormFullAuto( (int)SceneCount.Value,  scenes.Count, MovieHintText.Text, MovieText.Text);
            fullAutoForm.ShowDialog();
            
            string autoType = fullAutoForm.autoType;
            string reply;

            if (fullAutoForm.fullAutoFlag && autoType.Trim().Length > 0)
            {

                if (autoType == "FromSeed") // make Movie Text from Movie Seed 
                {
                    if (myMovie.myNoteTextList.Count > 1)

                    {
                        DialogResult result = MessageBox.Show("You already have at least 1 Note or Edit for this Movie Text. Proceeding will create a new Movie Text and eliminate your Notes and Edits.  Do you wnat to proceed?", "New Story Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // DisableTextChangedEvent(MovieText);
                    HideApplyButton(MovieText);

                    //MovieText.Text = 
                    updateRTBText(MovieText, GetGptModel() + " awaiting reply...\r\n \r\n" + MovieHintText.Text);

                    cursorTopRTB(MovieText);
                    reply = await doMakeMovieText(MovieHintText.Text);

                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                        return;
                    }

                    //MovieText.Text = reply;
                    updateRTBText(MovieText, reply);

                    myMovie.movieText = reply;

                    myMovie.myNoteTextList = new List<NotesMovieText>();

                    myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", "Base:"));
                    NotesList.DataSource = myMovie.myNoteTextList;
                    NotesList.DisplayMember = "myLabel";
                    Application.DoEvents();
                    ShowEditButton(MovieText);


                }

                if (autoType == "FromSeed" || autoType == "FromText")  // make scenes from Movie text 
                {
                    int nop = await doMakeScenesFromMovieText();
                }

                // Make all Scene texts

                SceneInScenesList.BeginUpdate();
                SceneInScenesList.DataSource = null;
                SceneInScenesList.DisplayMember = null;

                SceneInScenesList.DataSource = scenes;
                SceneInScenesList.DisplayMember = "Title";

                SceneInScenesList.SelectedIndex = 0;

                int emptySceneKount = emptySceneTexts();

                int sceneIndex = 0;
                int noteIndex = 0;

                for (int j = 0; j < scenes.Count; j++)
                {
                    
                    SceneInScenesList.SelectedIndex = j;

                    sceneIndex = SceneInScenesList.SelectedIndex;

                    updateRTBText(SceneText, $"writing Scene Text with {GetGptModel()} - awaiting reply...\r\n \r\n " + SceneHint.Text);

                    cursorTopRTB(SceneText);
               
                    reply = await MyGPT.makeSceneText(GetGptModel(), myMovie, scenes, SceneInScenesList.SelectedIndex + 1, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);

                    //SceneText.Text = reply;
                    updateRTBText(SceneText, reply);
                    updateRTBText(BeatSheetRichTextbox, "");

                    scenes[j].NarrativeText = reply;
                    scenes[j].myNoteTextList = new List<NotesSceneText>();
                    scenes[j].myNoteTextList.Add(new NotesSceneText(reply, "", "Base:", ""));

                    SceneInScenesList.SelectedIndex = j;

                    SceneNotesListbox.BeginUpdate();
                    SceneNotesListbox.DataSource = null;
                    SceneNotesListbox.DisplayMember = null;
                    SceneNotesListbox.DataSource = scenes[j].myNoteTextList;
                    SceneNotesListbox.DisplayMember = "myLabel";
                    SceneNotesListbox.SelectedIndex = SceneNotesListbox.Items.Count - 1;
                    SceneNotesListbox.EndUpdate();

                    Application.DoEvents();
                }

                // make All Scripts and Beat Sheets

                
                int scriptLength = 0;
                int textLength = 0;
                int notesListLength = 0;
                
                string tempBeatSheet = "";
                string tempSceneScript = "";
                int emptyScriptKount = emptyScriptTexts();

                for (int j = 0; j < scenes.Count; j++)
                {
                    notesListLength = scenes[j].myNoteTextList.Count;

                    scriptLength = scenes[j].myNoteTextList[notesListLength - 1].myScript.Trim().Length;

                    textLength = scenes[j].myNoteTextList[notesListLength - 1].myText.Trim().Length;
                    
                    SceneInScenesList.SelectedIndex = j;

                    sceneIndex = SceneInScenesList.SelectedIndex;

                    updateRTBText(BeatSheetRichTextbox, GetGptModel() + " - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);

                    cursorTopRTB(BeatSheetRichTextbox);

                    updateRTBText(SceneScriptRichTextbox, $"making Beat Sheet with {GetGptModel()} - awaiting reply...");
                    cursorTopRTB(SceneScriptRichTextbox);

                    reply = await MyGPT.makeBeatSheet(myMovie, scenes[j].NarrativeText, GetGptModel(), (StyleElements)StyleSceneList.SelectedItem, this);

                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                        // return;
                    }
                    else
                    {
                        tempBeatSheet = reply;
                        updateRTBText(BeatSheetRichTextbox, reply);
                    }


                    tempBeatSheet = reply;

                    updateRTBText(BeatSheetRichTextbox, reply);

                    updateRTBText(SceneScriptRichTextbox, $"making Scene Script with {GetGptModel()} - awaiting reply...\r\n \r\n " + scenes[j].NarrativeText);
                    string dialogFlavor, customFlavor;
                    dialogFlavor = getDialogFlavor();
                    if (dialogFlavor == "FlavorCustom")
                    {
                        customFlavor = FlavorCustomText.Text;
                    }
                    else
                    {
                        customFlavor = "";
                    }
                    reply = await MyGPT.makeSceneScript(myMovie, characters, tempBeatSheet, scenes[j].NarrativeText, GetGptModel(), GlobalDialogFormat.Checked, (StyleElements)ScriptStyleGuideListbox.SelectedItem, (StyleElements)StyleSceneList.SelectedItem, dialogFlavor,customFlavor, this);
                    isError = Utils.checkForGPTErrors(reply, this);
                    if (isError)
                    {
                        // return;
                    }

                    tempSceneScript = reply;
                    scenes[j].myNoteTextList[notesListLength - 1].myScripts = new List<NotesSceneScript>();

                    scenes[j].myNoteTextList[notesListLength - 1].myScripts.Add(new NotesSceneScript(tempBeatSheet, tempSceneScript, "", "Base:"));
                    if (Utils.dataVersion > 1)

                    {
                        ScriptNotesListbox.BeginUpdate();
                        ScriptNotesListbox.DataSource = null;
                        ScriptNotesListbox.DataSource = scenes[SceneInScenesList.SelectedIndex].myNoteTextList[notesListLength - 1].myScripts;
                        ScriptNotesListbox.DisplayMember = "myLabel";
                        ScriptNotesListbox.SelectedIndex = ScriptNotesListbox.Items.Count - 1;
                        ScriptNotesListbox.EndUpdate();

                    }

                    SceneInScenesList.SelectedIndex = j;
                    SceneScriptRichTextbox.Rtf = Utils.formatScript(reply,FormatScript.Checked);
                    scenes[j].SceneScript = reply;
                    scenes[j].myNoteTextList[notesListLength - 1].myScript = reply;

                    Application.DoEvents();
                }
            }
        }

        private void DebugWriteScriptFromSceneHintButton_Click(object sender, EventArgs e)
        {

        }

        private void DebugDumpMovieAndScenesButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            FormDumpJSON dumpJSONForm = new FormDumpJSON();
            
            string dumpFile;
            
            string scenesJSON;
            string movieJSON;
            string selectedPath;
            int sceneNumber = 0;
            
            dumpJSONForm.ShowDialog();
            
            if (dumpJSONForm.dumpJSONFlag)
            {

                using (CommonOpenFileDialog folderPicker = new CommonOpenFileDialog())
                {
                    folderPicker.IsFolderPicker = true;
                    folderPicker.Title = "Select a directory for JSON dump files";

                    if (folderPicker.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        MiniSceneObj miniScene;
                        MiniMovieObj miniMovieObj = new MiniMovieObj();
                        List<MiniSceneObj> sceneList = new List<MiniSceneObj>();

                        miniMovieObj.title = myMovie.title;
                        miniMovieObj.movieSeed = myMovie.movieHintText;
                        miniMovieObj.movieText = myMovie.movieText;


                        foreach (SceneObj scene in scenes)
                        {
                            sceneNumber++;
                            miniScene = new MiniSceneObj();
                            miniScene.sceneNumber = sceneNumber.ToString();
                            miniScene.sceneTitle = scene.Title;
                            miniScene.sceneSeed = scene.Hint;
                            miniScene.sceneText = scene.NarrativeText;
                            miniScene.sceneBeatSheet = scene.BeatSheetText;
                            miniScene.sceneScript = scene.SceneScript;
                            sceneList.Add(miniScene);

                        }

                        movieJSON = JsonConvert.SerializeObject(miniMovieObj);
                        scenesJSON = JsonConvert.SerializeObject(sceneList);


                        // selectedPath = folderBrowserDialog.SelectedPath;

                        selectedPath = folderPicker.FileName;

                        dumpFile = selectedPath + "\\movieDump.json";

                        try
                        {
                            File.WriteAllText(dumpFile, movieJSON);
                        }
                        catch
                        {
                            MessageBox.Show($"Writing to {dumpFile} failed.  Bad directory?");
                            return;
                        }

                        dumpFile = selectedPath + "\\scenesDump.json";

                        try
                        {
                            File.WriteAllText(dumpFile, scenesJSON);
                        }
                        catch
                        {
                            MessageBox.Show($"Writing to {dumpFile} failed.  Bad directory?");
                            return;
                        }



                        MessageBox.Show($"JSON Dump Files Successfully Written to {selectedPath}");
                    }
                }

                return;


                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    MiniSceneObj miniScene;
                    MiniMovieObj miniMovieObj = new MiniMovieObj();
                    List<MiniSceneObj> sceneList = new List<MiniSceneObj>();

                    miniMovieObj.title = myMovie.title;
                    miniMovieObj.movieSeed = myMovie.movieHintText;
                    miniMovieObj.movieText = myMovie.movieText;


                    foreach (SceneObj scene in scenes)
                    {
                        sceneNumber++;
                        miniScene = new MiniSceneObj();
                        miniScene.sceneNumber = sceneNumber.ToString();
                        miniScene.sceneTitle = scene.Title;
                        miniScene.sceneSeed = scene.Hint;
                        miniScene.sceneText = scene.NarrativeText;
                        miniScene.sceneBeatSheet = scene.BeatSheetText;
                        miniScene.sceneScript = scene.SceneScript;
                        sceneList.Add(miniScene);

                    }

                    movieJSON = JsonConvert.SerializeObject(miniMovieObj);
                    scenesJSON = JsonConvert.SerializeObject(sceneList);
                    

                    selectedPath = folderBrowserDialog.SelectedPath;

                    dumpFile = selectedPath + "\\movieDump.json";

                    try 
                    {
                        File.WriteAllText(dumpFile, movieJSON); 
                    }
                    catch 
                    {
                        MessageBox.Show($"Writing to {dumpFile} failed.  Bad directory?");
                        return;
                    }

                    dumpFile = selectedPath + "\\scenesDump.json";

                    try
                    {
                        File.WriteAllText(dumpFile, scenesJSON);
                    }
                    catch
                    {
                        MessageBox.Show($"Writing to {dumpFile} failed.  Bad directory?");
                        return;
                    }

                                        

                    MessageBox.Show("JSON Dump Files Successfully Written.");

                }
            }


            

            
            

        }

        private void DiscardMovieTextEdits_Click_1(object sender, EventArgs e)
        {
            updateRTBText(MovieText, Utils.originalMovieText);
            // DisableTextChangedEvent(MovieText);
            HideApplyButton(MovieText);
        }

        private void TimeLength_ValueChanged(object sender, EventArgs e)
        {
            Utils.dataDictionary["TimeLength"] = (int)TimeLength.Value;
            myMovie.timeLength = (int)TimeLength.Value;
            
        }

        private async void MakeExpansiveNotesButton_Click(object sender, EventArgs e)
        {

            if (checkSelected(MovieText)) { return; }

            NotesForMovieText.Text = $"Creating Notes to expand the Movie Text using {GetGptModel()}...";
            string reponse = await MyGPT.makeMovieTextLengthNote(GetGptModel(), MovieText.Text, (int)TimeLength.Value, this);
            NotesForMovieText.Text = reponse;

        }

        private void DialogFlavorPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void FlavorCustomText_TextChanged(object sender, EventArgs e)
        {
            if (SceneInScenesList.SelectedIndex > -1)
            {
                scenes[SceneInScenesList.SelectedIndex].CustomFlavor = FlavorCustomText.Text;
            }
            
        }

        private void ScenesListMovieTabRTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void MovieHintText_TextChanged(object sender, EventArgs e)
        {
            
            if (MovieHintText == this.ActiveControl)
            {
                myMovie.movieHintText = MovieHintText.Text;
            }
            


        }

        private void FormatScript_CheckedChanged(object sender, EventArgs e)
        {
            string scriptText = getSceneScriptText();
            SceneScriptRichTextbox.Rtf = Utils.formatScript(scriptText,FormatScript.Checked);
            Utils.dataDictionary["ScriptFormat"] = FormatScript.Checked;
        }

        private void ExportRTF_Click(object sender, EventArgs e)
        {
            FormExportRTF exportForm = new FormExportRTF(scenes, myMovie);
            exportForm.ShowDialog();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void CodeReWrite_TextChanged(object sender, EventArgs e)
        {

        }

        private void UITestFiniteStateButton_Click(object sender, EventArgs e)
        {
            double seconds;
            DateTime mostRecent;
            DateTime sevenBack;
            TimeSpan timeSpan;

            if (Utils.dateTimes.Count < 7)
            {
                Utils.dateTimes.Add(DateTime.Now);

            }

            if (Utils.dateTimes.Count >= 7)
            {
                mostRecent = Utils.dateTimes[Utils.dateTimes.Count - 1];
                sevenBack = Utils.dateTimes[Utils.dateTimes.Count - 7];
                timeSpan = mostRecent - sevenBack;
                seconds = timeSpan.TotalSeconds;
                if (seconds > 7.0)
                {
                    Utils.dateTimes = new List<DateTime>();
                }
                else
                {
                    FormMagicalMystery mmForm = new FormMagicalMystery();
                    mmForm.ShowDialog();
                    if (mmForm.mmSuccess)
                    {
                        MagicalMysteryTour.Text = "Ok ready to go.\r\n\r\n";
                    }
                    Utils.dateTimes = new List<DateTime>();
                }

            }

            
            
        }

        private void MovieSceneListLabel_Click(object sender, EventArgs e)
        {

        }

        private void CustomMagicSceneText_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void MakeExpansiveSceneNoteButton_Click(object sender, EventArgs e)
        {
            if (checkSelected(SceneText)) { return; }

            SceneNoteRichTextBox.Text = $"Creating Notes to expand the Scene Text using {GetGptModel()}...";
            string reponse = await MyGPT.makeSceneTextLengthNote(GetGptModel(), SceneText.Text,this);
            SceneNoteRichTextBox.Text = reponse;
        }

        private void button40_Click(object sender, EventArgs e)
        {
            SceneText.SelectionStart = 10;
            SceneText.SelectionLength = 20;
        }

        private void button40_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\"; // You can set this to any default path.
                openFileDialog.Filter = "shf files (*.shf)|*.shf"; // Example filter for text files.
                openFileDialog.FilterIndex = 2; // By default, show all files.
                openFileDialog.RestoreDirectory = true; // Restore the directory after closing dialog.

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the selected file.
                    string filePath = openFileDialog.FileName;

                    // Do whatever you want with the file path.
                    MessageBox.Show($"Selected file: {filePath}");
                }
            }
        }

        private void GatherCharFromMovieTextButton_Click(object sender, EventArgs e)
        {
            if (MovieText.Text.Length < 50) 
            {
                MessageBox.Show("Movie Text too short.  Must have at least 50 characters");
                return;
            }

            if (!MovieText.Text.Contains("<") || !MovieText.Text.Contains(">"))
            {
                MessageBox.Show("No names found in Movie Text delimitted by angle brackets <>. Example: <Mary>");
                return;

            }

            List<string> characterTags = Utils.ExtractTextBetweenAngleBrackets(MovieText.Text);
            characters = new List<CharacterObj>();

            foreach (string characterTag in characterTags) 
            
            {
                CharacterObj character = new CharacterObj();
                character.tagName= characterTag;
                characters.Add(character);
                           
            }

            CharactersMaster.DataSource= characters;
            CharactersMaster.DisplayMember = "tagName";

        }

        private async void GenAttsAllCharButton_Click(object sender, EventArgs e)
        {

            // make or remake all characters 

            int errorKount = 0;
            Boolean storyTextFlag  = true;
            Boolean successFlag = false;
            List<CharacterObj> tempCharacters = new List<CharacterObj>();

            //  string model = "gpt-3.5-turbo-16k-0613";
            if (characters.Count > 20)
            {
                MessageBox.Show("There is a temporary limit of 20 characters that can be processed.  Please delete your secondary characters and try again.");
                return;
            }

            if (characters.Count > 0)
            {
                //message box yes no 
                DialogResult dialogResult = MessageBox.Show("You may have an existing list of characters attributes.  Proceeding will delete any existing character profiles.  Do you want to proceed?", "Delete Existing Character Profiles?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            
            if (characters == null)
            {
                MessageBox.Show("No characters found.  Please click the 'Gather Characters' button or manualy add charcters.");
                return;
            }

            if (characters.Count == 0)
            {
                MessageBox.Show("No characters found.  Please click the 'Gather Characters' button or manualy add charcters.");
                return;
            }


            characterProfiles = new List<CharacterProfiles>();
            string jsonResponse = "";
            string storyText = "";
            storyTextFlag = true;
            if (MovieText.Text.Trim().Length < 10 && MovieHintText.Text.Trim().Length > 10 && MovieHintText.Text != "put your ideas here"  )
            {
                storyText = MovieHintText.Text.Trim();
            }
            else if (MovieText.Text.Trim().Length >= 10)
            {
                storyText = MovieText.Text.Trim();
            }
            if (storyText.Length == 0)
            {
                storyTextFlag= false;
            }

            successFlag= false;

            while (successFlag == false)
            {
                try
                {
                    if (CharactersMaster.Items.Count > 0)
                    {

                        if (storyTextFlag)
                        {
                            jsonResponse = await MyGPT.makeCharacterProfiles(GetGptModel(), storyText, characters, this, false, myMovie.startYear);
                        }
                        else
                        {

                            jsonResponse = await MyGPT.makeCharacterProfileFromBriefDescription(GetGptModel(), characters, this, myMovie.startYear);

                        }

                        jsonResponse = Utils.TrimOutsideBrackets(jsonResponse);

                        Utils.nop();
                    }

                    else { return; }

                    characterProfiles = JsonConvert.DeserializeObject<List<CharacterProfiles>>(jsonResponse);

                    tempCharacters = new List<CharacterObj>();

                    foreach (CharacterProfiles proChar in characterProfiles)

                    {
                        CharacterObj tempChar = new CharacterObj();

                        tempChar.tagName = proChar.Name;
                        tempChar.age = proChar.Age;
                        tempChar.backStory = proChar.BackStory;
                        tempChar.physicalDescription = proChar.Physical;
                        tempChar.personality = proChar.Personality;
                        tempChar.speechStyle = proChar.Speech;
                        tempCharacters.Add(tempChar);

                        CharacterObj fullChar = characters.Find(p => p.tagName == proChar.Name);

                        if (fullChar != null)

                        {
                            tempChar.briefDescription = fullChar.briefDescription;
                            tempChar.age = fullChar.age;
                        }

                    }
                    successFlag = true;
                }
                catch
                {
                    successFlag = false;
                    errorKount++;
                    updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Retry #{errorKount} ...", "Error in button41_Click(object sender, EventArgs e) very likely bad json list of Characters");
                    if (errorKount >= 3)
                    {
                        updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Aborting...", "Aborting from too many errors in button41_Click(object sender, EventArgs e) very likely bad json list of Characters");
                        updateGPTStatus("Request aborted.  Too many errors", Color.Black);
                        break;

                    }
                    
                    
                }

            }
            
            

            

            CharactersMaster.DataSource = null;
            characters = tempCharacters;
            CharactersMaster.DataSource= characters;
            CharactersMaster.DisplayMember = "tagName";
            CharactersMaster.SelectedIndex = 0;
            displayCharacterProfile();


        }

        private void  displayCharacterProfile()
        {
            if (characters == null) { return; }
            if (characters.Count == 0) { return; }
            
            CharacterAge.Text = characters[CharactersMaster.SelectedIndex].age;
            CharacterBiography.Text = characters[CharactersMaster.SelectedIndex].backStory;
            CharacterPhysicalDescription.Text = characters[CharactersMaster.SelectedIndex].physicalDescription;
            CharacterPersonality.Text = characters[CharactersMaster.SelectedIndex].personality;
            CharacterSpeakingStyle.Text = characters[CharactersMaster.SelectedIndex].speechStyle;
            CharactersYear.Text = myMovie.startYear;
        }

        private void CharactersMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CharactersMaster.SelectedIndex >= 0)
            {
                displayCharacterProfile();
                AskName.Text = $"Ask <{characters[CharactersMaster.SelectedIndex].tagName}> anything";
                CharacterNotes.Text = "";
                AskQuestion.Text = "";
                AskAnswer.Text = "";
            }

            BackStoryLocked.Checked = false;
            PersonalityLocked.Checked = false;
            SpeakingStyleLocked.Checked = false;
            PhysicalDescriptionLocked.Checked = false;

        }

        private async void RewriteMovieTextWithCharAttsButton_Click(object sender, EventArgs e)
        {
            string menuItem = "";

            if (NotesList.Items.Count <= 0)
            {
                MessageBox.Show("No \"Movie Text\" to rewrite");
                return;
            }

            if (characters == null)
            {
                MessageBox.Show("No characters found.  Please click the 'Gather Characters' button");
                return;
            }

            if (characters.Count == 0) 
            {
                MessageBox.Show("No characters found.  Please click the 'Gather Characters' button");
                return;
            }

            RadioButton rb = GetCheckedRadioButtonInPanel(RewriteStrength);

            if (rb == null)
            {
                MessageBox.Show("Please select a rewrite strength");
                return;
            }
            
            string response = "";

            if (rb == StrengthNormal)
            {
                response = await MyGPT.rewriteMovieTextWithCharacterDetails(GetGptModel(), MovieText.Text, characters, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
            }
            else if (rb == StrengthHeavy)
            {
                response = await MyGPT.heavyRewriteTextWithCharacterDetails(GetGptModel(), MovieText.Text, characters, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
                
            }
            else
            {
                MessageBox.Show("Please select a rewrite strength");
                return;
            }

            
            isError = Utils.checkForGPTErrors(response, this);
            if (isError)
            {
                return;
            }
            updateRTBText(MovieText, response);
            myMovie.movieText = response;

            if (myMovie.myNoteTextList.Count == 0)
            {
                myMovie.myNoteTextList = new List<NotesMovieText>();
                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Char", 0, myMovie.movieText.Length);

                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();
                myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));
            }
            else if (myMovie.myNoteTextList.Count > 0)
            {

                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Char", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, "", menuItem));
                
            }
            NotesList.DataSource = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = myMovie.myNoteTextList.Count - 1;
            Application.DoEvents();
            ShowEditButton(MovieText);
        }

        private async void button43_Click(object sender, EventArgs e)
        {
            string jsonResponse = "";
            int loopKount = 0;
            int errorKount = 0;
            Boolean successFlag = false;

            while (loopKount < 100)
            {
                errorKount = 0;
                try
                {
                    characterProfiles = new List<CharacterProfiles>();
                    string storyText = "";
                    if (MovieText.Text.Trim().Length < 10 && MovieHintText.Text.Trim().Length > 10)
                    {
                        storyText = MovieHintText.Text.Trim();
                    }
                    else if (MovieText.Text.Trim().Length >= 10)
                    {
                        storyText = MovieText.Text.Trim();
                    }
                    if (storyText.Length == 0)
                    {
                        MessageBox.Show("No Movie Text or Movie Seed to generate character profiles.   Please create Movie Seed or Movie Text");
                        return;
                    }
                    if (CharactersMaster.Items.Count > 0)
                    {
                        jsonResponse = await MyGPT.makeCharacterProfiles(GetGptModel(), MovieText.Text, characters, this, false, myMovie.startYear);
                        jsonResponse = Utils.TrimOutsideBrackets(jsonResponse);

                        Utils.nop();
                    }

                    else { return; }

                    characterProfiles = JsonConvert.DeserializeObject<List<CharacterProfiles>>(jsonResponse);

                    List<CharacterObj> tempCharacters = new List<CharacterObj>();

                    foreach (CharacterProfiles proChar in characterProfiles)

                    {
                        CharacterObj tempChar = new CharacterObj();
                        tempChar.tagName = proChar.Name;
                        tempChar.age = proChar.Age;
                        tempChar.backStory = proChar.BackStory;
                        tempChar.physicalDescription = proChar.Physical;
                        tempChar.personality = proChar.Personality;
                        tempChar.speechStyle = proChar.Speech;
                        tempCharacters.Add(tempChar);

                    }

                    CharactersMaster.DataSource = null;
                    characters = tempCharacters;
                    CharactersMaster.DataSource = characters;
                    CharactersMaster.DisplayMember = "tagName";

                    displayCharacterProfile();
                    loopKount++;
                    StressCount.Text = loopKount.ToString();
                }
                catch
                {
                    successFlag = false;
                    errorKount++;
                    updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Retry #{errorKount} ...", "Error in button41_Click(object sender, EventArgs e) very likely bad json list of Characters");
                    if (errorKount >= 3)
                    {
                        updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Aborting...", "Aborting from too many errors in button41_Click(object sender, EventArgs e) very likely bad json list of Characters");
                        updateGPTStatus("Request aborted.  Too many errors", Color.Black);
                        break;

                    }
                }
                
            }


        }

        private async void ApplyCharacterNotesButton_Click(object sender, EventArgs e)
        {
            int characterIndex = CharactersMaster.SelectedIndex;
            string selectedText = "";
            int startSelection = 0;
            string spliceInText = "";
            RichTextBox selectedBox;
            if (CharactersMaster.Items.Count == 0)
            {
                MessageBox.Show("No characters found.  Please click the 'Gather Characters' button");
                return;
            }
            if (CharacterNotes.Text.Trim().Length < 5)
            {
                MessageBox.Show("Notes must be at least 5 characters long");
                return;
            }

            RadioButton rb = GetCheckedRadioButtonInPanel(radioCharacters);
            List<RadioButton> radioButtons = new List<RadioButton>();
            if (rb == radioAllAttributes)
            {
                if (!BackStoryLocked.Checked) { radioButtons.Add(radioBackStory); }
                if (!PhysicalDescriptionLocked.Checked) { radioButtons.Add(radioPhysical); }
                if (!PersonalityLocked.Checked)  { radioButtons.Add(radioPersonality); }
                if (!SpeakingStyleLocked.Checked)  { radioButtons.Add(radioSpeakingStyle); }
            }

            else
            {
                radioButtons.Add(rb);
            }

            if (rb == null)
            {
                MessageBox.Show("No option selected for Notes");
                return;
            }

            foreach(RadioButton workRb in radioButtons)
            {
                switch (workRb.Name)
                {
                    case "radioBackStory":
                        selectedBox = CharacterBiography;
                        break;
                    case "radioPhysical":
                        selectedBox = CharacterPhysicalDescription;

                        break;
                    case "radioPersonality":
                        selectedBox = CharacterPersonality;
                        break;

                    case "radioSpeakingStyle":
                        selectedBox = CharacterSpeakingStyle;
                        break;
                    default:
                        selectedBox = null;

                        break;
                }
                if (selectedBox == null)
                {
                    MessageBox.Show("No option selected for Notes");
                    return;
                }
                string originalText = selectedBox.Text;
                string response = "";
                if (selectedBox.SelectedText.Length == 0 || rb == radioAllAttributes)   // no selected Text or doing all 4 attributes
                {

                    selectedBox.Text = $"applying note using {GetGptModel()}\r\n\r\n {selectedBox.Text}";
                    response = await MyGPT.rewriteCharacterAttribute(GetGptModel(), originalText, CharacterNotes.Text, workRb.Name, characters[characterIndex], this,myMovie.startYear);
                    selectedBox.Text = response;
                }

                else // selected text

                {
                    selectedText = selectedBox.SelectedText.Trim();
                    startSelection = selectedBox.SelectionStart;
                    spliceInText = "";

                    response = await MyGPT.rewriteSELECTEDCharacterAttribute(GetGptModel(), originalText, selectedText, CharacterNotes.Text, workRb.Name, characters[CharactersMaster.SelectedIndex], this);

                    spliceInText = response;
                    isError = Utils.checkForGPTErrors(response, this);
                    if (isError)
                    {
                        return;
                    }


                    response = Utils.spliceWithLocs(originalText, selectedText, startSelection, selectedText.Length, spliceInText);
                    updateRTBText(selectedBox, response);
                   


                }
                switch (workRb.Name)
                {
                    case "radioBackStory":
                        characters[characterIndex].backStory = response;
                        break;
                    case "radioPhysical":
                        characters[characterIndex].physicalDescription = response;

                        break;
                    case "radioPersonality":
                        characters[characterIndex].personality = response;
                        break;

                    case "radioSpeakingStyle":
                        characters[characterIndex].speechStyle = response;
                        break;
                    default:
                        selectedBox = null;

                        break;
                }
                displayCharacterProfile();
                if (selectedText.Trim().Length > 0)

                {
                    selectedBox.Focus();
                    selectedBox.SelectionStart = startSelection;
                    selectedBox.SelectionLength = spliceInText.Length;


                }

            }




        }

        private void radioBackStory_CheckedChanged(object sender, EventArgs e)
        {
            CharacterNotes.Clear();
        }

        private void radioPhysical_CheckedChanged(object sender, EventArgs e)
        {
            CharacterNotes.Clear();
        }

        private void radioPersonality_CheckedChanged(object sender, EventArgs e)
        {
            CharacterNotes.Clear();
        }

        private void radioSpeakingStyle_CheckedChanged(object sender, EventArgs e)
        {
            CharacterNotes.Clear();
        }

        private void CharacterBiography_TextChanged(object sender, EventArgs e)
        {
            if (!CharacterBiography.Text.Contains(Utils.workingTag()))
            {
                characters[CharactersMaster.SelectedIndex].backStory = CharacterBiography.Text;

            }
        }

        private void CharacterPersonality_TextChanged(object sender, EventArgs e)
        {
            if (!CharacterPersonality.Text.Contains(Utils.workingTag()))
            {
                characters[CharactersMaster.SelectedIndex].personality = CharacterPersonality.Text;
            }
        }

        private void CharacterPhysicalDescription_TextChanged(object sender, EventArgs e)
        {
            if (!CharacterPhysicalDescription.Text.Contains(Utils.workingTag()))
            {
                characters[CharactersMaster.SelectedIndex].physicalDescription = CharacterPhysicalDescription.Text;
            }
        }

        private void AddCharacterButton_Click(object sender, EventArgs e)
        {
            

            FormNewCharacter newCharacterForm = new FormNewCharacter();
            DialogResult result = newCharacterForm.ShowDialog();

            if (newCharacterForm.successFlag)
            {
                

                CharacterObj character = new CharacterObj();
                character.tagName = newCharacterForm.firstName;
                character.age = newCharacterForm.age.ToString();
                if (Utils.checkPropertyExistsInObj(character, "briefDescription"))
                {
                    character.briefDescription = newCharacterForm.description;
                }
                characters.Add(character);



                characters = characters.OrderBy(p => p.tagName).ToList();  // SortedDictionary

                int index = characters.FindIndex(p => p.tagName == newCharacterForm.firstName);
                CharactersMaster.DataSource = null;
                CharactersMaster.DataSource = characters;
                CharactersMaster.DisplayMember = "tagName";
                CharactersMaster.SelectedIndex = index;
                displayCharacterProfile();

                
            }
            else 
            {
                return;
            }
        }

        private async void RemakeCharacterButton_Click(object sender, EventArgs e)
        {
            Boolean useMCPs = true;

            if (CharactersMaster == null) { return; }
            if (CharactersMaster.SelectedIndex < 0) { return; };
            CharacterObj workCharacter = new CharacterObj();
            workCharacter = characters[CharactersMaster.SelectedIndex];
            string myTagName = workCharacter.tagName;

            if (workCharacter.age.Trim().Length == 0)
            {
                MessageBox.Show($"Character {workCharacter.tagName} requires age to make attributes");    
                return; 
            
            }
            string storyText = "";
            List<CharacterObj> workCharacters = new List<CharacterObj>();
            workCharacters.Add(workCharacter);

            string jsonResponse = "";

            // string myModel = "gpt-3.5-turbo-16k-0613";

            if (MovieText.Text.Trim().Length < 10 && MovieHintText.Text.Trim().Length > 10)
            {
                storyText = MovieHintText.Text.Trim();
            }
            else if (MovieText.Text.Trim().Length >= 10)
            {
                storyText = MovieText.Text.Trim();
            }
            
            if (storyText.Length == 0 || storyText == "put your ideas here")  // weak spot! 
            {
                useMCPs = false;
                if (workCharacter.briefDescription.Length < 5)
                {
                    FormCharacterDescription newCharacterDescriptionForm = new FormCharacterDescription(workCharacter);
                    newCharacterDescriptionForm.ShowDialog();

                    if (newCharacterDescriptionForm.successFlag)
                    {
                        workCharacters[0].briefDescription = newCharacterDescriptionForm.briefDescription;
                    }
                    else
                    {
                        return;
                    }
                        
                }
            }
           

            

            if (useMCPs)
            {
                jsonResponse = await MyGPT.makeCharacterProfiles(GetGptModel(), storyText, workCharacters, this, true, myMovie.startYear);
                
            }
            else
            {
                jsonResponse = await MyGPT.makeCharacterProfileFromBriefDescription(GetGptModel(),  workCharacters, this,  myMovie.startYear);
                
            }


            jsonResponse = Utils.TrimOutsideBrackets(jsonResponse);
            jsonResponse = Utils.replaceQuoteWithSingleQuotes(jsonResponse);

            characterProfiles = JsonConvert.DeserializeObject<List<CharacterProfiles>>(jsonResponse);

            

            foreach (CharacterProfiles proChar in characterProfiles)
            {
                if (proChar.Name == myTagName)
                {
                    if (!PersonalityLocked.Checked) { characters[CharactersMaster.SelectedIndex].personality = proChar.Personality; }
                    if (!BackStoryLocked.Checked) { characters[CharactersMaster.SelectedIndex].backStory = proChar.BackStory; }
                    if (!PhysicalDescriptionLocked.Checked) { characters[CharactersMaster.SelectedIndex].physicalDescription = proChar.Physical; }
                    if (!SpeakingStyleLocked.Checked) { characters[CharactersMaster.SelectedIndex].speechStyle = proChar.Speech; }
                    
                }
            }

            displayCharacterProfile();



        }

        private void DeleteCharacterButton_Click(object sender, EventArgs e)
        {
            string message = $"Are you sure you want to delete {characters[CharactersMaster.SelectedIndex].tagName}?";
            DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                int index = CharactersMaster.SelectedIndex;
                if (index > characters.Count - 2) { index = characters.Count - 2; }
                              
                characters.RemoveAt(CharactersMaster.SelectedIndex);
                CharactersMaster.DataSource = null;
                CharactersMaster.DataSource = characters;
                CharactersMaster.DisplayMember = "tagName";
                CharactersMaster.SelectedIndex = index;
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }

        private void CharacterAge_TextChanged(object sender, EventArgs e)
        {
            if (CharactersMaster.SelectedIndex >= 0)
            {
                characters[CharactersMaster.SelectedIndex].age = CharacterAge.Text;
            }
        }

        private async void MovieTextFromMovieSeedWithCharAttsButton_Click(object sender, EventArgs e)
        {
            string menuItem = "";
            // DisableTextChangedEvent(MovieText);
            HideApplyButton(MovieText);

            //MovieText.Text = 
            updateRTBText(MovieText, GetGptModel() + " awaiting reply...\r\n \r\n" + MovieHintText.Text);

            cursorTopRTB(MovieText);
            string reply = await MyGPT.makeMovieText(MovieHintText.Text, GetGptModel(), (int)TimeLength.Value, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this, true, characters);

            isError = Utils.checkForGPTErrors(reply, this);
            if (isError)
            {
                return;
            }


            //MovieText.Text = reply;
            updateRTBText(MovieText, reply);

            myMovie.movieText = reply;

            if (myMovie.myNoteTextList.Count == 0)
            {
                myMovie.myNoteTextList = new List<NotesMovieText>();
                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Base", 0, myMovie.movieText.Length);

                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();
                myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));
            }
            else if (myMovie.myNoteTextList.Count > 0)
            {

                menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Char", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem.PadRight(30 - menuItem.Length) + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, "", menuItem));
                updateRTBText(NotesForMovieText, "");

            }
            NotesList.DataSource = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = myMovie.myNoteTextList.Count - 1;
            Application.DoEvents();
            ShowEditButton(MovieText);
        }

        private async void AskCharacterButton_Click(object sender, EventArgs e)
        {

            if (AskQuestion.Text.Trim().Length == 0)
            {
                MessageBox.Show ("Please enter a question");
                return;
            }

            if (characters == null) { return; }
            if (characters.Count == 0) { return; }
            if (CharactersMaster.SelectedIndex < 0) { return; }

            string response = await MyGPT.askName(GetGptModel(), AskQuestion.Text.Trim(), characters[CharactersMaster.SelectedIndex], this,myMovie.startYear);
                
            AskAnswer.Text = response;
        }

        private void CharactersYear_TextChanged(object sender, EventArgs e)
        {
            myMovie.startYear = CharactersYear.Text;
        }

        

        private async void UITestStressTestButton_Click(object sender, EventArgs e)
        {
            string jsonResponse = "";
            int loopKount = 0;
            int errorKount = 0;
            int totalErrors = 0;
            Boolean successFlag = false;

            while (loopKount < 100)
            {
                errorKount = 0;
                try
                {
                    characterProfiles = new List<CharacterProfiles>();
                    string storyText = "";
                    if (MovieText.Text.Trim().Length < 10 && MovieHintText.Text.Trim().Length > 10)
                    {
                        storyText = MovieHintText.Text.Trim();
                    }
                    else if (MovieText.Text.Trim().Length >= 10)
                    {
                        storyText = MovieText.Text.Trim();
                    }
                    if (storyText.Length == 0)
                    {
                        MessageBox.Show("No Movie Text or Movie Seed to generate character profiles.   Please create Movie Seed or Movie Text");
                        return;
                    }
                    if (CharactersMaster.Items.Count > 0)
                    {
                        jsonResponse = await MyGPT.makeCharacterProfiles(GetGptModel(), MovieText.Text, characters, this, false, myMovie.startYear);
                        jsonResponse = Utils.TrimOutsideBrackets(jsonResponse);

                        Utils.nop();
                    }

                    else { return; }

                    characterProfiles = JsonConvert.DeserializeObject<List<CharacterProfiles>>(jsonResponse);

                    List<CharacterObj> tempCharacters = new List<CharacterObj>();

                    foreach (CharacterProfiles proChar in characterProfiles)

                    {
                        CharacterObj tempChar = new CharacterObj();
                        tempChar.tagName = proChar.Name;
                        tempChar.age = proChar.Age;
                        tempChar.backStory = proChar.BackStory;
                        tempChar.physicalDescription = proChar.Physical;
                        tempChar.personality = proChar.Personality;
                        tempChar.speechStyle = proChar.Speech;
                        tempCharacters.Add(tempChar);

                    }

                    CharactersMaster.DataSource = null;
                    characters = tempCharacters;
                    CharactersMaster.DataSource = characters;
                    CharactersMaster.DisplayMember = "tagName";

                    displayCharacterProfile();
                    loopKount++;
                    StressCount.Text = $"Successes: {loopKount} Total Fails: {totalErrors}";
                }
                catch
                {
                    successFlag = false;
                    errorKount++;
                    totalErrors++;
                    StressCount.Text = $"Successes: {loopKount} Total Fails: {totalErrors}";
                    updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Retry #{errorKount} ...", "Error in button41_Click(object sender, EventArgs e) very likely bad json list of Characters");
                    if (errorKount >= 3)
                    {
                        updateGPTErrorMsg($"Error creating attributes for a list of {characters.Count} characters. Aborting...", "Aborting from too many errors in button43_Click(object sender, EventArgs e) very likely bad json list of Characters");
                        updateGPTStatus("Request aborted.  Too many errors", Color.Black);
                        break;

                    }
                }

            }


        }

        private void USARatings_SelectedIndexChanged(object sender, EventArgs e)
        {
            myMovie.ratingUSA = USARatings.Text;
        }

        private void Genre_TextChanged(object sender, EventArgs e)
        {
            myMovie.genre = Genre.Text;
        }

        private void audience_TextChanged(object sender, EventArgs e)
        {
            myMovie.audience = Audience.Text;
        }

        private void Guidance_TextChanged(object sender, EventArgs e)
        {
            myMovie.guidance = Guidance.Text;
        }

        private void EditTitle_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = new DialogResult();


            if (titleChangeNotSave)
            {
                dialogResult = MessageBox.Show("You have changed movie title, but haved not saved Movie\r\n\r\n To make another title change you must save Movie. \r\n\r\n Save Movie Now? ", "Save", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveCurrentMovie();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }



            FormEditTitle formEditTitle = new FormEditTitle(myMovie, GetGptModel(), this);
            formEditTitle.ShowDialog();
            if (formEditTitle.finalTitle.Length > 0)
            {
                myMovie.title = formEditTitle.finalTitle;
                MovieTitle.Text = formEditTitle.finalTitle;
                TopMovieTitle.Text = "Title: " + formEditTitle.finalTitle;
                string newFileName = Utils.makeFilename(myMovie.title);
                string newFullPath = moviesDir + "\\" + newFileName;
                if (File.Exists(newFullPath))
                {
                    File.Delete(newFullPath);

                }
                File.Move(Utils.currentMovieFilename, newFullPath);

                Utils.currentMovieFilename = newFullPath;
                dialogResult = MessageBox.Show("Save Movie Now To Preserve New Title? ", "Save", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveCurrentMovie();
                }
                else if (dialogResult == DialogResult.No)
                {
                    titleChangeNotSave = true;
                }

            }
        }

        private void UseProfileMovieText_CheckedChanged(object sender, EventArgs e)
        {
            myMovie.useProfileMovieText = UseProfileMovieText.Checked;

        }

        private void UseProfileSceneText_CheckedChanged(object sender, EventArgs e)
        {
            myMovie.useProfileSceneText = UseProfileSceneText.Checked;
        }

        private void UseProfileScriptText_CheckedChanged(object sender, EventArgs e)
        {
            myMovie.useProfileSceneScript = UseProfileSceneScript.Checked;
        }

        private void UseProfileMakeScenes_CheckedChanged(object sender, EventArgs e)
        {
            myMovie.useProfileMakeScenes = UseProfileMakeScenes.Checked;
        }

        private void TimeLength_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        public static List<OpenRouterModel> _openRouterModels { get; set; }

        private async Task LoadOpenRouterModels()
        {
            try
            {
                // open router models json format is { data: [ { id: "model-id", name: "model-name" } ] }
                var openRouterModels = await Api.OpenRouterModels();
                var data = openRouterModels;
                data = data.Replace("\"Infinity\"", int.MaxValue.ToString());
                var models = JsonConvert.DeserializeObject<OpenRouterModels>(data);
                _openRouterModels = models.data;
                // sort _openRouterModels by name
                _openRouterModels.Sort((x, y) => x.name.CompareTo(y.name));
                // fill the openRouterModelsComboBox on the main thread
                BeginInvoke(new Action(() =>
                {
                    int index = 0;
                    int ctr = 0;
                    foreach (var model in _openRouterModels)
                    {
                        openRouterComboBox.Items.Add(model.name);
                        if (model.name == "Anthropic: Claude 3.5 Sonnet")
                            index = ctr;
                        ctr++;
                    }
                    // select anthropic sonnet 3.5
                    openRouterComboBox.SelectedIndex = index;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Open Router Models: " + ex.Message);
            }
        }

        private void openRouterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the id by searching _openRouterModels
            // get the selected indexes text from the combo
            var selectedModel = _openRouterModels.Where(x => x.name == openRouterComboBox.Text).FirstOrDefault();
            if (selectedModel != null)
            {
                __gptModel = "or/" + selectedModel.id;
                // uncheck all the radio buttons
            }
        }
      

        private async void AutoNewMakeScenesButton_Click(object sender, EventArgs e)
        {
            string sceneListLabel = "";

            if (MovieText.Text.Length < 1000)
            {
                MessageBox.Show("Not Enough Movie Text.  Need at least 1000 characters ");
            }
            else
            {
                sceneListLabel = Utils.rightOfArrow(myMovie.myNoteTextList[NotesList.SelectedIndex].myLabel);

                SceneDescriptions.Clear();
                SceneDescriptions.Text = Utils.makePendingMessage(GetGptModel());

                Utils.newScenesFlag = true;

                try
                {
                    scenes = await MyGPT.splitMakeScenesFromMovieText(GetGptModel(), MovieText.Text, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    updateGPTErrorMsg("Error in autoSceneCutter", ex.Message);
                    SceneDescriptions.Clear();
                    return;
                }

                DateTime currentTime = DateTime.Now;
                string formattedTime = currentTime.ToString("yyyy:MM:dd:HH:mm");
                MovieSceneListLabel.Text = "Scenes from " + sceneListLabel + " using: " + GetGptModel() + " @ " + formattedTime;

                Utils.dataDictionary["SceneMaker"] = MovieSceneListLabel.Text.Trim();

                SceneDescriptions.Clear();
                SceneDescriptions.Text = Utils.makePendingMessage(GetGptModel());

                updateRTBText(ScenesListMovieTabRTB, "");
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
                ScriptNotesListbox.DataSource = null;

                string rtfString = Utils.makeScenesRTF(scenes);

                ScenesListMovieTabRTB.Rtf = rtfString;
                Utils.newScenesFlag = true;
            }
        }

        private async void CharacterizeButton_Click(object sender, EventArgs e)
        {
            string response = await MyGPT.parallelAngleBracketsCharacterText(GetGptModel(), MovieText.Text, this);
            response = Utils.singleAngleBrackets(response);
            response = Utils.singleAngleBrackets(response);

            Boolean isError = Utils.checkForGPTErrors(response, this);
            if (isError)
            {
                return;
            }
            MovieText.Text = response;
            myMovie.movieText = MovieText.Text;
            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "<CH>", NotesList.SelectedIndex, myMovie.movieText.Length);
            menuItem = menuItem + " " + GetGptModel();

            myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));
            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;

            
            ShowEditButton(MovieText);


        }

        private async void AutoSceneCutterButton_Click(object sender, EventArgs e)
        {
            string response;
            try
            {
                response = await MyGPT.autoSceneCutter(GetGptModel(), MovieText.Text, this);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateGPTErrorMsg("Error in autoSceneCutter", ex.Message);
                return;
            }
            //string response = await MyGPT.splitMovieTextIntoScenes(GetGptModel(), MovieText.Text, "Adding Scene Breaks '===' to Movie Text", this);
            Boolean isError = Utils.checkForGPTErrors(response, this);
            if (isError)
            {
                return;
            }
            MovieText.Text = response;
            myMovie.movieText = MovieText.Text;

            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "====", NotesList.SelectedIndex, myMovie.movieText.Length);
            menuItem = menuItem + " " + GetGptModel();
            myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
            ShowEditButton(MovieText);
        }

        private void AddSceneBreakButton_Click(object sender, EventArgs e)
        {
            
            if (EditMovieText.Visible)
            {
                MessageBox.Show("Must be in Edit mode to add Scene Break ===.  Please press 'Edit Text' button");
                return;
            }
            
            string spliceInText = "\r\n===\r\n";
            int selectionStart = MovieText.SelectionStart;
            string workMovieText = Utils.addSceneBreak(MovieText.Text, selectionStart, spliceInText);
            
            MovieText.Text = workMovieText;
            MovieText.Focus();
            MovieText.SelectionStart = Math.Min(MovieText.Text.Length - 1,selectionStart + 5);
            myMovie.movieText = MovieText.Text;
        }

        private void DeleteSceneBreakButton_Click(object sender, EventArgs e)
        {
            if (EditMovieText.Visible)
            {
                MessageBox.Show("Must be in Edit mode to add Scene Break ===.  Please press 'Edit Text' button");
                return;
            }

            int location = MovieText.SelectionStart;
            int startLocation = location;
            int endLocation = location;
            char atLocation = MovieText.Text[location];

            if (atLocation != '=')
            {
                MessageBox.Show("Not at a Scene Break");
                return;
            }

            while (startLocation >= 0 && MovieText.Text[startLocation] == '=')
            {
                if (startLocation >= 1)
                { 
                    startLocation--; 
                }
                else
                {
                    break;
                }    
                
            }
            if (MovieText.Text[startLocation] != '=')
            {
                startLocation++;
            }

            while (endLocation <= MovieText.Text.Length -1 && MovieText.Text[endLocation] == '=')
            {
                if (endLocation <= MovieText.Text.Length - 2)
                {
                    endLocation++;
                }
                else
                {
                    break;
                }

            }
            if (MovieText.Text[endLocation] != '=')
            {
                endLocation--;
            }
            string response = Utils.RemoveSubstring(MovieText.Text, startLocation, (endLocation - startLocation) + 1);
            MovieText.Text = response;
            myMovie.movieText = response;
            MovieText.Focus();
            MovieText.SelectionStart = Math.Max(0, startLocation - 1);
        }

        private async void button54_Click(object sender, EventArgs e)
        {
            if (Utils.CountSubstringOccurrences(MovieText.Text, "===") < 4)
            {
                MessageBox.Show($"You must have marked at least 4 scenes with '==='.  The number found is {Utils.CountSubstringOccurrences(MovieText.Text, "===")}");
                return;
            }

            string paddedMovieText = Utils.PadEndIfNeeded(MovieText.Text, "===");
            string longSceneSeedsJSON;
            string[] longSceneSeeds;
            (longSceneSeedsJSON, longSceneSeeds) = Utils.spltMovieTextAtEqualsJSON(paddedMovieText);

            // string response = await MyGPT.splitMakeTitlesForScenes(GetGptModel(), paddedMovieText, this);

            
            string response = await MyGPT.splitMakeTitlesForScenesCompressedSeed(GetGptModel(), longSceneSeedsJSON, this);
            int longSceneSeedsKount = Utils.countSubStringOccurances(longSceneSeedsJSON,"\"text\"");
            int responeKount = Utils.countSubStringOccurances(response, "\"text\"");




            string sceneListLabel = "";
            isError = Utils.checkForGPTErrors(response, this);
            if (isError)
            {
                return;
            }

           

            if (response.Trim().Length == 0)
            {
                MessageBox.Show("bad data returned.  Try again.  If it persists, try a different model.");
                return;
            }

            response = Utils.TrimOutsideBrackets(response);

            /* string responseCompressed = await MyGPT.splitCompressText(GetGptModel(), response, this);

            // make compressed Scene Seeds for later use
            responseCompressed = Utils.TrimOutsideBrackets(responseCompressed);
            */

            scenes = new List<SceneObj>();


            scenes = Utils.splitMakeSceneObjList(response);

            if ((int)scenes.Count != longSceneSeeds.Length)
            {
                MessageBox.Show("Something went wrong.  Long Seed Short Seed Mismatch.  Please try again");
                return;
            }

            int loopKount = 0;

            foreach (SceneObj scene in scenes)
            {
                scene.compressedHint = scene.Hint;
                scene.Hint = longSceneSeeds[loopKount];
                loopKount++;
            }
            
            // merge back in long Scene Seeds 

            updateRTBText(ScenesListMovieTabRTB, "");
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


            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy:MM:dd:HH:mm");
            MovieSceneListLabel.Text = "Scenes from " + sceneListLabel + " using: " + GetGptModel() + " @ " + formattedTime;

            Utils.dataDictionary["SceneMaker"] = MovieSceneListLabel.Text.Trim();

            string rtfString = Utils.makeScenesRTF(scenes);

            ScenesListMovieTabRTB.Rtf = rtfString;
            Utils.newScenesFlag = true;
            Utils.nop();
        }

        private async void MakeScenesSplitButton_Click(object sender, EventArgs e)
        {
            if (Utils.CountSubstringOccurrences(MovieText.Text, "===") < 4)
            {
                MessageBox.Show($"You must have marked at least 4 scenes with '==='.  The number found is {Utils.CountSubstringOccurrences(MovieText.Text, "===")}");
                return;
            }

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy:MM:dd:HH:mm");
            var sceneListLabel = Utils.rightOfArrow(myMovie.myNoteTextList[NotesList.SelectedIndex].myLabel);
            MovieSceneListLabel.Text = "Scenes from " + sceneListLabel + " using: " + GetGptModel() + " @ " + formattedTime;

            Utils.dataDictionary["SceneMaker"] = MovieSceneListLabel.Text.Trim();

            SceneDescriptions.Clear();
            SceneDescriptions.Text = Utils.makePendingMessage(GetGptModel());

            try
            {
                scenes = await MyGPT.makeSplitScenesParallel(GetGptModel(), MovieText.Text, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                updateGPTErrorMsg("Error in makeSplitScenes_Button_Click", ex.Message);
                SceneDescriptions.Clear();
                return;
            }   

            // merge back in long Scene Seeds 

            updateRTBText(ScenesListMovieTabRTB, "");
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

            string rtfString = Utils.makeScenesRTF(scenes);

            ScenesListMovieTabRTB.Rtf = rtfString;
            Utils.newScenesFlag = true;
        }

        private async void ApplyNotesButton_Click(object sender, EventArgs e)
        {
            if (NotesForMovieText.Text.Length < 6)
            {
                MessageBox.Show("Must have at least 5 characters in Notes to make Movie Text");
                return;
            }

            HideApplyButton(MovieText);
            string originalMovieText = MovieText.Text;
            string originalNote = NotesForMovieText.Text;
            string originalSeed = MovieHintText.Text;
            int sourceIndex = NotesList.SelectedIndex;

            string selectedText = MovieText.SelectedText.Trim();
            string spliceInText = "";

            int startSelection = MovieText.SelectionStart;

            if (selectedText.Length == 0)
            {
                //MovieText.Text = GetGptModel() + " applying Notes to Text....\r\n \r\n" + MovieText.Text;
                updateRTBText(MovieText, GetGptModel() + " applying Notes to Text....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);

                (string response, string rewritePlan) = await MyGPT.applyNotesForMovieText_PlanStyle(GetGptModel(), originalMovieText, originalSeed, originalNote, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);
                //string analysis = await MyGPT.analyzeAppliedNotes(rewritePlan, originalMovieText, originalNote, response, this);

                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }
                //MovieText.Text = response;
                updateRTBText(MovieText, response);

                myMovie.movieText = response;
                NotesTextKount += 1;

                string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Note", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));

                //FormLargeView formLargeView = new FormLargeView(originalMovieText, myMovie.movieText, analysis, "Movie Text");
                //formLargeView.StartPosition = FormStartPosition.CenterScreen;
                //formLargeView.ShowDialog();
            }
            else if (selectedText.Length < 3000)
            {
                updateRTBText(MovieText, GetGptModel() + " applying Notes to Selected Region of Movie Text....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);

                string response = await MyGPT.notesForSELECTEDMovieText(GetGptModel(), originalMovieText, selectedText, startSelection, originalNote, this);

                spliceInText = response;
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }

                response = Utils.spliceWithLocs(originalMovieText, selectedText, startSelection, selectedText.Length, spliceInText);

                myMovie.movieText = response;

                updateRTBText(MovieText, response);
                cursorTopRTB(MovieText);

                string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Note", NotesList.SelectedIndex, myMovie.movieText.Length);
                menuItem = menuItem + " " + GetGptModel();

                myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));
            } 
            else
            {
                // let user know that they can't select more than 3000 characters
                MessageBox.Show("You can't select more than 3000 characters to apply Notes to.  Please select less than 2000 characters");
                return;
            }

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;

            // EnableTextChangedEvent(MovieText);
            ShowEditButton(MovieText);

            Application.DoEvents();

            if (selectedText.Trim().Length > 0)

            {
                MovieText.Focus();
                MovieText.SelectionStart = startSelection;
                MovieText.SelectionLength = spliceInText.Length;
            }

        }

        private async void FormApp1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Movie", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    e.Cancel = true; 
                    isClosing = true; 

                    myMovie.movieHintText = MovieHintText.Text;
                    saveCurrentMovie(); 
                    LastSaved.Text = "Last saved: " + DateTime.Now.ToString("HH:mm:ss");
                    await Task.Delay(1000);
                    this.Close(); 
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private async void expandHere_Button_Click(object sender, EventArgs e)
        {
            string originalMovieText = MovieText.Text;
            string movieTextNote = NotesForMovieText.Text;
            int cursorLocation = MovieText.SelectionStart;

            string markedMovieText = "";
            string response = "";
            string spliceInText = "";

            bool isError = false;

            if (NotesForMovieText.Text.Length > 6)
            {
                updateRTBText(MovieText, GetGptModel() + " Expanding at cursor....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);

                markedMovieText = Utils.SpliceInAtCursor(originalMovieText, "XXX", cursorLocation);

                response = await MyGPT.expandHereWithNote(GetGptModel(), markedMovieText, movieTextNote, cursorLocation, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);

                spliceInText = response;
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }

                response = Utils.SpliceInAtCursor(originalMovieText, spliceInText, cursorLocation);

                myMovie.movieText = response;

                updateRTBText(MovieText, response);
                cursorTopRTB(MovieText);
            }

            else
            {
                updateRTBText(MovieText, GetGptModel() + " Expanding at cursor....\r\n \r\n" + MovieText.Text);

                cursorTopRTB(MovieText);

                markedMovieText = Utils.SpliceInAtCursor(originalMovieText, "XXX", cursorLocation);

                response = await MyGPT.expandHere(GetGptModel(), markedMovieText, cursorLocation, (StyleElements)MovieTextStylesGuideListbox.SelectedItem, this);



                spliceInText = response;
                isError = Utils.checkForGPTErrors(response, this);
                if (isError)
                {
                    return;
                }

                response = Utils.SpliceInAtCursor(originalMovieText, spliceInText, cursorLocation);

                myMovie.movieText = response;

                updateRTBText(MovieText, response);
                cursorTopRTB(MovieText);
            }    


            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Expd", NotesList.SelectedIndex, myMovie.movieText.Length);
            menuItem = menuItem + " " + GetGptModel();

            myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
            ShowEditButton(MovieText);

            if (response.Trim().Length > 0)

            {
                MovieText.Focus();
                MovieText.SelectionStart = cursorLocation;
                MovieText.SelectionLength = spliceInText.Length;
            }


        }

        private async void GenerateOutlineButton_Click(object sender, EventArgs e)
        {
            string originalMovieText = MovieText.Text;
            var outline = await MyGPT.summarizeMovieText_V3(GetGptModel(), originalMovieText, this, "Generating outline");

            isError = Utils.checkForGPTErrors(outline, this);
            if (isError)
            {
                return;
            }

            // open the FormOutline
            FormOutline formOutline = new FormOutline(outline, this);
            formOutline.StartPosition = FormStartPosition.CenterScreen;
            formOutline.ShowDialog();
        }

        public void SetMovieHintText(string text)
        {
            MovieHintText.Text = text;
        }

        private async void GenerateOutline2Button_Click(object sender, EventArgs e)
        {
            string originalMovieText = MovieText.Text;
            var outline = await MyGPT.generateOutlineOldStyle(GetGptModel(), originalMovieText, this, "Generating outline");

            isError = Utils.checkForGPTErrors(outline, this);
            if (isError)
            {
                return;
            }

            // open the FormOutline
            FormOutline formOutline = new FormOutline(outline, this);
            formOutline.StartPosition = FormStartPosition.CenterScreen;
            formOutline.ShowDialog();
        }

        public async void CopyEdit(string style)
        {
            string response = await MyGPT.copyEditParallel(GetGptModel(), style, MovieText.Text, this);
            var isError = Utils.checkForGPTErrors(response, this);
            if (isError)
            {
                return;
            }
            MovieText.Text = response;
            myMovie.movieText = MovieText.Text;
            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "CpEd", NotesList.SelectedIndex, myMovie.movieText.Length);
            menuItem = menuItem + " " + GetGptModel();

            myMovie.myNoteTextList.Add(new NotesMovieText(myMovie.movieText, NotesForMovieText.Text, menuItem));
            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
        }
        
        private async Task CopyEdit_Enrich()
        {
            string workMovieText = MovieText.Text;
            MovieText.Text = $"Running Enrichment functionality using {GetGptModel()}...\r\n\r\n" + workMovieText;
            List<string> startParagraphs = new List<string>();
            List<string> doneParagraphs = new List<string>();

            startParagraphs = workMovieText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            startParagraphs = startParagraphs.Select(s => s.Trim()).ToList();
            startParagraphs = Utils.noWhiteSpaceList(startParagraphs);

            int phaseKount = 0;
            int phaseTotal = startParagraphs.Count;
            string response = "";
            string movieNote = "Add more detail.  Make move vivid.   Do not add any commentary or introduction.  Only provide the rewritten text.";
            phaseKount = 0;
            foreach (string para in startParagraphs)
            {
                phaseKount++;
                if (para.Contains(">") && para.Contains("<"))
                {
                    response = await MyGPT.enrichenMovieText(GetGptModel(), phaseKount, phaseTotal, workMovieText, para, movieNote, this);
                    isError = Utils.checkForGPTErrors(response, this);
                    if (isError)
                    {
                        return;
                    }
                    doneParagraphs.Add(response);


                    workMovieText = "";
                    foreach (string para2 in doneParagraphs)
                    {
                        workMovieText += para2.Trim() + "\r\n\r\n";
                    }
                    if (phaseKount < startParagraphs.Count - 1)
                    {
                        for (int i = phaseKount; i < startParagraphs.Count; i++)
                        {
                            workMovieText += startParagraphs[i].Trim() + "\r\n\r\n";
                        }

                    }
                    workMovieText = workMovieText.Trim();

                }
                else
                {
                    doneParagraphs.Add(para);
                    updateGPTStatus($"phase {phaseKount} of {phaseTotal} Enrichen Movie Text", Color.Red);
                    Thread.Sleep(4500);
                }



            }

            MovieText.Text = workMovieText;

            myMovie.movieText = MovieText.Text;

            string menuItem = makeMovieMenuLabel(myMovie.myNoteTextList, "Rich", NotesList.SelectedIndex, myMovie.movieText.Length);
            menuItem = menuItem + " " + GetGptModel();
            myMovie.myNoteTextList.Add(new NotesMovieText(MovieText.Text, "", menuItem));

            NotesList.DataSource = null;
            NotesList.DisplayMember = null;
            NotesList.DataSource = myMovie.myNoteTextList;
            NotesList.DisplayMember = "myLabel";
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
            ShowEditButton(MovieText);

        }

        private void CopyEditButton_Click(object sender, EventArgs e)
        {
            // open FormCopyEdit as dialog and pass this
            FormCopyEdit formCopyEdit = new FormCopyEdit(this);
            formCopyEdit.StartPosition = FormStartPosition.CenterScreen;
            formCopyEdit.ShowDialog();
        }

        private void MovieTextStylesCopyButton_Click(object sender, EventArgs e)
        {
            copyTextRTB(MovieTextStylesGuideRichTextBox);
        }

        private void MovieTextStylesGuideListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MovieTextStylesGuideListbox.SelectedIndex > -1)
            {
                MovieTextStylesGuideRichTextBox.Text = movieTextStyleGuides[MovieTextStylesGuideListbox.SelectedIndex].style.Trim();
                if (movieTextStyleGuides[MovieTextStylesGuideListbox.SelectedIndex].label == "-none-")
                {
                    MovieTextStylesGuideRichTextBox.ReadOnly = true;
                }
                else
                {
                    MovieTextStylesGuideRichTextBox.ReadOnly = false;
                }
            }
            updateMovieTextStylesToDictionary(MovieTextStylesGuideListbox);
        }

        private void MovieTextStyleGuideDeleteButton_Click(object sender, EventArgs e)
        {
            string myStyle = movieTextStyleGuides[MovieTextStylesGuideListbox.SelectedIndex].label;

            int startIndex = MovieTextStylesGuideListbox.SelectedIndex;

            string jsonString;

            if (myStyle == "-none-")
            {
                MessageBox.Show("You can not delete the \"-none-\" style");
                return;
            }
            DialogResult result = MessageBox.Show($"Proceeding will permanenetly delete style:\r\n{myStyle} \r\n\r\n Do you want to proceed?", "Delete Style", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            List<StyleElements> tempStyleGuides = null;
            tempStyleGuides = movieTextStyleGuides;
            tempStyleGuides.RemoveAt(startIndex);

            movieTextStyleGuides = tempStyleGuides;

            if (startIndex > 0)
            {
                MovieTextStylesGuideListbox.BeginUpdate();
                MovieTextStylesGuideListbox.DataSource = null;
                MovieTextStylesGuideListbox.DisplayMember = null;
                MovieTextStylesGuideListbox.DataSource = movieTextStyleGuides;
                MovieTextStylesGuideListbox.DisplayMember = "label";
                MovieTextStylesGuideListbox.SelectedIndex = startIndex - 1;
                MovieTextStylesGuideListbox.EndUpdate();
            }
            else
            {
                MovieTextStylesGuideListbox.BeginUpdate();
                MovieTextStylesGuideListbox.DataSource = null;
                MovieTextStylesGuideListbox.DisplayMember = null;
                MovieTextStylesGuideListbox.DataSource = styles;
                MovieTextStylesGuideListbox.DisplayMember = "label";
                MovieTextStylesGuideListbox.SelectedIndex = 0;
                MovieTextStylesGuideListbox.EndUpdate();
            }

            jsonString = JsonConvert.SerializeObject(movieTextStyleGuides);
            File.WriteAllText(movieTextStyleGuideFile, jsonString);

            updateMovieTextStylesToDictionary(MovieTextStylesGuideListbox);
        }

        private void MovieTextStylesAddStyleButton_Click(object sender, EventArgs e)
        {
            string jsonString;
            int listboxCount = MovieTextStylesGuideListbox.Items.Count;
            if (NewMovieTextStyleGuideTextBox.Text.Length >= 5)
            {
                movieTextStyleGuides.Add(new StyleElements($"{NewMovieTextStyleGuideTextBox.Text}", "guide", "style guide here..."));
                MovieTextStylesGuideListbox.DataSource = null;
                MovieTextStylesGuideListbox.DataSource = movieTextStyleGuides;
                MovieTextStylesGuideListbox.DisplayMember = "label";
                MovieTextStylesGuideListbox.SelectedIndex = MovieTextStylesGuideListbox.Items.Count - 1;
                jsonString = JsonConvert.SerializeObject(movieTextStyleGuides);
                File.WriteAllText(movieTextStyleGuideFile, jsonString);
                NewMovieTextStyleGuideTextBox.Text = "";
                DisplayStyleGuideRTB.Text = movieTextStyleGuides[(MovieTextStylesGuideListbox.Items.Count) - 1].style;
            }
            else
            {
                MessageBox.Show("At least 5 characters required for Style Guide ");
            }

            updateMovieTextStylesToDictionary(MovieTextStylesGuideListbox);

        }

        private void MovieTextStylesSaveButton_Click(object sender, EventArgs e)
        {
            int index = MovieTextStylesGuideListbox.SelectedIndex;
            string newStyle = MovieTextStylesGuideRichTextBox.Text;
            string label = movieTextStyleGuides[index].label;

            movieTextStyleGuides[index] = new StyleElements { label = label, type = "guide", style = newStyle };
            string jsonString = JsonConvert.SerializeObject(movieTextStyleGuides);
            File.WriteAllText(movieTextStyleGuideFile, jsonString);
            MovieTextStylesSaveLabel.Text = "Last saved: " + DateTime.Now.ToString("HH:mm:ss");//MovieTextStylesSaveLabel
        }

        private void MovieTextChatButton_Click(object sender, EventArgs e)
        {
            var systemPrompt = Prompts.FillPrompt(Prompts.SystemPrompt_MovieTextChatter_V1,
                new Dictionary<string, string>()
                {
                    { "movieSeed", MovieHintText.Text },
                    { "movieProfile", Utils.getProfilePrompt(this, "MovieText") },
                    { "movieText", MovieText.Text },
                });

            FormChatter formChatter = new FormChatter(GetGptModel(), systemPrompt, "Movie Text", this);
            formChatter.StartPosition = FormStartPosition.CenterScreen;
            formChatter.ShowDialog();
        }

        bool ignoreNextKeyChange = false;
        private void openRouterKeyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreNextKeyChange)
            {
                ignoreNextKeyChange = false;
                return;
            }
            if (openRouterKeyTextBox.Text == null || openRouterKeyTextBox.Text.Length != 73 || ignoreNextKeyChange) return;
            Api.openRouterKey = openRouterKeyTextBox.Text;
            _ = LoadOpenRouterModels();
            // save key to file
            try
            {
                string filePath = rootDir + "key.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(Api.openRouterKey);
                }
                //MessageBox.Show("Key saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to file: {ex.Message}");
            }
        }

        private void LoadKey()
        {
            try
            {
                string filePath = rootDir + "key.txt";
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    content = content.Trim();
                    ignoreNextKeyChange = true;
                    openRouterKeyTextBox.Text = content;
                    Api.openRouterKey = content;
                }
                else
                {
                    MessageBox.Show("Make sure to enter your OpenRouter key after opening a project.");
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading from file: {ex.Message}");
            }
        }
    }
}


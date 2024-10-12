using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptHelper
{

    public struct moviesListItem
    {
        public string myTitle { get; set; }
        public string myFilename { get; set; }  

    }
    public partial class FormStart : Form
    {
        List<moviesListItem> myMoviesList;

        bool loggingIn = false;
        string rootDir = Utils.rootDir();
        string moviesDir = Utils.moviesDir();

        public bool Exited { get; internal set; }
        public bool LoggedOut { get; internal set; }

        public FormStart()
        {
            InitializeComponent();

            // enable cut and paste in Richtextboxes

            Utils.EnableFormRightClickForCutPaste(this);
			this.StartPosition = FormStartPosition.CenterScreen;
            Exited = true;
            LoggedOut = false;
            Version.Text = "ScriptHelper Version: " + Utils.programVersion;
            //updateStatusText();
        }

        private async void Start_Load(object sender, EventArgs e)
        {
            //if (Utils.User?.UserData == null)
            //{
            //    var isValid = await login();
            //    if (!isValid)
            //    {
            //        Exited = true;
            //        LoggedOut = true;
            //        Close();
            //        return;
            //    }
            //}

            List<moviesListItem> myMovies = getMovies(moviesDir);

            PreviousMovies.DataSource = null;
            PreviousMovies.DataSource = myMovies;
            PreviousMovies.DisplayMember = "myTitle";
        }

        //public async Task<bool> login()
        //{
        //    updateStatusText();
        //    loggingIn = true;
        //    Cursor = Cursors.WaitCursor;
        //    FormLogin loginForm = new FormLogin();
        //    var isValid = await loginForm.ValidateLogin();
        //    if (!isValid)
        //    {
        //        //this.Hide();
        //        loginForm.StartPosition = FormStartPosition.CenterScreen;
        //        loginForm.ShowDialog();
        //        //this.Show();
        //        loggingIn = false;
        //    }
        //    updateStatusText();
        //    Cursor = Cursors.Default;
        //    return loginForm.IsValid;
        //}

        private List<moviesListItem> getMovies(string myDirectory)
        {
            List<object> containerList = new List<object>();
            string theTitle = "";
            string jsonString = "";
            string innerJsonString = "";
            DirectoryInfo directoryInfo = new DirectoryInfo(myDirectory);
            FileInfo[] files = directoryInfo.GetFiles()
                                             .OrderByDescending(f => f.LastWriteTime)
                                             .ToArray();

            myMoviesList = new List<moviesListItem>();


            foreach(FileInfo file in files)
            {
                string filePath = myDirectory + "\\" + file.Name;

                jsonString = File.ReadAllText(filePath);

                containerList = JsonConvert.DeserializeObject<List<object>>(jsonString);

                innerJsonString = containerList[1].ToString();


                MovieObj movieObj = new MovieObj();
                movieObj = JsonConvert.DeserializeObject<MovieObj>(innerJsonString);
                
                
                myMoviesList.Add(new moviesListItem { myFilename = file.FullName, myTitle = movieObj.title});

            }


            return myMoviesList;

        }

        private void CreateNewMovie_Click(object sender, EventArgs e)
        {
            if (MovieTitle.Text.Length > 5)
            {
                string theTitle = "";
                MovieObj newMovie = new MovieObj();
                newMovie.title = MovieTitle.Text.Trim();
                theTitle = newMovie.title;

                List<SceneObj> newScenes = new List<SceneObj>();
                List<CharacterObj> newCharacters = new List<CharacterObj>();
                List<SceneCharacter> newCharactersInScenes = new List<SceneCharacter>();
                Utils.dataDictionary = new Dictionary<string, object>();
                List<object> listContainer= new List<object>();

                


                listContainer.Add(Utils.dataVersion);
                listContainer.Add(newMovie);
                listContainer.Add(newScenes);
                listContainer.Add(Utils.dataDictionary);
                listContainer.Add(newCharacters);
                listContainer.Add(newCharactersInScenes); 

                
                

                string jsonString = JsonConvert.SerializeObject(listContainer);

                string fileName = Utils.makeFilename(theTitle);
                string outPath = moviesDir + "\\" + fileName;


                File.WriteAllText(outPath, jsonString);

                Utils.currentMovieFilename = outPath;
                Exited = false;
                this.Close();

            }
            else
            {
                MessageBox.Show("Title must be at least 6 chracters long");
                Utils.nop();
            }
            
        }

        

        private void ExitStart_Click(object sender, EventArgs e)
        {
            Exited = true;
            Environment.Exit(0);
            this.Close();
        }

        private void OpenMovie_Click(object sender, EventArgs e)
        {
            int movieIndex = -1;
            movieIndex = PreviousMovies.SelectedIndex;

            if (movieIndex > -1) 
            {
                Utils.currentMovieFilename = myMoviesList[movieIndex].myFilename;
            }
            Exited = false;
            this.Close();
        }

        private void DeleteMovie_Click(object sender, EventArgs e)
        {

            string selectedTitle = "";
            int movieIndex = -1;
            movieIndex = PreviousMovies.SelectedIndex;

            if (PreviousMovies.SelectedIndex > -1)
            {

                selectedTitle = myMoviesList[movieIndex].myTitle;
            }

            else

            {

                MessageBox.Show("no movies in the list");
                return;
            }                
                
            
            
            
            DialogResult result = MessageBox.Show("Do you want to delete \"" + selectedTitle + "\"", "Delete Movie", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
            {
                // User clicked Cancel
            }
            else if (result == DialogResult.OK)
            {
                string myFile = myMoviesList[movieIndex].myFilename.Trim();
                File.Delete(myFile);
                List<moviesListItem> myMovies = getMovies(moviesDir);

                PreviousMovies.DataSource = null;
                PreviousMovies.DataSource = myMovies;
                PreviousMovies.DisplayMember = "myTitle";


            }

        }

        //private async void btnLogout_Click(object sender, EventArgs e)
        //{
        //    // if they chose to logout, make them log in next time.
        //    Properties.Settings settings = Properties.Settings.Default;
        //    settings.ApiKey = string.Empty;
        //    settings.Save();
        //    Utils.User = null;
        //    Exited = true;
        //    var isValid = await login();
        //    if (isValid)
        //    {
        //        LoggedOut = false;
        //    }
        //    else
        //    {
        //        Exited = true;
        //        LoggedOut = true;
        //        Close();
        //        return;
        //    }
        //}

        //private void updateStatusText()
        //{ 
        //    if (Utils.User == null)
        //    {
        //        lblStatus.Text = "Logging you in...";
        //    }
        //    else
        //    {
        //        lblStatus.Text = $"Logged in as {Utils.User.UserData.Name} ({Utils.User.UserData.UserName})";
        //        this.Text = $"ScriptHelper Start : {Utils.User?.UserData?.Name}";

        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            
            string fileName = "";
            string destinationPath = "";
            

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\"; // You can set this to any default path.
                openFileDialog.Filter = "shf files (*.shf)|*.shf"; // Example filter for text files.
                openFileDialog.FilterIndex = 2; // By default, show all files.
                

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the selected file.
                    string filePath = openFileDialog.FileName;

                    fileName = Path.GetFileName(filePath);

                    destinationPath = moviesDir + "\\" + fileName;
                    

                    if (File.Exists(destinationPath)) 
                    {
                        DialogResult result = MessageBox.Show("File already exists, do you want to replace it?", "Overwrite?", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            File.Copy(filePath, destinationPath, true);
                            List<moviesListItem> myMovies = getMovies(moviesDir);

                            PreviousMovies.DataSource = null;
                            PreviousMovies.DataSource = myMovies;
                            PreviousMovies.DisplayMember = "myTitle";

                        }
                        else if (result == DialogResult.No)
                        {
                            // User clicked on "No". Do the necessary tasks here.
                            return;
                        }

                    }
                    else
                    {
                        File.Copy(filePath, destinationPath, true);
                        List<moviesListItem> myMovies = getMovies(moviesDir);

                        PreviousMovies.DataSource = null;
                        PreviousMovies.DataSource = myMovies;
                        PreviousMovies.DisplayMember = "myTitle";
                    }
                                       
                    
                    
                    
                    
                    
                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (myMoviesList.Count == 0) 
            {
                MessageBox.Show("No Movies To Export");
                return;
            }

            moviesListItem selectedMovie = (moviesListItem)PreviousMovies.SelectedItem;

            string originalFileName = selectedMovie.myFilename;
            string title = selectedMovie.myTitle;
            string rootFilename = Path.GetFileNameWithoutExtension(originalFileName);
           
            string extension = Path.GetExtension(originalFileName);

            string fileContents = File.ReadAllText(originalFileName);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Optional: Set a default file name
            saveFileDialog.FileName = $"{rootFilename}.shf";

            // Optional: Set a default file extension
            saveFileDialog.DefaultExt = ".shf";

            // Optional: Set file filter options
            saveFileDialog.Filter = "Shf files (*.shf)|*.shf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name and write a file to the chosen path
                string saveFileName = saveFileDialog.FileName;
                // File.WriteAllText(saveFileName, fileContents);
                File.Copy(originalFileName, saveFileName, true);
                
                
            }

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MovieObj newMovie = new MovieObj();
            FormNewMovie newMovieForm = new FormNewMovie();
            newMovieForm.ShowDialog();
            if (newMovieForm.abortFlag)
            {
                return;
            }
            newMovie = newMovieForm.newMovie;
            

            List<SceneObj> newScenes = new List<SceneObj>();
            List<CharacterObj> newCharacters = new List<CharacterObj>();
            List<SceneCharacter> newCharactersInScenes = new List<SceneCharacter>();
            Utils.dataDictionary = new Dictionary<string, object>();
            List<object> listContainer = new List<object>();




            listContainer.Add(Utils.dataVersion);
            listContainer.Add(newMovie);
            listContainer.Add(newScenes);
            listContainer.Add(Utils.dataDictionary);
            listContainer.Add(newCharacters);
            listContainer.Add(newCharactersInScenes);




            string jsonString = JsonConvert.SerializeObject(listContainer);

            string fileName = Utils.makeFilename(newMovie.title);
            string outPath = moviesDir + "\\" + fileName;


            File.WriteAllText(outPath, jsonString);

            Utils.currentMovieFilename = outPath;
            Exited = false;
            this.Close();

        }
    }
}

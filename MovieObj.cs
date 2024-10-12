using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHelper
{
    
    public class MovieObj
    {
        // Movie Parameters
        public string title { get; set; } = "";

        public string genre { get; set; } = "";

        public string audience { get; set; } = "";

        public string ratingUSA { get; set; } = "";

        public string guidance { get; set; } = "";

        public Boolean useProfileMovieText { get; set; } = true;

        public Boolean useProfileSceneText { get; set; } = true;    
        public Boolean useProfileSceneScript { get; set; } = true;

        public Boolean useProfileMakeScenes { get; set; } = true;

        public int timeLength { get; set; } = 100;

        // end movie parameters

        public string movieHintText { get; set; } = "";
        public string movieText { get; set; } = "";
        public string movieTextBackup { get; set; } = "";
        public string movieTextCompiled { get; set; } = "";

        public string startYear { get; set; } = "";
        public string movieFullScript { get; set; } = ""; 
        public Guid id { get; set; }

        public List<NotesMovieText> myNoteTextList { get; set; }


        public MovieObj() 
        
        { 
            id= Guid.NewGuid();
            movieHintText = "put your ideas here";
            myNoteTextList = new List<NotesMovieText>();
            
        }

        




    }
}

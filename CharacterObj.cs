using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHelper
{
    // currently not using enums
    public enum Sex
    {
        Male,
        Female
    }

    public enum GenderRole
    {
        CisMale,
        CisFemale,
        TransMale,
        TransFemale

    }
    public enum SexualOrientation
    {
        Straight,
        Homosexual,
        Bisexual,
        Fluid
    }

    public enum Education
    {
         None,
         InSchool,
         LessThanHighSchool,
         Vocational,
         HighSchool,
         SomeCollege,
         Associates,
         Bachelors,
         Masters,
         Phd,
         Professional,
    }

    public struct CharacterProfiles
    {

        public string Age { get; set; }
            public string Name { get; set; }
            public string BackStory { get; set; }
            public string Physical { get; set; }
            public string Personality { get; set; }
            public string Speech { get; set; }
            

            // what's this for ?
            public void characterProfiles(string element1, string element2, string element3, string element4, string element5, string element6)
            {
                Age = element1;
                Name = element2;
                BackStory = element3;
                Physical = element4;
                Personality = element5;
                Speech = element6;
            
            }
    }

    public struct CharacterNameAge
    {
        public string Name { get; set; }
        public string Age { get; set; }
               
    }
    public class CharacterObj
    {
        Guid id;
        public string tagName { get; set; } = "";// from angle brackets 
        public string fullName { get; set; } = "";
        public string age { get; set; } = "";

        public string physicalDescription { get; set; } = "";
        public string backStory { get; set; } = "";

        public string personality { get; set; } = "";
        public string speechStyle { get; set; } = "";

        public string briefDescription { get; set; } = "";

        public int heightInches { get; set; }
        public int weightPounds { get; set; }
        public Sex sex { get; set; }
        public GenderRole genderRole { get; set; }
        public SexualOrientation sexualOrientation { get; set; }
        
        
        public CharacterObj()
        {
            id = Guid.NewGuid();

        }

    }
}

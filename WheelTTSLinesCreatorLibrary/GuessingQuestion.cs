using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    /// <summary>
    /// structure of TheWheelGuessing.jet file
    /// </summary>
    internal struct GuessingQuestions
    {
        public List<GuessingQuestion> content;
    }

    /// <summary>
    /// structure of each guessing question
    /// </summary>
    internal struct GuessingQuestion
    {
        public List<string> altSpellings;
        public string answer;
        public string category;
        public List<Clue> clues;
        public string followup;
        public string id;
        public string isValid;
        public string prompt;
        public string reveal;
        public bool us;
        public bool x;
    }

    /// <summary>
    /// structure of each guessing question clue
    /// </summary>
    internal struct Clue
    {
        public string clue;
        public string clueComment;
    }
}

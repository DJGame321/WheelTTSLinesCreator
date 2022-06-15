using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    /// <summary>
    /// Stores all paths required for the game
    /// </summary>
    public struct FilePaths
    {
        /// <summary>
        /// Path to the game's directory
        /// </summary>
        public string GamePath { get; set; }

        /// <summary>
        /// Path in which the audio files are outputted to
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Path to the miscellaneous lines file
        /// </summary>
        public string MiscellaneousLinesPath { get; set; }

        /// <summary>
        /// Path to the bank lines file
        /// </summary>
        public string BankLinesPath { get; set; }
    }
}

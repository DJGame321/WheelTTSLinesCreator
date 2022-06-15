using System.Collections.Generic;
using System.Speech.Synthesis;

namespace WheelTTSLinesCreatorLibrary
{
    public interface ITTSVoice
    {
        /// <summary>
        /// speech synthesizer
        /// </summary>
        SpeechSynthesizer SpeechSynthesizer { get; }

        /// <summary>
        /// Fetches a list of installed text to speech voices
        /// </summary>
        /// <returns>
        /// A list of strings containing the names of each installed voice
        /// </returns>
        List<string> GetVoices();

        /// <summary>
        /// Sets the text to speech voice
        /// </summary>
        /// <param name="voice">
        /// Name of voice (a list of these can be fethed from the GetVoices() method)
        /// </param>
        void SetVoice(string voice);

        /// <summary>
        /// Speaks the input prompt to the default audio device
        /// </summary>
        /// <param name="prompt">
        /// The prompt the text to speech voice will read
        /// </param>
        void SpeakToDefaultAudioDevice(string prompt);

        /// <summary>
        /// Speaks the input prompt and outputs it to a wav file at the input path with the specified file name
        /// </summary>
        /// <param name="prompt">
        /// The prompt the text to speech voice will read
        /// </param>
        /// <param name="path">
        /// The .wav file is saved into this directory
        /// </param>
        /// <param name="fileName">
        /// The .wav file's name
        /// </param>
        void SpeakToWavFile(string prompt, string path, string fileName);
    }
}
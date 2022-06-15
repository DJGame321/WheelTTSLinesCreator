using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

namespace WheelTTSLinesCreatorLibrary
{
    internal class TTSVoice : ITTSVoice
    {
        /// <summary>
        /// speech synthesizer
        /// </summary>
        public SpeechSynthesizer SpeechSynthesizer { get; private set; } = new SpeechSynthesizer();

        /// <summary>
        /// Fetches a list of installed text to speech voices
        /// </summary>
        /// <returns>
        /// A list of strings containing the names of each installed voice
        /// </returns>
        public List<string> GetVoices()
        {
            List<InstalledVoice> installedVoices = SpeechSynthesizer.GetInstalledVoices().ToList();
            List<string> stringVoices = new List<string>();

            foreach (InstalledVoice installedVoice in installedVoices)
            {
                stringVoices.Add(installedVoice.VoiceInfo.Name);
            }

            return stringVoices;
        }

        /// <summary>
        /// Sets the text to speech voice
        /// </summary>
        /// <param name="voice">
        /// Name of voice (a list of these can be fethed from the GetVoices() method)
        /// </param>
        public void SetVoice(string voice)
        {
            SpeechSynthesizer.SelectVoice(voice);
        }

        /// <summary>
        /// Speaks the input prompt to the default audio device
        /// </summary>
        /// <param name="prompt">
        /// The prompt the text to speech voice will read
        /// </param>
        public void SpeakToDefaultAudioDevice(string prompt)
        {
            if (SpeechSynthesizer.State != SynthesizerState.Speaking)
            {
                SpeechSynthesizer.SetOutputToDefaultAudioDevice();
                SpeechSynthesizer.SpeakAsync(prompt);
            }
        }

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
        public void SpeakToWavFile(string prompt, string path, string fileName)
        {
            SpeechSynthesizer.SetOutputToWaveFile($@"{path}\{ fileName }.wav", new SpeechAudioFormatInfo(44100, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
            SpeechSynthesizer.Speak(prompt);
            SpeechSynthesizer.SetOutputToNull();
        }
    }
}

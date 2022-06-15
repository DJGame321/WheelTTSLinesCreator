using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    public static class WheelTTSLinesCreatorFactory
    {
        public static ITTSVoice CreateTTSVoice()
        {
            return new TTSVoice();
        }

        public static IAudioFileCreatorManager CreateAudioFileCreatorManager()
        {
            return new AudioFileCreatorManager();
        }
    }
}

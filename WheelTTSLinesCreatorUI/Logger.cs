using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelTTSLinesCreatorLibrary;

namespace WheelTTSLinesCreatorUI
{
    class Logger : ILogger
    {
        public void LogNewLine(string message)
        {
            WheelTTSLinesCreatorForm.instance.LogLine(message);
        }
    }
}

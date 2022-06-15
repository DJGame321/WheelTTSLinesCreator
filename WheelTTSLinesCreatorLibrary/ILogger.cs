using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    public interface ILogger
    {
        /// <summary>
        /// logs a new line
        /// </summary>
        /// <param name="message"></param>
        void LogNewLine(string message);
    }
}
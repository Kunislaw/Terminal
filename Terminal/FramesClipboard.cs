using System;

namespace Terminal
{
    public class FramesClipboard
    {
        public string creationTimestamp;
        public string lastModification = null;
        public string name { get; set; }
        public Frame frame { get; set; }

        public FramesClipboard()
        {
            creationTimestamp = DateTime.Now.ToString("d-M-y HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        public FramesClipboard(string frameName, Frame newFrame)
        {
            creationTimestamp = DateTime.Now.ToString("d-M-y HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            name = frameName;
            frame = newFrame;
        }

    }
}

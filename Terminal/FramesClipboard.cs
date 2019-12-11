using System;

namespace Terminal
{
    public class FramesClipboard
    {
        public string creationTimestamp;
        public string lastModification = null;
        public string name { get; set; }
        public byte[] frame { get; set; }

        public FramesClipboard()
        {
            creationTimestamp = DateTime.Now.ToString("d-M-y HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }
}

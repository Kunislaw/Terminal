using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Terminal
{
    public class Frame
    {
        public byte[] frameStructure {get; set;}
        public string format { get; set; }

        public Frame() {}

        public Frame(byte[] frameBytes)
        {
            frameStructure = frameBytes;       
        }

        public Frame(string stringToProcess, bool ASCII = true)
        {
            frameStructure = str2ByteArray(stringToProcess, ASCII).ToArray();
        }

        public Frame(string stringToProcess, string pattern, bool ASCII = true)
        {
            List<byte> bytesFromTextBox = str2ByteArray(stringToProcess, ASCII);
            frameStructure = null;
            if(bytesFromTextBox != null)
            {
                List<byte> byteArray = new List<byte>();
                string[] splittedPattern = pattern.Split(' ');
                foreach (string splittedValue in splittedPattern)
                {
                    if (splittedValue == "[data]")
                    {
                        byteArray.AddRange(bytesFromTextBox);
                    }
                    else
                    {
                        byteArray.Add(Convert.ToByte(splittedValue.Substring(2), 16));
                    }
                }
                if(byteArray.Count > 0)
                {
                    frameStructure = byteArray.ToArray();
                }
            }
        }

        private List<byte> str2ByteArray(string stringToProcess, bool ASCII = true)
        {
            List<byte> byteArray = new List<byte>();
            if (stringToProcess.Length < 1) return null;
            if (ASCII)
            {
                format = "ASCII";
                foreach (char c in stringToProcess)
                {
                    byteArray.Add((byte)c);
                }
            }
            else
            {
                format = "HEX";
                string[] splittedHEXs = stringToProcess.Split(' ');
                foreach (string element in splittedHEXs)
                {
                    if (Regex.IsMatch(element, @"^0x[0-9A-F]{2}$"))
                    {
                        byteArray.Add(Convert.ToByte(element.Substring(2), 16));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return byteArray;
        }
    }
}

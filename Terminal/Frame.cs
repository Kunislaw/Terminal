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
            Checksums checksums = new Checksums();
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
                    if(splittedValue == "[crc8]")
                    {
                        byte crc8 = checksums.CRC8(byteArray.ToArray());
                        byteArray.Add(crc8);
                    }
                    if (splittedValue == "[crc16]")
                    {
                        byte[] crc16 = checksums.CRC16(byteArray.ToArray());
                        byteArray.AddRange(crc16);

                    }
                    if (splittedValue == "[crc32]")
                    {
                        byte[] crc32 = BitConverter.GetBytes(checksums.CRC32(byteArray.ToArray()));
                        byteArray.AddRange(crc32);
                    }
                    if (splittedValue == "[bcc]")
                    {
                        byte bcc = checksums.BCC(byteArray.ToArray());
                        byteArray.Add(bcc);
                    }
                    if (splittedValue == "[parity]")
                    {
                        byte parity = checksums.parity(byteArray.ToArray());
                        byteArray.Add(parity);
                    }
                    if (Regex.IsMatch(splittedValue.Substring(2), @"^[A-F0-9]{2}$"))
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

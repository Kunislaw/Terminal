using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class Checksums
    {
        static byte[] CRC8Table =
        {
            0, 94, 188, 226, 97, 63, 221, 131, 194, 156, 126,
         32, 163, 253, 31, 65, 157, 195, 33, 127, 252, 162,
         64, 30, 95, 1, 227, 189, 62, 96, 130, 220, 35,
         125, 159, 193, 66, 28, 254, 160, 225, 191, 93, 3,
         128, 222, 60, 98, 190, 224, 2, 92, 223, 129, 99,
         61, 124, 34, 192, 158, 29, 67, 161, 255, 70, 24,
         250, 164, 39, 121, 155, 197, 132, 218, 56, 102,
         229, 187, 89, 7, 219, 133, 103, 57, 186, 228, 6,
         88, 25, 71, 165, 251, 120, 38, 196, 154, 101, 59,
         217, 135, 4, 90, 184, 230, 167, 249, 27, 69, 198,
         152, 122, 36, 248, 166, 68, 26, 153, 199, 37, 123,
         58, 100, 134, 216, 91, 5, 231, 185, 140, 210, 48,
         110, 237, 179, 81, 15, 78, 16, 242, 172, 47, 113,
         147, 205, 17, 79, 173, 243, 112, 46, 204, 146,
         211, 141, 111, 49, 178, 236, 14, 80, 175, 241, 19,
         77, 206, 144, 114, 44, 109, 51, 209, 143, 12, 82,
         176, 238, 50, 108, 142, 208, 83, 13, 239, 177,
         240, 174, 76, 18, 145, 207, 45, 115, 202, 148, 118,
         40, 171, 245, 23, 73, 8, 86, 180, 234, 105, 55,
         213, 139, 87, 9, 235, 181, 54, 104, 138, 212, 149,
         203, 41, 119, 244, 170, 72, 22, 233, 183, 85, 11,
         136, 214, 52, 106, 43, 117, 151, 201, 74, 20, 246,
         168, 116, 42, 200, 150, 21, 75, 169, 247, 182, 232,
         10, 84, 215, 137, 107, 53
        };
        public byte CRC8(byte[] data)
        {
            byte crc = 0;
            if (data != null && data.Length > 0)
            {
                foreach (byte b in data)
                {
                    crc = CRC8Table[crc ^ b];
                }
            }
            return crc;
        }
        public byte[] CRC16(byte[] data)
        {
            ushort crc = 0;
            ushort patternOne = 0x8000;
            ushort patternTwo= 0x8005;
            foreach (byte oneByte in data)
            {
                crc ^= (ushort)(oneByte << 8);
                for(byte k = 0; k < 8; k++)
                {
                    ushort tmp = (ushort)(crc & patternOne);
                    if (tmp > 0)
                    {
                        crc = (ushort)((crc << 1) ^ patternTwo);
                    }
                    else
                    {
                        crc = (ushort)(crc << 1);
                    }
                }
            }
            byte crcFirstByte = (byte)(crc & 0xFF);
            byte crcSecondByte = (byte)((crc >> 8) & 0xFF);
            return new byte[] { crcFirstByte, crcSecondByte};
        }
        public byte[] CRC32(byte[] data)
        {
            byte[] a = { 1, 2, 3 };
            return a;
            //Dokonczyc
        }
        public byte BCC(byte[] data)
        {
            byte bcc = 0;
            foreach(byte onebyte in data)
            {
                bcc ^= onebyte;
            }
            return bcc;
        }
        public byte parity(byte[] data)
        {
            short parity = 0;
            foreach (byte oneByte in data)
            {
                for (byte i = 0; i < 8; i++)
                {
                    int tmp = oneByte & (1 << i);

                    if (tmp > 0)
                    {
                        parity++;
                    }
                }
            }
            if (parity % 2 == 0)
            {
                return 0x01;
            } else
            {
                return 0x00;
            }
        }

    }
}

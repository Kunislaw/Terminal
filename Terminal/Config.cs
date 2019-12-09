using Newtonsoft.Json;
using System;
using System.IO;

namespace Terminal
{
    class Config
    {
        public string lastCOM { get; set; }
        public int lastSpeed { get; set; }
        public int bitsNumber { get; set; }
        public int parity { get; set; }
        public int stopsBits { get; set; }
        public int handShaking { get; set; }

        public Config()
        {
            lastCOM = "";
            lastSpeed = 0;
            bitsNumber = 0;
            parity = 0;
            stopsBits = 0;
            handShaking = 0;
        }

        public void setConfig(string lCOM, int lSpeed, int lBitsNumber, int lParity, int lstopsBits, int lHandShaking)
        {
            lastCOM = lCOM;
            lastSpeed = lSpeed;
            bitsNumber = lBitsNumber;
            parity = lParity;
            stopsBits = lstopsBits;
            handShaking = lHandShaking;
        }

        public void saveConfig()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "TerminalConfig.json")))
            {
                outputFile.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
        public void readConfig()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (File.Exists(Path.Combine(docPath, "TerminalConfig.json")))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(docPath, "TerminalConfig.json")))
                {
                    string data = sr.ReadToEnd();
                    Config tmpConfig = JsonConvert.DeserializeObject<Config>(data);
                    lastCOM = tmpConfig.lastCOM;
                    lastSpeed = tmpConfig.lastSpeed;
                    bitsNumber = tmpConfig.bitsNumber;
                    parity = tmpConfig.parity;
                    stopsBits = tmpConfig.stopsBits;
                    handShaking = tmpConfig.handShaking;
                }
            }
        }
    }

}

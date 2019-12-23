using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Controls;

namespace Terminal
{
    public class Config
    {
        public string lastCOM { get; set; }
        public int lastSpeed { get; set; }
        public int bitsNumber { get; set; }
        public Parity parity { get; set; }
        public StopBits stopsBits { get; set; }
        public Handshake handShaking { get; set; }
        public List<FramesClipboard> framesClipboard { get; set; }

        public Config()
        {
            lastCOM = "";
            lastSpeed = 0;
            bitsNumber = 0;
            parity = 0;
            stopsBits = 0;
            handShaking = 0;
            framesClipboard = new List<FramesClipboard>();
        }

        public void setConfig(string lCOM, int lSpeed, int lBitsNumber, Parity lParity, StopBits lstopsBits, Handshake lHandShaking)
        {
            lastCOM = lCOM;
            lastSpeed = lSpeed;
            bitsNumber = lBitsNumber;
            parity = lParity;
            stopsBits = lstopsBits;
            handShaking = lHandShaking;
        }

        public void addFrame(string name, Frame frame)
        {
            FramesClipboard frameClipboard = new FramesClipboard(name, frame);
            framesClipboard.Add(frameClipboard);
        }

        public void refreshFrameList(MainWindow mW)
        {
            mW.FramesListBox.Items.Clear();
            ContextMenu cm = App.Current.MainWindow.FindResource("framesClipboard") as ContextMenu;
            for (int i = 0; i < framesClipboard.Count; i++)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.ContextMenu = cm;
                listBoxItem.Content = framesClipboard[i].name;
                mW.FramesListBox.Items.Add(listBoxItem);
            }
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
                    framesClipboard = tmpConfig.framesClipboard;
                }
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Terminal
{
    class Config
    {
        private string lastCOM { get; set; }
        private int lastSpeed { get; set; }
        private int bitsNumber { get; set; }
        private int parity { get; set; }
        private int stopsBits { get; set; }
        private int handShaking { get; set; }

        public void saveConfig()
        {
            //Todo dorobic zapisywanie ostatnio ustawionego configu
        }
        public void readConfig()
        {
            //Todo dorobic odczytywanie ostatnio ustawionego configu
        }
    }

}

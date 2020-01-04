using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for SetTransmitPattern.xaml
    /// </summary>
    public partial class SetTransmitPattern : Window
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public SetTransmitPattern()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            TransmitPatternTextBox.Text = mainWindow.patternTransmsit;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string[] splittedValues = TransmitPatternTextBox.Text.Split(' ');
            bool allValuesCorrect = true;
            foreach(string splittedValue in splittedValues)
            {
                if(!Regex.IsMatch(splittedValue, @"^(\[data\]|\[crc8\]|\[crc16\]|\[crc32\]|\[bcc\]|\[parity\]|0x[A-F0-9]{2})$"))
                {
                    allValuesCorrect = false;
                }
            }
            if (allValuesCorrect)
            {
                mainWindow.patternTransmsit = TransmitPatternTextBox.Text;
                Close();
            }
        }
    }
}

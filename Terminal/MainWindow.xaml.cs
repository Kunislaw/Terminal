using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Terminal
{

    public partial class MainWindow : Window
    {
        private SerialPortCommunication serialPortCommunication;
        private Config config = new Config();
        public ObservableCollection<ComboBoxItem> CbItems { get; set; }
        public ComboBoxItem SelectedCbItem { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CbItems = new ObservableCollection<ComboBoxItem>();
            foreach(string item in SerialPort.GetPortNames())
            {
                CbItems.Add(new ComboBoxItem { Content = item });
            }
            serialPortCommunication = new SerialPortCommunication(this);
            config.readConfig();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            string portName = COMComboBox.Text;
            int baudRate = int.Parse(BaudrateComboBox.Text);
            int dataBits = int.Parse(BitsComboBox.Text);


            Handshake handshake = 0;
            Parity parity = Parity.None;
            StopBits stopBits = StopBits.None;

            if (HandshakeComboBox.Text == "None")
                handshake = Handshake.None;

            if (ParityComboBox.Text == "None")
                parity = Parity.None;
            else if (ParityComboBox.Text == "Odd")
                parity = Parity.Odd;
            else if (ParityComboBox.Text == "Even")
                parity = Parity.Even;
            else if (ParityComboBox.Text == "Mark")
                parity = Parity.Mark;
            else if (ParityComboBox.Text == "Space")
                parity = Parity.Space;

            if (StopBitsComboBox.Text == "None")
                stopBits = StopBits.None;
            if (StopBitsComboBox.Text == "1")
                stopBits = StopBits.One;
            if (StopBitsComboBox.Text == "1.5")
                stopBits = StopBits.OnePointFive;
            if (StopBitsComboBox.Text == "2")
                stopBits = StopBits.Two;

            serialPortCommunication.SetSerialPortParameters(portName, baudRate, dataBits, handshake, parity, stopBits);

            if (serialPortCommunication.Open())
            {
                ClosedText.Foreground = Brushes.Green;
                ClosedText.Text = "Otwarty";
                SendButton.IsEnabled = true;
                OpenButton.IsEnabled = false;
                CloseButton.IsEnabled = true;
                SendTextBox.IsEnabled = true;
                config.saveConfig();
            }

                
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPortCommunication.Close())
            {
                ClosedText.Foreground = Brushes.Red;
                ClosedText.Text = "Zamknięty";
                SendButton.IsEnabled = false;
                OpenButton.IsEnabled = true;
                CloseButton.IsEnabled = false;
                SendTextBox.IsEnabled = false;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            List<byte> bufferToSend = new List<byte>();
            foreach(char c in SendTextBox.Text)
            {
                bufferToSend.Add((byte)c);
            }
            serialPortCommunication.Send(bufferToSend.ToArray());
            appendTextToConsole(SendTextBox.Text + "\n", Brushes.Blue, true);
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            RTBConsole.Document.Blocks.Clear();
        }
        public void appendTextToConsole(String text, SolidColorBrush color, bool timestamp)
        {
            TextRange range = new TextRange(
                    RTBConsole.Document.ContentEnd,
                    RTBConsole.Document.ContentEnd);
            if (timestamp)
            {
                range.Text = "[" + DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "]: ";
            }
            range.Text += text;
            range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        private void RTBConsole_TextChanged(object sender, TextChangedEventArgs e)
        {
            RTBConsole.ScrollToEnd();
        }
    }

}

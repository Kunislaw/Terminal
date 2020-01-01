using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Terminal
{
    public partial class MainWindow : Window
    {
        private SerialPortCommunication serialPortCommunication;
        public Config config { get; set; }= new Config();
        public ObservableCollection<ComboBoxItem> CbItems { get; set; }
        public ComboBoxItem SelectedCbItem { get; set; }
        public string currentLoggingFile = null;

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
            
            // Ramki
            config.refreshFrameList(this);

            // Ustawienia polaczenia
            COMComboBox.Text = config.lastCOM;
            BaudrateComboBox.Text = config.lastSpeed.ToString();
            BitsComboBox.Text = config.bitsNumber.ToString();
            HandshakeComboBox.Text = config.handShaking.ToString();
            ParityComboBox.Text = config.parity.ToString();
         
            if (config.stopsBits.ToString() == "One")
                StopBitsComboBox.Text = "1";
            if (config.stopsBits.ToString() == "OnePointFive")
                StopBitsComboBox.Text = "1.5";
            if (config.stopsBits.ToString() == "Two")
                StopBitsComboBox.Text = "2";

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
                COMComboBox.IsEnabled = false;
                BaudrateComboBox.IsEnabled = false;
                BitsComboBox.IsEnabled = false;
                ParityComboBox.IsEnabled = false;
                StopBitsComboBox.IsEnabled = false;
                HandshakeComboBox.IsEnabled = false;
                config.setConfig(portName, baudRate, dataBits, parity, stopBits, handshake);
                config.saveConfig();
            } else
                MessageBox.Show("The semaphore timeout period has expired.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                
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
                COMComboBox.IsEnabled = true;
                BaudrateComboBox.IsEnabled = true;
                BitsComboBox.IsEnabled = true;
                ParityComboBox.IsEnabled = true;
                StopBitsComboBox.IsEnabled = true;
                HandshakeComboBox.IsEnabled = true;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = new Frame();
            if (RadioButton_ASCII.IsChecked == true)
            {
                frame = new Frame(SendTextBox.Text, true);
            }
            if (RadioButton_HEX.IsChecked == true)
            {
                frame = new Frame(SendTextBox.Text, false);
            }

            if (frame.frameStructure != null)
            {
                serialPortCommunication.Send(frame.frameStructure);
                appendTextToConsole(SendTextBox.Text + "\n", Brushes.Blue, true, true);
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            RTBConsole.Document.Blocks.Clear();
        }
        public void appendTextToConsole(string text, SolidColorBrush color, bool timestamp, bool sending)
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
            if (StartLogButton.IsEnabled == false && currentLoggingFile != null)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), currentLoggingFile), true))
                {
                    string preparedString = "";
                    if (timestamp) preparedString += "[" + DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "]: ";
                    if (sending) preparedString += "(T) ";
                    else preparedString += "(R) ";
                    preparedString += text;
                    outputFile.WriteAsync(preparedString);
                }
            }
        }

        private void RTBConsole_TextChanged(object sender, TextChangedEventArgs e)
        {
            RTBConsole.ScrollToEnd();
        }

        private void MenuItem_Click_Add(object sender, RoutedEventArgs e)
        {
            AddFrame addFrame = new AddFrame();
            addFrame.Show();
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            EditFrame editFrame = new EditFrame();
            editFrame.Show();
        }

        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)FramesListBox.SelectedItem;

            for (int i = 0; i < config.framesClipboard.Count; i++)
                if (config.framesClipboard[i].name.Equals((string)selectedItem.Content))
                    config.framesClipboard.RemoveAt(i);

            FramesListBox.Items.Remove(selectedItem);
            config.saveConfig();
            config.readConfig();
            config.refreshFrameList(this);
        }

        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)FramesListBox.SelectedItem;
            FramesClipboard searchedFromFrameClipboard = config.framesClipboard.Find((item) => item.name.Equals(selectedItem.Content));
            if(RadioButton_ASCII.IsChecked == true)
            {
                SendTextBox.Text = Encoding.Default.GetString(searchedFromFrameClipboard.frame.frameStructure);
            }
            if (RadioButton_HEX.IsChecked == true)
            {
                string formattedString = "";
                foreach(byte oneByte in searchedFromFrameClipboard.frame.frameStructure)
                {
                    formattedString += "0x" + oneByte.ToString("X") + " ";
                }
                formattedString = formattedString.Remove(formattedString.Length - 1);
                SendTextBox.Text = formattedString;
            }
        }
        private void StartLogButton_Click(object sender, RoutedEventArgs e)
        {
            if(StartLogButton.IsEnabled == true)
            {
                currentLoggingFile = DateTime.Now.ToString("HH_mm_ss_dd_MM_yyyy") + ".txt";
                StartLogButton.IsEnabled = false;
                StopLogButton.IsEnabled = true;
            }
        }
        private void StopLogButton_Click(object sender, RoutedEventArgs e)
        {
            if(StopLogButton.IsEnabled == true)
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Process.Start("explorer.exe", "/select," + Path.Combine(docPath,currentLoggingFile));
                currentLoggingFile = null;
                StartLogButton.IsEnabled = true;
                StopLogButton.IsEnabled = false;
            }
        }
        private void SetReceivePatternButton_Click(object sender, RoutedEventArgs e)
        {
            //tutaj ustawiamy o jakim wzorze maja byc wyswietlane bajty przychodze, jezeli * to wszystko ma byc wyswietlane
            //potrebne nowe okno do otwierania takie jak przy definicji ramki
        }
        private void SetTransmitPatternButton_Click(object sender, RoutedEventArgs e)
        {
            //tutaj ustawiamy strukture ramki do wysylania np. 0xFF [data] [crc8] 0xFF
            //potrzebne nowe okno do otwierania takie jak przy definicji ramki
        }
    }

}

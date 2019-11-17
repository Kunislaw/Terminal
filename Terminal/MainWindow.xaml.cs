using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Terminal
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private SerialPortCommunication serialPortCommunication = new SerialPortCommunication();
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
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            string portName = COMComboBox.Text;
            int baudRate = int.Parse(BaudrateComboBox.Text);
            int dataBits = int.Parse(BitsComboBox.Text);
            Handshake handshake = 0;
            Parity parity = Parity.None;
            StopBits stopBits = StopBits.None;

            // To do
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
            }
            else
            {
                ClosedText.Foreground = Brushes.Red;
                ClosedText.Text = "Zamknięty";
            }
                
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPortCommunication.Close())
            {
                ClosedText.Foreground = Brushes.Red;
                ClosedText.Text = "Zamknięty";
            }
            else
            {
                ClosedText.Foreground = Brushes.Green;
                ClosedText.Text = "Otwarty";
            }
        }
    }
}

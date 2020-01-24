using System;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Terminal
{
    class SerialPortCommunication
    {
        private SerialPort serialPort;
        private string ReceivedString { get; set; }
        private Time time = new Time();
        private MainWindow mainWindow;
        private long[] messageMilis = new long[2];
        private bool whichTime;


        public SerialPortCommunication(MainWindow mW)
        {
            mainWindow = mW;
            messageMilis[0] = time.getMillitsFrom1970();
            messageMilis[1] = time.getMillitsFrom1970() - 5;
            whichTime = false;
        }
        public void SetSerialPortParameters(string portName, int baudRate, int dataBits, Handshake handshake, Parity parity, StopBits stopBits)
        {
            try
            {
                serialPort = new SerialPort();
                serialPort.PortName = portName;
                serialPort.BaudRate = baudRate;
                serialPort.DataBits = dataBits;
                serialPort.Handshake = handshake;
                serialPort.Parity = parity;
                serialPort.StopBits = stopBits;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortReceivedData);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool Open()
        {
            try {
                serialPort.Open();
            } catch(Exception e) {
                MessageBox.Show("Nie udało się otworzyć portu\nPowód: " + e.Message, "Terminal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool Close()
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie udało się zamknąć portu\nPowód: " + e.Message, "Terminal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public void Send(byte[] buffer)
        {
            serialPort.Write(buffer, 0, buffer.Length);
        }


        public void SerialPortReceivedData(object sender, SerialDataReceivedEventArgs eventArgs)
        {

            byte[] buffer = new byte[serialPort.ReadBufferSize];

            int bytesRead = serialPort.Read(buffer, 0, buffer.Length);

            ReceivedString = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if(Math.Abs(messageMilis[0] - messageMilis[1]) > 3)
            {
                mainWindow.Dispatcher.Invoke(() => mainWindow.appendTextToConsole(ReceivedString + "\n", Brushes.Red, true, false));

            } else
            {
                mainWindow.Dispatcher.Invoke(() => mainWindow.appendTextToConsole(ReceivedString, Brushes.Red, true, false));

            }

            messageMilis[whichTime?1:0] = time.getMillitsFrom1970();

            whichTime = !whichTime;
        }
    }
}

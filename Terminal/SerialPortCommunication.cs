using System;
using System.IO.Ports;
using System.Text;

namespace Terminal
{
    class SerialPortCommunication
    {
        private SerialPort serialPort = new SerialPort();
        private string ReceivedString { get; set; }

        private const int EOT = 0x4;

        public void SetSerialPortParameters(string portName, int baudRate, int dataBits, Handshake handshake, Parity parity, StopBits stopBits)
        {
            try
            {
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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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

            ReceivedString += Encoding.ASCII.GetString(buffer, 0, bytesRead);
  
            // Jeśli znaleziony znak End-Of-Transmission
            if (ReceivedString.IndexOf((char)EOT) > -1)
            {
                // Istotne dane
                string workingString = ReceivedString.Substring(0, ReceivedString.IndexOf((char)EOT));

                ReceivedString = ReceivedString.Substring(ReceivedString.IndexOf((char)EOT));
                Console.WriteLine(workingString);
            }
        }
    }
}

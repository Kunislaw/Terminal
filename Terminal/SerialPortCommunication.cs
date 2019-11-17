﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class SerialPortCommunication
    {
        private SerialPort serialPort = new SerialPort();
        private String receivedString { get; set; }


        public void setSerialPortParameters(String portName, int baudRate, int dataBits, Handshake handshake, Parity parity, StopBits stopBits)
        {
            try
            {
                serialPort.PortName = portName;
                serialPort.BaudRate = baudRate;
                serialPort.DataBits = dataBits;
                serialPort.Handshake = handshake;
                serialPort.Parity = parity;
                serialPort.StopBits = stopBits;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPortReceivedData);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void Open()
        {
            serialPort.Open();
        }
        public void Close()
        {
            serialPort.Close();
        }
        public void Send(byte[] buffer)
        {
            serialPort.Write(buffer, 0, buffer.Length);
        }
        void serialPortReceivedData(object sender, SerialDataReceivedEventArgs eventArgs)
        {
            byte[] buffer = new byte[serialPort.ReadBufferSize];

            int bytesRead = serialPort.Read(buffer, 0, buffer.Length);

            receivedString += Encoding.ASCII.GetString(buffer, 0, bytesRead);
  
            if (receivedString.IndexOf((char)0x4) > -1)
            {
                string workingString = receivedString.Substring(0, receivedString.IndexOf((char)0x4));
                receivedString = receivedString.Substring(receivedString.IndexOf((char)0x4));
                Console.WriteLine(workingString);
            }
        }
    }
}
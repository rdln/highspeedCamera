using System;
using System.IO.Ports;
using System.Text;

namespace hscCtrl.Serial
{
    class SerialPortComm : ISerialPortComm
    {
        private readonly SerialPort serialPort;

        public event Action<string> OnNewMessage;

        public SerialPortComm(string port)
        {
            serialPort = InitSerialPort(port);
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Open()
        {
            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }

        public void Write(string message)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);
            serialPort.Write(messageBytes, 0, messageBytes.Length);
        }

        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        private SerialPort InitSerialPort(string port)
        {
            return new SerialPort(port)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None,
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var message = serialPort.ReadLine();
            var handler = OnNewMessage;
            handler?.Invoke(message);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    serialPort.DataReceived -= SerialPort_DataReceived;
                    serialPort.Close();
                    serialPort.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

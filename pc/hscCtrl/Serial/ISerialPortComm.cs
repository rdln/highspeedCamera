using System;

namespace hscCtrl.Serial
{
    internal interface ISerialPortComm : IDisposable
    {
        event Action<string> OnNewMessage;
        void Write(string message);
        void Open();
        void Close();
    }
}

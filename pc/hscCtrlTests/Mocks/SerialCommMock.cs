using hscCtrl.Serial;
using System;
using System.Threading.Tasks;

namespace hscCtrlTests.Mocks
{
    class SerialCommMock : ISerialPortComm
    {
        public Action OnClose { get; set; }
        public Action OnOpen { get; set; }
        public Action<string> OnWrite { get; set; }
        public Action OnDispose { get; set; }

        public event Action<string> OnNewMessage;

        public void Close()
        {
            OnClose?.Invoke();
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        public void Open()
        {
            OnOpen?.Invoke();
        }

        public void Write(string message)
        {
            OnWrite?.Invoke(message);
        }

        public void SimulateMessageReceived(string message)
        {
            OnNewMessage?.Invoke(message);
        }
    }
}

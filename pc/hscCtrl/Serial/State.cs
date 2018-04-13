using System.Collections.Generic;
using System.Linq;

namespace hscCtrl.Serial
{
    class State
    {
        private readonly List<StateTransfer> transfers = new List<StateTransfer>();

        public State()
        { }

        public State(IEnumerable<StateTransfer> transfers)
        {
            this.transfers.AddRange(transfers);
        }

        public State Handle(string message)
        {
            var transfer = transfers.FirstOrDefault(t => t.CanTransfer(message));
            return transfer?.GetNextState();
        }

        public void AddTransfer(StateTransfer transfer)
        {
            transfers.Add(transfer);
        }
    }
}

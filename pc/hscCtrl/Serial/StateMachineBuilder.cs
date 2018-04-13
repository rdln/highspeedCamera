using System;

namespace hscCtrl.Serial
{
    class StateMachineBuilder
    {
        const string ReadyMessage = "ready";
        const string DoneMessage = "done";

        public Action OpenFn { get; set; }
        public Action WriteInstructionsFn { get; set; }
        public Action DoneFn { get; set; }

        public State BuildStateMachine()
        {
            var waitReadyState = new State();
            var waitDoneState = new State();
            
            waitReadyState.AddTransfer(new StateTransfer((message) => ReadyMessage.Equals(message), WriteInstructionsFn, waitDoneState));
            waitReadyState.AddTransfer(new StateTransfer((message) => !ReadyMessage.Equals(message), waitReadyState));

            waitDoneState.AddTransfer(new StateTransfer((message) => DoneMessage.Equals(message), DoneFn));
            waitDoneState.AddTransfer(new StateTransfer((message) => !DoneMessage.Equals(message), waitDoneState));

            return waitReadyState;
        }
    }
}

using System;

namespace hscCtrl.Serial
{
    class StateTransfer
    {
        private readonly Func<string, bool> condition;
        private readonly Action onTransfer;
        private readonly State newState;

        public StateTransfer(Func<string, bool> condition, Action onTransfer, State newState)
        {
            this.condition = condition;
            this.onTransfer = onTransfer;
            this.newState = newState;
        }

        public StateTransfer(Func<string, bool> condition, State newState)
            : this(condition, null, newState)
        { }

        public StateTransfer(Func<string, bool> condition, Action onTransfer)
            : this(condition, onTransfer, null)
        { }

        public StateTransfer(Func<string, bool> condition)
            : this(condition, null, null)
        { }

        public bool CanTransfer(string message)
        {
            return condition(message);
        }

        public State GetNextState()
        {
            onTransfer?.Invoke();
            return newState;
        }
    }
}

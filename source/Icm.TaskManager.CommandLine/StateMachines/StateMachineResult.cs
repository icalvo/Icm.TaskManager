namespace Icm.ChoreManager.CommandLine.StateMachines
{
    public class StateMachineResult<TInput, TState, TOutput>
    {
        public StateMachineResult(TInput input, TState state, TOutput output)
        {
            Input = input;
            State = state;
            Output = output;
        }

        public TInput Input { get; }
        public TState State { get; }
        public TOutput Output { get; }
    }
}
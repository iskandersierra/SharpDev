namespace SharpDev.Cqrs
{
    public class CommandHandlerContext
    {
        public CommandHandlerContext(dynamic command, dynamic state, string stateName)
        {
            Command = command;
            State = state;
            StateName = stateName;
        }

        public dynamic Command { get; }
        public dynamic State { get; }
        public string StateName { get; }
    }
}

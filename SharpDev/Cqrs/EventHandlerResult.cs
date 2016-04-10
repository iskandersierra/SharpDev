namespace SharpDev.Cqrs
{
    public class EventHandlerResult
    {
        public EventHandlerResult(dynamic state, string stateName)
        {
            State = state;
            StateName = stateName;
        }

        public dynamic State { get; }
        public string StateName { get; }
    }
}
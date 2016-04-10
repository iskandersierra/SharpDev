namespace SharpDev.Cqrs
{
    public class EventHandlerContext
    {
        public EventHandlerContext(dynamic @event, dynamic state, string stateName)
        {
            Event = @event;
            State = state;
            StateName = stateName;
        }

        public dynamic Event { get; }
        public dynamic State { get; }
        public string StateName { get; }
    }
}
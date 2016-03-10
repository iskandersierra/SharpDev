namespace SharpDev.Modeling
{
    public class CommandPostcondition<TCommand, TTarget> : 
        CommandCondition<TCommand, TTarget>
        where TCommand : class
        where TTarget : class
    {
        public CommandPostcondition(TCommand command, TTarget target) : base(command, target)
        {
        }
    }
}
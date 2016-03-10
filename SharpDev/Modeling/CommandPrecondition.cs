namespace SharpDev.Modeling
{
    public class CommandPrecondition<TCommand, TTarget> : 
        CommandCondition<TCommand, TTarget>
        where TCommand : class
        where TTarget : class
    {
        public CommandPrecondition(TCommand command, TTarget target) : base(command, target)
        {
        }
    }
}
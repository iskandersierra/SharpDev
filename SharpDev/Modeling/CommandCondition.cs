using System;

namespace SharpDev.Modeling
{
    public class CommandCondition<TCommand, TTarget>
        where TCommand : class
        where TTarget : class
    {
        public CommandCondition(TCommand command, TTarget target)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (target == null) throw new ArgumentNullException(nameof(target));

            Command = command;
            Target = target;
        }

        public TCommand Command { get; }
        public TTarget Target { get; }
    }
}

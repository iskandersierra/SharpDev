using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using SharpDev.Cqrs;

namespace SharpDev.Samples.Accounting
{
    public class CreateAccountCommandHandler
    {
        public IEnumerable<dynamic> Handle(CommandHandlerContext context)
        {
            dynamic created = new ExpandoObject();
            created.id = context.State.id;
            created.owner = context.Command.owner;

            yield return created;
        }
    }

    public class DepositAccountCommandHandler
    {
        public IEnumerable<dynamic> Handle(CommandHandlerContext context)
        {
            dynamic deposited = new ExpandoObject();
            deposited.id = context.State.id;
            deposited.amount = context.Command.amount;

            yield return deposited;
        }
    }

    public class WithdrawAccountCommandHandler
    {
        public IEnumerable<dynamic> Handle(CommandHandlerContext context)
        {
            dynamic withdrawn = new ExpandoObject();
            withdrawn.id = context.State.id;
            withdrawn.amount = context.Command.amount;

            yield return withdrawn;
        }
    }

    public class CloseAccountCommandHandler
    {
        public IEnumerable<dynamic> Handle(CommandHandlerContext context)
        {
            dynamic closed = new ExpandoObject();
            closed.id = context.State.id;

            yield return closed;
        }
    }

    public class AccountStates
    {
        public static readonly string New = @"new";
        public static readonly string Open = @"open";
        public static readonly string Closed = @"closed";
    }

    public class AccountCreatedEventHandler
    {
        public EventHandlerResult OnEvent(EventHandlerContext context)
        {
            dynamic state = new ExpandoObject();
            state.id = context.State.id;
            state.owner = context.Event.owner;
            state.balance = 0.0;

            return new EventHandlerResult(state, AccountStates.Open);
        }
    }

    public class AccountDepositedEventHandler
    {
        public EventHandlerResult OnEvent(EventHandlerContext context)
        {
            dynamic state = new ExpandoObject();
            state.id = context.State.id;
            state.owner = context.Event.owner;
            state.balance = context.State.balance + context.Event.amount;

            return new EventHandlerResult(state, context.StateName);
        }
    }

    public class AccountWithdrawnEventHandler
    {
        public EventHandlerResult OnEvent(EventHandlerContext context)
        {
            dynamic state = new ExpandoObject();
            state.id = context.State.id;
            state.owner = context.Event.owner;
            state.balance = context.State.balance - context.Event.amount;

            return new EventHandlerResult(state, context.StateName);
        }
    }

    public class AccountClosedEventHandler
    {
        public EventHandlerResult OnEvent(EventHandlerContext context)
        {
            dynamic state = new ExpandoObject();
            state.id = context.State.id;
            state.owner = context.Event.owner;
            state.balance = 0.0;
            return new EventHandlerResult(state, AccountStates.Open);
        }
    }

    public class AccountAmountMustBePositive
    {
        private static readonly string[] MemberNames = { "amount" };

        public IEnumerable<ValidationResult> Validate(double amount)
        {
            if (!(amount >= 0.01))
                yield return new ValidationResult("amount must be positive", MemberNames);
        }
    }

    public class AccountAmountMustHaveTwoDecimalPlaces
    {
        private static readonly string[] MemberNames = { "amount" };

        public IEnumerable<ValidationResult> Validate(double amount)
        {
            if (!(Math.Round(amount, 2) == amount))
                yield return new ValidationResult("amount must have two decimal places at most", MemberNames);
        }
    }

    public class AccountBalanceMustBeNonNegative
    {
        private static readonly string[] MemberNames = { "balance" };
        public IEnumerable<ValidationResult> Validate(dynamic state)
        {
            if (!(state.balance >= 0.0))
                yield return new ValidationResult("Balance must be non negative", MemberNames);
        }
    }

    public class AccountOwnerCannotBeEmpty
    {
        private static readonly string[] MemberNames = { "owner" };
        public IEnumerable<ValidationResult> Validate(dynamic state)
        {
            if (!(state.owner != null))
                yield return new ValidationResult("Owner cannot be empty", MemberNames);
        }
    }

    public class AccountCreateOwnerCannotBeEmpty
    {
        private static readonly string[] MemberNames = { "owner" };
        public IEnumerable<ValidationResult> Validate(dynamic create)
        {
            if (!(create.owner != null))
                yield return new ValidationResult("Owner cannot be empty", MemberNames);
        }
    }
}

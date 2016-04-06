using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpDev.Messaging;

namespace SharpDev.EventSourcing
{
    public interface ICommandSenderProvider
    {
        Task<ICommandSender> GetSenderAsync(string tenantId, CancellationToken ct);
    }

    public interface ICommandSender
    {
        Task<SendCommandResult> SendCommandAsync(CommandEnvelope commandEnvelope, CancellationToken ct);
    }

    public enum SendCommandResultType
    {
        /// <summary>
        /// The command has been completed by this process
        /// </summary>
        Completed,
        /// <summary>
        /// The command has been enqueued for processing
        /// </summary>
        Accepted,
        /// <summary>
        /// Command has invalid parameters. Produces a BadRequest
        /// </summary>
        ValidationFailed,
        /// <summary>
        /// Some business rules has failed. Produces Forbidden
        /// </summary>
        BusinessPreconditionFailed,
        /// <summary>
        /// An error ocurred while processing the command
        /// </summary>
        ProcessingError,
        /// <summary>
        /// After processing the command the entity would reach an unacceptable state, so this is considered a broken business rule
        /// </summary>
        BusinessPostconditionFailed,
        /// <summary>
        /// Could not persist events to event store
        /// </summary>
        PersistenceFailed,
    }

    public class SendCommandResult
    {
        public SendCommandResult(SendCommandResultType type)
        {
            if (!IsSuccessType(type)) throw new ArgumentOutOfRangeException(nameof(type));
            if (!Enum.IsDefined(typeof (SendCommandResultType), type))
                throw new ArgumentOutOfRangeException(nameof(type));
            Type = type;
        }

        public SendCommandResult(SendCommandResultType type, IEnumerable<KeyValuePair<string, string>> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            if (IsSuccessType(type)) throw new ArgumentOutOfRangeException(nameof(type));
            if (!Enum.IsDefined(typeof(SendCommandResultType), type))
                throw new ArgumentOutOfRangeException(nameof(type));
            Type = type;
            Errors = errors;
        }

        public SendCommandResult(SendCommandResultType type, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (!Enum.IsDefined(typeof (SendCommandResultType), type))
                throw new ArgumentOutOfRangeException(nameof(type));
            if (type.IsNotOneOf(SendCommandResultType.ProcessingError, SendCommandResultType.PersistenceFailed)) throw new ArgumentOutOfRangeException(nameof(type));
            Type = type;
            Exception = exception;
        }

        public bool Success => IsSuccessType(Type);

        public SendCommandResultType Type { get; }

        public IEnumerable<KeyValuePair<string, string>> Errors { get; }

        public Exception Exception { get; }

        private static bool IsSuccessType(SendCommandResultType type)
        {
            return type.IsOneOf(SendCommandResultType.Accepted, SendCommandResultType.Completed);
        }
    }
}

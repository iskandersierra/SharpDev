using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDev.EventSourcing
{
    [Serializable]
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException()
        {
        }

        public OptimisticConcurrencyException(string message) : base(message)
        {
        }

        public OptimisticConcurrencyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected OptimisticConcurrencyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

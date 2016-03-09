using System;
using System.Globalization;
using System.Security.Claims;

namespace SharpDev.Messaging
{
    public class CommandEnvelope
    {
        public string CommandIdentifier { get; set; }
        public Lazy<object> Command { get; set; }

        public string ExpectedVersion { get; set; }
        public string CommandId { get; set; }
        public string CorrelationId { get; set; }

        public ClaimsPrincipal Principal { get; set; }
        public CultureInfo CurrentCulture { get; set; }
        public CultureInfo CurrentUICulture { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using SharpDev.Modeling;

namespace SharpDev
{
    public class ParseCommandResult
    {
        public ParseCommandResult(object parsedCommand, string commandType, DomainVersion version, ClaimsPrincipal user, CultureInfo culture, IDictionary<string, object> metadata)
        {
            if (parsedCommand == null) throw new ArgumentNullException(nameof(parsedCommand));
            if (commandType == null) throw new ArgumentNullException(nameof(commandType));
            if (version == null) throw new ArgumentNullException(nameof(version));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            ParsedCommand = parsedCommand;
            CommandType = commandType;
            Version = version;
            User = user;
            Culture = culture;
            Metadata = metadata;
        }

        public ParseCommandResult(IEnumerable<string> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            Errors = errors;
        }

        public bool Success => ParsedCommand != null;
        public object ParsedCommand { get; }
        public string CommandType { get; }
        public DomainVersion Version { get; }
        public ClaimsPrincipal User { get; }
        public CultureInfo Culture { get; }
        public IDictionary<string, object> Metadata { get; }
        public IEnumerable<string> Errors { get; }
    }

}
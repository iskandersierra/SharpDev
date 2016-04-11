using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpDev.Modeling
{
    [Serializable]
    public class DomainVersion : IComparable<DomainVersion>, IEquatable<DomainVersion>
    {
        private readonly List<string> _parts;
        private readonly List<bool> _isNumeric;

        public DomainVersion(int majorVersion) 
            : this(majorVersion.ToString(CultureInfo.InvariantCulture)) { }
        public DomainVersion(int majorVersion, int minorVersion) 
            : this(majorVersion.ToString(CultureInfo.InvariantCulture), 
                  minorVersion.ToString(CultureInfo.InvariantCulture)) { }
        public DomainVersion(int majorVersion, int minorVersion, int revisionNumber) 
            : this(majorVersion.ToString(CultureInfo.InvariantCulture), 
                  minorVersion.ToString(CultureInfo.InvariantCulture),
                  revisionNumber.ToString(CultureInfo.InvariantCulture)) { }
        public DomainVersion(int majorVersion, int minorVersion, int revisionNumber, int buildNumber) 
            : this(majorVersion.ToString(CultureInfo.InvariantCulture), 
                  minorVersion.ToString(CultureInfo.InvariantCulture),
                  revisionNumber.ToString(CultureInfo.InvariantCulture),
                  buildNumber.ToString(CultureInfo.InvariantCulture)) { }
        public DomainVersion(int majorVersion, int minorVersion, params string[] detailedVersions) 
            : this(new string[] { majorVersion.ToString(CultureInfo.InvariantCulture), 
                  minorVersion.ToString(CultureInfo.InvariantCulture)}.Concat(detailedVersions)) { }
        public DomainVersion(params string[] parts) : this((IEnumerable<string>)parts) { }

        public DomainVersion(IEnumerable<string> parts)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            _parts = new List<string>();
            _isNumeric = new List<bool>();

            var foundEmpty = false;
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    foundEmpty = true;
                }
                else
                {
                    if (foundEmpty)
                        throw new FormatException("Invalid version format");
                    var p = part.Trim();
                    _parts.Add(p);
                    _isNumeric.Add(Regex.IsMatch(p, @"^\d+$")); 
                }
            }
            Parts = new ReadOnlyCollection<string>(_parts);
        }

        public string MajorVersion => GetPartOr(0, "0");

        public string MinorVersion => GetPartOr(1, "1");

        public string RevisionNumber => GetPartOr(2, string.Empty);

        public string BuildNumber => GetPartOr(3, string.Empty);

        public IReadOnlyList<string> Parts { get; }

        public static DomainVersion Parse(string value)
        {
            DomainVersion version;
            if (!TryParse(value, out version))
                throw new FormatException("Invalid version format");
            return version;
        }

        public static bool TryParse(string value, out DomainVersion version)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var parts = value.Split('.').Select(s => s.Trim());
            version = null;
            if (parts.Any(p => p.IsEmpty())) return false;
            version = new DomainVersion(parts);
            return true;
        }

        private string GetPartOr(int index, string defaultValue)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Parts.Count) return defaultValue;
            return Parts[index];
        }

        public int CompareTo(DomainVersion other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            var minCount = Math.Min(_parts.Count, other._parts.Count);
            int comp = 0;

            for (int i = 0; i < minCount; i++)
            {
                var isNum1 = _isNumeric[i];
                var isNum2 = other._isNumeric[i];
                var part1 = _parts[i];
                var part2 = other._parts[i];
                if (isNum1 && isNum2)
                {
                    comp = part1.Length.CompareTo(part2.Length);
                    if (comp != 0) return comp;
                }
                comp = StringComparer.InvariantCultureIgnoreCase.Compare(part1, part2);
                if (comp != 0) return comp;
            }

            comp = _parts.Count.CompareTo(other._parts.Count);
            return comp;
        }

        public bool Equals(DomainVersion other)
        {
            if (other == null) return false;
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return string.Join(".", _parts);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DomainVersion) obj);
        }

        public override int GetHashCode()
        {
            var result =_parts.Aggregate(0, 
                (hash, part) => { unchecked { return hash*397 ^ part.GetHashCode(); } });
            return result;
        }

        public static bool operator ==(DomainVersion version1, DomainVersion version2)
        {
            if (version1 == null && version2 == null) return true;
            if (version1 == null) return false;
            return version1.Equals(version2);
        }

        public static bool operator !=(DomainVersion version1, DomainVersion version2)
        {
            return !(version1 == version2);
        }

        public static bool operator <(DomainVersion version1, DomainVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException(nameof(version1));
            if (version2 == null) throw new ArgumentNullException(nameof(version2));
            return version1.CompareTo(version2) < 0;
        }

        public static bool operator <=(DomainVersion version1, DomainVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException(nameof(version1));
            if (version2 == null) throw new ArgumentNullException(nameof(version2));
            return version1.CompareTo(version2) <= 0;
        }

        public static bool operator >(DomainVersion version1, DomainVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException(nameof(version1));
            if (version2 == null) throw new ArgumentNullException(nameof(version2));
            return version1.CompareTo(version2) > 0;
        }

        public static bool operator >=(DomainVersion version1, DomainVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException(nameof(version1));
            if (version2 == null) throw new ArgumentNullException(nameof(version2));
            return version1.CompareTo(version2) >= 0;
        }
    }
}

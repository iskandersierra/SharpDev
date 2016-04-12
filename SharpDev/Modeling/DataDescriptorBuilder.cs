using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDev.Modeling
{
    public sealed class DataDescriptorBuilder
    {
        private string _name;
        private string _uniqueId;

        private DataDescriptorBuilder()
        {
        }

        public static DataDescriptorBuilder Create()
        {
            return new DataDescriptorBuilder();
        }

        public DataDescriptorBuilder Name(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            _name = name;
            return this;
        }

        public DataDescriptorBuilder UniqueId(string uniqueId)
        {
            if (uniqueId == null) throw new ArgumentNullException(nameof(uniqueId));
            _uniqueId = uniqueId;
            return this;
        }

        public IDataDescriptor Build()
        {
            return new ExpandoObjectDescriptor(_uniqueId, _name, new DataPropertyDescriptorCollection(new IDataPropertyDescriptor[0]));
        }

        class ExpandoObjectDescriptor : IDataDescriptor
        {
            public ExpandoObjectDescriptor(string uniqueId, string name, DataPropertyDescriptorCollection properties)
            {
                UniqueId = uniqueId;
                Name = name;
                Properties = properties;
            }

            public string UniqueId { get; }
            public string Name { get; }
            public DataPropertyDescriptorCollection Properties { get; }
            public object CreateInstance()
            {
                return new ExpandoObject();
            }
        }
    }
}

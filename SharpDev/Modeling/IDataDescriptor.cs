using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpDev.Modeling
{
    public interface IDataDescriptor
    {
        string UniqueId { get; }

        string Name { get; }

        DataPropertyDescriptorCollection Properties { get; }

        object CreateInstance();
    }

    public class DataPropertyDescriptorCollection : 
        ReadOnlyDictionary<string, IDataPropertyDescriptor>, 
        IReadOnlyCollection<IDataPropertyDescriptor>
    {

        public DataPropertyDescriptorCollection(IEnumerable<IDataPropertyDescriptor> list) : 
            base(list.ToDictionary(p => p.Name))
        {
        }

        public IEnumerator<IDataPropertyDescriptor> GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }

    public interface IDataPropertyDescriptor
    {
        string Name { get; }

        DataPropertyType Type { get; }

        // Single multiplicity
        object GetValue(object instance);
        void SetValue(object instance, object value);

        // Non-single multiplicity
        IEnumerable Enumerate(object instance); 

        // Set multiplicity
        void Add(object instance, object value);
        void Remove(object instance, object value);
        void Clear(object instance);
        int GetCount(object instance);
        bool Contains(object instance, object value);

        // List multiplicity
        void Insert(object instance, int index, object value);
        void RemoveAt(object instance, int index);
        int IndexOf(object instance, object value);
    }

    public class DataPropertyType
    {
        bool IsData { get; }

        DataMultiplicity Multiplicity { get; }

        IDataPropertyDescriptor DataType { get; }

        Type ClrType { get; }
    }

    public enum DataMultiplicity
    {
        Single,
        Set,
        List,
    }
}

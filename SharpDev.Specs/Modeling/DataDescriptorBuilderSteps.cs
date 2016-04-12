using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SharpDev.Modeling
{
    [Binding]
    public sealed class DataDescriptorBuilderSteps
    {
        [Given(@"A new data descriptor builder")]
        public void GivenANewDataDescriptorBuilder()
        {
            ScenarioContext.Current.Set(DataDescriptorBuilder.Create());
        }
        [Given(@"Name ""(.*)"" is given to the data descriptor builder")]
        public void GivenNameIsGivenToTheDataDescriptorBuilder(string name)
        {
            var builder = ScenarioContext.Current.Get<DataDescriptorBuilder>();
            var newBuilder = builder.Name(name);

            newBuilder.Should().NotBeNull("builder.Name(...) should return itself");
        }
        [Given(@"UniqueId ""(.*)"" is given to the data descriptor builder")]
        public void GivenUniqueIdIsGivenToTheDataDescriptorBuilder(string uniqueId)
        {
            var builder = ScenarioContext.Current.Get<DataDescriptorBuilder>();
            var newBuilder = builder.UniqueId(uniqueId);

            newBuilder.Should().NotBeNull("builder.UniqueId(...) should return itself");
        }
        [Given(@"A property named ""(.*)"" of type single string is added")]
        public void GivenAPropertyNamedOfTypeSingleStringIsAdded(string name)
        {
            var builder = ScenarioContext.Current.Get<DataDescriptorBuilder>();
            //var newBuilder = builder.Property(name, );

            //newBuilder.Should().NotBeNull("builder.UniqueId(...) should return itself");
        }

        [When(@"A data descriptor is created")]
        public void WhenADataDescriptorIsCreated()
        {
            var builder = ScenarioContext.Current.Get<DataDescriptorBuilder>();
            var descriptor = builder.Build();

            ScenarioContext.Current.Set(descriptor);
        }
        [When(@"A new instance of data descriptor is created")]
        public void WhenANewInstanceOfDataDescriptorIsCreated()
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();
            var instance = descriptor.CreateInstance();

            ScenarioContext.Current.Set(instance, "instance");
        }

        [Then(@"The data descriptor is not null")]
        public void ThenTheDataDescriptorIsNotNull()
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();

            descriptor.Should().NotBeNull("descriptor cannot be null");
        }
        [Then(@"The data descriptor name is ""(.*)""")]
        public void ThenTheDataDescriptorNameIs(string name)
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();

            descriptor.Name.Should().Be(name);
        }
        [Then(@"The data descriptor unique id is ""(.*)""")]
        public void ThenTheDataDescriptorUniqueIdIs(string uniqueId)
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();

            descriptor.UniqueId.Should().Be(uniqueId);
        }
        [Then(@"The data descriptor properties are not null")]
        public void ThenTheDataDescriptorPropertiesAreNotNull()
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();

            descriptor.Properties.Should().NotBeNull("properties should not be null");
        }
        [Then(@"The data descriptor properties has Count (.*)")]
        public void ThenTheDataDescriptorPropertiesHasCount(int propertiesCount)
        {
            var descriptor = ScenarioContext.Current.Get<IDataDescriptor>();

            descriptor.Properties.Count.Should().Be(propertiesCount, "because thats the amount of expected properties");
        }
        [Then(@"The data descriptor instance is not null")]
        public void ThenTheDataDescriptorInstanceIsNotNull()
        {
            var instance = ScenarioContext.Current.Get<object>("instance");

            instance.Should().NotBeNull("because CreateInstance cannot be null");
        }
        [Then(@"The data descriptor instance is a dictionary")]
        public void ThenTheDataDescriptorInstanceIsADictionary()
        {
            var instance = ScenarioContext.Current.Get<object>("instance");

            instance.Should().BeAssignableTo<IDictionary<string, object>>();
        }
        [Then(@"The data descriptor instance is a dynamic object")]
        public void ThenTheDataDescriptorInstanceIsADynamicObject()
        {
            var instance = ScenarioContext.Current.Get<object>("instance");

            instance.Should().BeAssignableTo<IDynamicMetaObjectProvider>();
        }
    }
}

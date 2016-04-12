Feature: DataDescriptorBuilder
	Allows the description of data types using artificial constructs instead of pre-existing .NET classes

@modeling
Scenario: Create empty data descriptor
	Given A new data descriptor builder
	And Name "EmptyData" is given to the data descriptor builder
	And UniqueId "MyDomain.EmptyData" is given to the data descriptor builder
	When A data descriptor is created
	Then The data descriptor is not null
	And The data descriptor name is "EmptyData"
	And The data descriptor unique id is "MyDomain.EmptyData"
	And The data descriptor properties are not null
	And The data descriptor properties has Count 0

Scenario: Create new instance from empty data descriptor
	Given A new data descriptor builder
	And Name "EmptyData" is given to the data descriptor builder
	And UniqueId "MyDomain.EmptyData" is given to the data descriptor builder
	When A data descriptor is created
	And A new instance of data descriptor is created
	Then The data descriptor instance is not null
	And The data descriptor instance is a dictionary
	And The data descriptor instance is a dynamic object

Scenario: Create data descriptor with one property
	Given A new data descriptor builder
	And Name "SimpleData" is given to the data descriptor builder
	And UniqueId "MyDomain.SimpleData" is given to the data descriptor builder
	And A property named "Name" of type single string is added
	When A data descriptor is created
	Then The data descriptor is not null
	And The data descriptor name is "SimpleData"
	And The data descriptor unique id is "MyDomain.SimpleData"
	And The data descriptor properties are not null
	And The data descriptor properties has Count 1


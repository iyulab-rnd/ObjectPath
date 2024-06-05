# ObjectPath

ObjectPath: A simple and intuitive library for accessing object values using string path expressions in .NET.

## Installation

Install the ObjectPath library via NuGet:

```bash
dotnet add package ObjectPath
```

## Usage

ObjectPath allows you to access values within objects using string path expressions.

### Basic Usage

```csharp
using ObjectPathLibrary;

// Example object
var obj = new 
{
    Name = "John",
    Age = 30,
    Address = new 
    {
        City = "New York",
        Street = "123 Main St"
    }
};

// Access values using path expressions
var name = obj.GetValueByPath("Name"); // John
var city = obj.GetValueByPath("Address.City"); // New York
```

### JSON Example

```csharp
using System.Text.Json;
using ObjectPathLibrary;

// JSON string
var json = @"
{
    ""name"": ""John"",
    ""age"": 30,
    ""address"": {
        ""city"": ""New York"",
        ""street"": ""123 Main St""
    }
}";

var jsonDocument = JsonDocument.Parse(json);
var jsonElement = jsonDocument.RootElement;

// Access values using path expressions
var name = jsonElement.GetValueByPath("name"); // John
var street = jsonElement.GetValueByPath("address.street"); // 123 Main St
```

### Array Example

ObjectPath supports accessing array and list elements using index notation.

```csharp
using ObjectPathLibrary;

// Example object with arrays and lists
var obj = new 
{
    Numbers = new int[] { 1, 2, 3, 4, 5 },
    People = new[]
    {
        new { Name = "John", Age = 30 },
        new { Name = "Jane", Age = 25 }
    }
};

// Access array elements
var firstNumber = obj.GetValueByPath("Numbers[0]"); // 1
var secondPersonName = obj.GetValueByPath("People[1].Name"); // Jane
```

### Case Sensitivity

By default, ObjectPath is case-insensitive. You can enable case sensitivity by setting `ignoreCase` to `false`.

```csharp
var obj = new 
{
    Name = "John",
    age = 30
};

var name = ObjectPath.GetValue(obj, "Name", ignoreCase: false); // John
var age = ObjectPath.GetValue(obj, "age", ignoreCase: false); // 30

// Throws InvalidObjectPathException
var invalid = ObjectPath.GetValue(obj, "name", ignoreCase: false);
```

### Dictionary Access

You can also access values in dictionaries using path expressions.

```csharp
using ObjectPathLibrary;
using Xunit;

[Fact]
public void DictionaryAccessTest()
{
    var dic = new Dictionary<string, object>
    {
        ["Name"] = "John",
        ["Age"] = 30,
        ["Address"] = new Dictionary<string, object>
        {
            ["City"] = "New York",
            ["Street"] = "123 Main St"
        }
    };

    // Act
    var name = dic.GetValueByPath("Name");
    var age = dic.GetValueByPath("Age");
    var city = dic.GetValueByPath("Address.City");
    var street = dic.GetValueByPath("Address.Street");

    var address = dic.GetValueByPath("Address") as IDictionary<string, object>;
    var addressExpando = address!.ToExpando();

    // Assert
    Assert.Equal("John", name);
    Assert.Equal(30, age);
    Assert.Equal("New York", city);
    Assert.Equal("123 Main St", street);

    Assert.Equal("New York", address["City"]);
    Assert.Equal("123 Main St", address["Street"]);

    Assert.Equal("New York", addressExpando?.City);
    Assert.Equal("123 Main St", addressExpando?.Street);
}
```

### Handling Exceptions

Use `GetValueByPathOrNull` to safely get values without throwing exceptions.

```csharp
using ObjectPathLibrary;

var obj = new 
{
    Name = "John",
    Address = new 
    {
        City = "New York"
    }
};

// Access values using path expressions
var name = obj.GetValueByPathOrNull("Name"); // John
var nonExistent = obj.GetValueByPathOrNull("NonExistentProperty"); // null
```

## ObjectPathExtensions.cs

```csharp
namespace ObjectPathLibrary
{
    public static class ObjectPathExtensions
    {
        public static object? GetValueByPath(this object obj, string path)
        {
            return ObjectPath.GetValue(obj, path);
        }

        public static object? GetValueByPathOrNull(this object obj, string path)
        {
            try
            {
                return ObjectPath.GetValue(obj, path);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
```

## Features

- Access nested properties and fields.
- Supports JSON elements.
- Supports array and list index access.
- Case-insensitive by default, with an option for case sensitivity.
- Handles complex nested structures.
- Provides safe access with `GetValueByPathOrNull`.
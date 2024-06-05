```markdown
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
using ObjectPath;

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
var name = ObjectPath.GetValue(obj, "Name"); // John
var city = ObjectPath.GetValue(obj, "Address.City"); // New York
```

### JSON Example

```csharp
using System.Text.Json;
using ObjectPath;

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
var name = ObjectPath.GetValue(jsonElement, "name"); // John
var street = ObjectPath.GetValue(jsonElement, "address.street"); // 123 Main St
```

### Array Example

ObjectPath supports accessing array and list elements using index notation.

```csharp
using ObjectPath;

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
var firstNumber = ObjectPath.GetValue(obj, "Numbers[0]"); // 1
var secondPersonName = ObjectPath.GetValue(obj, "People[1].Name"); // Jane
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

## Features

- Access nested properties and fields.
- Supports JSON elements.
- Supports array and list index access.
- Case-insensitive by default, with an option for case sensitivity.
- Handles complex nested structures.
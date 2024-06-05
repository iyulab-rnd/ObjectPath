using System.Text.Json;
using Xunit;
using ObjectPathLibrary;

namespace ObjectPathLibrary.Tests
{
    public class ObjectPathTests
    {
        [Fact]
        public void GetValue_ReturnsCorrectValue_ForSimpleObject()
        {
            // Arrange
            var obj = new { Name = "John", Age = 30 };

            // Act
            var name = ObjectPath.GetValue(obj, "Name");
            var age = ObjectPath.GetValue(obj, "Age");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(30, age);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForNestedObject()
        {
            // Arrange
            var obj = new
            {
                Name = "John",
                Address = new
                {
                    City = "New York",
                    Street = "123 Main St"
                }
            };

            // Act
            var city = ObjectPath.GetValue(obj, "Address.City");
            var street = ObjectPath.GetValue(obj, "Address.Street");

            // Assert
            Assert.Equal("New York", city);
            Assert.Equal("123 Main St", street);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForListIndex()
        {
            // Arrange
            var obj = new
            {
                Numbers = new List<int> { 1, 2, 3 }
            };

            // Act
            var first = ObjectPath.GetValue(obj, "Numbers[0]");
            var second = ObjectPath.GetValue(obj, "Numbers[1]");

            // Assert
            Assert.Equal(1, first);
            Assert.Equal(2, second);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForDictionaryKey()
        {
            // Arrange
            var obj = new
            {
                Dict = new Dictionary<string, object>
                {
                    ["Name"] = "John",
                    ["Age"] = 30
                }
            };

            // Act
            var name = ObjectPath.GetValue(obj, "Dict.Name");
            var age = ObjectPath.GetValue(obj, "Dict.Age");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(30, age);
        }

        [Fact]
        public void GetValue_ReturnsNull_ForInvalidPath()
        {
            // Arrange
            var obj = new { Name = "John", Age = 30 };

            // Assert
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "InvalidPath"));
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForNestedListIndex()
        {
            // Arrange
            var obj = new
            {
                Matrix = new List<List<int>>
                {
                    new List<int> { 1, 2, 3 },
                    new List<int> { 4, 5, 6 },
                    new List<int> { 7, 8, 9 }
                }
            };

            // Act
            var value1 = ObjectPath.GetValue(obj, "Matrix[0][1]");
            var value2 = ObjectPath.GetValue(obj, "Matrix[1][2]");
            var value3 = ObjectPath.GetValue(obj, "Matrix[2][0]");

            // Assert
            Assert.Equal(2, value1);
            Assert.Equal(6, value2);
            Assert.Equal(7, value3);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForArrayIndex()
        {
            // Arrange
            var obj = new
            {
                Numbers = new int[] { 1, 2, 3, 4, 5 }
            };

            // Act
            var first = ObjectPath.GetValue(obj, "Numbers[0]");
            var last = ObjectPath.GetValue(obj, "Numbers[4]");

            // Assert
            Assert.Equal(1, first);
            Assert.Equal(5, last);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForNestedDictionary()
        {
            // Arrange
            var obj = new
            {
                Dict = new Dictionary<string, object>
                {
                    ["Name"] = "John",
                    ["Address"] = new Dictionary<string, object>
                    {
                        ["City"] = "New York",
                        ["Street"] = "123 Main St"
                    }
                }
            };

            // Act
            var name = ObjectPath.GetValue(obj, "Dict.Name");
            var city = ObjectPath.GetValue(obj, "Dict.Address.City");
            var street = ObjectPath.GetValue(obj, "Dict.Address.Street");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal("New York", city);
            Assert.Equal("123 Main St", street);
        }

        [Fact]
        public void GetValue_ReturnsNull_ForInvalidIndexAccess()
        {
            // Arrange
            var obj = new
            {
                Numbers = new List<int> { 1, 2, 3 }
            };

            // Assert
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "Numbers[3]"));
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "Numbers[-1]"));
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForClassObject()
        {
            // Arrange
            var person = new Person
            {
                Name = "John",
                Age = 30,
                Address = new Address
                {
                    City = "New York",
                    Street = "123 Main St"
                }
            };

            // Act
            var name = ObjectPath.GetValue(person, "Name");
            var age = ObjectPath.GetValue(person, "Age");
            var city = ObjectPath.GetValue(person, "Address.City");
            var street = ObjectPath.GetValue(person, "Address.Street");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(30, age);
            Assert.Equal("New York", city);
            Assert.Equal("123 Main St", street);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForRecordObject()
        {
            // Arrange
            var employee = new Employee("John", 1, new Department("Sales", new Manager("Jane", "jane@example.com")));

            // Act
            var name = ObjectPath.GetValue(employee, "Name");
            var id = ObjectPath.GetValue(employee, "Id");
            var departmentName = ObjectPath.GetValue(employee, "Department.Name");
            var managerName = ObjectPath.GetValue(employee, "Department.Manager.Name");
            var managerEmail = ObjectPath.GetValue(employee, "Department.Manager.Email");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(1, id);
            Assert.Equal("Sales", departmentName);
            Assert.Equal("Jane", managerName);
            Assert.Equal("jane@example.com", managerEmail);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForArrayOfObjects()
        {
            // Arrange
            var people = new Person[]
            {
                new Person
                {
                    Name = "John",
                    Age = 30,
                    Address = new Address
                    {
                        City = "New York",
                        Street = "123 Main St"
                    }
                },
                new Person
                {
                    Name = "Jane",
                    Age = 25,
                    Address = new Address
                    {
                        City = "London",
                        Street = "456 Oxford St"
                    }
                },
                new Person
                {
                    Name = "Alice",
                    Age = 35,
                    Address = new Address
                    {
                        City = "Paris",
                        Street = "789 Champs-Élysées"
                    }
                }
            };

            // Act
            var name1 = ObjectPath.GetValue(people, "[0].Name");
            var age1 = ObjectPath.GetValue(people, "[0].Age");
            var city1 = ObjectPath.GetValue(people, "[0].Address.City");

            var name2 = ObjectPath.GetValue(people, "[1].Name");
            var age2 = ObjectPath.GetValue(people, "[1].Age");
            var street2 = ObjectPath.GetValue(people, "[1].Address.Street");

            var name3 = ObjectPath.GetValue(people, "[2].Name");
            var age3 = ObjectPath.GetValue(people, "[2].Age");
            var city3 = ObjectPath.GetValue(people, "[2].Address.City");
            var street3 = ObjectPath.GetValue(people, "[2].Address.Street");

            // Assert
            Assert.Equal("John", name1);
            Assert.Equal(30, age1);
            Assert.Equal("New York", city1);

            Assert.Equal("Jane", name2);
            Assert.Equal(25, age2);
            Assert.Equal("456 Oxford St", street2);

            Assert.Equal("Alice", name3);
            Assert.Equal(35, age3);
            Assert.Equal("Paris", city3);
            Assert.Equal("789 Champs-Élysées", street3);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForJsonElement()
        {
            // Arrange
            var json = @"
    {
        ""name"": ""John"",
        ""age"": 30,
        ""address"": {
            ""city"": ""New York"",
            ""street"": ""123 Main St""
        },
        ""phoneNumbers"": [
            {
                ""type"": ""home"",
                ""number"": ""212-555-1234""
            },
            {
                ""type"": ""office"",
                ""number"": ""212-555-5678""
            }
        ]
    }";

            var jsonDocument = JsonDocument.Parse(json);
            var jsonElement = jsonDocument.RootElement;

            // Act
            var name = ObjectPath.GetValue(jsonElement, "name");
            var age = ObjectPath.GetValue(jsonElement, "age");
            var city = ObjectPath.GetValue(jsonElement, "address.city");
            var street = ObjectPath.GetValue(jsonElement, "address.street");
            var phoneType1 = ObjectPath.GetValue(jsonElement, "phoneNumbers[0].type");
            var phoneNumber1 = ObjectPath.GetValue(jsonElement, "phoneNumbers[0].number");
            var phoneType2 = ObjectPath.GetValue(jsonElement, "phoneNumbers[1].type");
            var phoneNumber2 = ObjectPath.GetValue(jsonElement, "phoneNumbers[1].number");

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(30m, age);
            Assert.Equal("New York", city);
            Assert.Equal("123 Main St", street);
            Assert.Equal("home", phoneType1);
            Assert.Equal("212-555-1234", phoneNumber1);
            Assert.Equal("office", phoneType2);
            Assert.Equal("212-555-5678", phoneNumber2);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForDeeplyNestedObject()
        {
            // Arrange
            var obj = new
            {
                Level1 = new
                {
                    Level2 = new
                    {
                        Level3 = new
                        {
                            Level4 = new
                            {
                                Value = "Deep Nested Value"
                            }
                        }
                    }
                }
            };

            // Act
            var value = ObjectPath.GetValue(obj, "Level1.Level2.Level3.Level4.Value");

            // Assert
            Assert.Equal("Deep Nested Value", value);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForDifferentDataTypes()
        {
            // Arrange
            var obj = new
            {
                BoolValue = true,
                DateValue = new DateTime(2023, 1, 1),
                EnumValue = DayOfWeek.Monday
            };

            // Act
            var boolValue = ObjectPath.GetValue(obj, "BoolValue");
            var dateValue = ObjectPath.GetValue(obj, "DateValue");
            var enumValue = ObjectPath.GetValue(obj, "EnumValue");

            // Assert
            Assert.True((bool)boolValue);
            Assert.Equal(new DateTime(2023, 1, 1), (DateTime)dateValue);
            Assert.Equal(DayOfWeek.Monday, (DayOfWeek)enumValue);
        }

        [Fact]
        public void GetValue_IsCaseSensitive_WhenIgnoreCaseIsFalse()
        {
            // Arrange
            var obj = new
            {
                Name = "John",
                age = 30
            };

            // Act
            var name = ObjectPath.GetValue(obj, "Name", ignoreCase: false);
            var age = ObjectPath.GetValue(obj, "age", ignoreCase: false);

            // Assert
            Assert.Equal("John", name);
            Assert.Equal(30, age);
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "name", ignoreCase: false));
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "Age", ignoreCase: false));
        }

        [Fact]
        public void GetValue_IsCaseInsensitive_WhenIgnoreCaseIsTrue()
        {
            // Arrange
            var obj = new
            {
                Name = "John",
                age = 30
            };

            // Act
            var name1 = ObjectPath.GetValue(obj, "Name", ignoreCase: true);
            var name2 = ObjectPath.GetValue(obj, "name", ignoreCase: true);
            var age1 = ObjectPath.GetValue(obj, "age", ignoreCase: true);
            var age2 = ObjectPath.GetValue(obj, "Age", ignoreCase: true);

            // Assert
            Assert.Equal("John", name1);
            Assert.Equal("John", name2);
            Assert.Equal(30, age1);
            Assert.Equal(30, age2);
        }

        [Fact]
        public void GetValue_ReturnsNull_ForNullProperty()
        {
            // Arrange
            var obj = new
            {
                Name = "John",
                Address = (string?)null
            };

            // Act
            var address = ObjectPath.GetValue(obj, "Address");

            // Assert
            Assert.Null(address);
        }

        [Fact]
        public void GetValue_ReturnsCorrectValue_ForSpecialCharactersInPath()
        {
            // Arrange
            var obj = new
            {
                Special = new
                {
                    PropertyName = "Value with special characters"
                }
            };

            // Act
            var value = ObjectPath.GetValue(obj, "Special.PropertyName");

            // Assert
            Assert.Equal("Value with special characters", value);
        }

        [Fact]
        public void GetValue_ThrowsException_ForInvalidPath()
        {
            // Arrange
            var obj = new { Name = "John", Age = 30 };

            // Act & Assert
            Assert.Throws<InvalidObjectPathException>(() => ObjectPath.GetValue(obj, "InvalidPath"));
        }

        // 추가 테스트: 큰 데이터 구조 테스트
        [Fact]
        public void GetValue_ReturnsCorrectValue_ForLargeDataStructure()
        {
            // Arrange
            var largeObj = new Dictionary<string, object>();
            for (int i = 0; i < 1000; i++)
            {
                largeObj[$"key{i}"] = $"value{i}";
            }

            // Act & Assert
            for (int i = 0; i < 1000; i++)
            {
                var value = ObjectPath.GetValue(largeObj, $"key{i}");
                Assert.Equal($"value{i}", value);
            }
        }

        // 추가 테스트: 다른 데이터 타입 테스트
        [Fact]
        public void GetValue_ReturnsCorrectValue_ForVariousDataTypes()
        {
            // Arrange
            var obj = new
            {
                ByteValue = (byte)1,
                ShortValue = (short)2,
                LongValue = (long)3,
                FloatValue = (float)4.5,
                DoubleValue = (double)6.7,
                DecimalValue = (decimal)8.9,
                CharValue = 'A',
                StringValue = "Hello",
                BoolValue = true
            };

            // Act
            var byteValue = ObjectPath.GetValue(obj, "ByteValue");
            var shortValue = ObjectPath.GetValue(obj, "ShortValue");
            var longValue = ObjectPath.GetValue(obj, "LongValue");
            var floatValue = ObjectPath.GetValue(obj, "FloatValue");
            var doubleValue = ObjectPath.GetValue(obj, "DoubleValue");
            var decimalValue = ObjectPath.GetValue(obj, "DecimalValue");
            var charValue = ObjectPath.GetValue(obj, "CharValue");
            var stringValue = ObjectPath.GetValue(obj, "StringValue");
            var boolValue = ObjectPath.GetValue(obj, "BoolValue");

            // Assert
            Assert.Equal((byte)1, byteValue);
            Assert.Equal((short)2, shortValue);
            Assert.Equal((long)3, longValue);
            Assert.Equal((float)4.5, floatValue);
            Assert.Equal((double)6.7, doubleValue);
            Assert.Equal((decimal)8.9, decimalValue);
            Assert.Equal('A', charValue);
            Assert.Equal("Hello", stringValue);
            Assert.True((bool)boolValue);
        }
    }
}

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace ObjectPathLibrary
{
    public static class ObjectPath
    {
        private static readonly char[] Separator = new[] { '.', '[', ']' };

        private static readonly ConcurrentDictionary<(Type, string, bool), PropertyInfo?> PropertyCache = new();
        private static readonly ConcurrentDictionary<(Type, string, bool), FieldInfo?> FieldCache = new();

        public static object? GetValue(object? obj, string path, bool ignoreCase = true)
        {
            if (obj == null) return null;

            var segments = path.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;

            while (obj != null && index < segments.Length)
            {
                var currentSegment = segments[index];

                if (obj is JsonElement jsonElement)
                {
                    obj = HandleJsonElement(jsonElement, currentSegment, segments, ref index, ignoreCase);
                }
                else if (int.TryParse(currentSegment, out var arrayIndex))
                {
                    obj = HandleArrayIndex(obj, arrayIndex);
                }
                else
                {
                    obj = HandleObjectProperty(obj, currentSegment, ignoreCase);
                }

                index++;
            }

            return obj;
        }

        private static object? HandleJsonElement(JsonElement jsonElement, string currentSegment, string[] segments, ref int index, bool ignoreCase)
        {
            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                if (jsonElement.TryGetProperty(currentSegment, out var jsonProperty) ||
                    (ignoreCase && TryGetPropertyIgnoreCase(jsonElement, currentSegment, out jsonProperty)))
                {
                    return GetJsonElementValue(jsonProperty);
                }
                else
                {
                    throw new InvalidObjectPathException($"Property '{currentSegment}' not found.");
                }
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array && int.TryParse(currentSegment, out var jsonArrayIndex) &&
                     jsonArrayIndex >= 0 && jsonArrayIndex < jsonElement.GetArrayLength())
            {
                return GetJsonElementValue(jsonElement[jsonArrayIndex]);
            }
            else if (index == segments.Length - 1)
            {
                return GetJsonElementValue(jsonElement);
            }
            else
            {
                throw new InvalidObjectPathException($"Invalid array index '{currentSegment}'.");
            }
        }

        private static object? HandleArrayIndex(object obj, int arrayIndex)
        {
            if (obj is IList list && arrayIndex >= 0 && arrayIndex < list.Count)
            {
                return list[arrayIndex];
            }
            else if (obj is Array array && arrayIndex >= 0 && arrayIndex < array.Length)
            {
                return array.GetValue(arrayIndex);
            }
            else
            {
                throw new InvalidObjectPathException($"Invalid array index '{arrayIndex}'.");
            }
        }

        private static object? HandleObjectProperty(object obj, string currentSegment, bool ignoreCase)
        {
            if (obj is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue(currentSegment, out var dictValue) ||
                    (ignoreCase && TryGetValueIgnoreCase(dict, currentSegment, out dictValue)))
                {
                    return dictValue;
                }
                else
                {
                    throw new InvalidObjectPathException($"Property '{currentSegment}' not found.");
                }
            }
            else
            {
                var objType = obj.GetType();
                var propertyInfo = GetCachedPropertyInfo(objType, currentSegment, ignoreCase);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(obj);
                }

                var fieldInfo = GetCachedFieldInfo(objType, currentSegment, ignoreCase);
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(obj);
                }

                throw new InvalidObjectPathException($"Property or field '{currentSegment}' not found.");
            }
        }

        private static PropertyInfo? GetCachedPropertyInfo(Type type, string propertyName, bool ignoreCase)
        {
            var key = (type, propertyName, ignoreCase);
            if (!PropertyCache.TryGetValue(key, out var propertyInfo))
            {
                var flags = ignoreCase
                    ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
                    : BindingFlags.Public | BindingFlags.Instance;
                propertyInfo = type.GetProperty(propertyName, flags);
                PropertyCache[key] = propertyInfo;
            }
            return propertyInfo;
        }

        private static FieldInfo? GetCachedFieldInfo(Type type, string fieldName, bool ignoreCase)
        {
            var key = (type, fieldName, ignoreCase);
            if (!FieldCache.TryGetValue(key, out var fieldInfo))
            {
                var flags = ignoreCase
                    ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
                    : BindingFlags.Public | BindingFlags.Instance;
                fieldInfo = type.GetField(fieldName, flags);
                FieldCache[key] = fieldInfo;
            }
            return fieldInfo;
        }

        private static bool TryGetPropertyIgnoreCase(JsonElement jsonElement, string propertyName, out JsonElement jsonProperty)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    jsonProperty = property.Value;
                    return true;
                }
            }

            jsonProperty = default;
            return false;
        }

        private static bool TryGetValueIgnoreCase<TValue>(IDictionary<string, TValue> dict, string key, out TValue? value)
        {
            foreach (var kvp in dict)
            {
                if (string.Equals(kvp.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    value = kvp.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        private static object? GetJsonElementValue(JsonElement jsonElement)
        {
            return jsonElement.ValueKind switch
            {
                JsonValueKind.String => jsonElement.GetString(),
                JsonValueKind.Number => jsonElement.GetDecimal(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => jsonElement
            };
        }
    }
}
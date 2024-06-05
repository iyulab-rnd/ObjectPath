using System.Dynamic;

namespace ObjectPathLibrary
{
    public static class DictionaryExtensions
    {
        public static dynamic ToExpando(this IDictionary<string, object?> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object?>)expando!;

            foreach (var kvp in dictionary)
            {
                if (kvp.Value is IDictionary<string, object?> dict)
                {
                    expandoDict.Add(kvp.Key, dict.ToExpando());
                }
                else
                {
                    expandoDict.Add(kvp.Key, kvp.Value);
                }
            }

            return expando;
        }
    }
}
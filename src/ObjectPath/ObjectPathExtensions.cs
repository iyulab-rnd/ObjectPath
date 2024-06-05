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

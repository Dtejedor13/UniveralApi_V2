namespace UniversalAPI.Extensions
{
    public static class GenericsExtension
    {
        public static bool IsGenericEnumerable(this object t)
        {
            return t.GetType().GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}

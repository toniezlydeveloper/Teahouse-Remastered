using System.Reflection;

namespace Utilities
{
    public static class ReflectionExtensionMethods
    {
        public static TValue GetComponent<TValue, TComponent>(this string valueName, TComponent component)
        {
            FieldInfo fieldInfo = typeof(TComponent).GetField(valueName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (TValue)fieldInfo!.GetValue(component);
        }

        public static void CallMethod<TComponent>(this string methodName, TComponent value, params object[] parameters)
        {
            MethodInfo methodInfo = typeof(TComponent).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            methodInfo!.Invoke(value, parameters);
        }
    }
}
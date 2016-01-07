using System;
using System.Reflection;

namespace com.ootii.Helpers
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Grabs an attribute from the class type and returns it
        /// </summary>
        /// <param name="rObjectType">Object type who has the attribute value</param>
        public static T GetAttribute<T>(Type rObjectType)
        {
            object[] lAttributes = rObjectType.GetCustomAttributes(typeof(T), true);
            if (lAttributes == null || lAttributes.Length == 0) { return default(T); }

            return (T)lAttributes[0];           
        }

        /// <summary>
        /// Sets the property value if the property exists
        /// </summary>
        public static void SetProperty(object rObject, string rName, object rValue)
        {
            Type lType = rObject.GetType();
            PropertyInfo[] lProperties = lType.GetProperties();
            if (lProperties != null && lProperties.Length > 0)
            {
                for (int i = 0; i < lProperties.Length; i++)
                {
                    if (lProperties[i].Name == rName && lProperties[i].CanWrite)
                    {
                        lProperties[i].SetValue(rObject, rValue, null);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the specified type exists
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsTypeValid(string rType)
        {
            try
            {
                Type lType = Type.GetType(rType);
                return (lType != null);
            }
            catch
            {
                return false;
            }
        }
    }
}


namespace Pdf2Svg
{


    public static class Helpers
    {


        public static string CombinePaths(string first, params string[] others)
        {
            // Put error checking in here :)
            string path = first;
            foreach (string section in others)
            {
                path = System.IO.Path.Combine(path, section);
            }
            return path;
        } // End Function CombinePaths in .NET 2.0 


        private static void CheckIsEnum<T>(bool withFlags)
        {
            if (!typeof(T).IsEnum)
                throw new System.ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));

            if (withFlags && !System.Attribute.IsDefined(typeof(T), typeof(System.FlagsAttribute)))
                throw new System.ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }


        public static bool IsFlagSet<T>(this T value, T flag) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = System.Convert.ToInt64(value);
            long lFlag = System.Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }


        public static System.Collections.Generic.IEnumerable<T> GetFlags<T>(this T value) where T : struct
        {
            CheckIsEnum<T>(true);

            System.Array allValues = System.Enum.GetValues(typeof(T));
            for (int i = 0; i < allValues.Length; ++i)
            {
                T flag = (T)allValues.GetValue(i);
                if (value.IsFlagSet(flag))
                    yield return flag;
            }

        }


        public static string ListFlags<T>(this T value) where T : struct
        {
            string retValue = "";
            System.Collections.Generic.IEnumerable<T> foo = GetFlags<T>(value);
            
            foreach (T thisFlag in foo)
            {
                retValue += thisFlag.ToString() + "\r\n";
            }

            return retValue;
        }


        public static T SetFlags<T>(this T value, T flags, bool on) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = System.Convert.ToInt64(value);
            long lFlag = System.Convert.ToInt64(flags);
            if (on)
            {
                lValue |= lFlag;
            }
            else
            {
                lValue &= (~lFlag);
            }
            return (T)System.Enum.ToObject(typeof(T), lValue);
        }


        public static T SetFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, true);
        }


        public static T ClearFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, false);
        }


        public static T CombineFlags<T>(this System.Collections.Generic.IEnumerable<T> flags) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = 0;
            foreach (T flag in flags)
            {
                long lFlag = System.Convert.ToInt64(flag);
                lValue |= lFlag;
            }
            return (T)System.Enum.ToObject(typeof(T), lValue);
        }


        public static string GetDescription<T>(this T value) where T : struct
        {
            CheckIsEnum<T>(false);
            string name = System.Enum.GetName(typeof(T), value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = typeof(T).GetField(name);
                if (field != null)
                {
                    System.ComponentModel.DescriptionAttribute attr = 
                        System.Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute))
                        as System.ComponentModel.DescriptionAttribute
                    ;

                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }


    } // End Class Helpers


} // End Namespace Pdf2Svg

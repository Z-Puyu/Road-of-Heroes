using System;
<<<<<<< HEAD
using Game.util.errors;

namespace Game.util.enums {
    public static class EnumUtil {
        public static E As<E>(this Enum value) where E : Enum {
            if (Enum.IsDefined(typeof(E), value)) {
                return (E)value;
            }
            throw new EnumConversionException(value, typeof(E));
=======
using System.ComponentModel;

namespace Game.util.enums {
    public static class EnumUtil {
        public static E As<E>(this Enum e) where E : Enum {
            if (Enum.IsDefined(typeof(E), e)) {
                return (E)e;
            }
            return default;
        }

        public static string ToText(this Enum e) {
            object[] descriptions = e.GetType()
                                     .GetField(e.ToString())
                                     .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptions.Length == 0
                ? e.ToString()
                : ((DescriptionAttribute)descriptions[0]).Description;
>>>>>>> e50a7f5edd12946b0af396b056629f5c7b368333
        }
    }
}
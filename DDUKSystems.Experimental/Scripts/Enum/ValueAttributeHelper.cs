using System; // Enum


namespace DDUKSystems
{
    public static class ValueAttributeHelper
    {
        /// <summary>
        /// Enum Value로부터 위에 선언한 ValueAttribute의 Value를 추출해 반환한다.
        /// </summary>
        public static string ExtractValueFromEnum(Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(ValueAttribute), false) as ValueAttribute[];
            if (attributes.Length == 0)
                return string.Empty;

            return attributes[0].Value;
        }
    }
}
using System; // Attribute


namespace DDUKSystems
{
    /// <summary>
    /// 열거체값과 문자열값을 연결.
    /// </summary>
    public class ValueAttribute : Attribute
    {
        private string m_Value;
        public string Value => m_Value;

        public ValueAttribute(string value)
        {
            m_Value = value;
        }
    }
}
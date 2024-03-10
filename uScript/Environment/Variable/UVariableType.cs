namespace uScript
{
	/// <summary>
	/// 변수 타입.
	/// JSON과 동일한 구조로 사용하여 일관적인 호환성을 유지.
	/// </summary>
	public enum UVariableType
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 비교값.
		/// </summary>
		Boolean,

		/// <summary>
		/// 숫자값.
		/// </summary>
		Number,

		/// <summary>
		/// 문자열값.
		/// </summary>
		String,

		/// <summary>
		/// 오브젝트값.
		/// </summary>
		Object,

		/// <summary>
		/// 배열값.
		/// </summary>
		Array,
	}
}
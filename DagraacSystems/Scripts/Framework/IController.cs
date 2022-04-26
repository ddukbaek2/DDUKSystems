namespace DagraacSystems
{
	/// <summary>
	/// 실제 컨텐츠레벨에서 연결될 대상 객체.
	/// </summary>
	public interface IController
	{
		void Push();
		void Pop();
		void SetModel(Model model);
		void UnsetModel(Model model);
		void UpdateModel();
		void Tick(float deltaTime);
	}
}
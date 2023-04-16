namespace DagraacSystems
{
	/// <summary>
	/// 실제 컨텐츠에서 모델과 연동되는 대상 객체.
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// 모델 로드.
		/// 모델에서 사용해야하는 리소스 역시 이 타이밍에 로드해야함.
		/// </summary>
		void LoadModel(Model model);
		void LoadModelComplete();

		/// <summary>
		/// 모델 언로드.
		/// 모델에서 사용중인 리소스 역시 이 타이밍에 언로드해야함.
		/// </summary>
		void UnlodModel();
		void UnloadModelComplete();

		/// <summary>
		/// 모델의 내부 데이터가 변경됨.
		/// </summary>
		void ModifyModel(Model modifiedModel);

		/// <summary>
		/// 주기적으로 호출됨.
		/// </summary>
		void UpdateModel(float deltaTime);

		//void Connection();
	}
}
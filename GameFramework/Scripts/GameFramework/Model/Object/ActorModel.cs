using DagraacSystems;


namespace GameFramework
{
	/// <summary>
	/// 액터 모델.
	/// </summary>
	public class ActorModel : FModel
	{
		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnActive()
		{
			base.OnActive();
		}

		protected override void OnDeactive()
		{
			base.OnDeactive();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
		}

		protected virtual void OnSpawn()
		{
		}

		protected virtual void OnAppear()
		{
		}

		protected virtual void OnDeath()
		{
		}

		protected virtual void OnDespawn()
		{
		}

	
		protected void OnAttack(ActorModel defensive)
		{
		}

		protected void OnHitAttack(ActorModel offensive)
		{
		}

		protected void OnSkill(ActorModel defensive, SkillModel skill)
		{
		}

		protected void OnHitSkill(ActorModel offensive, SkillModel skill)
		{
		}

		protected void OnItem()
		{
		}

		protected void OnHitItem()
		{
		}
	}
}
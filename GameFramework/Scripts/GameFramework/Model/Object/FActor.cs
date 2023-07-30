using DagraacSystems;


namespace DagraacSystems.Game
{
	/// <summary>
	/// 액터 모델.
	/// </summary>
	public class FActor : FModel
	{
		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		protected override void OnActive()
		{
			base.OnActive();
		}

		protected override void OnDeactive()
		{
			base.OnDeactive();
		}

		protected override void OnTick(float _tick)
		{
			base.OnTick(_tick);
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

	
		protected void OnAttack(FActor defensive)
		{
		}

		protected void OnHitAttack(FActor offensive)
		{
		}

		protected void OnSkill(FActor defensive, FSkill skill)
		{
		}

		protected void OnHitSkill(FActor offensive, FSkill skill)
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
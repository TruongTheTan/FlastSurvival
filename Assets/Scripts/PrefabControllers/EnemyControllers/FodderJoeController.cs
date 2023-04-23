namespace Assets.Scripts.DesignPatterns.StrategyPattern
{
	public class FodderJoeController : Enemy
	{


		protected override void Awake()
		{
			base._currentEnemy = gameObject;
			_health = 100;
			_speedAmount = 2;
			base.EnemyDamage = _damage = 20;
			_point = 10;
			base.Awake();
		}
	}
}

using Assets.Scripts.DesignPatterns.StrategyPattern;

namespace Assets.Scripts.PrefabControllers.EnemyControllers
{
	public class ExplosiveDaveController : Enemy
	{
		protected override void Awake()
		{
			base._currentEnemy = gameObject;
			_health = 50;
			_speedAmount = 3;
			EnemyDamage = _damage = 100;
			_point = 5;
			base.Awake();
		}
	}
}

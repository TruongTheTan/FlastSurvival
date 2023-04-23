using Assets.Scripts.DesignPatterns.StrategyPattern;

public class BigDaddyController : Enemy
{


	protected override void Awake()
	{
		base._currentEnemy = gameObject;
		_health = 200;
		_speedAmount = 1f;
		EnemyDamage = _damage = 50;
		_point = 30;
		base.Awake();
	}
}

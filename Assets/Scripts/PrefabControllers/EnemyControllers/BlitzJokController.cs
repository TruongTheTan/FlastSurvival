using Assets.Scripts.DesignPatterns.StrategyPattern;

public class BlitzJokController : Enemy
{


	protected override void Awake()
	{
		base._currentEnemy = gameObject;
		_health = 75;
		_speedAmount = 2.5f;
		EnemyDamage = _damage = 20;
		_point = 20;
		base.Awake();
	}




}

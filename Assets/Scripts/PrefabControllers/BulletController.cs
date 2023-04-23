using Assets.Scripts.DesignPatterns.StrategyPattern;
using Assets.Scripts.PrefabControllers.EnemyControllers;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	private static int _damage;
	private Timer _bulletTimer;
	private bool _isBounceable;
	private int _bounceTimes;

	// Start is called before the first frame update
	void Start()
	{
		_bulletTimer = gameObject.AddComponent<Timer>();
		InstantiateBulletProperties();
	}



	// Update is called once per frame
	void Update()
	{
		if (_bulletTimer.Finished)
		{
			Destroy(gameObject);
		}
	}



	private void OnCollisionEnter2D(Collision2D collision)
	{
		DamageTheEnemy(collision);
	}



	private void DamageTheEnemy(Collision2D collision)
	{


		GameObject enemy = collision.gameObject;



		if (enemy.CompareTag("Enemy"))
		{
			switch (enemy.GetComponent<SpriteRenderer>().sprite.name.Trim())
			{
				case DataPreserve.FODDER_JOE_SPRITE_NAME:
					enemy.GetComponent<FodderJoeController>().ReceiveDamage(_damage);
					break;


				case DataPreserve.BIG_DADDY_SPRITE_NAME:
					enemy.GetComponent<BigDaddyController>().ReceiveDamage(_damage);
					break;


				case DataPreserve.BLITZ_JOK_SPRITE_NAME:

					break;


				case DataPreserve.EXPLOSIVE_DAVE_SPRITE_NAME:
					enemy.GetComponent<ExplosiveDaveController>().ReceiveDamage(_damage);
					break;
			}

		}
		Destroy(gameObject);


		/*
		GameObject enemy = collision.gameObject;


		if (enemy.CompareTag("Enemy"))
		{

			if (enemy.name.Equals("CloseCombatEnemy(Clone)"))
			{
				enemy.GetComponent<CloseCombatEnemyController>().ReceiveDamage(_damage);
			}
			else if (enemy.name.Equals("RangedEnemy(Clone)"))
			{
				enemy.GetComponent<RangedEnemyController>().ReceiveDamage(_damage);
			}

			Destroy(gameObject);
		}


		if (!_isBounceable || _bounceTimes <= 0)
		{
		}
		else
		{
			_bounceTimes--;
		}*/
	}



	private void InstantiateBulletProperties()
	{
		switch (gameObject.tag)
		{
			case DataPreserve.PISTOL_TAG:

				if (DataPreserve.gunLevel == 0)
					SetDamage(20);
				_bulletTimer.Duration = 3;

				break;

			case DataPreserve.ASSAULT_RIFLE_TAG:

				if (DataPreserve.gunLevel == 0)
					SetDamage(40);
				_bulletTimer.Duration = 3;

				break;

			case DataPreserve.SHOTGUN_TAG:

				if (DataPreserve.gunLevel == 0)
					SetDamage(70);
				_bulletTimer.Duration = 1;

				break;
		}
		_bulletTimer.Run();
		GetComponent<Rigidbody2D>().velocity = transform.right * 25f;
	}


	public static void SetDamage(int damage)
	{
		_damage = damage;
	}

	public void SetBounceable(bool bounce, int bounceTimes)
	{
		_isBounceable = bounce;
		_bounceTimes = bounceTimes;
	}
}
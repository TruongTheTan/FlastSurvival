using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, ICombatable
{
	#region Game Objects reference

	[SerializeField]
	private GameObject _bullet;
	private GameObject _player;
	private GameObject _healthBars;
	private GameObject _healthBarPrefab;
	private GameObject _currentHealthBar;
	private GameObject _expBarController;

	#endregion



	#region Enemy stats

	private int _point = 0;
	private bool _isRanged;
	private int _damage = 0;
	private float _speed = 0;
	private float _health = 100f;

	#endregion




	private float _timer;
	private float _distance;
	private Vector3 _directionToPlayer;
	private bool _isCollidingPlayer = false;
	private HealthBarController _healthBarController;



	private const string FODDER_JOE_SPRITE_NAME = "Fodder Joe";
	private const string BLITZ_JOK_SPRITE_NAME = "Blitz Jok";
	private const string BIG_DADDY_SPRITE_NAME = "Big Daddy";
	private const string EXPLOSIVE_DAVE_SPRITE_NAME = "Explosive Dave";



	// Start is called before the first frame update
	private void Awake()
	{
		_isRanged = false;
		_player = GameObject.FindGameObjectWithTag("Player");
		_expBarController = GameObject.Find("ExpBar");
	}

	void Start()
	{
		InstantiateData();
		if (_isRanged) { StartCoroutine(nameof(RangedAttack)); }
	}

	private void Update()
	{
		DestroyWhenTooFarFromPlayer();
		_distance = Vector2.Distance(_player.transform.position, gameObject.transform.position);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		Vector3 direction = (_player.transform.position - transform.position).normalized;
		gameObject.transform.Translate(_speed * Time.deltaTime * direction, Space.Self);
		_timer += 1;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject == _player)
		{
			_isCollidingPlayer = true;
			StartCoroutine(Melee());
			ExplodeWhenCollideToPlayer();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == _player)
		{
			_isCollidingPlayer = false;
		}
	}



	IEnumerator RangedAttack()
	{
		while (_isRanged && _distance <= 10)
		{
			_directionToPlayer = (_player.transform.position - gameObject.transform.position).normalized;

			float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;

			GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.Euler(transform.position.x, transform.position.y, angle));

			bullet.GetComponent<Rigidbody2D>().velocity = _directionToPlayer * 5f;

			yield return new WaitForSeconds(1.5f);
		}
	}






	private void InstantiateData()
	{
		//IEnemyFactory enemyFactory = null;
		string enemySpriteName = string.Empty;

		Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

		switch (currentSprite.name)
		{
			case FODDER_JOE_SPRITE_NAME:
				enemySpriteName = FODDER_JOE_SPRITE_NAME;
				//enemyFactory = new CloseCombatEnemyFactory();
				_health = 100f;
				_speed = 2;
				_damage = 20;
				_point = 10;
				break;

			case BLITZ_JOK_SPRITE_NAME:
				enemySpriteName = BLITZ_JOK_SPRITE_NAME;

				_health = 75f;
				_speed = 2.5f;
				_damage = 20;
				_point = 20;
				_isRanged = true;
				//enemyFactory = new RangedEnemyFactory();
				break;

			case BIG_DADDY_SPRITE_NAME:
				enemySpriteName = BIG_DADDY_SPRITE_NAME;

				//enemyFactory = new CloseCombatEnemyFactory();
				_health = 200f;
				_speed = 1f;
				_damage = 50;
				_point = 30;
				break;

			case EXPLOSIVE_DAVE_SPRITE_NAME:
				enemySpriteName = EXPLOSIVE_DAVE_SPRITE_NAME;

				//enemyFactory = new CloseCombatEnemyFactory();
				_health = 50f;
				_speed = 3;
				_damage = 100;
				_point = 5;
				break;

			default: break;
		}


		_healthBars = GameObject.Find("EnemyHealthBar");

		GameObject healthBar = Instantiate(_healthBarPrefab, _healthBars.transform);

		_currentHealthBar = healthBar;
		_healthBarController = healthBar.GetComponent<HealthBarController>();
		_healthBarController.SetData(gameObject, _health);

		if (_isRanged)
		{
			_bullet = (GameObject)Resources.Load("Prefabs/Bullet/Bullet_9");
		}

		DataPreserve.totalEnemiesOnMap++;
	}





	public IEnumerator Melee()
	{
		// Attack player every second
		while (_isCollidingPlayer)
		{
			_player.GetComponent<PlayableCharacterController>().ReceiveDamaged(_damage);
			yield return new WaitForSeconds(1f);
		}
	}


	public void ReceiveDamaged(int damage)
	{
		if (_health > damage)
		{
			_health -= damage;
			_healthBarController.OnHealthChanged(_health);
		}
		else
		{
			DropGunOrItem();

			DataPreserve.totalEnemiesOnMap--;
			DataPreserve.enemyKilled++;
			DataPreserve.totalScore += (_point * DataPreserve.enemyKilled) + (DataPreserve.gameRound * 100);

			// Add XP to player
			_expBarController.GetComponent<ExpBarController>().OnExpChanged(_point);
			_player.GetComponent<PlayableCharacterController>().UpgradePlayerLevel();

			Destroy(_currentHealthBar);
			Destroy(gameObject);
		}
	}

	private void DropGunOrItem()
	{
		if (Random.Range(1, 10) == 5)
		{
			// Drop items
			if (Random.Range(0, 1f) > 0.5f)
				GetComponent<LootBag>().DropSupportItem(transform.position);

			// Drop guns
			else
				GetComponent<LootBagWeapon>().DropGun(transform.position);
		}
	}

	private void DestroyWhenTooFarFromPlayer()
	{
		if (Vector3.Distance(transform.position, _player.transform.position) >= 25f)
		{
			Destroy(_currentHealthBar);
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// <para>
	/// Function use for Explosive Dave. Create an explosion animation when collide to player.
	/// </para>
	///
	/// <para>
	/// Destroy Enemy instantly when collide, damage the player
	/// </para>
	/// </summary>
	private void ExplodeWhenCollideToPlayer()
	{
		// Explode when collide to player (for Dave)
		if (gameObject.GetComponent<SpriteRenderer>().sprite.name.Equals("Explosive Dave"))
		{
			DataPreserve.totalEnemiesOnMap--;
			GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion/Explosion");

			Instantiate(explosionPrefab, transform.position, Quaternion.identity);

			Destroy(_currentHealthBar);
			Destroy(gameObject);
		}
	}
}
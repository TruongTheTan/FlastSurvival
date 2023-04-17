using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public abstract class AbstractEnemy : MonoBehaviour
	{

		#region Enemy's properties
		protected int _point;
		protected int _damage;
		protected int _health;
		protected float _speedAmount;
		#endregion


		#region Game Objects reference
		protected GameObject _player;
		protected GameObject _healthBars;
		protected GameObject _currentEnemy;
		protected GameObject _enemyHealthBar;
		#endregion



		protected ExpBarController _expBarController;
		protected HealthBarController _healthBarController;


		public abstract IEnumerator AttackPlayer();



		protected virtual void Start()
		{
			this.InstantiateData();
		}


		protected virtual void Update()
		{
			this.MoveToPlayer();
			this.DestroyWhenTooFarFromPlayer();
		}



		protected void InstantiateData()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_expBarController = GameObject.Find("ExpBar").GetComponent<ExpBarController>();

			Sprite currentSprite = _currentEnemy.GetComponent<SpriteRenderer>().sprite;

			switch (currentSprite.name)
			{
				case DataPreserve.FODDER_JOE_SPRITE_NAME:
					_health = 100;
					_speedAmount = 2;
					_damage = 20;
					_point = 10;
					break;

				case DataPreserve.BLITZ_JOK_SPRITE_NAME:
					_health = 75;
					_speedAmount = 2.5f;
					_damage = 20;
					_point = 20;
					break;

				case DataPreserve.BIG_DADDY_SPRITE_NAME:
					_health = 200;
					_speedAmount = 1f;
					_damage = 50;
					_point = 30;
					break;

				case DataPreserve.EXPLOSIVE_DAVE_SPRITE_NAME:
					_health = 50;
					_speedAmount = 3;
					_damage = 100;
					_point = 5;
					break;

				default: break;
			}


			_healthBars = GameObject.Find("EnemyHealthBar");
			_enemyHealthBar = Instantiate(Resources.Load<GameObject>("Prefabs/HealthBar/HealthBar"), _healthBars.transform);

			_healthBarController = _enemyHealthBar.GetComponent<HealthBarController>();
			_healthBarController.SetData(_currentEnemy, _health);

			DataPreserve.totalEnemiesOnMap++;
		}




		protected void MoveToPlayer()
		{
			Vector3 direction = (_player.transform.position - _currentEnemy.transform.position).normalized;
			_currentEnemy.transform.Translate(_speedAmount * Time.deltaTime * direction, Space.Self);
		}




		protected void DestroyWhenTooFarFromPlayer()
		{
			if (Vector3.Distance(_currentEnemy.transform.position, _player.transform.position) >= 25f)
			{
				Destroy(_enemyHealthBar);
				Destroy(_currentEnemy);
			}
		}




		protected void DropGunOrItem()
		{
			if (Random.Range(1, 10) == 5)
			{
				// Drop items
				if (Random.Range(0, 1f) > 0.5f)
					_currentEnemy.GetComponent<LootBag>().DropSupportItem(transform.position);

				// Drop guns
				else
					_currentEnemy.GetComponent<LootBagWeapon>().DropGun(transform.position);
			}
		}




		public virtual void ReceiveDamage(int damage)
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
				_expBarController.OnExpChanged(_point);
				_player.GetComponent<PlayableCharacterController>().UpgradePlayerLevel();

				Destroy(_enemyHealthBar);
				Destroy(_currentEnemy);
			}
		}
	}
}
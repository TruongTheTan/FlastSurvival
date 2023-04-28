using UnityEngine;

namespace Assets.Scripts.Models.Enemies
{
	public abstract class Enemy : MonoBehaviour
	{
		#region Enemy's properties

		protected int _point;
		protected int _damage;
		protected int _health;
		protected float _speedAmount;

		#endregion



		#region Get, set

		public int EnemyDamage { get => _damage; set { _damage = value; } }
		public GameObject EnemyHealthBar { get => _enemyHealthBar; }

		#endregion

		#region Game Objects reference

		protected GameObject _healthBars;
		protected GameObject _currentEnemy;
		protected GameObject _enemyHealthBar;
		protected GameObject _playerReference;

		#endregion



		protected ExpBarController _expBarController;
		protected HealthBarController _healthBarController;







		protected virtual void Awake()
		{
			InstantiateData();
		}




		protected virtual void Update()
		{
			MoveToPlayer();
			DestroyWhenTooFarFromPlayer();
		}







		#region Other methods

		private void InstantiateData()
		{
			_playerReference = DataPreserve.player;
			DataPreserve.totalEnemiesOnMap++;
			_healthBars = GameObject.Find("EnemyHealthBar");


			_enemyHealthBar = Instantiate(Resources.Load<GameObject>("Prefabs/HealthBar/HealthBar"), _healthBars.transform);

			_healthBarController = _enemyHealthBar.GetComponent<HealthBarController>();
			_healthBarController.SetData(_currentEnemy, _health);

			_expBarController = GameObject.Find("ExpBar").GetComponent<ExpBarController>();
		}



		private void MoveToPlayer()
		{
			Vector3 direction = (_playerReference.transform.position - _currentEnemy.transform.position).normalized;
			_currentEnemy.transform.Translate(_speedAmount * Time.deltaTime * direction, Space.Self);
		}




		private void DestroyWhenTooFarFromPlayer()
		{
			if (Vector3.Distance(_currentEnemy.transform.position, _playerReference.transform.position) >= 25f)
			{
				DataPreserve.totalEnemiesOnMap--;
				Destroy(_enemyHealthBar);
				Destroy(_currentEnemy);
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
				_playerReference.GetComponent<PlayableCharacterController>().UpgradePlayerLevel();

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



		#endregion
	}
}

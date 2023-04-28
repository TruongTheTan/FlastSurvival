using UnityEngine;

namespace Assets.Scripts.DesignPatterns.FactoryMethod
{
	public class RangedCombatEnemyFactory : IEnemyFactory
	{
		private readonly GameObject _blitzJokPrefab;
		private readonly GameObject _enemyBulletPrefab;
		private static RangedCombatEnemyFactory _instance;



		private RangedCombatEnemyFactory()
		{
			_blitzJokPrefab = Resources.Load<GameObject>("Prefabs/Enemy/BlitzJok");
			_enemyBulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet_9");
		}




		public static RangedCombatEnemyFactory GetInstance()
		{
			return _instance ??= new RangedCombatEnemyFactory();
		}




		public void CreateEnemy(int randomSpawnNumber, Vector3 spawnPosition)
		{
			GameObject enemySpawned;

			switch (randomSpawnNumber)
			{
				case (int)EnemyEnum.BlitzJok:
					enemySpawned = MonoBehaviour.Instantiate(_blitzJokPrefab, spawnPosition, Quaternion.identity);

					BlitzJokController blitzJokController = enemySpawned.GetComponent<BlitzJokController>();
					blitzJokController.RangedCombatBehavior = new ShootBulletCombat(_enemyBulletPrefab, enemySpawned);
					break;
			}
		}
	}
}

using Assets.Scripts.DesignPatterns.StrategyPattern.Concreates;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns.FactoryMethod
{
	public class CloseCombatEnemyFactory : IEnemyFactory
	{
		private readonly GameObject _fodderJoePrefab;
		private readonly GameObject _bigDaddyPrefab;
		private readonly GameObject _explosiveDavePrefab;
		private static CloseCombatEnemyFactory _instance;




		private CloseCombatEnemyFactory()
		{
			_fodderJoePrefab = Resources.Load<GameObject>("Prefabs/Enemy/FodderJoe");
			_bigDaddyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/BigDaddy");
			_explosiveDavePrefab = Resources.Load<GameObject>("Prefabs/Enemy/ExplosiveDave");
		}




		public static CloseCombatEnemyFactory GetInstance()
		{
			return _instance ??= new CloseCombatEnemyFactory();
		}




		public void CreateEnemy(int randomSpawnNumber, Vector3 spawnPosition)
		{
			GameObject enemySpawned;
			switch (randomSpawnNumber)
			{
				case (int)EnemyEnum.FodderJoe:
					enemySpawned = MonoBehaviour.Instantiate(_fodderJoePrefab, spawnPosition, Quaternion.identity);

					FodderJoeController fodderJoeController = enemySpawned.GetComponent<FodderJoeController>();
					fodderJoeController.CloseCombatBehavior = new MeleeCombat(fodderJoeController.EnemyDamage);
					break;




				case (int)EnemyEnum.BigDaddy:
					enemySpawned = MonoBehaviour.Instantiate(_bigDaddyPrefab, spawnPosition, Quaternion.identity);

					BigDaddyController bigDaddyJoeController = enemySpawned.GetComponent<BigDaddyController>();
					bigDaddyJoeController.CloseCombatBehavior = new MeleeCombat(bigDaddyJoeController.EnemyDamage);
					break;




				case (int)EnemyEnum.ExplosiveDave:
					enemySpawned = MonoBehaviour.Instantiate(_explosiveDavePrefab, spawnPosition, Quaternion.identity);

					ExplosiveDaveController explosiveDaveController = enemySpawned.GetComponent<ExplosiveDaveController>();
					explosiveDaveController.CloseCombatBehavior = new SuicideCombat(explosiveDaveController.EnemyDamage, enemySpawned, explosiveDaveController.EnemyHealthBar);
					break;
			}
		}
	}
}

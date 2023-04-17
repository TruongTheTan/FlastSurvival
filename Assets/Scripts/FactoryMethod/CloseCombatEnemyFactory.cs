using UnityEngine;

namespace Assets.Scripts.FactoryMethod
{
	public class CloseCombatEnemyFactory : AbstractEnemyFactory
	{
		public override GameObject CreateEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
		{
			return Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
		}
	}
}

using UnityEngine;

namespace Assets.Scripts.FactoryMethod
{
	public abstract class AbstractEnemyFactory : MonoBehaviour
	{
		public abstract GameObject CreateEnemy(GameObject enemyPrefab, Vector3 spawnPosition);
	}
}

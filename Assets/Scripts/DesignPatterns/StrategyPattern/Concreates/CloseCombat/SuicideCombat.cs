using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DesignPatterns.StrategyPattern.Concreates
{
	public class SuicideCombat : ICloseCombatBehavior
	{

		private readonly int _damge;
		private readonly GameObject _currentEnemy;
		private readonly GameObject _enemyHealthBar;



		public SuicideCombat(int damge, GameObject currentEnemy, GameObject enemyHealthBar)
		{
			_damge = damge;
			_currentEnemy = currentEnemy;
			_enemyHealthBar = enemyHealthBar;
		}



		public IEnumerator CloseCombat()
		{
			DataPreserve.totalEnemiesOnMap--;

			GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion/Explosion");

			MonoBehaviour.Instantiate(explosionPrefab, _currentEnemy.transform.position, Quaternion.identity);


			DataPreserve.player.GetComponent<PlayableCharacterController>().ReceiveDamaged(_damge);

			MonoBehaviour.Destroy(_enemyHealthBar);
			MonoBehaviour.Destroy(_currentEnemy);

			yield return new WaitForSeconds(0);
		}
	}
}

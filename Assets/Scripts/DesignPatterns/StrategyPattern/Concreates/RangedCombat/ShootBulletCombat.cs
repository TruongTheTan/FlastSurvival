using System.Collections;
using Assets.Scripts.DesignPatterns.StrategyPattern;
using UnityEngine;

public class ShootBulletCombat : IRangedCombatBehavior
{


	private readonly GameObject _bullet;
	private readonly GameObject _currentEnemy;

	//public static float Distance { get; set; }

	public ShootBulletCombat(GameObject bullet, GameObject currentEnemy)
	{
		_bullet = bullet;
		_currentEnemy = currentEnemy;
	}




	public IEnumerator RangedCombat(float distanceFromPlayer)
	{
		Vector3 directionToPlayer;
		GameObject player = DataPreserve.player;

		Debug.Log(distanceFromPlayer);

		while (distanceFromPlayer <= 10)
		{
			directionToPlayer = (player.transform.position - _currentEnemy.transform.position).normalized;



			float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.Euler(_currentEnemy.transform.position.x, _currentEnemy.transform.position.y, angle);


			GameObject bullet = MonoBehaviour.Instantiate(_bullet, _currentEnemy.transform.position, rotation);


			bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 5f;

			yield return new WaitForSeconds(1.5f);
		}
	}
}


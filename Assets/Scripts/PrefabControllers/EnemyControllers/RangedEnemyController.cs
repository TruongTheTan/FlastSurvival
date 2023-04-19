using System.Collections;
using Assets.Scripts.Models;
using UnityEngine;

public class RangedEnemyController : AbstractEnemy
{
	private float _distance;
	private GameObject _bullet;
	private Vector3 _directionToPlayer;


	// Start is called before the first frame update
	protected override void Start()
	{
		base._currentEnemy = gameObject;
		base.Start();

		_bullet = Resources.Load<GameObject>("Prefabs/Bullet/Bullet_9");
		StartCoroutine(AttackPlayer());
	}




	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		_distance = Vector2.Distance(this._player.transform.position, this.gameObject.transform.position);
	}





	// Shoot player
	public override IEnumerator AttackPlayer()
	{
		while (_distance <= 10)
		{
			Vector3 currentEnemyPosition = this.transform.position;

			_directionToPlayer = (this._player.transform.position - currentEnemyPosition).normalized;

			float angle = Mathf.Atan2(this._directionToPlayer.y, this._directionToPlayer.x) * Mathf.Rad2Deg;


			Quaternion enemyRotation = Quaternion.Euler(currentEnemyPosition.x, currentEnemyPosition.y, angle);


			GameObject bullet = Instantiate(_bullet, currentEnemyPosition, enemyRotation);
			bullet.GetComponent<Rigidbody2D>().velocity = _directionToPlayer * 5f;


			yield return new WaitForSeconds(1.5f);
		}
	}





	public override void ReceiveDamage(int damage)
	{
		base.ReceiveDamage(damage);
	}
}

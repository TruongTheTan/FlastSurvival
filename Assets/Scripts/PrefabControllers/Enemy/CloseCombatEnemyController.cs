using System.Collections;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.PrefabControllers.Enemy
{
	public class CloseCombatEnemyController : AbstractEnemy
	{
		private bool _isCollidingPlayer = false;



		protected override void Start()
		{
			base._currentEnemy = gameObject;
			base.Start();
		}



		protected override void Update()
		{
			base.Update();
		}



		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.Equals(_player))
			{
				_isCollidingPlayer = true;
				StartCoroutine(AttackPlayer());
				ExplodeWhenCollideToPlayer();
			}
		}



		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.Equals(_player))
				_isCollidingPlayer = false;
		}



		// Use for Explosive Dave
		private void ExplodeWhenCollideToPlayer()
		{

			// Explode when collide to player (for Dave)
			if (GetComponent<SpriteRenderer>().sprite.name.Equals(DataPreserve.EXPLOSIVE_DAVE_SPRITE_NAME))
			{
				DataPreserve.totalEnemiesOnMap--;
				GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion/Explosion");

				Instantiate(explosionPrefab, transform.position, Quaternion.identity);

				Destroy(_enemyHealthBar);
				Destroy(gameObject);
			}
		}


		// Melee
		public override IEnumerator AttackPlayer()
		{
			// Attack player every second
			while (_isCollidingPlayer)
			{
				_player.GetComponent<PlayableCharacterController>().ReceiveDamaged(_damage);
				yield return new WaitForSeconds(1f);
			}
		}




		public override void ReceiveDamage(int damage)
		{
			base.ReceiveDamage(damage);
		}
	}
}

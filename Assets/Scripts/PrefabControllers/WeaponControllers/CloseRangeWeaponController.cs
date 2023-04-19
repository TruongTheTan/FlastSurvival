using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.PrefabControllers.Enemy;
using UnityEngine;

namespace Assets.Scripts.PrefabControllers.WeaponControllers
{
	public class CloseRangeWeaponController : AbstractWeapon
	{
		private GameObject _gun;
		private GameObject _swordSprite;
		private bool _isMeleeing = false;
		private int _currentDamgeMelee = 70;
		private PlayableCharacterController _playableCharacterController;

		public bool IsMeleeing { get => _isMeleeing; }


		private void Start()
		{
			_playableCharacterController = FindObjectOfType<PlayableCharacterController>();
			_gun = _playableCharacterController.Gun;
			_swordSprite = _playableCharacterController.SwordSprite;
		}



		private void OnTriggerEnter2D(Collider2D collision)
		{
			GameObject enemy = collision.gameObject;

			// Player close combat (melee)
			if (enemy.CompareTag("Enemy"))
			{
				CloseCombatEnemyController closeCombatEnemyController = enemy.GetComponent<CloseCombatEnemyController>();
				RangedEnemyController rangedEnemyController = enemy.GetComponent<RangedEnemyController>();

				if (closeCombatEnemyController != null)
					closeCombatEnemyController.ReceiveDamage(_currentDamgeMelee);

				if (rangedEnemyController != null)
					rangedEnemyController.ReceiveDamage(_currentDamgeMelee);
			}

			/*
			else if (!isCollideToEnemy && DataPreserve.gunLevel == 3)
			{
				Destroy(enemy);
			}*/
		}





		public IEnumerator Melee()
		{
			_isMeleeing = true;

			BoxCollider2D collider = _swordSprite.AddComponent<BoxCollider2D>();

			collider.size = new Vector2(0.6761025f, 0.235665f);
			collider.isTrigger = true;

			Vector3 currentAngle = _gun.transform.eulerAngles;
			Vector3 up = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z + 90);
			Vector3 down = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z - 90);

			float duration = 1;
			float elapsedTime = 0;

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;

				float t = Mathf.Clamp01(elapsedTime / duration);
				_gun.transform.eulerAngles = Vector3.Lerp(up, down, t * 5);

				yield return null;
			}

			_isMeleeing = false;
			_gun.transform.eulerAngles = currentAngle;

			Destroy(collider);
			StopCoroutine(Melee());
		}



		public override void UpgradeWeapon(string weaponTagName)
		{
			this.UpgradeSword(weaponTagName);
		}






		private void UpgradeSword(string weaponTagName)
		{
			if (weaponTagName.Equals(DataPreserve.SWORD_TAG))
			{
				switch (_currentGunLevel)
				{
					case 2: _weaponDamage = 100; break;
					case 3: _weaponDamage = 100; break;
				}
			}
		}


	}
}

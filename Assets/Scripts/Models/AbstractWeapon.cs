using UnityEngine;

namespace Assets.Scripts.Models
{
	public abstract class AbstractWeapon : MonoBehaviour
	{
		protected int _weaponDamage = 0;
		protected int _currentGunLevel = 0;

		public abstract void UpgradeWeapon(string weaponTagName);


		public void InitializeWeaponProperties(string weaponTagName, Sprite weaponSprite)
		{
			transform.tag = weaponTagName;
			transform.GetComponent<SpriteRenderer>().sprite = weaponSprite;
		}
	}
}

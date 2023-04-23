using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.PrefabControllers.WeaponControllers
{


	public class RangedWeaponController : AbstractWeapon
	{
		protected GameObject _bulletPrefab;



		private void Start()
		{
			this._bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet_4");
		}





		public void Shoot()
		{
			string currentGunSpriteName = GetComponent<SpriteRenderer>().sprite.name;

			if (currentGunSpriteName != DataPreserve.SWORD_SPRITE.name)
			{
				GameObject bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);


				switch (currentGunSpriteName)
				{
					case "Gun_10":
						bullet.tag = DataPreserve.PISTOL_TAG;
						break;

					case "Gun_5":
						bullet.tag = DataPreserve.ASSAULT_RIFLE_TAG;
						break;

					case "Gun_11":
						bullet.tag = DataPreserve.SHOTGUN_TAG;
						break;
				}
			}

			/*
			else if (currentGunSpriteName == DataPreserve.SWORD_SPRITE.name && !_playableCharacterController.IsMeleeing)
			{
				StartCoroutine(nameof(_playableCharacterController.Melee));
			}*/
		}






		public override void UpgradeWeapon(string weaponTagName)
		{
			_currentGunLevel = DataPreserve.gunLevel;

			// Avoid upgrade gun when its still default level (level = 0, bad performance)
			if (_currentGunLevel > 0)
			{
				switch (weaponTagName.Trim())
				{
					case "Pistol": UpgradePistol(); break;
					case "ShotGun": UpgradeShotGun(); break;
					case "AssaultRifle": UpgradeAssaultRifle(); break;
				}
				BulletController.SetDamage(_weaponDamage);
			}
		}








		private void UpgradePistol()
		{
			switch (_currentGunLevel)
			{
				case 1: _weaponDamage = 30; break;
				case 2: _weaponDamage = 30; break;
				case 3: _weaponDamage = 40; break;
			}
		}



		private void UpgradeAssaultRifle()
		{
			switch (_currentGunLevel)
			{
				case 1: _weaponDamage = 60; break;
				case 2: _weaponDamage = 60; break;
				case 3: _weaponDamage = 80; break;
			}
		}



		private void UpgradeShotGun()
		{
			switch (_currentGunLevel)
			{
				case 0: _weaponDamage = 70; break;
				case 1: _weaponDamage = 70; break;
				case 2: _weaponDamage = 70; break;
				case 3: _weaponDamage = 105; break;
			}
		}




	}
}

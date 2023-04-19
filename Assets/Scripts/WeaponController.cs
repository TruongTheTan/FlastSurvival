using Assets.Scripts.PrefabControllers.WeaponControllers;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

	private GameObject _gunSprite;
	private GameObject _swordSprite;



	private void Start()
	{
		_gunSprite = transform.GetChild(0).gameObject;
		_swordSprite = transform.GetChild(1).gameObject;

		RandomWeaponType();
	}


	private void RandomWeaponType()
	{
		if (DataPreserve.isNewGame)
		{
			string weaponTagName = "";
			Sprite weaponSprite = null;


			switch (4)
			{
				case 1:
					weaponTagName = DataPreserve.PISTOL_TAG;
					weaponSprite = DataPreserve.PISTOL_SPRITE;
					break;

				case 2:
					weaponTagName = DataPreserve.ASSAULT_RIFLE_TAG;
					weaponSprite = DataPreserve.ASSAULT_RIFLE_SPRITE;
					break;

				case 3:
					weaponTagName = DataPreserve.SHOTGUN_TAG;
					weaponSprite = DataPreserve.SHOTGUN_SPRITE;
					break;

				case 4:
					weaponTagName = DataPreserve.SWORD_TAG;
					weaponSprite = DataPreserve.SWORD_SPRITE;
					break;
			}

			// Use the close combat weapon
			if (weaponTagName == DataPreserve.SWORD_TAG)
			{
				_gunSprite.SetActive(false);
				_swordSprite.GetComponent<CloseRangeWeaponController>().InitializeWeaponProperties(weaponTagName, weaponSprite);
			}
			else
			{
				_swordSprite.SetActive(false);
				_gunSprite.GetComponent<RangedWeaponController>().InitializeWeaponProperties(weaponTagName, weaponSprite);
			}
		}
	}
}

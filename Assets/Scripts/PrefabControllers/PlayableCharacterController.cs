using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayableCharacterController : MonoBehaviour
{
	private Joystick _joystick;

	private float _horizontalMove = 0f;
	private float _verticalMove = 0f;
	private float _moveAmount = 2.5f;

	[SerializeField]
	private GameObject _bulletPrefab;

	private GameObject _gun;
	private GameObject _gunSprite;

	private GameObject _healthBarReference;
	private GameObject _expBarReference;
	private Collider2D _collision;
	private Button _changeWeaponButton;
	private readonly string[] _weaponTypes = { "Sword", "Pistol", "ShotGun", "AssaultRifle" };
	private bool _isMeleeing = false;

	//Default current and max health to 100
	private int _currentHealthPoint = 100;
	private int _maxHealthPoint = 100;
	private int _maxExp = 100;
	private int _level = 1;

	private TextMeshProUGUI _changeWeaponText;

	private float _defaultSpeed = 2.5f;
	private bool _pickedUpInvicibleItem = false;

	private float _invicibleTimer = 0;
	private float _speedBuffTimer = 0;

	private Text _speedBuffTimerText;
	private Text _invicibleTimerText;


	private void Start()
	{
		InstantiateData();
		InstantiatePlayerStatBySelectedNumber();
	}

	// Update is called once per frame
	private void Update()
	{
		CharacterMovement();
		AimToClosestEnemy();
		UpdateSpeedBuffTime();
		UpdateInvicibleTime();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_collision = collision;
		PickUpGun();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_changeWeaponButton.image.gameObject.SetActive(false);
		DataPreserve.allowPickUpWeapon = false;
		_changeWeaponText.text = "Change";
	}

	private void InstantiateData()
	{
		if (_joystick == null)
			_joystick = FindObjectOfType<Joystick>();
#if UNITY_ANDROID || UNITY_IOS
		_joystick.gameObject.SetActive(true);
#else
        //joystick.gameObject.SetActive(false);
#endif

		_gun = transform.GetChild(0).gameObject;
		_gunSprite = _gun.transform.GetChild(0).gameObject;

		_healthBarReference = GameObject.Find("HealthBar");
		_healthBarReference.GetComponent<PlayerHealthBarController>().SetData(_maxHealthPoint);

		_expBarReference = GameObject.Find("ExpBar");
		_expBarReference.GetComponent<ExpBarController>().SetData(_maxExp);

		_changeWeaponText = FindObjectsOfType<TextMeshProUGUI>()[1];

		_changeWeaponButton = FindObjectsOfType<Button>()[0];
		_changeWeaponButton.image.gameObject.SetActive(false);


		_speedBuffTimerText = GameObject.Find("SpeedBuffTimer").GetComponent<Text>();
		_invicibleTimerText = GameObject.Find("InvicibleTimer").GetComponent<Text>();

		_speedBuffTimerText.text = string.Empty;
		_invicibleTimerText.text = string.Empty;
	}

	private void CharacterMovement()
	{
		_horizontalMove = _joystick.Horizontal;
		_verticalMove = _joystick.Vertical;

		Vector3 movement = new Vector3(_horizontalMove, _verticalMove);
		transform.Translate(_moveAmount * Time.deltaTime * movement.normalized);
	}

	private void AimToClosestEnemy()
	{
		GameObject[] enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");

		// If there are at least 1 enemy on map
		if (enemiesOnMap.Length > 0)
		{
			float distanceToClosetEnemy = Mathf.Infinity;
			Collider2D[] colliderDetectedWithinRadius = Physics2D.OverlapCircleAll(transform.position, 7.3f, 1);

			// Find closet enemy
			foreach (Collider2D colliderComponent in colliderDetectedWithinRadius)
			{
				// Point to enemy by collider tag name
				if (colliderComponent.CompareTag("Enemy"))
				{
					GameObject currentEnemy = colliderComponent.gameObject;

					float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

					// Get the closet enemy and aim to it
					if (distanceToEnemy < distanceToClosetEnemy)
					{
						distanceToClosetEnemy = distanceToEnemy;

						/* Aim to rearest enemy */
						Vector3 aimDirection = (currentEnemy.transform.position - transform.position).normalized;
						float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

						_gun.transform.eulerAngles = new Vector3(0, 0, angle);
					}
				}
			}
		}
	}

	public void Shoot()
	{
		if (_gunSprite.GetComponent<SpriteRenderer>().sprite.name != "Gun_3")
		{
			GameObject bullet = Instantiate(_bulletPrefab, _gunSprite.transform.position, _gunSprite.transform.rotation);

			switch (_gunSprite.GetComponent<SpriteRenderer>().sprite.name)
			{
				case "Gun_5":
					bullet.tag = _weaponTypes[3];
					break;

				case "Gun_10":
					bullet.tag = _weaponTypes[1];
					break;

				case "Gun_11":
					bullet.tag = _weaponTypes[2];
					break;
			}

			Debug.Log(bullet.tag);
		}
		else if (_gunSprite.GetComponent<SpriteRenderer>().sprite.name == "Gun_3" && !_isMeleeing)
		{
			StartCoroutine(nameof(Melee));
		}
	}

	IEnumerator Melee()
	{
		_isMeleeing = true;
		BoxCollider2D collider = _gunSprite.AddComponent<BoxCollider2D>();
		collider.size = new Vector2(0.6761025f, 0.235665f);
		collider.isTrigger = true;
		Vector3 currentAngle = _gun.transform.eulerAngles;
		Vector3 up = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z + 90);
		Vector3 down = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z - 90);

		float elapsedTime = 0;
		float duration = 1;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Clamp01(elapsedTime / duration);
			_gun.transform.eulerAngles = Vector3.Lerp(up, down, t * 5);
			yield return null;
		}

		_gun.transform.eulerAngles = currentAngle;
		_isMeleeing = false;
		Destroy(collider);
		StopCoroutine(nameof(Melee));
	}

	public void PickUpGun()
	{
		GameObject gunGameObject = _collision.gameObject;

		// Check if collision is a weapon
		if (_weaponTypes.Contains(gunGameObject.tag))
		{
			_changeWeaponButton.image.gameObject.SetActive(true);

			SpriteRenderer gunSpriteRender = _gunSprite.GetComponent<SpriteRenderer>();

			bool isTheSameGun = false;
			string playerGunSpriteName = gunSpriteRender.sprite.name;
			string gunGameObjectSpriteName = gunGameObject.GetComponent<SpriteRenderer>().sprite.name;

			// Change text
			if (gunGameObjectSpriteName.Equals(playerGunSpriteName))
			{
				isTheSameGun = true;
				_changeWeaponText.text = "Pick Up";
			}

			// Change new or pick up the same gun (Include Upgrade)
			if (DataPreserve.allowPickUpWeapon)
			{
				// Upgrade gun if pick the same gun
				if (isTheSameGun)
				{
					int currentGunLevel = DataPreserve.gunLevel;

					// Upgrade range, avoid infinite upgrade (bad performance)
					if (currentGunLevel >= 0 && currentGunLevel <= 3)
					{
						DataPreserve.gunLevel++;
						GunController.UpgradeGunByLevel(gunGameObject.tag);
					}
				}
				// Pick up new gun
				else
				{
					DataPreserve.gunLevel = 0;
					gunSpriteRender.sprite = _collision.gameObject.GetComponent<SpriteRenderer>().sprite;
				}
				Debug.Log($"Gun level: {DataPreserve.gunLevel}");
				Destroy(gunGameObject);
			}
		}
	}

	public void Damaged(int damage)
	{

		if (!_pickedUpInvicibleItem)
		{
			if (_currentHealthPoint > damage)
			{
				_currentHealthPoint -= damage;
				_healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
				Debug.Log($"Player current HP: {_currentHealthPoint}");
			}
			else
			{
				SceneManager.LoadScene("SceneGameOver");
			}
		}
	}



	public void HealthBuff(int among)
	{
		if (_maxHealthPoint > _currentHealthPoint)
		{
			_currentHealthPoint += among;
		}
		else
		{
			_currentHealthPoint = _maxHealthPoint;
		}
		_healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
	}


	public void SpeedBuff()
	{
		_moveAmount += _moveAmount / 100 * 20;
		_speedBuffTimer = 10;
	}


	private void UpdateSpeedBuffTime()
	{
		if (_speedBuffTimer > 0)
		{
			_speedBuffTimer -= Time.deltaTime;
			_speedBuffTimerText.text = $"Speed time remain: {(int)_speedBuffTimer}";
		}
		// Reset speed to deafault
		else
		{
			_moveAmount = _defaultSpeed;
			_speedBuffTimerText.text = string.Empty;
		}
	}


	public void SetInvicibleTime()
	{
		_pickedUpInvicibleItem = true;
		_invicibleTimer = 10f;
	}


	private void UpdateInvicibleTime()
	{
		// Reduce invicible time
		if (_pickedUpInvicibleItem)
		{
			_invicibleTimer -= Time.deltaTime;
			_invicibleTimerText.text = $"Invicible time remain: {(int)_invicibleTimer}";


			// Invicible time no longer effected
			if (_invicibleTimer <= 0)
			{
				_pickedUpInvicibleItem = false;
				_invicibleTimerText.text = string.Empty;
			}
		}
	}



	public void UpgratePlayer()
	{
		float currentExp = _expBarReference.GetComponent<ExpBarController>().GetCurrentExp();
		if (currentExp == _maxExp)
		{
			_level += 1;
			_maxExp += 50;
			_expBarReference.GetComponent<ExpBarController>().SetData(_maxExp);

			GameObject upgrade = GameObject.Find("PlayGameSceneEventHandler");
			upgrade.GetComponent<UpgradeEventHandler>().Upgrade();
		}
	}
	private void InstantiatePlayerStatBySelectedNumber()
	{
		int selectedNumber = DataPreserve.characterSelectedNumber;

		if (selectedNumber == 2)
		{
			_moveAmount = 3;
			_maxHealthPoint = 75;
			_currentHealthPoint = 75;
		}
		else if (selectedNumber == 3)
		{
			_moveAmount = 2;
			_maxHealthPoint = 125;
			_currentHealthPoint = 125;
		}
		_defaultSpeed = _moveAmount;
	}
}
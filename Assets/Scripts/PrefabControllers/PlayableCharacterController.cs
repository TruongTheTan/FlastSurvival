using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayableCharacterController : MonoBehaviour, ICombatable
{
	private Joystick _joystick;

	private float _horizontalMove = 0f;
	private float _verticalMove = 0f;
	private float _moveAmount = 2.5f;


	private GameObject _gun;
	private GameObject _gunSprite;

	private GameObject _healthBarReference;
	private GameObject _expBarReference;
	private Collider2D _collision;
	private Button _changeWeaponButton;
	private bool _isMeleeing = false;

	//Default current and max health to 100
	private int _currentHealthPoint = 100;

	private int _maxHealthPoint = 100;
	private int _maxExp = 100;
	private int _level = 1;

	private TextMeshProUGUI _changeWeaponText;

	private float _defaultSpeed = 2.5f;// reset to default speed when no longer effect by Speed up item
	private bool _pickedUpInvicibleItem = false;

	private float _invicibleTimer = 0;
	private float _speedBuffTimer = 0;

	private Text _speedBuffTimerText;
	private Text _invicibleTimerText;
	private Text _levelText;

	#region Gets, Sets

	public int CurrentHealthPoint { get => _currentHealthPoint; }
	public int MaxHealthPoint { get => _maxHealthPoint; set => _maxHealthPoint = value; }
	public int MaxExp { get => _maxExp; set => _maxExp = value; }
	public int Level { get => _level; set => _level = value; }
	public GameObject GunSprite { get => _gunSprite; }
	public float DefaultSpeed { get => _defaultSpeed; }
	public bool IsMeleeing { get => _isMeleeing; }

	#endregion



	private void Awake()
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
		_healthBarReference.GetComponent<PlayerHealthBarController>().SetHealthPoint(_maxHealthPoint);

		_expBarReference = GameObject.Find("ExpBar");
		_expBarReference.GetComponent<ExpBarController>().SetData(_maxExp);

		_changeWeaponText = GameObject.Find("ChangeWeaponText").GetComponent<TextMeshProUGUI>();

		_changeWeaponButton = GameObject.Find("Picked").GetComponent<Button>();
		_changeWeaponButton.image.gameObject.SetActive(false);

		_speedBuffTimerText = GameObject.Find("SpeedBuffTimer").GetComponent<Text>();
		_invicibleTimerText = GameObject.Find("InvicibleTimer").GetComponent<Text>();
		_levelText = GameObject.Find("CurrentLevel").GetComponent<Text>();


		_speedBuffTimerText.text = string.Empty;
		_invicibleTimerText.text = string.Empty;
		_levelText.text = $"Lv: {_level}";

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
		_gunSprite.GetComponent<GunController>().Shoot();
	}

	public IEnumerator Melee()
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
		string[] _weaponTypes = { "Sword", "Pistol", "ShotGun", "AssaultRifle" };

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

	public void ReceiveDamaged(int damage)
	{
		if (!_pickedUpInvicibleItem)
		{
			if (_currentHealthPoint > damage)
			{
				_currentHealthPoint -= damage;
				_healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
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
		// Speed effect count down
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

	public void UpgradePlayerLevel()
	{
		if (_level <= 15)
		{
			float currentExp = _expBarReference.GetComponent<ExpBarController>().GetCurrentExp;

			// Increase player level, upgrade available number when XP is full filled
			if (currentExp >= _maxExp)
			{
				_level += 1;
				_maxExp += 50;
				_levelText.text = $"Lv: {_level}";

				_expBarReference.GetComponent<ExpBarController>().SetData(_maxExp);

				// Increase upgrade available number
				if (_level % 5 == 0)
				{
					DataPreserve.numberOfUpgrades++;
					GameObject.Find("PlayGameSceneEventHandler").GetComponent<UpgradePlayerEventHandler>().OpenUpgradePanel();
				}
			}
		}
	}


	public void UpgradeHealth()
	{
		_maxHealthPoint += 10;
	}


	public void UpgradeSpeed()
	{
		_defaultSpeed = _moveAmount += 0.5f;
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

	public void LoadSaveData(SaveData data)
	{
		_maxHealthPoint = data.MaxHealth;
		_maxExp = data.RequiredExp;
		_level = data.Level;
		DataPreserve.gunLevel = data.WeaponLevel;

		_levelText.text = $"Lv: {_level}";


		switch (data.WeaponType)
		{
			case 0:
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.SWORD_SPRITE;
				GunController.UpgradeGunByLevel(DataPreserve.ASSAULT_RIFLE_TAG);
				break;

			case 1:
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.PISTOL_SPRITE;
				GunController.UpgradeGunByLevel(DataPreserve.ASSAULT_RIFLE_TAG);
				break;

			case 2:
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.SHOTGUN_SPRITE;
				GunController.UpgradeGunByLevel(DataPreserve.ASSAULT_RIFLE_TAG);
				break;

			case 3:
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.ASSAULT_RIFLE_SPRITE;
				GunController.UpgradeGunByLevel(DataPreserve.ASSAULT_RIFLE_TAG);
				break;
		}

		_healthBarReference = GameObject.Find("HealthBar");
		_healthBarReference.GetComponent<PlayerHealthBarController>().SetHealthPoint(_maxHealthPoint);

		_expBarReference = GameObject.Find("ExpBar");
		_expBarReference.GetComponent<ExpBarController>().SetData(_maxExp);
		_expBarReference.GetComponent<ExpBarController>().OnExpChanged(data.CurrentExp);

		_defaultSpeed = _moveAmount = data.CurrentSpeed;

		ReceiveDamaged(_maxHealthPoint - data.CurrentHealth);
	}
}
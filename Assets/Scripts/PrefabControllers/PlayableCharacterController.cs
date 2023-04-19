using System.Linq;
using Assets.Scripts.PrefabControllers.WeaponControllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayableCharacterController : MonoBehaviour
{
	private GameObject _gun;
	private GameObject _gunSprite;
	private Collider2D _collision;
	private GameObject _swordSprite;

	#region Player's stats

	private int _level = 1;
	private int _maxExp = 100;
	private int _health = 100;
	private float _speedAmount = 0;
	private float _verticalMove = 0;
	private int _maxHealthPoint = 500;
	private float _horizontalMove = 0;
	private float _defaultSpeed = 2.5f;// reset to default speed when no longer effect by Speed up item

	#endregion


	#region Effect items

	private float _invicibleTimer = 0;
	private float _speedBuffTimer = 0;
	private bool _pickedUpInvicibleItem = false;

	#endregion

	#region GUI Objects

	private Joystick _joystick;

	private Text _levelText;
	private Text _speedBuffTimerText;
	private Text _invicibleTimerText;
	private Button _changeWeaponButton;
	private TextMeshProUGUI _changeWeaponText;

	#endregion


	#region Controllers

	private ExpBarController _expBarController;
	private RangedWeaponController _rangedWeaponController;
	private CloseRangeWeaponController _closeWeaponController;
	private PlayerHealthBarController _playerHealthBarController;

	#endregion


	#region Gets, Sets

	public GameObject Gun { get => this._gun; }
	public int CurrentHealthPoint { get => this._health; }
	public GameObject GunSprite { get => this._gunSprite; }
	public float DefaultSpeed { get => this._defaultSpeed; }
	public GameObject SwordSprite { get => this._swordSprite; }
	public int Level { get => this._level; set => this._level = value; }
	public int MaxExp { get => this._maxExp; set => this._maxExp = value; }
	public int MaxHealthPoint { get => this._maxHealthPoint; set => this._maxHealthPoint = value; }


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
		this._collision = collision;
		PickUpGun();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_changeWeaponText.text = "Change";
		DataPreserve.allowPickUpWeapon = false;
		_changeWeaponButton.image.gameObject.SetActive(false);
	}

	private void InstantiateData()
	{
		if (_joystick == null)
			this._joystick = FindObjectOfType<Joystick>();

		_joystick.gameObject.SetActive(true);


		this._gun = transform.GetChild(0).gameObject;
		this._gunSprite = _gun.transform.GetChild(0).gameObject;
		this._swordSprite = _gun.transform.GetChild(1).gameObject;



		this._rangedWeaponController = _gunSprite.GetComponent<RangedWeaponController>();
		this._closeWeaponController = _swordSprite.GetComponent<CloseRangeWeaponController>();



		this._playerHealthBarController = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBarController>();
		this._playerHealthBarController.SetHealthPoint(_maxHealthPoint);

		this._expBarController = GameObject.Find("ExpBar").GetComponent<ExpBarController>();
		this._expBarController.SetData(_maxExp);


		this._changeWeaponText = GameObject.Find("ChangeWeaponText").GetComponent<TextMeshProUGUI>();

		this._changeWeaponButton = GameObject.Find("PlayerPickUpGunButton").GetComponent<Button>();
		this._changeWeaponButton.image.gameObject.SetActive(false);

		this._speedBuffTimerText = GameObject.Find("SpeedBuffTimerText").GetComponent<Text>();
		this._invicibleTimerText = GameObject.Find("InvicibleTimerText").GetComponent<Text>();
		this._levelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();


		this._speedBuffTimerText.text = string.Empty;
		this._invicibleTimerText.text = string.Empty;
		this._levelText.text = $"Lv: {_level}";

	}



	private void CharacterMovement()
	{
		this._horizontalMove = _joystick.Horizontal;
		this._verticalMove = _joystick.Vertical;


		Vector3 movement = new Vector3(_horizontalMove, _verticalMove);
		transform.Translate(_speedAmount * Time.deltaTime * movement.normalized);
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
		if (_gunSprite.activeSelf)
			this._rangedWeaponController.Shoot();
	}



	public void Melee()
	{
		if (this._swordSprite.activeSelf && this._closeWeaponController.IsMeleeing == false)
			StartCoroutine(this._closeWeaponController.Melee());
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


				if (_swordSprite.activeSelf == false)
				{
					if (gunGameObject.CompareTag(DataPreserve.SWORD_TAG))
					{
						_swordSprite.SetActive(true);
						_gunSprite.SetActive(false);
					}
				}


				// Upgrade gun if pick the same gun
				if (isTheSameGun)
				{
					int currentGunLevel = DataPreserve.gunLevel;

					// Upgrade range, avoid infinite upgrade (bad performance)
					if (currentGunLevel >= 0 && currentGunLevel <= 3)
					{
						DataPreserve.gunLevel++;

						if (gunGameObject.CompareTag("Sword"))
							_gunSprite.GetComponent<CloseRangeWeaponController>().UpgradeWeapon(gunGameObject.tag);

						else
							_gunSprite.GetComponent<RangedWeaponController>().UpgradeWeapon(gunGameObject.tag);
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
			if (_health > damage)
			{
				_health -= damage;
				_playerHealthBarController.OnHealthChanged(_health);
			}
			else
				SceneManager.LoadScene("SceneGameOver");
		}
	}

	public void HealthBuff(int among)
	{
		if (_maxHealthPoint > _health)
			_health += among;
		else
			this._health = _maxHealthPoint;

		_playerHealthBarController.OnHealthChanged(_health);
	}

	public void SpeedBuff()
	{
		this._speedBuffTimer = 10;
		_speedAmount += _speedAmount / 100 * 20;
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
			this._speedAmount = _defaultSpeed;
			_speedBuffTimerText.text = string.Empty;
		}
	}

	public void SetInvicibleTime()
	{
		this._pickedUpInvicibleItem = true;
		this._invicibleTimer = 10f;
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
				this._pickedUpInvicibleItem = false;
				_invicibleTimerText.text = string.Empty;
			}
		}
	}

	public void UpgradePlayerLevel()
	{
		if (_level <= 15)
		{
			float currentExp = _expBarController.GetCurrentExp;

			// Increase player level, upgrade available number when XP is full filled
			if (currentExp >= _maxExp)
			{
				_level += 1;
				_maxExp += 50;
				_levelText.text = $"Lv: {_level}";

				_expBarController.SetData(_maxExp);

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
		this._health += 10;
		this._maxHealthPoint += 10;
	}



	public void UpgradeSpeed()
	{
		this._defaultSpeed = _speedAmount += 0.5f;
	}



	private void InstantiatePlayerStatBySelectedNumber()
	{
		int selectedNumber = DataPreserve.characterSelectedNumber;

		this._health = 500;
		this._speedAmount = 2.5f;

		if (selectedNumber == 2)
		{
			this._health = 500;
			this._speedAmount = 3;
			this._maxHealthPoint = 500;
		}
		else if (selectedNumber == 3)
		{
			this._health = 500;
			this._speedAmount = 2;
			this._maxHealthPoint = 500;
		}
		this._defaultSpeed = _speedAmount;
	}



	public void LoadSaveData(SaveData data)
	{
		this._maxHealthPoint = data.MaxHealth;
		this._maxExp = data.RequiredExp;
		this._level = data.Level;
		DataPreserve.gunLevel = data.WeaponLevel;

		_levelText.text = $"Lv: {_level}";


		switch (data.WeaponType)
		{
			case 0:
				_closeWeaponController.UpgradeWeapon(DataPreserve.SWORD_TAG);
				_swordSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.SWORD_SPRITE;
				break;

			case 1:
				_rangedWeaponController.UpgradeWeapon(DataPreserve.PISTOL_TAG);
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.PISTOL_SPRITE;
				break;

			case 2:
				_rangedWeaponController.UpgradeWeapon(DataPreserve.SHOTGUN_TAG);
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.SHOTGUN_SPRITE;
				break;

			case 3:
				_rangedWeaponController.UpgradeWeapon(DataPreserve.ASSAULT_RIFLE_TAG);
				_gunSprite.GetComponent<SpriteRenderer>().sprite = DataPreserve.ASSAULT_RIFLE_SPRITE;
				break;
		}

		_playerHealthBarController.SetHealthPoint(_maxHealthPoint);

		_expBarController.SetData(_maxExp);
		_expBarController.OnExpChanged(data.CurrentExp);

		this._defaultSpeed = this._speedAmount = data.CurrentSpeed;

		ReceiveDamaged(_maxHealthPoint - data.CurrentHealth);
	}
}
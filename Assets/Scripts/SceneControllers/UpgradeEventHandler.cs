using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEventHandler : MonoBehaviour
{
	[SerializeField]
	private Sprite _john;

	[SerializeField]
	private Sprite _ariah;

	[SerializeField]
	private Sprite _steve;

	private GameObject _upgradePanelReference;
	private GameObject _displayCharacterReference;

	private Image _avatar;

	private TextMeshProUGUI _currentMaxHP;
	private TextMeshProUGUI _currentMaxSpeed;
	private TextMeshProUGUI _upgradePanelTitle;


	private GameObject _player;
	private PlayableCharacterController _playerController;



	private void Awake()
	{
		_upgradePanelReference = GameObject.Find("UpgradePanel");
		_displayCharacterReference = GameObject.Find("CharacterDisplay");

		_avatar = GameObject.Find("ImageAvatar").GetComponent<Image>();


		switch (DataPreserve.characterSelectedNumber)
		{
			case 1:
				_displayCharacterReference.GetComponent<Image>().sprite = _john;
				_avatar.sprite = _john;
				break;
			case 2:
				_displayCharacterReference.GetComponent<Image>().sprite = _ariah;
				_avatar.sprite = _ariah;
				break;
			case 3:
				_displayCharacterReference.GetComponent<Image>().sprite = _steve;
				_avatar.sprite = _steve;
				break;
		}


		_currentMaxHP = GameObject.Find("HealthCurrent").GetComponent<TextMeshProUGUI>();
		_currentMaxSpeed = GameObject.Find("SpeedCurrent").GetComponent<TextMeshProUGUI>();
		_upgradePanelTitle = GameObject.Find("PanelTitle").GetComponent<TextMeshProUGUI>();

		_upgradePanelReference.SetActive(false);
	}


	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerController = _player.GetComponent<PlayableCharacterController>();

		_currentMaxHP.text = $"Current Health: {_playerController.MaxHealthPoint}";
		_currentMaxSpeed.text = $"Current Speed: {_playerController.DefaultSpeed}";
	}


	public void OpenUpgradePanel()
	{
		if (_playerController.Level % 5 == 0)
		{
			_upgradePanelReference.SetActive(true);
			_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade available: {DataPreserve.numberOfUpgrades}";
			Time.timeScale = 0;
		}
	}

	public void UpgradeHealthClick()
	{
		if (DataPreserve.numberOfUpgrades > 0 && DataPreserve.numberOfUpgrades <= 15)
		{
			_playerController.UpgradeHealth();
			DataPreserve.numberOfUpgrades--;
		}


	}

	public void UpgradeSpeedClick()
	{

		if (DataPreserve.numberOfUpgrades > 0 && DataPreserve.numberOfUpgrades <= 15)
		{
			_playerController.UpgradeSpeed();
			DataPreserve.numberOfUpgrades--;
		}
	}

	public void UpgradeBackClick()
	{
		_upgradePanelReference.SetActive(false);
		Time.timeScale = 1f;
	}
}
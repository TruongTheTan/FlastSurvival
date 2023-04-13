using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePlayerEventHandler : MonoBehaviour
{
	private Image _avatar;
	private PlayableCharacterController _playerController;
	private GameObject _player, _upgradePanelReference, _displayCharacterReference;
	private TextMeshProUGUI _currentMaxHP, _currentMaxSpeed, _upgradePanelTitle;



	private void Awake()
	{
		_upgradePanelReference = GameObject.Find("UpgradePanel");
		_displayCharacterReference = GameObject.Find("CharacterDisplay");

		_avatar = GameObject.Find("ImageAvatar").GetComponent<Image>();

		_currentMaxHP = GameObject.Find("HealthCurrent").GetComponent<TextMeshProUGUI>();
		_currentMaxSpeed = GameObject.Find("SpeedCurrent").GetComponent<TextMeshProUGUI>();
		_upgradePanelTitle = GameObject.Find("PanelTitle").GetComponent<TextMeshProUGUI>();

		DisplayPlayerAvatarInUpgradePanel();

		_upgradePanelReference.SetActive(false);
	}


	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerController = _player.GetComponent<PlayableCharacterController>();

		_currentMaxHP.text = $"Current Health: {_playerController.MaxHealthPoint}";
		_currentMaxSpeed.text = $"Current Speed: {_playerController.DefaultSpeed}";
	}




	private void DisplayPlayerAvatarInUpgradePanel()
	{
		Sprite currentPlayerSprite = null;

		switch (DataPreserve.characterSelectedNumber)
		{
			case 1: currentPlayerSprite = DataPreserve.JOHN_SPRITE; break;
			case 2: currentPlayerSprite = DataPreserve.ARIAH_SPRITE; break;
			case 3: currentPlayerSprite = DataPreserve.STEVE_SPRITE; break;
		}
		_avatar.sprite = _displayCharacterReference.GetComponent<Image>().sprite = currentPlayerSprite;
	}


	public void OpenUpgradePanel()
	{
		_upgradePanelReference.SetActive(true);
		_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade available: {DataPreserve.numberOfUpgrades}";
		Time.timeScale = 0;
	}



	public void UpgradeHealthClick()
	{
		if (DataPreserve.numberOfUpgrades > 0)
		{
			_playerController.UpgradeHealth();
			DataPreserve.numberOfUpgrades--;

			_currentMaxHP.text = $"Current Health: {_playerController.MaxHealthPoint}";
			_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade {DataPreserve.numberOfUpgrades}";
		}
	}



	public void UpgradeSpeedClick()
	{

		if (DataPreserve.numberOfUpgrades > 0)
		{
			_playerController.UpgradeSpeed();

			DataPreserve.numberOfUpgrades--;

			_currentMaxSpeed.text = $"Current Speed: {_playerController.DefaultSpeed}";
			_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade {DataPreserve.numberOfUpgrades}";
		}
	}


	public void UpgradeBackClick()
	{
		_upgradePanelReference.SetActive(false);
		Time.timeScale = 1f;
	}
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePlayerEventHandler : MonoBehaviour
{
	[SerializeField]
	private Sprite _johnSprite;

	[SerializeField]
	private Sprite _ariahSprite;

	[SerializeField]
	private Sprite _steveSprite;

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
			case 1: currentPlayerSprite = _johnSprite; break;
			case 2: currentPlayerSprite = _ariahSprite; break;
			case 3: currentPlayerSprite = _steveSprite; break;
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
			_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade available: {DataPreserve.numberOfUpgrades}";
		}
	}



	public void UpgradeSpeedClick()
	{

		if (DataPreserve.numberOfUpgrades > 0)
		{
			_playerController.UpgradeSpeed();
			DataPreserve.numberOfUpgrades--;
			_currentMaxSpeed.text = $"Current Speed: {_playerController.DefaultSpeed}";
			_upgradePanelTitle.text = $"Choose a stat to upgrade:\nUpgrade available: {DataPreserve.numberOfUpgrades}";
		}
	}


	public void UpgradeBackClick()
	{
		_upgradePanelReference.SetActive(false);
		Time.timeScale = 1f;
	}
}
using UnityEngine;
/// <summary>
/// This class use for handling button click events related to player
/// </summary>
public class PlayerEventHandler : MonoBehaviour
{
	private GameObject _player;
	private GameObject _gunSprite;
	private GameObject _swordSprite;
	private PlayableCharacterController _playerController;


	// Start is called before the first frame update
	void Start()
	{
		this._player = GameObject.FindGameObjectWithTag("Player");
		GameObject weapon = _player.transform.GetChild(0).gameObject;

		this._gunSprite = weapon.transform.GetChild(0).gameObject;
		this._swordSprite = weapon.transform.GetChild(1).gameObject;

		this._playerController = _player.GetComponent<PlayableCharacterController>();
	}



	public void PlayerShootButtonClick()
	{
		if (this._gunSprite.activeSelf)
			this._playerController.Shoot();
		else
			this._playerController.Melee();
	}



	public void PickupWeaponButtonClick()
	{
		DataPreserve.allowPickUpWeapon = true;
		this._playerController.PickUpGun();
	}
}

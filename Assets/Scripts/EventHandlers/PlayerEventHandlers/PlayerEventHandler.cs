using UnityEngine;
/// <summary>
/// This class use for handling button click events related to player
/// </summary>
public class PlayerEventHandler : MonoBehaviour
{

	private PlayableCharacterController _playerController;


	// Start is called before the first frame update
	void Start()
	{
		_playerController = FindObjectOfType<PlayableCharacterController>();
	}



	public void PlayerShootButtonClick()
	{
		_playerController.Shoot();
	}



	public void PickupWeaponButtonClick()
	{
		DataPreserve.allowPickUpWeapon = true;
		_playerController.PickUpGun();
	}
}

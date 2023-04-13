using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
	//Only add this script to camera after Player is spawned
	private readonly float _followSpeed = 3f;
	private GameObject _playerReference;



	//Awake is called when the script instance is being loaded
	private void Awake()
	{
		_playerReference = GameObject.FindGameObjectWithTag("Player");
	}



	private void LateUpdate()
	{
		Vector3 currentPlayerPosition = _playerReference.transform.position;

		Vector3 newPosition = new Vector3(currentPlayerPosition.x, currentPlayerPosition.y, -10f);
		transform.position = Vector3.Lerp(transform.position, newPosition, _followSpeed * Time.deltaTime);
	}
}
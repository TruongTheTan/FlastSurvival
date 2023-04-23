using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
	//Only add this script to camera after Player is spawned
	private readonly float _followSpeed = 3f;




	private void LateUpdate()
	{
		Vector3 currentPlayerPosition = DataPreserve.player.transform.position;

		Vector3 newPosition = new Vector3(currentPlayerPosition.x, currentPlayerPosition.y, -10f);
		transform.position = Vector3.Lerp(transform.position, newPosition, _followSpeed * Time.deltaTime);
	}
}
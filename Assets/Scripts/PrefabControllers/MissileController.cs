using UnityEngine;

public class MissileController : MonoBehaviour
{
	[SerializeField]
	private Transform _player;
	private Rigidbody2D _rigidbody2D;
	private readonly float _rotateSpeed = 5;
	private readonly float _speedAmount = 5;


	// Start is called before the first frame update
	void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		//_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}


	private void Update()
	{
		Vector3 dir = (_player.transform.position - transform.position).normalized;

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotateSpeed);
		_rigidbody2D.velocity = new Vector2(dir.x * _speedAmount, dir.y * _speedAmount);

	}


	void FixedUpdate()
	{
		/*
		Vector2 direction = (Vector2)_player.position - _rigidbody2D.position;
		direction.Normalize();

		float rotateAmount = Vector3.Cross(direction, transform.up).z;

		_rigidbody2D.angularVelocity = -rotateAmount * _rotateSpeed;
		_rigidbody2D.velocity = transform.up * _speedAmount;
		*/
	}
}

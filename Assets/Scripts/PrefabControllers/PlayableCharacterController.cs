using UnityEngine;

public class PlayableCharacterController : MonoBehaviour
{
    [SerializeField]
    private Joystick _joystick;

    private float _horizontalMove = 0f;
    private float _verticalMove = 0f;
    private readonly float _moveAmount = 2.5f;

    private void Awake()
    {
        if (_joystick == null)
        {
            _joystick = FindObjectOfType<Joystick>();
        }
    }

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        _joystick.gameObject.SetActive(true);
#else
        joystick.gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    private void Update()
    {
        CharacterMovement();
    }

    private void CharacterMovement()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");

#if UNITY_ANDROID || UNITY_IOS
        _horizontalMove = _joystick.Horizontal;
        _verticalMove = _joystick.Vertical;
#endif

        Vector3 movement = new Vector3(_horizontalMove, _verticalMove);
        transform.Translate(_moveAmount * Time.deltaTime * movement.normalized);
    }
}
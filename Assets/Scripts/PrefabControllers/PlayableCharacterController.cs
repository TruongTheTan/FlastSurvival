using UnityEngine;

public class PlayableCharacterController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;

    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private readonly float moverPerfame = 0.01f;

    private void Awake()
    {
        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
        }
    }
    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        joystick.gameObject.SetActive(true);
#else
        joystick.gameObject.SetActive(false);
#endif
    }
    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
    }

    private void CharacterMovement()
    {
        float moveX = 0;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

#if UNITY_ANDROID || UNITY_IOS
        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;
#endif

        if (horizontalMove != 0)
        {
            moveX = horizontalMove < 0 ? -moverPerfame : moverPerfame;
        }

        float moveY = 0;

        if (verticalMove != 0)
        {
            moveY = verticalMove < 0 ? -moverPerfame : moverPerfame;
        }

        if (moveX != 0 || moveY != 0)
        {
            var pos = transform.position;
            transform.position = new Vector3(pos.x + moveX, pos.y + moveY, pos.z);
        }
    }
}
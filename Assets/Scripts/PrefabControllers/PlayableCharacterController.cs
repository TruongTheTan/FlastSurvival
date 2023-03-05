using UnityEngine;

public class PlayableCharacterController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;

    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private readonly float moverPerfame = 0.01f;

    private GameObject gun;

    [SerializeField]
    private GameObject bulletPrefab;


    private void Awake()
    {
        gun = transform.GetChild(0).gameObject;
    }

    private void Start()
    {

#if UNITY_ANDROID || UNITY_IOS
        joystick.gameObject.SetActive(true);
#else
        //joystick.gameObject.SetActive(false);
#endif

    }
    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        AutoAimToClosetEnemy();
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



    private void AutoAimToClosetEnemy()
    {
        float distanceToClosetEnemy = Mathf.Infinity;

        GameObject closetEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


        if (enemies.Length > 0)
        {
            /* Find closet enemy */
            foreach (GameObject currentEnemy in enemies)
            {
                float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

                // Find by distance between player and enemies
                if (distanceToEnemy < distanceToClosetEnemy)
                {
                    distanceToClosetEnemy = distanceToEnemy;
                    closetEnemy = currentEnemy;
                }
            }

            Debug.DrawLine(gun.transform.position, closetEnemy.transform.position);

            /*Aim to the nearest Enemy*/
            Vector3 aimDirection = (closetEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            gun.transform.eulerAngles = new Vector3(0, 0, angle);
        }


    }
}

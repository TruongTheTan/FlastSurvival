﻿using UnityEngine;

public class PlayableCharacterController : MonoBehaviour
{
    private Joystick _joystick;

    private float _horizontalMove = 0f;
    private float _verticalMove = 0f;
    private readonly float _moveAmount = 2.5f;

    [SerializeField]
    private GameObject bulletPrefab;
    private GameObject gun;


    private void Awake()
    {
        if (_joystick == null)
        {
            _joystick = FindObjectOfType<Joystick>();
        }
        gun = transform.GetChild(0).gameObject;
    }



    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        _joystick.gameObject.SetActive(true);
#else
        //joystick.gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    private void Update()
    {
        CharacterMovement();
        AimToClosetEnemy();
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




    private void AimToClosetEnemy()
    {
        GameObject[] enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");

        // If there are at least 1 enemy on map
        if (enemiesOnMap.Length > 0)
        {
            float distanceToClosetEnemy = Mathf.Infinity;
            Collider2D[] colliderDetectedWithinRadius = Physics2D.OverlapCircleAll(transform.position, 10, 1);


            // Find closet enemy
            foreach (GameObject currentEnemy in enemiesOnMap)
            {
                // Find enemy within the player collider radius
                foreach (Collider2D colliderComponent in colliderDetectedWithinRadius)
                {
                    // Find by Collider component in radius
                    if (currentEnemy.GetComponent<Collider2D>().Equals(colliderComponent))
                    {
                        Vector3 currentEnemyPosition = currentEnemy.transform.position;
                        float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

                        // Get the closet enemy and aim to it
                        if (distanceToEnemy < distanceToClosetEnemy)
                        {
                            distanceToClosetEnemy = distanceToEnemy;

                            /* Aim to rearest enemy */
                            Vector3 aimDirection = (currentEnemy.transform.position - transform.position).normalized;
                            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

                            gun.transform.eulerAngles = new Vector3(0, 0, angle);
                        }
                    }
                }
            }
        }
    }
}

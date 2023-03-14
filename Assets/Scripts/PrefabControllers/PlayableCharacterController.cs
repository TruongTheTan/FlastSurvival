using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayableCharacterController : MonoBehaviour
{
    private Joystick _joystick;

    private float _horizontalMove = 0f;
    private float _verticalMove = 0f;
    private float _moveAmount = 2.5f;

    [SerializeField]
    private GameObject _bulletPrefab;

    private GameObject _gun;
    private GameObject _gunSprite;

    private GameObject _healthBarReference;
    private Collider2D _collision;
    private Button _changeWeaponButton;
    private readonly string[] _weaponTypes = { "Sword", "Pistol", "ShotGun", "AssaultRifle" };

    //Default current and max health to 100
    private int _currentHealthPoint = 100;
    private int _maxHealthPoint = 100;



    private void Start()
    {
        InstantiateData();
    }


    // Update is called once per frame
    private void Update()
    {
        CharacterMovement();
        AimToClosetEnemy();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collision = collision;
        PickUpGun();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        _changeWeaponButton.image.gameObject.SetActive(false);
        DataPreserve.allowPickUpWeapon = false;
    }



    private void InstantiateData()
    {
#if UNITY_ANDROID || UNITY_IOS
        _joystick.gameObject.SetActive(true);
#else
        //joystick.gameObject.SetActive(false);
#endif
        if (_joystick == null)
            _joystick = FindObjectOfType<Joystick>();

        _gun = transform.GetChild(0).gameObject;
        _gunSprite = _gun.transform.GetChild(0).gameObject;

        _healthBarReference = GameObject.Find("HealthBar");
        _healthBarReference.GetComponent<PlayerHealthBarController>().SetData(_maxHealthPoint);

        _changeWeaponButton = FindObjectsOfType<Button>()[0];
        _changeWeaponButton.image.gameObject.SetActive(false);
    }

    private void CharacterMovement()
    {
        _horizontalMove = _joystick.Horizontal;
        _verticalMove = _joystick.Vertical;

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
            Collider2D[] colliderDetectedWithinRadius = Physics2D.OverlapCircleAll(transform.position, 7.3f, 1);

            // Find closet enemy
            foreach (Collider2D colliderComponent in colliderDetectedWithinRadius)
            {
                // Point to enemy by collider tag name
                if (colliderComponent.CompareTag("Enemy"))
                {
                    GameObject currentEnemy = colliderComponent.gameObject;

                    float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

                    // Get the closet enemy and aim to it
                    if (distanceToEnemy < distanceToClosetEnemy)
                    {
                        distanceToClosetEnemy = distanceToEnemy;

                        /* Aim to rearest enemy */
                        Vector3 aimDirection = (currentEnemy.transform.position - transform.position).normalized;
                        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

                        _gun.transform.eulerAngles = new Vector3(0, 0, angle);
                    }
                }
            }
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunSprite.transform.position, _gunSprite.transform.rotation);

        switch (_gunSprite.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Gun_5":
                bullet.tag = _weaponTypes[3];
                break;

            case "Gun_10":
                bullet.tag = _weaponTypes[1];
                break;

            case "Gun_11":
                bullet.tag = _weaponTypes[2];
                break;
        }
    }

    public void PickUpGun()
    {
        string weaponTagName = _collision.gameObject.tag;

        // Check if collision is a weapon
        if (_weaponTypes.Contains(weaponTagName))
        {
            _changeWeaponButton.image.gameObject.SetActive(true);

            // Change gun sprite if press "Change" button
            if (DataPreserve.allowPickUpWeapon)
                _gunSprite.GetComponent<SpriteRenderer>().sprite = _collision.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void Damaged(int damage)
    {
        if (_currentHealthPoint > damage)
        {
            _currentHealthPoint -= damage;
            _healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
        }
        else
        {
            SceneManager.LoadScene("SceneGameOver");
        }
    }

    public void HealtBuff(int among)
    {
        if (_maxHealthPoint > _currentHealthPoint)
        {
            _currentHealthPoint += among;
            _healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
        }
        else
        {
            _currentHealthPoint = _maxHealthPoint;
            _healthBarReference.GetComponent<PlayerHealthBarController>().OnHealthChanged(_currentHealthPoint);
        }
    }
}
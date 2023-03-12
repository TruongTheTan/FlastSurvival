using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform m_target;
    [SerializeField] private float speed;
    private GameObject m_gameObject;
    private Rigidbody2D Rigidbody2Dm_Rigidbody2;

    [SerializeField]
    private GameObject _healthBarPrefab;

    private GameObject _healthBars;
    private float _maxHealth = 100f;
    private HealthBarController _healthBarController;
    private float timer;

    // Start is called before the first frame update
    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
        m_gameObject = m_target.gameObject;
    }

    void Start()
    {
        InstantiateData();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = (m_target.position - transform.position).normalized;
        gameObject.transform.Translate(direction * Time.deltaTime * speed, Space.Self);
        timer += 1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == m_gameObject)
        {
            Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DecreaseHPWhenGetAttackByPlayer(collision);
    }

    private void Attack()
    {
        Debug.Log("Attack Player!!");
    }

    private void InstantiateData()
    {
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        switch (currentSprite.name)
        {
            case "Big Daddy":
                _maxHealth = 200f;
                break;

            case "Blitz Jok":
                _maxHealth = 75f;
                break;

            case "Fodder Joe":
                _maxHealth = 100f;
                break;

            case "Explosive Dave":
                _maxHealth = 50f;
                break;

            default: break;
        }

        _healthBars = GameObject.Find("HealthBars");
        GameObject healthBar = Instantiate(_healthBarPrefab, _healthBars.transform);
        _healthBarController = healthBar.GetComponent<HealthBarController>();
        _healthBarController.SetData(gameObject, _maxHealth);
    }

    private void DecreaseHPWhenGetAttackByPlayer(Collision2D collision)
    {
        // Decrease health by weapon's bullet (Include upgraded weapon  )
        switch (collision.gameObject.tag)
        {
            case "PistolRifle": break;
            case "AssaultRileBullet": break;
            case "ShotGunBullet": break;
            case "Sword": break;
        }
        _healthBarController.OnHealthChanged(_maxHealth);

        if (timer > 2)
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            Destroy(gameObject);
        }
    }
}
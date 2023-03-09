using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform m_target;
    [SerializeField] float speed;
    GameObject m_gameObject;
    Rigidbody2D Rigidbody2Dm_Rigidbody2;



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

using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform m_target;
    private float _speed;
    private GameObject _player;

    [SerializeField]
    private GameObject _healthBarPrefab;

    private GameObject _healthBars;
    private GameObject _currentHealthBar;
    private float _health = 100f;
    private int _damage = 0;
    private bool _isCollidingPlayer = false;
    private HealthBarController _healthBarController;
    private float _timer;
    private int _point;
    private Vector3 _directionToPlayer;

    private GameObject _bullet;
    private bool _isRanged;

    // Start is called before the first frame update
    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
        _player = m_target.gameObject;
        _isRanged = false;
    }

    void Start()
    {
        InstantiateData();
        if (_isRanged) { StartCoroutine(nameof(RangedAttack)); }
    }

    private void Update()
    {
        DestroyWhenTooFarFromPlayer();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = (m_target.position - transform.position).normalized;
        gameObject.transform.Translate(direction * Time.deltaTime * _speed, Space.Self);
        _timer += 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _player)
        {
            _isCollidingPlayer = true;
            StartCoroutine(DealDamageEverySecond());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == _player)
        {
            _isCollidingPlayer = false;
        }
    }

    private void Attack()
    {
        _player.GetComponent<PlayableCharacterController>().Damaged(_damage);
    }

    IEnumerator RangedAttack()
    {
        while (_isRanged && Vector2.Distance(m_target.position, transform.position) <= 20)
        {
            _directionToPlayer = (m_target.position - gameObject.transform.position).normalized;
            float angle = Vector3.Angle(_directionToPlayer, Vector3.right);
            GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);

            bullet.transform.eulerAngles = new Vector3(bullet.transform.position.x, bullet.transform.position.y, angle);
            bullet.GetComponent<Rigidbody2D>().velocity = _directionToPlayer * 5f;
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void InstantiateData()
    {
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        switch (currentSprite.name)
        {
            case "Fodder Joe":
                _health = 100f;
                _speed = 2;
                _damage = 20;
                _point = 10;
                break;

            case "Blitz Jok":
                _health = 75f;
                _speed = 2.5f;
                _damage = 20;
                _point = 20;
                _isRanged = true;
                break;

            case "Big Daddy":
                _health = 200f;
                _speed = 1f;
                _damage = 50;
                _point = 30;
                break;

            case "Explosive Dave":
                _health = 50f;
                _speed = 3;
                _damage = 100;
                _point = 5;
                break;

            default: break;
        }

        _healthBars = GameObject.Find("HealthBars");

        GameObject healthBar = Instantiate(_healthBarPrefab, _healthBars.transform);

        _currentHealthBar = healthBar;
        _healthBarController = healthBar.GetComponent<HealthBarController>();
        _healthBarController.SetData(gameObject, _health);

        if (_isRanged)
        {
            _bullet = (GameObject)Resources.Load("Prefabs/Bullet/Bullet_9");
        }

        DataPreserve.totalEnemiesOnMap++;
    }

    IEnumerator DealDamageEverySecond()
    {
        while (_isCollidingPlayer)
        {
            Attack();
            yield return new WaitForSeconds(1f);
        }
    }

    public void Damaged(int damage)
    {
        if (_health > damage)
        {
            _health -= damage;
            _healthBarController.OnHealthChanged(_health);
        }
        else
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            DataPreserve.enemyKilled++;
            DataPreserve.totalScore += (_point * DataPreserve.enemyKilled) + (DataPreserve.gameRound * 100);
            Destroy(_currentHealthBar);
            Destroy(gameObject);
        }
    }

    private void DestroyWhenTooFarFromPlayer()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) >= 25f)
        {
            DataPreserve.totalEnemiesOnMap--;
            Destroy(_currentHealthBar);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        StopCoroutine(nameof(RangedAttack));
    }
}
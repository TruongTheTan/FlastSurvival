using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform m_target;
    [SerializeField] private float _speed;
    private GameObject m_gameObject;
    private Rigidbody2D Rigidbody2Dm_Rigidbody2;

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
        gameObject.transform.Translate(direction * Time.deltaTime * _speed, Space.Self);
        _timer += 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == m_gameObject)
        {
            _isCollidingPlayer = true;
            StartCoroutine(DealDamageEverySecond());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == m_gameObject)
        {
            _isCollidingPlayer = false;
        }
    }

    private void Attack()
    {
        m_gameObject.GetComponent<PlayableCharacterController>().Damaged(_damage);
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
    }

    IEnumerator DealDamageEverySecond()
    {
        while (_isCollidingPlayer)
        {
            yield return new WaitForSeconds(1f);
            Attack();
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
            DataPreserve.enemyKilled++;
            DataPreserve.totalScore += (_point * DataPreserve.enemyKilled) + (DataPreserve.gameRound * 100);
            Destroy(_currentHealthBar);
            Destroy(gameObject);
        }
    }
}
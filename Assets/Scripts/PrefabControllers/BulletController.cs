using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int _damage;
    private Timer _bulletTimer;

    // Start is called before the first frame update
    void Start()
    {
        _bulletTimer = gameObject.AddComponent<Timer>();

        switch (gameObject.tag)
        {
            case "Pistol":
                _damage = 20;
                _bulletTimer.Duration = 3;
                break;

            case "AssaultRifle":
                _damage = 40;
                _bulletTimer.Duration = 3;
                break;

            case "Shotgun":
                _damage = 70;
                _bulletTimer.Duration = 1;
                break;
        }

        _bulletTimer.Run();

        GetComponent<Rigidbody2D>().velocity = transform.right * 25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bulletTimer.Finished)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().Damaged(_damage);
        }

        Destroy(gameObject);
    }
}
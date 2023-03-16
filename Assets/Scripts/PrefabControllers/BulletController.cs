using UnityEngine;

public class BulletController : MonoBehaviour
{
    private static int _damage;
    private Timer _bulletTimer;
    private bool _isBounceable;
    private int _bounceTimes;

    // Start is called before the first frame update
    void Start()
    {
        _bulletTimer = gameObject.AddComponent<Timer>();
        InstantiateBulletProperties();
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

        if (!_isBounceable || _bounceTimes <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _bounceTimes--;
        }
    }




    private void InstantiateBulletProperties()
    {
        switch (gameObject.tag)
        {
            case "Pistol":
                if (DataPreserve.gunLevel == 0)
                    SetDamage(20);
                _bulletTimer.Duration = 3;
                break;

            case "AssaultRifle":
                if (DataPreserve.gunLevel == 0)
                    SetDamage(40);
                _bulletTimer.Duration = 3;
                break;

            case "ShotGun":
                if (DataPreserve.gunLevel == 0)
                    SetDamage(70);
                _bulletTimer.Duration = 1;
                break;
        }
        _bulletTimer.Run();

        GetComponent<Rigidbody2D>().velocity = transform.right * 25f;

        Debug.Log($"Damage setted: {_damage}");
    }


    public static void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetBounceable(bool bounce, int bounceTimes)
    {
        _isBounceable = bounce;
        _bounceTimes = bounceTimes;
    }
}
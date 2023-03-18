using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Sprite _swordSprite;

    [SerializeField]
    private Sprite _pistolSprite;

    [SerializeField]
    private Sprite _shotGunSprite;

    [SerializeField]
    private Sprite _assaultRifleSprite;

    private static int _currentGunLevel = 0;
    private static int _newGunDamage = 0;
    private int _currentDamgeMelee = 0;

    // Start is called before the first frame update
    void Start()
    {
        RandomGunType();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.GetComponent<SpriteRenderer>().sprite.name == "Gun_3")
        {
            collision.gameObject.GetComponent<EnemyController>().Damaged(_currentDamgeMelee);
        }
    }

    private void RandomGunType()
    {
        string weaponTagName = "";
        Sprite weaponSprite = null;
        switch (Random.Range(1, 4))
        {
            case 1:
                weaponTagName = "Pistol";
                weaponSprite = _pistolSprite;
                break;

            case 2:
                weaponTagName = "AssaultRifle";
                weaponSprite = _assaultRifleSprite;
                break;

            case 3:
                weaponTagName = "ShotGun";
                weaponSprite = _shotGunSprite;
                break;

            case 4:
                weaponTagName = "Sword";
                weaponSprite = _swordSprite;
                _currentDamgeMelee = 70;
                break;
        }
        transform.tag = weaponTagName;
        transform.GetComponent<SpriteRenderer>().sprite = weaponSprite;
    }

    public static void UpgradeGunByLevel(string weaponTagName)
    {
        _currentGunLevel = DataPreserve.gunLevel;

        // Avoid upgrade gun when its still default level (level = 0, bad performance)
        if (_currentGunLevel > 0)
        {
            switch (weaponTagName)
            {
                case "Sword": UpgradeSword(); break;
                case "Pistol": UpgradePistol(); break;
                case "ShotGun": UpgradeShotGun(); break;
                case "AssaultRifle": UpgradeAssaultRifle(); break;
            }
            BulletController.SetDamage(_newGunDamage);
        }
    }

    private static void UpgradeSword()
    {
        switch (_currentGunLevel)
        {
            case 1: _newGunDamage = 70; break;
            case 2: _newGunDamage = 100; break;
            case 3: _newGunDamage = 100; break;
        }
    }

    private static void UpgradePistol()
    {
        switch (_currentGunLevel)
        {
            case 1: _newGunDamage = 30; break;
            case 2: _newGunDamage = 30; break;
            case 3: _newGunDamage = 40; break;
        }
    }

    private static void UpgradeAssaultRifle()
    {
        switch (_currentGunLevel)
        {
            case 1: _newGunDamage = 60; break;
            case 2: _newGunDamage = 60; break;
            case 3: _newGunDamage = 80; break;
        }
    }

    private static void UpgradeShotGun()
    {
        switch (_currentGunLevel)
        {
            case 0: _newGunDamage = 70; break;
            case 1: _newGunDamage = 70; break;
            case 2: _newGunDamage = 70; break;
            case 3: _newGunDamage = 105; break;
        }
    }
}
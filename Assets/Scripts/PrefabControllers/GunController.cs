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

    // Start is called before the first frame update
    void Start()
    {
        RandomGunType();
    }

    // Update is called once per frame
    void Update()
    {
    }






    private void RandomGunType()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        string weaponTagName = "";
        Sprite weaponSprite = null;
        Vector2 boxCollider2DSize = new Vector2();

        switch (Random.Range(1, 4))
        {
            case 1:
                weaponTagName = "Pistol";
                weaponSprite = _pistolSprite;
                boxCollider2DSize = new Vector2(0.3291424f, 0.1410481f);
                break;

            case 2:
                weaponTagName = "AssaultRifle";
                weaponSprite = _assaultRifleSprite;
                boxCollider2DSize = new Vector2(0.6761025f, 0.235665f);
                break;

            case 3:
                weaponTagName = "ShotGun";
                boxCollider2DSize = new Vector2(0.6761025f, 0.235665f);
                weaponSprite = _shotGunSprite;
                break;

            case 4:
                weaponTagName = "Sword";
                boxCollider2DSize = new Vector2(0.6761025f, 0.235665f);
                weaponSprite = _swordSprite;
                break;
        }
        transform.tag = weaponTagName;
        boxCollider2D.size = boxCollider2DSize;
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

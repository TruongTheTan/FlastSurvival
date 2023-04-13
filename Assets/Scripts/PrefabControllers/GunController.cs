using UnityEngine;

public class GunController : MonoBehaviour
{

    private GameObject _bulletPrefab;
    private PlayableCharacterController _playableCharacterController;

    private static int _currentGunLevel = 0;
    private static int _newGunDamage = 0;
    private int _currentDamgeMelee = 0;


    private void Start()
    {
        RandomGunType();
        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet_4");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject enemy = collision.gameObject;
        bool isCollideToEnemy = enemy.CompareTag("Enemy");

        // Player close combat (melee)
        if (isCollideToEnemy && GetComponent<SpriteRenderer>().sprite.name == DataPreserve.SWORD_SPRITE.name)
            enemy.GetComponent<EnemyController>().ReceiveDamaged(_currentDamgeMelee);

        /*
        else if (!isCollideToEnemy && DataPreserve.gunLevel == 3)
        {
            Destroy(enemy);
        }*/
    }





    public void Shoot()
    {
        string currentGunSpriteName = GetComponent<SpriteRenderer>().sprite.name;

        if (currentGunSpriteName != DataPreserve.SWORD_SPRITE.name)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);

            switch (currentGunSpriteName)
            {
                case "Gun_10":
                    bullet.tag = DataPreserve.PISTOL_TAG;
                    break;

                case "Gun_5":
                    bullet.tag = DataPreserve.ASSAULT_RIFLE_TAG;
                    break;

                case "Gun_11":
                    bullet.tag = DataPreserve.SHOTGUN_TAG;
                    break;
            }
        }
        else if (currentGunSpriteName == DataPreserve.SWORD_SPRITE.name && !_playableCharacterController.IsMeleeing)
        {
            StartCoroutine(nameof(_playableCharacterController.Melee));
        }
    }



    private void RandomGunType()
    {
        if (DataPreserve.isNewGame)
        {
            string weaponTagName = "";
            Sprite weaponSprite = null;


            switch (Random.Range(1, 4))
            {
                case 1:
                    weaponTagName = DataPreserve.PISTOL_TAG;
                    weaponSprite = DataPreserve.PISTOL_SPRITE;
                    break;

                case 2:
                    weaponTagName = DataPreserve.ASSAULT_RIFLE_TAG;
                    weaponSprite = DataPreserve.ASSAULT_RIFLE_SPRITE;
                    break;

                case 3:
                    weaponTagName = DataPreserve.SHOTGUN_TAG;
                    weaponSprite = DataPreserve.SHOTGUN_SPRITE;
                    break;

                case 4:
                    weaponTagName = DataPreserve.SWORD_TAG;
                    weaponSprite = DataPreserve.SWORD_SPRITE;
                    _currentDamgeMelee = 70;
                    break;
            }
            transform.tag = weaponTagName;
            transform.GetComponent<SpriteRenderer>().sprite = weaponSprite;
        }
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


    #region Upgrade guns
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
    #endregion
}
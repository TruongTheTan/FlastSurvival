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


    private int gunTypeNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        RandomGunType();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy gun when player picked up
        if (DataPreserve.allowPickUpWeapon)
            Destroy(gameObject);
    }





    private void RandomGunType()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        string weaponTagName = "";
        Sprite weaponSprite = null;
        Vector2 boxCollider2DSize = new Vector2();


        switch (gunTypeNumber = Random.Range(1, 4))
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
                weaponSprite = _pistolSprite;
                boxCollider2DSize = new Vector2(0.3291424f, 0.1410481f);
                break;
            case 4:
                weaponTagName = "Sword";
                weaponSprite = _pistolSprite;
                boxCollider2DSize = new Vector2(0.3291424f, 0.1410481f);
                break;
        }
        transform.tag = weaponTagName;
        boxCollider2D.size = boxCollider2DSize;
        transform.GetComponent<SpriteRenderer>().sprite = weaponSprite;
    }

}

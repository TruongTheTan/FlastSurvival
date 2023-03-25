using System.Linq;
using UnityEngine;

public class LootBagWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _lootWeaponPrefab;

    private LootWeapon[] _weapons;

    private void Awake()
    {
        _weapons = Resources.LoadAll<LootWeapon>("LootWeapon");
    }

    private LootWeapon GetLootWeapon()
    {
        return _weapons.FirstOrDefault(x => x.DropIndicator == Random.Range(1, 4));
    }

    public void DropGun(Vector3 position)
    {
        LootWeapon drop = GetLootWeapon();
        GameObject weaponDropped = Instantiate(_lootWeaponPrefab, position, Quaternion.identity);
        weaponDropped.GetComponent<SpriteRenderer>().sprite = drop.WeaponSprite;
        weaponDropped.tag = drop.WeaponTag;

        Destroy(weaponDropped, 5);
    }
}
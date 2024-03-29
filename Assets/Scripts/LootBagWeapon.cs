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
        int randomize = Random.Range(1, 4);
        LootWeapon weapon = _weapons.FirstOrDefault(x => x.DropIndicator == randomize);
        return weapon;
    }

    public void InstantiateLoot(Vector3 position)
    {
        LootWeapon drop = GetLootWeapon();
        GameObject weaponDropped = Instantiate(_lootWeaponPrefab, position, Quaternion.identity);
        weaponDropped.GetComponent<SpriteRenderer>().sprite = drop.WeaponSprite;
        weaponDropped.tag = drop.WeaponTag;
    }
}
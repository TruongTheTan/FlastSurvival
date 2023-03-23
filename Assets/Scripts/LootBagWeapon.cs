using System.Linq;
using UnityEngine;

public class LootBagWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject lootWeaponPrefab;

    private LootWeapon[] weapons;

    private void Awake()
    {
        weapons = Resources.LoadAll("LootWeapon") as LootWeapon[];
    }

    private LootWeapon GetLootWeapon()
    {
        int randomize = Random.Range(1, 4);
        LootWeapon weapon = weapons.FirstOrDefault(x => x.DropIndicator == randomize);
        return weapon;
    }

    public void InstantiateLoot(Vector3 position)
    {
        LootWeapon drop = GetLootWeapon();
        GameObject weaponDropped = Instantiate(lootWeaponPrefab, position, Quaternion.identity);
        weaponDropped.GetComponent<SpriteRenderer>().sprite = drop.WeaponSprite;
        weaponDropped.tag = drop.WeaponTag;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject dropPrabs;
    public List<Loot> loots = new List<Loot>();

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> pos  = new List<Loot>();
        foreach(Loot  item in loots)
        {
            if(randomNumber <= item.dropChange)
            {
                pos.Add(item);
            }
        }
        if(pos.Count > 0)
        {
            Loot dropped = pos[Random.Range(0,pos.Count)];
            return dropped;
        }
        return null;
    }
    public void InstantiateLoot(Vector3 spawn)
    {
        Loot droped = GetDroppedItem();
        if(droped != null)
        {
            GameObject loot = Instantiate(dropPrabs, spawn, Quaternion.identity);
            loot.GetComponent<SpriteRenderer>().sprite = droped.lootSprite;


            float drop = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
            loot.GetComponent<Rigidbody2D>().AddForce(dropDirection * drop, ForceMode2D.Force);
        }
    }
}

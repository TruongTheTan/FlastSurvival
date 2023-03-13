using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public string lootType;
    public int dropChange;
    public int among;

    public Loot(string lootName, int dropChange)
    {
        this.lootName = lootName;
        this.dropChange = dropChange;
    }
	public void Apply(GameObject gameObject)
	{
		
	}
}

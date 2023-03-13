using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EffecItem : MonoBehaviour
{
	public List<Loot> items;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			foreach (var item in items)
			{
				if (gameObject.GetComponent<Sprite>().Equals(item.lootSprite))
				{
					switch (gameObject.tag)
					{
						case "Healing":
							Destroy(gameObject);
							collision.GetComponent<PlayerHealthBarController>().OnHealthChanged(item.among);
							break;
						case "Incincible Time":
							Destroy(gameObject);
							collision.GetComponent<SpriteRenderer>().color = Color.white;
							break;
						case "Speed Up":
							Destroy(gameObject);
							collision.GetComponent<SpriteRenderer>().color = Color.yellow;
							break;
					}

				}
			}
		}		
	}
}

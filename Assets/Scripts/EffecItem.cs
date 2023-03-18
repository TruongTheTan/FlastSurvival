using UnityEngine;

public class EffecItem : MonoBehaviour
{
    [SerializeField]
    private Loot _health;

    [SerializeField]
    private Loot _invincible;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.Equals(_health.lootSprite))
            {
                collision.gameObject.GetComponent<PlayableCharacterController>().HealtBuff(_health.among);
                Destroy(gameObject);
            }
            else if (gameObject.GetComponent<SpriteRenderer>().sprite.Equals(_invincible.lootSprite))
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
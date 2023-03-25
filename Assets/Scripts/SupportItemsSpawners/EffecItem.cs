using UnityEngine;

public class EffecItem : MonoBehaviour
{
    [SerializeField]
    private Loot _health;

    [SerializeField]
    private Loot _invicible;

    [SerializeField]
    private Loot _speed;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayableCharacterController player = collision.gameObject.GetComponent<PlayableCharacterController>();
            Sprite supportItemSprite = GetComponent<SpriteRenderer>().sprite;


            if (supportItemSprite.Equals(_health.lootSprite))
                player.HealthBuff(_health.among);


            else if (supportItemSprite.Equals(_invicible.lootSprite))
                player.SetInvicibleTime();


            else
                player.SpeedBuff();

            Destroy(gameObject);
        }
    }
}
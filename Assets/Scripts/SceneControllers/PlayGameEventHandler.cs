using UnityEngine;

public class PlayGameEventHandler : MonoBehaviour
{
    private PlayableCharacterController _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayableCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }




    public void PlayerShoot()
    {
        _player.Shoot();
    }
}

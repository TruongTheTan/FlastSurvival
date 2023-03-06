using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    //Only add this script to camera after Player is spawned
    private readonly float _followSpeed = 3f;

    private GameObject _playerReference;

    //Awake is called when the script instance is being loaded
    private void Awake()
    {
        _playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (_playerReference == null)
        {
            _playerReference = GameObject.FindGameObjectWithTag("Player");
        }
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 newPosition = new Vector3(_playerReference.transform.position.x, _playerReference.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, newPosition, _followSpeed * Time.deltaTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Joystick joystick;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    float moverPerfame = 0.01f;
    private void Start()
    {
        #if UNITY_ANDROID || UNITY_IOS
        joystick.gameObject.SetActive(true);
        #else
        joystick.gameObject.SetActive(false);
        #endif
    }
    // Update is called once per frame
    void Update()
    {
        float moveX = 0;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        #if UNITY_ANDROID || UNITY_IOS
        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;
        #endif
        if (horizontalMove != 0)
        {
            moveX = horizontalMove < 0 ? -moverPerfame : moverPerfame;
        }
        float moveY = 0;
        if(verticalMove != 0)
        {
            moveY = verticalMove < 0 ? -moverPerfame: moverPerfame;
        }
        if(moveX != 0 || moveY != 0)
        {
            var pos = gameObject.transform.position;
            gameObject.transform.position = new Vector3(pos.x + moveX,pos.y + moveY,pos.z);
        }
    }
}

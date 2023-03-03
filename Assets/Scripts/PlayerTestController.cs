using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.left * 10 * Time.deltaTime);
    }
}
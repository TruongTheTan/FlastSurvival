using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private float _maxHealth;

    public Slider healthBar;
    public GameObject objectToFollow;

    public void SetData(GameObject follow, float maxHP)
    {
        healthBar = GetComponent<Slider>();
        objectToFollow = follow;
        FollowObject();
        _maxHealth = maxHP;

        healthBar.maxValue = _maxHealth;
        healthBar.value = _maxHealth;
    }

    public void OnHealthChanged(float hp)
    {
        healthBar.value = hp;
    }

    private void Update()
    {
        FollowObject();
    }

    //Follow directly above object
    private void FollowObject()
    {
        Vector2 newPosition = new Vector2(objectToFollow.transform.position.x, objectToFollow.transform.position.y + 1);
        transform.position = newPosition;
    }
}
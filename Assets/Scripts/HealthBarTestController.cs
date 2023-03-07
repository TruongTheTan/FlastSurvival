using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTestController : MonoBehaviour
{
    private float _maxHealth;
    private GameObject HUD;

    public Slider healthBar;
    public GameObject objectToFollow;

    // Start is called before the first frame update
    private void Awake()
    {
    }

    public void SetData(GameObject follow, float maxHP)
    {
        healthBar = GetComponent<Slider>();
        objectToFollow = follow;
        FollowObject();
        _maxHealth = maxHP;
        healthBar.value = _maxHealth;
    }

    private void OnHealthChanged(float hp)
    {
        healthBar.value = hp;
    }

    private void Update()
    {
        FollowObject();
    }

    private void FollowObject()
    {
        Vector2 newPosition = new Vector2(objectToFollow.transform.position.x, objectToFollow.transform.position.y + 1);
        transform.position = newPosition;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private float _maxHealth;

    private Slider _healthBar;
    private GameObject _objectToFollow;



    private void Update()
    {
        FollowObject();
    }

    //Follow directly above object
    private void FollowObject()
    {
        Vector2 newPosition = new Vector2(_objectToFollow.transform.position.x, _objectToFollow.transform.position.y + 1);
        transform.position = newPosition;
    }



    public void SetData(GameObject follow, float maxHP)
    {
        _healthBar = GetComponent<Slider>();
        _objectToFollow = follow;
        FollowObject();
        _maxHealth = maxHP;

        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _maxHealth;
    }

    public void OnHealthChanged(float hp)
    {
        _healthBar.value = hp;

        if (_healthBar.value <= 0)
            Destroy(gameObject);
    }
}
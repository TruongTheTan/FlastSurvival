using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarController : MonoBehaviour
{
    private float _maxHealth;
    private Slider _healthBarSlider;

    private void SetData(float maxHP)
    {
        _healthBarSlider = GetComponent<Slider>();
        _maxHealth = maxHP;
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _maxHealth;
    }

    private void OnHealthChanged(float hp)
    {
        _healthBarSlider.value = hp;
    }
}

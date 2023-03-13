using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarController : MonoBehaviour
{
    private float _maxHealth;
    private Slider _healthBarSlider;

    public void SetData(float maxHP)
    {
        _healthBarSlider = GetComponent<Slider>();
        _maxHealth = maxHP;
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _maxHealth;
    }

    public void OnHealthChanged(float hp)
    {
        if(_healthBarSlider.value < _healthBarSlider.maxValue)
        {
            _healthBarSlider.value = hp;
        }
    }
}

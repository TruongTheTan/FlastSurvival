using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    private float _maxExp;
    private Slider _expBarSlider;

    public void SetData(float maxExp)
    {
        _expBarSlider = GetComponent<Slider>();
        _maxExp = maxExp;
        _expBarSlider.maxValue = _maxExp;
        _expBarSlider.value = 0;
    }

    public void OnExpChanged(float exp)
    {
        _expBarSlider.value += exp;
    }
    public float GetCurrentExp()
    {
        return _expBarSlider.value;
    }
}

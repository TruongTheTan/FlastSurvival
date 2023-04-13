using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarController : MonoBehaviour
{
	private float _maxHealth;
	private Slider _healthBarSlider;

	public void SetHealthPoint(float maxHealth)
	{
		_maxHealth = maxHealth;
		_healthBarSlider = GetComponent<Slider>();

		_healthBarSlider.maxValue = _maxHealth;
		_healthBarSlider.value = _maxHealth;
	}


	public void OnHealthChanged(float newHealth)
	{
		_healthBarSlider.value = newHealth;
	}
}

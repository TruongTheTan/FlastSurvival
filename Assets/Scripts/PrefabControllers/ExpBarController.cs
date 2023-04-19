using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
	private float _maxExp;
	private Slider _expBarSlider;
	public float GetCurrentExp { get => _expBarSlider.value; }

	public void SetData(float maxExp)
	{
		_expBarSlider = GetComponent<Slider>();
		_maxExp = maxExp;
		_expBarSlider.maxValue = _maxExp;
		_expBarSlider.value = 0;
	}

	public void OnExpChanged(float enemyPoint)
	{
		_expBarSlider.value += enemyPoint;
	}

}

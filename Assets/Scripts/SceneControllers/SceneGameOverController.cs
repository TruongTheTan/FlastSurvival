using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGameOverController : MonoBehaviour
{
	private TextMeshProUGUI _totalScoreText;
	private TextMeshProUGUI _survivedTimeText;

	private readonly string _totalScorePrefix = "Total Score: ";
	private readonly string _survivedTimePrefix = "Survived Time: ";



	private void Awake()
	{
		_totalScoreText = GameObject.Find("TotalScoreText").GetComponent<TextMeshProUGUI>();
		_survivedTimeText = GameObject.Find("SurvivedTimeText").GetComponent<TextMeshProUGUI>();

		_totalScoreText.text = _totalScorePrefix + DataPreserve.totalScore.ToString();
		_survivedTimeText.text = _survivedTimePrefix + DisplayTotalSurvivedTime();
	}



	private string DisplayTotalSurvivedTime()
	{
		int minutes = Mathf.FloorToInt(DataPreserve.survivedTime / 60f);
		int seconds = Mathf.FloorToInt(DataPreserve.survivedTime % 60f);
		return minutes.ToString() + ":" + seconds.ToString();
	}



	public void ExitButtonClick()
	{
		//Reset saved score and time
		DataPreserve.totalScore = 0;
		DataPreserve.survivedTime = 0;

		//Back to main menu scene
		SceneManager.LoadScene("SceneMainMenu");
	}
}